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
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class ActivityController : MacheteController
    {
        private readonly IActivityService serv;
        private readonly LookupCache lcache;
        private readonly IMapper map;
        private readonly IDefaults def;
        private CultureInfo CI;

        public ActivityController(
            IActivityService aServ, 
            LookupCache lc,
            IDefaults def,
            IMapper map)
        {
            this.lcache = lc;
            this.serv = aServ;
            this.map = map;
            this.def = def;
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
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Index()
        {
            var model = new ActivityIndex();
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
                model.authenticated = 1;
            else model.authenticated = 0;
            model.CI = (System.Globalization.CultureInfo)Session["Culture"];
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
            if (!User.Identity.IsAuthenticated) vo.authenticated = false;
            dataTableResult<DTO.ActivityList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.ActivityList, ViewModel.ActivityList>(e)
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
        /// GET: /Activity/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Create()
        {
            var m = map.Map<Domain.Activity, ViewModel.Activity>(new Domain.Activity()
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
        public JsonResult Create(Domain.Activity activ, string userName)
        {
            UpdateModel(activ);
            activ.firstID = activ.ID;

            if (activ.nameID == 0)
            {
                if (activ.typeID == lcache.getByKeys(LCategory.activityType, LActType.Assembly))
                    activ.nameID = lcache.getByKeys(LCategory.activityName, LActName.Assembly);
                else if (activ.typeID == lcache.getByKeys(LCategory.activityType, LActType.OrgMtg))
                    activ.nameID = lcache.getByKeys(LCategory.activityName, LActName.OrgMtg);
                else
                    throw new MacheteIntegrityException("Something went wrong with Activity Types.");
            }

            if (activ.dateEnd < activ.dateStart)
                return Json(new { jobSuccess = false, rtnMessage = "End date must be greater than start date." });

            Domain.Activity firstAct = serv.Create(activ, userName);
            var result = map.Map<Domain.Activity, ViewModel.Activity>(firstAct);
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
            Domain.Activity firstAct = serv.Get(id);
            var m = map.Map<Domain.Activity, ViewModel.ActivitySchedule>(firstAct);
            m.def = def;
            return PartialView("CreateMany", m);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult CreateMany(ActivitySchedule actSched, string userName)
        {
            UpdateModel(actSched); // copy values from form to object. why this is necessary if the object is being passed as arg, I don't know.
            Domain.Activity firstActivity = serv.Get(actSched.firstID);
            var instances = actSched.stopDate.Subtract(actSched.dateStart).Days;
            var length = actSched.dateEnd.Subtract(actSched.dateStart).TotalMinutes;

            for (var i = 0; i <= instances; ++i) // This should skip right over firstAct.
            {
                var date = actSched.dateStart.AddDays(i);
                var day = (int)date.DayOfWeek;

                if (day == 0 && !actSched.sunday) break;
                else if (day == 1 && !actSched.monday) break;
                else if (day == 2 && !actSched.tuesday) break;
                else if (day == 3 && !actSched.wednesday) break;
                else if (day == 4 && !actSched.thursday) break;
                else if (day == 5 && !actSched.friday) break;
                else if (day == 6 && !actSched.saturday) break;
                else
                {
                    var activ = new Domain.Activity();
                    activ.nameID = actSched.name;
                    activ.typeID = actSched.type;
                    activ.dateStart = date;
                    activ.dateEnd = date.AddMinutes(length);
                    activ.recurring = true;
                    activ.firstID = firstActivity.ID;
                    activ.teacher = actSched.teacher;
                    activ.notes = actSched.notes;

                    Domain.Activity act = serv.Create(activ, userName);
                }
            }
            
            return Json(new
            {
                sNewRef = "", //EditTabRef(firstActivity),
                sNewLabel = "", //EditTabLabel(firstActivity),
                iNewID = firstActivity.ID,
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
            var m = map.Map<Domain.Activity, ViewModel.Activity>(serv.Get(id));
            m.def = def;
            return PartialView("Edit", m);
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
            Domain.Activity activity = serv.Get(id);
            UpdateModel(activity);
            serv.Save(activity, userName);
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

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult DeleteMany(int id, string userName)
        {
            Domain.Activity firstToDelete = serv.Get(id);
            IEnumerable<Domain.Activity> allToDelete = serv.GetAll()
                .Where(w => w.firstID == firstToDelete.firstID && w.dateStart >= firstToDelete.dateStart);

            foreach (Domain.Activity toDelete in allToDelete)
            {
                serv.Delete(toDelete.ID, userName);
            }

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
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Assign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            serv.AssignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        //
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
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
