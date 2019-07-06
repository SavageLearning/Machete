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
using System.Threading.Tasks;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;
using Activity = Machete.Domain.Activity;
using ActivityList = Machete.Service.DTO.ActivityList;

namespace Machete.Web.Controllers
{

        public class ActivityController : MacheteController
    {
        private readonly IActivityService _serv;
        private readonly IMapper _map;
        private readonly IDefaults _defaults;
        private readonly TimeZoneInfo _clientTimeZoneInfo;
        private readonly TimeZoneInfo _serverTimeZoneInfo;

        public ActivityController(
            IActivityService aServ, 
            IDefaults defaults,
            IMapper map,
            ITenantService tenantService
        )
        {
            _serv = aServ;
            _map = map;
            _defaults = defaults;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
            _serverTimeZoneInfo = TimeZoneInfo.Local;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Activity/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, Check-in")]
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var vo = _map.Map<jQueryDataTableParam, viewOptions>(param);

            var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;            
            
            dataTableResult<ActivityList> list = _serv.GetIndexView(vo, culture.TwoLetterISOLanguageName);

            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var result = list.query
                .Select(
                    e => _map.Map<ActivityList, ViewModel.ActivityList>(e)
                ).AsEnumerable();
                
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }

        /// <summary>
        /// GET: /Activity/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Create()
        {
            var now = DateTime.Now;
            var utcNow = TimeZoneInfo.ConvertTimeToUtc(now, _serverTimeZoneInfo);
            var clientNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, _clientTimeZoneInfo);
            
            var activity = new Activity
            {
                dateStart = clientNow,
                dateEnd = clientNow.AddHours(1)
            };
            var m = _map.Map<Activity, ViewModel.Activity>(activity);
            m.def = _defaults;
            
            return PartialView("Create", m);
        }
        /// <summary>
        /// POST: /Activity/Create
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="userName"></param>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<JsonResult> Create(Activity activity, string userName)
        {
            if (!await TryUpdateModelAsync(activity, "")) return Json(new {jobSuccess = false});
            if (activity.dateEnd < activity.dateStart)
                return Json(new { jobSuccess = false, rtnMessage = "End date must be greater than start date." });

            // leave for now, can be substituted with a reference method
            var assemblyType = _defaults.byKeys(LCategory.activityType, LActType.Assembly);
            var assemblyName = _defaults.byKeys(LCategory.activityName, LActName.Assembly);
            var orgMtgType = _defaults.byKeys(LCategory.activityType, LActType.OrgMtg);
            var orgMtgName = _defaults.byKeys(LCategory.activityName, LActName.OrgMtg);
            var activityNameEmpty = activity.nameID == 0;
            activity.notes = activity.notes ?? "";

            if (activity.typeID == assemblyType && activityNameEmpty) activity.nameID = assemblyName;
            if (activity.typeID == orgMtgType && activityNameEmpty) activity.nameID = orgMtgName;
            activity.firstID = activity.ID;
            //

            activity.dateStart = TimeZoneInfo.ConvertTimeToUtc(activity.dateStart, _clientTimeZoneInfo);
            activity.dateEnd = TimeZoneInfo.ConvertTimeToUtc(activity.dateEnd, _clientTimeZoneInfo);
            activity = _serv.Create(activity, userName);
    
            var result = _map.Map<Activity, ViewModel.Activity>(activity);
            // there are no dates to worry about in this mapping
    
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID,
                jobSuccess = true
            });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult CreateMany(int id)
        {
            Activity firstAct = _serv.Get(id);

            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            var m = _map.Map<Activity, ActivitySchedule>(firstAct);

            return PartialView("CreateMany", m);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ViewResult> CreateMany(ActivitySchedule actSched, string userName)
        {
            var instances = actSched.stopDate.Subtract(actSched.dateStart).Days;
            if (!await TryUpdateModelAsync(actSched) || instances == 0) {
                ModelState.AddModelError("ActivitySchedule", "Select an appropriate length of time for these events."); 
                return View("CreateMany", actSched); //Json(new { jobSuccess = false });
            }

            var length = actSched.dateEnd.Subtract(actSched.dateStart).TotalMinutes;
            var utcDate = TimeZoneInfo.ConvertTimeToUtc(actSched.dateStart, _clientTimeZoneInfo);

            for (var i = 1; i <= instances; i++) 
            {
                var currentDate = utcDate.AddDays(i);
                var day = (int)currentDate.DayOfWeek;

                if (day == 0 && !actSched.sunday) continue;
                if (day == 1 && !actSched.monday) continue;
                if (day == 2 && !actSched.tuesday) continue;
                if (day == 3 && !actSched.wednesday) continue;
                if (day == 4 && !actSched.thursday) continue;
                if (day == 5 && !actSched.friday) continue;
                if (day == 6 && !actSched.saturday) continue;

                var activity = new Activity
                {
                    nameID = actSched.name,
                    typeID = actSched.type,
                    dateStart = currentDate,
                    dateEnd = currentDate.AddMinutes(length),
                    recurring = true,
                    firstID = actSched.firstID,
                    teacher = actSched.teacher,
                    notes = actSched.notes ?? ""
                };

                _serv.Create(activity, userName);
            }

            // Machete: A series of good intentions, marinated in panic ~C
//            var result = _map.Map<Activity, ViewModel.Activity>(firstActivity);
//            return Json(new
//            {
//                sNewRef = result.tabref,
//                sNewLabel = result.tablabel,
//                iNewID = firstActivity.ID,
//                jobSuccess = true
//            });
            return View("Index");
        }
        /// <summary>
        /// GET: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Edit(int id)
        {
            var activity = _serv.Get(id);
            var viewModel = _map.Map<Activity, ViewModel.Activity>(activity);
            viewModel.def = _defaults;
            viewModel.dateStart = TimeZoneInfo.ConvertTimeFromUtc(activity.dateStart, _clientTimeZoneInfo);
            viewModel.dateEnd = TimeZoneInfo.ConvertTimeFromUtc(activity.dateEnd, _clientTimeZoneInfo);

            return PartialView("Edit", viewModel);
        }
        /// <summary>
        /// POST: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<JsonResult> Edit(Activity activity, string userName)
        {
            if (!await TryUpdateModelAsync(activity, "")) return Json(new {jobSuccess = false});
            if (activity.dateEnd < activity.dateStart)
                return Json(new { jobSuccess = false, rtnMessage = "End date must be greater than start date." });

            // leave for now, can be substituted with a reference method
            var assemblyType = _defaults.byKeys(LCategory.activityType, LActType.Assembly);
            var assemblyName = _defaults.byKeys(LCategory.activityName, LActName.Assembly);
            var orgMtgType = _defaults.byKeys(LCategory.activityType, LActType.OrgMtg);
            var orgMtgName = _defaults.byKeys(LCategory.activityName, LActName.OrgMtg);
            var activityNameEmpty = activity.nameID == 0;

            if (activity.typeID == assemblyType && activityNameEmpty) activity.nameID = assemblyName;
            if (activity.typeID == orgMtgType && activityNameEmpty) activity.nameID = orgMtgName;
            activity.firstID = activity.ID;
            activity.notes = activity.notes ?? "";

            activity.dateStart = TimeZoneInfo.ConvertTimeToUtc(activity.dateStart, _clientTimeZoneInfo);
            activity.dateEnd = TimeZoneInfo.ConvertTimeToUtc(activity.dateEnd, _clientTimeZoneInfo);
            
            _serv.Save(activity, userName);
    
            var result = _map.Map<Activity, ViewModel.Activity>(activity);
            // there are no dates to worry about in this mapping
    
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID,
                jobSuccess = true
            });
        }

        /// <summary>
        /// POST /Activity/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName">Automatically populated by UserNameFilter.</param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Delete(int id, string userName)
        {
            _serv.Delete(id, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            });
        }
        // POST /Activity/DeleteMany/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult DeleteMany(int id, string userName)
        {
            Activity firstToDelete = _serv.Get(id);
            List<int> allToDelete = _serv.GetAll()
                .Where(w => w.firstID == firstToDelete.firstID && w.dateStart >= firstToDelete.dateStart)
                .Select(s => s.ID).ToList();

            foreach (int toDelete in allToDelete)
            {
                _serv.Delete(toDelete, userName);
            }

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            });
        }
        // POST /Activity/Assign/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Assign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            _serv.AssignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true
            });
        }
        // POST /Activity/Unassign/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Unassign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            _serv.UnassignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true
            });
        }
    }
}
