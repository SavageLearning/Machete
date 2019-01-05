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
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Activity = Machete.Domain.Activity;
using ActivityList = Machete.Service.DTO.ActivityList;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class ActivityController : MacheteController
    {
        private readonly IActivityService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        private CultureInfo CI;

        public ActivityController(
            IActivityService aServ, 
            IDefaults def,
            IMapper map)
        {
            serv = aServ;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Index()
        {
            var model = new ActivityIndex();
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
                model.authenticated = 1;
            else model.authenticated = 0;
            model.CI = Session["Culture"];
            return View(model);
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
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            
            var userIdentity = new ClaimsIdentity("Cookies");
            if (!userIdentity.IsAuthenticated) vo.authenticated = false;
            dataTableResult<ActivityList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<ActivityList, ViewModel.ActivityList>(e)
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
            var m = map.Map<Activity, ViewModel.Activity>(new Activity
            {
                dateStart = DateTime.Now,
                dateEnd = DateTime.Now.AddHours(1)
            });
            m.def = def;
            return PartialView("Create", m);
        }
        /// <summary>
        /// POST: /Activity/Create
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<JsonResult> Create(Activity activ, string userName)
        {
            if (await TryUpdateModelAsync(activ, "")) {
                activ.firstID = activ.ID;
    
                if (activ.nameID == 0)
                {
                    if (activ.typeID == def.byKeys(LCategory.activityType, LActType.Assembly))
                        activ.nameID = def.byKeys(LCategory.activityName, LActName.Assembly);
                    else if (activ.typeID == def.byKeys(LCategory.activityType, LActType.OrgMtg))
                        activ.nameID = def.byKeys(LCategory.activityName, LActName.OrgMtg);
                    else
                        throw new MacheteIntegrityException("Something went wrong with Activity Types.");
                }
    
                if (activ.dateEnd < activ.dateStart)
                    return Json(new { jobSuccess = false, rtnMessage = "End date must be greater than start date." });
    
                Activity firstAct = serv.Create(activ, userName);
                var result = map.Map<Activity, ViewModel.Activity>(firstAct);
    
                if (activ.recurring)
                {
                    result.tablabel = "Recurring event with " + firstAct.teacher;
                    result.tabref = "/Activity/CreateMany/" + Convert.ToString(firstAct.ID);
                }
    
                return Json(new
                {
                    sNewRef = result.tabref,
                    sNewLabel = result.tablabel,
                    iNewID = result.ID,
                    jobSuccess = true
                });
            } else {
                return Json(new { jobSuccess = false });
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult CreateMany(int id)
        {
            Activity firstAct = serv.Get(id);
            var m = map.Map<Activity, ActivitySchedule>(firstAct);
            m.def = def;
            return PartialView("CreateMany", m);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<JsonResult> CreateMany(ActivitySchedule actSched, string userName)
        {
            if (await TryUpdateModelAsync(actSched)) {
                var firstActivity = serv.Get(actSched.firstID);
                var instances = actSched.stopDate.Subtract(actSched.dateStart).Days;
                var length = actSched.dateEnd.Subtract(actSched.dateStart).TotalMinutes;
                
                for (var i = 0; i <= instances; ++i) // This should skip right over firstAct.
                {
                    var date = actSched.dateStart.AddDays(i);
                    var day = (int)date.DayOfWeek;

                    if (day == 0 && !actSched.sunday) continue;
                    if (day == 1 && !actSched.monday) continue;
                    if (day == 2 && !actSched.tuesday) continue;
                    if (day == 3 && !actSched.wednesday) continue;
                    if (day == 4 && !actSched.thursday) continue;
                    if (day == 5 && !actSched.friday) continue;
                    if (day == 6 && !actSched.saturday) continue;
                    var activ = new Activity();
                    activ.nameID = actSched.name;
                    activ.typeID = actSched.type;
                    activ.dateStart = date;
                    activ.dateEnd = date.AddMinutes(length);
                    activ.recurring = true;
                    activ.firstID = firstActivity.ID;
                    activ.teacher = actSched.teacher;
                    activ.notes = actSched.notes;

                    serv.Create(activ, userName);
                }
                var result = map.Map<Activity, ViewModel.Activity>(firstActivity);
                return Json(new
                {
                    sNewRef = result.tabref,
                    sNewLabel = result.tablabel,
                    iNewID = firstActivity.ID,
                    jobSuccess = true
                });
            } else {
                return Json(new { jobSuccess = false });
            }
        }
        /// <summary>
        /// GET: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Edit(int id)
        {
            var m = map.Map<Activity, ViewModel.Activity>(serv.Get(id));
            m.def = def;
            return PartialView("Edit", m);
        }
        /// <summary>
        /// POST: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<JsonResult> Edit(int id, string userName)
        {
            var activity = serv.Get(id);
            
            if (await TryUpdateModelAsync(activity)) {
                serv.Save(activity, userName);
                return Json(new { jobSuccess = true });
            }
            
            return Json(new { jobSuccess = false });
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
            serv.Delete(id, userName);

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
            Activity firstToDelete = serv.Get(id);
            List<int> allToDelete = serv.GetAll()
                .Where(w => w.firstID == firstToDelete.firstID && w.dateStart >= firstToDelete.dateStart)
                .Select(s => s.ID).ToList();

            foreach (int toDelete in allToDelete)
            {
                serv.Delete(toDelete, userName);
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
            serv.AssignList(personID, actList, userName);

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
            serv.UnassignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true
            });
        }
    }
}
