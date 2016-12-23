#region COPYRIGHT
// File:     WorkerController.cs
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
using AutoMapper;
using Machete.Data;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerController : MacheteController
    {
        private readonly IWorkerService serv;
        private readonly IImageService imageServ;
        private readonly IWorkerCache wcache;
        private readonly IMapper map;
        private readonly IDefaults def;
        System.Globalization.CultureInfo CI;

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService,
                                IImageService  imageServ,
                                IWorkerCache wc,
            IDefaults def,
            IMapper map)
        {
            this.wcache = wc;
            this.serv = workerService;
            this.imageServ = imageServ;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Administrator, Teacher, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<DTO.WorkerList> list = serv.GetIndexView(vo);
            var result = list.query
            .Select(
                e => map.Map<DTO.WorkerList, ViewModel.WorkerList>(e)
            ).AsEnumerable();
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wkr"></param>
        /// <returns></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")] 
        public ActionResult Create(int ID)
        {
            // TODO handle exception of next worker number
            var nextnum = serv.GetNextWorkerNum();
            var w = map.Map<Domain.Worker, ViewModel.Worker>(new Domain.Worker()
            {
                dwccardnum = nextnum
            });
            w.def = def;
            return PartialView("Create", w);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="userName"></param>
        /// <param name="imagefile"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")]
        public ActionResult Create(Domain.Worker worker, string userName, HttpPostedFileBase imagefile)
        {
            UpdateModel(worker);
            if (imagefile != null) updateImage(worker, imagefile);
            Worker newWorker = serv.Create(worker, userName);
            var result = map.Map<Domain.Worker, ViewModel.Worker>(newWorker);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker w = serv.Get(id);
            var m = map.Map<Domain.Worker, ViewModel.Worker>(w);
            m.def = def;
            return PartialView(m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_model"></param>
        /// <param name="userName"></param>
        /// <param name="imagefile"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")]
        public ActionResult Edit(int id, Worker _model, string userName, HttpPostedFileBase imagefile)
        {
            Worker worker = serv.Get(id);
            UpdateModel(worker);
            
            if (imagefile != null) updateImage(worker, imagefile);                
            serv.Save(worker, userName);
            return Json(new
            {
                jobSuccess = true
            }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {            
            serv.Delete(id, user);

            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult RefreshCache()
        {
            wcache.Refresh();
            return Json(new
            {
                status = "OK"
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="imagefile"></param>
        private void updateImage(Worker worker, HttpPostedFileBase imagefile)
        {
            // TODO: Move this to the business layer
            if (worker == null) throw new MacheteNullObjectException("updateImage called with null worker");
            if (imagefile == null) throw new MacheteNullObjectException("updateImage called with null imagefile");
            if (worker.ImageID != null)
            {
                Image image = imageServ.Get((int)worker.ImageID);
                image.ImageMimeType = imagefile.ContentType;
                image.parenttable = "Workers";
                image.filename = imagefile.FileName;
                image.recordkey = worker.ID.ToString();
                image.ImageData = new byte[imagefile.ContentLength];
                imagefile.InputStream.Read(image.ImageData,
                                           0,
                                           imagefile.ContentLength);
                imageServ.Save(image, this.User.Identity.Name);
            }
            else
            {
                Image image = new Image();
                image.ImageMimeType = imagefile.ContentType;
                image.parenttable = "Workers";
                image.recordkey = worker.ID.ToString();
                image.ImageData = new byte[imagefile.ContentLength];
                imagefile.InputStream.Read(image.ImageData,
                                           0,
                                           imagefile.ContentLength);
                Image newImage = imageServ.Create(image, this.User.Identity.Name);
                worker.ImageID = newImage.ID;
            }
        }
    }
}