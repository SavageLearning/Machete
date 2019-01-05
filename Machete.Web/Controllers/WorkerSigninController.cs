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
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerSigninController : MacheteController
    {
        private readonly IWorkerSigninService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        private CultureInfo CI;        
        public WorkerSigninController(IWorkerSigninService workerSigninService, 
            IDefaults def,
            IMapper map)
        {
            serv = workerSigninService;
            this.map = map;
            this.def = def;
        }

        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
        }
        //
        // GET: /WorkerSignin/
        // Initial page creation
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index()
        {        
            return View();
        }
        //
        // POST: /WorkerSignin/Index -- records a signin
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardnum, DateTime dateforsignin, string userName)
        {
            var wsi = serv.CreateSignin(dwccardnum, dateforsignin, userName);
            var result = map.Map<WorkerSignin, ViewModel.WorkerSignin>(wsi);
            return Json(result);

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
            serv.moveDown(id, userName);
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
        /// and moves the preceeding set member into their spot.
        /// </summary>
        /// <param name="id">The Worker ID of the person to be moved down.</param>
        /// <param name="userName">The username of the person making the request.</param>
        /// <returns>Json (bool jobSuccess, string status)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveUp(int id, string userName)
        {
            serv.moveUp(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                workerID = id
            });
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
            var record = serv.Get(id);
            if (record.WorkAssignmentID != null)
            {
                return Json(new
                    {
                        jobSuccess = false,
                        rtnMessage = "You cannot delete a signin that has been associated with an Assignment. Disassociate the sigin with the assignment first."
                    });
            }

            serv.Delete(id, userName);            
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
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<WorkerSigninList> was = serv.GetIndexView(vo);
            var result = was.query
                .Select(e => map.Map<WorkerSigninList, ViewModel.WorkerSigninList>(e))
                .AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = was.totalCount,
                iTotalDisplayRecords = was.filteredCount,
                aaData = result
            });
        }
    }
}
