#region COPYRIGHT
// File:     EventController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    public class EventController : MacheteController
    {
        private readonly IEventService _eventServ;
        private readonly IImageService _imageServ;
        private readonly IMapper _map;
        private readonly IDefaults _def;

        public EventController(IEventService eventService, 
            IImageService imageServ, 
            IDefaults def,
            IMapper map)
        {
            _eventServ = eventService;
            _imageServ = imageServ;
            _map = map;
            _def = def;
        }

        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {            
            //Get all the records
            var vo = _map.Map<jQueryDataTableParam, viewOptions>(param);
            dataTableResult<EventList> list = _eventServ.GetIndexView(vo);
            //return what's left to datatables
            var result = list.query
                .Select(e => _map.Map<EventList, ViewModel.EventList>(e))
                .AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }
        //
        // GET: /Event/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(int personID)
        {
            var m = _map.Map<Event, ViewModel.Event>(new Event
            {
                dateFrom = DateTime.Today,
                dateTo = DateTime.Today,
                PersonID = personID
            });
            m.def = _def;
            return PartialView("Create", m);
        }

        //
        // POST: /Event/Create
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<ActionResult> Create(Event evnt, string userName)
        {
            var couldUpdateModel = await TryUpdateModelAsync(evnt);
            if (!couldUpdateModel) return Json(new {jobSuccess = false});
            
            var newEvent = _eventServ.Create(evnt, userName);
            var result = _map.Map<Event, ViewModel.Event>(newEvent);
            return Json(new {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = newEvent.ID,
                jobSuccess = true
            });
        }

        //
        // GET: /Event/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            var m = _map.Map<Event, ViewModel.Event>(_eventServ.Get(id));
            m.def = _def;
            return PartialView("Edit", m);
        }

        // POST: /Event/Edit/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit(int id, string userName)
        {
            var evnt = _eventServ.Get(id);
            var couldUpdateModel = await TryUpdateModelAsync(evnt);
            if (!couldUpdateModel) return Json(new { status = "Not OK" });
            
            _eventServ.Save(evnt, userName);
            return Json(new {status = "OK"});
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> AddImage(int id, string userName, IFormFile imagefile)
        {
            if (imagefile == null) throw new MacheteNullObjectException("AddImage called with null imagefile");
            
            var joiner = new JoinEventImage();

            var evnt = _eventServ.Get(id);

            var image = new Image();
            image.ImageMimeType = imagefile.ContentType;
            image.parenttable = "Events";
            image.filename = imagefile.FileName;
            image.recordkey = id.ToString();
            using (var stream = new MemoryStream())
            {
                await imagefile.CopyToAsync(stream);
                image.ImageData = stream.ToArray();
            }
            
            // This should be unnecessary; image will have ID
            Image newImage = _imageServ.Create(image, userName);

            joiner.ImageID = newImage.ID;
            joiner.EventID = evnt.ID;
            joiner.datecreated = DateTime.Now;
            joiner.dateupdated = DateTime.Now;
            joiner.updatedby = userName;
            joiner.createdby = userName;

            _eventServ.JoinEventImages(evnt, joiner, userName);
            
            _eventServ.Save(evnt, userName);
            
            return Json(new
            {
                status = "OK"                
            });
        }

        // GET: /Event/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {   
            _eventServ.Delete(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            });
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteImage(int evntID, int jeviID, string user)
        {
            var evnt = _eventServ.Get(evntID);
            var joinEventImage = evnt.JoinEventImages.Single(e => e.ID == jeviID);
            var deletedImageId = joinEventImage.ID;
            _imageServ.Delete(joinEventImage.ImageID, user);
            evnt.JoinEventImages.Remove(joinEventImage);

            return Json(new
            {
                status = "OK",
                deletedID = deletedImageId
            });
        }
    }
}
