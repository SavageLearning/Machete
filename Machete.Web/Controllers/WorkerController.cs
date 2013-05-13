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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Machete.Domain;
//using Machete.Helpers;
using Machete.Service;
using Machete.Web.ViewModel;
using Machete.Web.Models;
using Elmah;
using NLog;
using Machete.Web.Helpers;
using System.Web.Routing;
using AutoMapper;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerController : MacheteController
    {
        private readonly IWorkerService serv;
        private readonly IImageService imageServ;
        System.Globalization.CultureInfo CI;

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService,
                                IImageService  imageServ)
        {
            this.serv = workerService;
            this.imageServ = imageServ;
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
        [Authorize(Roles = "Manager, Administrator, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Worker> list = serv.GetIndexView(vo);
            var result = from p in list.query select new
            { 
                tabref = "/Worker/Edit/" + Convert.ToString(p.ID),
                tablabel =  p.Person.firstname1 + ' ' + p.Person.lastname1,
                WID =    p.ID.ToString(),
                recordid = p.ID.ToString(),
                dwccardnum =  Convert.ToString(p.dwccardnum),
                active =  Convert.ToString(p.active),
                wkrStatus = _getStatus(p),
                firstname1 = p.Person.firstname1, 
                firstname2 = p.Person.firstname2, 
                lastname1 = p.Person.lastname1, 
                lastname2 = p.Person.lastname2, 
                memberexpirationdate = Convert.ToString(p.memberexpirationdate)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wkr"></param>
        /// <returns></returns>
        string _getStatus(Worker wkr)
        {
            if (wkr.memberStatus == Worker.iActive) // blue
                return "active";
            if (wkr.memberStatus == Worker.iInactive) // blue
                return "inactive";
            if (wkr.memberStatus == Worker.iExpired) // blue
                return "expired";
            if (wkr.memberStatus == Worker.iSanctioned) // blue
                return "sanctioned";
            if (wkr.memberStatus == Worker.iExpelled) // blue
                return "expelled";
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(int ID)
        {
            var _model = new Worker();
            _model.ID = ID;
            _model.RaceID = Lookups.getDefaultID(LCategory.race);
            _model.countryoforiginID = Lookups.getDefaultID(LCategory.countryoforigin);
            _model.englishlevelID = 0;
            _model.neighborhoodID = Lookups.getDefaultID(LCategory.neighborhood);
            _model.maritalstatus = Lookups.getDefaultID(LCategory.maritalstatus);
            return PartialView(_model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="userName"></param>
        /// <param name="imagefile"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")]
        public ActionResult Create(Worker worker, string userName, HttpPostedFileBase imagefile)
        {
            UpdateModel(worker);
            if (imagefile != null) updateImage(worker, imagefile);
            Worker newWorker = serv.Create(worker, userName);
            return Json(new
            {
                //sNewRef = _getTabRef(newWorker),
                //sNewLabel = _getTabLabel(newWorker),
                iNewID = newWorker.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker _worker = serv.Get(id);
            return PartialView(_worker);
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
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")]
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
            serv.RefreshCache();
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