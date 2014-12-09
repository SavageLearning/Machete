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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Domain;
using Machete.Data;
using Machete.Web.Models;
using System.Web.Routing;
using AutoMapper;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ActivitySigninController : MacheteController
    {
        private readonly IActivitySigninService serv;
        private readonly IWorkerService wServ;
        private readonly LookupCache lcache;
        private System.Globalization.CultureInfo CI;

        public ActivitySigninController(IActivitySigninService serv, 
                                 IWorkerService wServ,
                                 LookupCache lc)
        {
            this.serv = serv;
            this.wServ = wServ;
            this.lcache = lc;
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
        //[Authorize(Roles = "Manager, Administrator, Check-in")]
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
        //[Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardnum, int activityID)
        {
            var _asi = new ActivitySignin();
            // Tthe card just swiped
            _asi.dateforsignin = DateTime.Now;
            _asi.activityID = activityID;
            _asi.dwccardnum = dwccardnum;            
            //
            //
            Worker w = serv.CreateSignin(_asi, this.User.Identity.Name);
            //Get picture from checkin, show with next view
            Image checkin_image = serv.getImage(dwccardnum);
            string imageRef = "/Content/images/NO-IMAGE-AVAILABLE.jpg";
            if (checkin_image != null)
            {
                imageRef = "/Image/GetImage/" + checkin_image.ID;
            }

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
        [Authorize(Roles = "Administrator, Manager, Check-in")]
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
        //[Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<asiView> was = serv.GetIndexView(vo);

            //return what's left to datatables
            var result = from p in was.query
                         select new
                         {
                             WSIID = p.ID,
                             recordid = p.ID.ToString(),
                             dwccardnum = p.dwccardnum,
                             fullname = p.fullname,
                             firstname1 = p.firstname1,
                             firstname2 = p.firstname2,
                             lastname1 = p.lastname1,
                             lastname2 = p.lastname2,
                             dateforsignin = p.dateforsignin,
                             dateforsigninstring = p.dateforsignin.ToShortDateString(),
                             memberStatus = lcache.textByID(p.memberStatus, CI.TwoLetterISOLanguageName),
                             memberInactive = p.w.isInactive,
                             memberSanctioned = p.w.isSanctioned,
                             memberExpired = p.w.isExpired,
                             memberExpelled = p.w.isExpelled,
                             imageID = p.imageID,
                             expirationDate = p.expirationDate.ToShortDateString(),
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = was.totalCount,
                iTotalDisplayRecords = was.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
