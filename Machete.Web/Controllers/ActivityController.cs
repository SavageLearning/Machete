#region COPYRIGHT
// File:     ActivityController.cs
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
using System.Web.Mvc;
using System.Web.Routing;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using System.Globalization;
using AutoMapper;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class ActivityController : MacheteController
    {
        private readonly IActivityService serv;
        private CultureInfo CI;

        public ActivityController(IActivityService aServ)
        {
            this.serv = aServ;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET: /Activity/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            if (!User.Identity.IsAuthenticated) vo.authenticated = false;
            dataTableResult<Activity> list = serv.GetIndexView(vo);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = from p in list.query
                         select dtResponse(ref p)
            },
            JsonRequestBehavior.AllowGet);
        }
        private object dtResponse(ref Activity p)
        {
            return new
            {
                tabref = EditTabRef(p),
                tablabel = EditTabLabel(p),
                name = LookupCache.textByID(p.name, CI.TwoLetterISOLanguageName),
                type = LookupCache.textByID(p.type, CI.TwoLetterISOLanguageName),
                count = p.Signins.Count(),
                teacher = p.teacher,
                dateStart = p.dateStart.ToString(),
                dateEnd = p.dateEnd.ToString(),
                AID = Convert.ToString(p.ID),
                recordid = Convert.ToString(p.ID),
                dateupdated = Convert.ToString(p.dateupdated),
                Updatedby = p.Updatedby
            };
        }
        private string EditTabRef(Activity emp)
        {
            if (emp == null) return null;
            return "/Activity/Edit/" + Convert.ToString(emp.ID);
        }
        private string EditTabLabel(Activity emp)
        {
            if (emp == null) return null;
            return emp.dateStart.ToString() + " - " + 
                    LookupCache.textByID(emp.name, CI.TwoLetterISOLanguageName) + " - " +
                    emp.teacher;
        }
        /// <summary>
        /// GET: /Activity/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Create()
        {
            var _model = new Activity();
            _model.dateStart = DateTime.Today;
            _model.dateEnd = DateTime.Today;
            //_model.city = "Seattle";
            //_model.state = "WA";
            //_model.blogparticipate = false;
            //_model.referredby = Lookups.emplrreferenceDefault;
            return PartialView("Create", _model);
        }
        /// <summary>
        /// POST: /Activity/Create
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Create(Activity activ, string userName)
        {
            UpdateModel(activ);
            Activity newActivity = serv.Create(activ, userName);

            return Json(new
            {
                sNewRef = EditTabRef(newActivity),
                sNewLabel = EditTabLabel(newActivity),
                iNewID = newActivity.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// GET: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Edit(int id)
        {
            Activity employer = serv.Get(id);
            return PartialView("Edit", employer);
        }
        /// <summary>
        /// POST: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Edit(int id, FormCollection collection, string userName)
        {
            Activity employer = serv.Get(id);
            UpdateModel(employer);
            serv.Save(employer, userName);
            return Json(new
            {
                jobSuccess = true
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>s
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Delete(int id, string userName)
        {
            serv.Delete(id, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager,Teacher")]
        public JsonResult Assign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            serv.AssignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true//,
                //deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager,Teacher")]
        public JsonResult Unassign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            serv.UnassignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true//,
                //deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
