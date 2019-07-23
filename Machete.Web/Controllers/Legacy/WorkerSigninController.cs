#region COPYRIGHT
// File:     WorkerSigninController.cs
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
using System.Globalization;
using System.Linq;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkerSignin = Machete.Web.ViewModel.WorkerSignin;

namespace Machete.Web.Controllers
{
        public class WorkerSigninController : MacheteController
    {
        private readonly IWorkerSigninService _serv;
        private readonly IMapper _map;
        private readonly TimeZoneInfo _clientTimeZoneInfo;

        public WorkerSigninController(
            IWorkerSigninService workerSigninService,
            ITenantService tenantService,
            IMapper map)
        {
            _serv = workerSigninService;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
            _map = map;
        }

        //
        // GET: /WorkerSignin/
        // Initial page creation
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index()
        {
            var dateTime = DateTime.Now;
            var serverTime = TimeZoneInfo.Local;
            var dateforsignin = TimeZoneInfo.ConvertTimeToUtc(dateTime, serverTime);

            var model = new WorkerSignin
            {
                dateforsignin = TimeZoneInfo.ConvertTime(dateforsignin, _clientTimeZoneInfo)
            };

            return View(model);
        }
        //
        // POST: /WorkerSignin/Index -- records a signin
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardnum, DateTime dateforsignin, string userName)
        {
            try
            {
                var dateforsigninUTC = TimeZoneInfo.ConvertTimeToUtc(dateforsignin, _clientTimeZoneInfo);
                var wsi = _serv.CreateSignin(dwccardnum, dateforsigninUTC, userName);
                var result = _map.Map<Domain.WorkerSignin, WorkerSignin>(wsi);
                return Json(result);
            }
            catch (NullReferenceException)
            {
                return Json(new { jobSuccess = false });
            }
        }

        // GET: /WorkerSignin/Delete/5
       /// <summary>
       /// This method deletes a signin from the master Worker Signins list for the day.
       /// </summary>
       /// <param name="id">The Worker ID of the worker.</param>
       /// <param name="userName">The user performing the action.</param>
       /// <returns>Json (bool jobSuccess, string status, int deletedID)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public JsonResult Delete(int id, string userName)
        {
            var record = _serv.Get(id);
            if (record.WorkAssignmentID != null)
            {
                return Json(new
                    {
                        jobSuccess = false,
                        rtnMessage = "You cannot delete a signin that has been associated with an Assignment. Disassociate the sigin with the assignment first."
                    });
            }

            _serv.Delete(id, userName);            
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                deletedID = id
            });
        }

        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var vo = _map.Map<jQueryDataTableParam, viewOptions>(param);

            dataTableResult<WorkerSigninList> was = _serv.GetIndexView(vo);

            var result = was.query
                .Select(e => _map.Map<WorkerSigninList, ViewModel.WorkerSigninList>(e))
                .ToList();
                
            return Json(new
            {
                param.sEcho,
                iTotalRecords = was.totalCount,
                iTotalDisplayRecords = was.filteredCount,
                aaData = result
            });
        }
        
        /// <summary>
        /// This method invokes IWorkerSigninService.moveDown,
        /// which moves a worker down in numerical order in the daily 
        /// ('lottery') list,
        /// and moves the proceeding (next) set member into their spot.
        /// </summary>
        /// <param name="id">The Worker ID of the person to be moved down.</param>
        /// <param name="userName">The username of the person making the request.</param>
        /// <returns>Json (bool jobSuccess, string status)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveDown(int id, string userName)
        {
            _serv.moveDown(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK", 
                workerID = id
            });
        }

        /// <summary>
        /// This method invokes IWorkerSigninService.moveUp,
        /// which moves a worker up in numerical order in the 
        /// daily ('lottery') list,
        /// and moves the prece    eding set member into their spot.
        /// </summary>
        /// <param name="id">The Worker ID of the person to be moved down.</param>
        /// <param name="userName">The username of the person making the request.</param>
        /// <returns>Json (bool jobSuccess, string status)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveUp(int id, string userName)
        {
            _serv.moveUp(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                workerID = id
            });
        }
    }
}
