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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.Models;
using System.Web.Routing;
using AutoMapper;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EventController : MacheteController
    {
        private readonly IEventService _serv;
        private readonly IImageService iServ;
        System.Globalization.CultureInfo CI;
        //
        //
        public EventController(IEventService eventService, IImageService imageServ)
        {
            this._serv = eventService;
            this.iServ = imageServ;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        //
        //
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {            
            //Get all the records
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Event> list = _serv.GetIndexView(vo);
            //return what's left to datatables
            var result = from p in list.query select new
            {
                tabref = _getTabRef(p),
                tablabel = _getTabLabel(p, CI.TwoLetterISOLanguageName),
                recordid = p.ID,
                notes = p.notes,
                datefrom = p.dateFrom.ToShortDateString(),
                dateto = p.dateTo == null ? "" : ((DateTime)p.dateTo).ToShortDateString(),
                fileCount = p.JoinEventImages.Count(),
                type = LookupCache.byID(p.eventType, CI.TwoLetterISOLanguageName),
                dateupdated = Convert.ToString(p.dateupdated),
                Updatedby = p.Updatedby
            };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        //
        private string _getTabRef(Event evnt)
        {
            return "/Event/Edit/" + Convert.ToString(evnt.ID);
        }
        //
        //
        private string _getTabLabel(Event evnt, string locale)
        {
            return evnt.dateFrom.ToShortDateString() + " " + LookupCache.byID(evnt.eventType, locale);
        }
        //
        // GET: /Event/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(int PersonID)
        {
            Event eventobj = new Event();
            eventobj.dateFrom = DateTime.Today;
            eventobj.dateTo = DateTime.Today;
            eventobj.PersonID = PersonID;
            return PartialView(eventobj);
        }

        //
        // POST: /Event/Create
        [HttpPost, UserNameFilter]
        public ActionResult Create(Event evnt, string userName)
        {
            UpdateModel(evnt);
            Event newEvent = _serv.Create(evnt, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newEvent),
                sNewLabel = _getTabLabel(newEvent, CI.TwoLetterISOLanguageName),
                iNewID = newEvent.ID
            },
            JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Event/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            var evnt = _serv.Get(id);
            return PartialView("Edit", evnt);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id, string user)
        {
            Event evnt = _serv.Get(id);
            UpdateModel(evnt);
            _serv.Save(evnt, user);
  
            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }
        //
        // AddImage
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult AddImage(int id, string user, HttpPostedFileBase imagefile)
        {
            if (imagefile == null) throw new MacheteNullObjectException("AddImage called with null imagefile");
            JoinEventImage joiner = new JoinEventImage();
            Event evnt = _serv.Get(id);
            // TODO:The following code should be in the Service layer
            Image image = new Image();
            image.ImageMimeType = imagefile.ContentType;
            image.parenttable = "Events";
            image.filename = imagefile.FileName;
            image.recordkey = id.ToString();
            image.ImageData = new byte[imagefile.ContentLength];
            imagefile.InputStream.Read(image.ImageData,
                                       0,
                                       imagefile.ContentLength);
            Image newImage = iServ.Create(image, user);
            joiner.ImageID = newImage.ID;
            joiner.EventID = evnt.ID;
            joiner.datecreated = DateTime.Now;
            joiner.dateupdated = DateTime.Now;
            joiner.Updatedby = user;
            joiner.Createdby = user;
            // TODO: This tightly couples the MVC straight down to EF. 
            // breaks layering. Should be abstracted.
            evnt.JoinEventImages.Add(joiner);
            _serv.Save(evnt, user);
            var foo = iServ.Get(newImage.ID).ImageData;
            
            return Json(new
            {
                status = "OK"                
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Event/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {   
            _serv.Delete(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteImage(int evntID, int jeviID, string user)
        {
            int deletedJEVI = 0;
            Event evnt = _serv.Get(evntID);
            JoinEventImage jevi = evnt.JoinEventImages.Single(e => e.ID == jeviID);
            deletedJEVI = jevi.ID;
            iServ.Delete(jevi.ImageID, user);
            evnt.JoinEventImages.Remove(jevi);

            return Json(new
            {
                status = "OK",
                deletedID = deletedJEVI
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
