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
using System.Linq;
using AutoMapper;
using Machete.Service.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
        public class ActivitySigninController : MacheteController
    {
        private readonly IActivitySigninService serv;
        private readonly IMapper map;
        private TimeZoneInfo _clientTimeZoneInfo;

        public ActivitySigninController(
            IActivitySigninService serv,
            IMapper map,
            ITenantService tenantService
        )
        {
            this.serv = serv;
            this.map = map;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }

        /// <summary>
        /// GET /Activity/Index
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Administrator, Check-in, Teacher")]
        public ActionResult Index()
        {
            return View("~/Views/Shared/ActivitySigninIndex.cshtml");
        }

        /// <summary>
        /// POST /Activity/Index
        /// </summary>
        /// <param name="dwccardnum"></param>
        /// <param name="activityID"></param>
        /// <param name="userName"></param>
        [HttpPost]
        [UserNameFilter]
        [Authorize(Roles = "Manager, Administrator, Check-in, Teacher")]
        public ActionResult Index(int dwccardnum, int activityID, string userName)
        {
            var _asi = new ActivitySignin();

            var utcTime = DateTime.UtcNow;
            
            _asi.dateforsignin = utcTime;
            _asi.activityID = activityID;
            _asi.dwccardnum = dwccardnum;

            //Get picture from checkin, show with next view
            string imageRef = serv.getImageRef(dwccardnum);

            Worker w;
            try
            {
                w = serv.CreateSignin(_asi, userName);
            }
            catch (NullReferenceException)
            {
                return Json(new { jobSuccess = false });
            }
            return Json(new
            {
                memberExpired = w.isExpired,
                memberInactive = w.isInactive,
                memberSanctioned = w.isSanctioned,
                memberExpelled = w.isExpelled,
                imageRef,
                expirationDate = w.memberexpirationdate
            });
        }

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
            });
        }

        [Authorize(Roles = "Administrator, Manager, Check-in, Teacher")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            
            dataTableResult<ActivitySigninList> list = serv.GetIndexView(vo);

            var result = list.query
                .Select(
                    e => map.Map<ActivitySigninList, ViewModel.ActivitySigninList>(e)
                ).AsEnumerable();
            
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }
    }
}
