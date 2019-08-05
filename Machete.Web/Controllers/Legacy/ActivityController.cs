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
using Machete.Data.Identity;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly UserManager<MacheteUser> _userManager;

        public ActivityController(
            IActivityService aServ, 
            IDefaults defaults,
            UserManager<MacheteUser> userManager,
            IMapper map,
            ITenantService tenantService
        )
        {
            _serv = aServ;
            _map = map;
            _userManager = userManager;
            _defaults = defaults;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
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
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;

            var vo = _map.Map<jQueryDataTableParam, viewOptions>(param);
            
            dataTableResult<ActivityList> list = _serv.GetIndexView(vo);

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
        public async Task<ActionResult> Create()
        {
            var utcNow = DateTime.UtcNow;
            
            var activity = new Activity
            {
                dateStart = utcNow,
                dateEnd = utcNow.AddHours(1)
            };

            var teachers = await _userManager.GetUsersInRoleAsync(UserRoles.Teacher);
            var teacherNames = teachers.Select(teach => teach.UserName).ToList();
            
            MapperHelpers.Defaults = _defaults;
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.UserNames = teacherNames;
            
            var m = _map.Map<Activity, ViewModel.Activity>(activity);
            
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

            activity.notes = activity.notes ?? "";
            activity.firstID = activity.ID;
            activity.dateStart = TimeZoneInfo.ConvertTimeToUtc(activity.dateStart, _clientTimeZoneInfo);
            activity.dateEnd = TimeZoneInfo.ConvertTimeToUtc(activity.dateEnd, _clientTimeZoneInfo);

            activity = _serv.Create(activity, userName);
    
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var result = _map.Map<Activity, ViewModel.Activity>(activity);
    
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID,
                jobSuccess = true
            });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> CreateMany(int id)
        {
            Activity firstAct = _serv.Get(id);

            var teachers = await _userManager.GetUsersInRoleAsync(UserRoles.Teacher);
            var teacherNames = teachers.Select(teach => teach.UserName).ToList();
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            MapperHelpers.UserNames = teacherNames;
            
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
                return View("CreateMany", actSched);
            }

            var length = actSched.dateEnd.Subtract(actSched.dateStart).TotalMinutes;
            var utcDate = TimeZoneInfo.ConvertTimeToUtc(actSched.dateStart, _clientTimeZoneInfo);

            for (var i = 1; i <= instances; i++) 
            {
                var currentDate = utcDate.AddDays(i);
                var day = currentDate.DayOfWeek;

                switch (day)
                {
                    case DayOfWeek.Sunday when !actSched.sunday:
                    case DayOfWeek.Monday when !actSched.monday:
                    case DayOfWeek.Tuesday when !actSched.tuesday:
                    case DayOfWeek.Wednesday when !actSched.wednesday:
                    case DayOfWeek.Thursday when !actSched.thursday:
                    case DayOfWeek.Friday when !actSched.friday:
                    case DayOfWeek.Saturday when !actSched.saturday:
                        continue;
                    default:
                    {
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
                        break;
                    }
                }
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
        public async Task<ActionResult> Edit(int id)
        {
            var activity = _serv.Get(id);

            var teachers = await _userManager.GetUsersInRoleAsync(UserRoles.Teacher);
            var teacherNames = teachers.Select(teach => teach.UserName).ToList();
            
            MapperHelpers.Defaults = _defaults;
            MapperHelpers.StartDate = TimeZoneInfo.ConvertTimeFromUtc(activity.dateStart, _clientTimeZoneInfo);
            MapperHelpers.EndDate = TimeZoneInfo.ConvertTimeFromUtc(activity.dateEnd, _clientTimeZoneInfo);
            MapperHelpers.UserNames = teacherNames;
            
            var viewModel = _map.Map<Activity, ViewModel.Activity>(activity);

            return PartialView("Edit", viewModel);
        }
        /// <summary>
        /// POST: /Activity/Edit/5
        /// </summary>
        /// <param name="activity">The activity to be updated.</param>
        /// <param name="userName">Name of the current user from UserNameFilter.</param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<JsonResult> Edit(Activity activity, string userName)
        {
            if (!await TryUpdateModelAsync(activity, "")) return Json(new {jobSuccess = false});
            if (activity.dateEnd < activity.dateStart)
                return Json(new { jobSuccess = false, rtnMessage = "End date must be greater than start date." });

            activity.firstID = activity.ID;
            activity.notes = activity.notes ?? "";
            activity.dateStart = TimeZoneInfo.ConvertTimeToUtc(activity.dateStart, _clientTimeZoneInfo);
            activity.dateEnd = TimeZoneInfo.ConvertTimeToUtc(activity.dateEnd, _clientTimeZoneInfo);
            
            _serv.Save(activity, userName);
    
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;

            var result = _map.Map<Activity, ViewModel.Activity>(activity);
    
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
