#region COPYRIGHT
// File:     ActivitySigninController.cs
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
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ActivitySigninController : MacheteController
    {
        private readonly IActivitySigninService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        private System.Globalization.CultureInfo CI;

        public ActivitySigninController(
            IActivitySigninService serv, 
            IDefaults def,
            IMapper map)
        {
            this.serv = serv;
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
        [Authorize(Roles = "Manager, Administrator, Check-in, Teacher")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwccardnum"></param>
        /// <param name="activityID"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator, Check-in, Teacher")]
        public ActionResult Index(int dwccardnum, int activityID)
        {
            var _asi = new ActivitySignin();
            // Tthe card just swiped
            _asi.dateforsignin = DateTime.Now;
            _asi.activityID = activityID;
            _asi.dwccardnum = dwccardnum;            
            //
            //
            string imageRef = serv.getImageRef(dwccardnum);
            Worker w = serv.CreateSignin(_asi, this.User.Identity.Name);
            //Get picture from checkin, show with next view

            return Json(new
            {
                memberExpired = w.isExpired,
                memberInactive = w.isInactive,
                memberSanctioned = w.isSanctioned,
                memberExpelled = w.isExpelled,
                imageRef = imageRef,
                expirationDate = w.memberexpirationdate
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Check-in, Teacher")]
        public ActionResult Delete(int id, string userName)
        {
            serv.Delete(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Check-in, Teacher")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<DTO.ActivitySigninList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.ActivitySigninList, ViewModel.ActivitySigninList>(e)
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

    }
}
