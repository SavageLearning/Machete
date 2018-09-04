#region COPYRIGHT
// File:     WorkAssignmentController.cs
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
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkAssignmentController : MacheteController
    {
        private readonly IWorkAssignmentService waServ;
        private readonly IWorkOrderService woServ;
        private readonly IWorkerSigninService wsiServ;
        private readonly IMapper map;
        private readonly IDefaults def;
        private System.Globalization.CultureInfo CI;
        public WorkAssignmentController(IWorkAssignmentService workAssignmentService,
            IWorkOrderService workOrderService,
            IWorkerSigninService signinService,
            IDefaults def,
            IMapper map)

        {
            this.waServ = workAssignmentService;
            this.woServ = workOrderService;
            this.wsiServ = signinService;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        #region Index
        //
        // GET: /WorkAssignment/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in")]
        public ActionResult Index()
        {
            WorkAssignmentIndex wai = new WorkAssignmentIndex();
            wai.todaysdate = System.String.Format("{0:MM/dd/yyyy}", DateTime.Today);
            wai.def = def;
            return View(wai);
        }

        #endregion

        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records            
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Service.DTO.WorkAssignmentsList> was = waServ.GetIndexView(vo);
            var result = was.query
                .Select(e => map.Map<Service.DTO.WorkAssignmentsList, ViewModel.WorkAssignmentsList>(e))
                .AsEnumerable();
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = was.totalCount,
                iTotalDisplayRecords = was.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }          
        //
        // GET: /WorkAssignment/Create
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Create
        public ActionResult Create(int WorkOrderID, string _description)
        {
            var wa = map.Map<Domain.WorkAssignment, ViewModel.WorkAssignment>(new Domain.WorkAssignment()
            {
                active = true,
                workOrderID = WorkOrderID,
                skillID = def.getDefaultID(LCategory.skill),
                hours = def.hoursDefault,
                days = def.daysDefault,
                hourlyWage = def.hourlyWageDefault,
                description = _description
            });
            wa.def = def;
        return PartialView("Create", wa);
    }

    //
    // POST: /WorkAssignment/Create
    //
    [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(Domain.WorkAssignment assignment, string userName)
        {
            UpdateModel(assignment);
            assignment.workOrder = woServ.Get(assignment.workOrderID);
            var newAssignment = waServ.Create(assignment, userName);
            var result = map.Map<Domain.WorkAssignment, ViewModel.WorkAssignment>(newAssignment);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        
        //
        // POST: /WorkAssignment/Edit/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Duplicate
        public ActionResult Duplicate(int id, string userName)
        {
            //
            // TODO: Move duplication functionality to the service layer
            Domain.WorkAssignment _assignment = waServ.Get(id);
            Domain.WorkAssignment duplicate = _assignment;
            duplicate.workerAssigned = null;
            duplicate.workerAssignedID = null;
            duplicate.workerSiginin = null;
            duplicate.workerSigninID = null;
            var saved = waServ.Create(duplicate, userName);
            var result = map.Map<Domain.WorkAssignment, ViewModel.WorkAssignment>(saved);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID
            },
            JsonRequestBehavior.AllowGet);

        }
        #endregion

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        #region Assign
        public ActionResult Assign(int waid, int wsiid, string userName)
        {
            Domain.WorkerSignin signin = wsiServ.Get(wsiid);
            Domain.WorkAssignment assignment = waServ.Get(waid);
            waServ.Assign(assignment, signin, userName);

            return Json(new
            {
                jobSuccess = true
            }, JsonRequestBehavior.AllowGet);            
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Unassign(int? waid, int? wsiid, string userName)
        {
            waServ.Unassign(waid, wsiid, userName);
            return Json(new
            {
                jobSuccess = true,
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //
        // GET: /WorkAssignment/Edit/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Edit
        public ActionResult Edit(int id)
        {
            Domain.WorkAssignment wa = waServ.Get(id);
            var m = map.Map<Domain.WorkAssignment, ViewModel.WorkAssignment>(wa);
            m.def = def;
            return PartialView("Edit", m);
        }
        //
        // POST: /WorkAssignment/Edit/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, int? workerAssignedID, string userName)
        {
            Domain.WorkAssignment asmt = waServ.Get(id);    
            //Update from HTML attributes
            UpdateModel(asmt);
            waServ.Save(asmt, workerAssignedID, userName);
                
            return Json(new { jobSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion      
        //
        //GET: /WorkAssignment/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region View
        public ActionResult View(int id)
        {
            Domain.WorkAssignment workAssignment = waServ.Get(id);
            
            return View(workAssignment);
        }
        #endregion
        #region Delete
        //
        // POST: /WorkAssignment/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Delete(int id, FormCollection collection, string user)
        {
            waServ.Delete(id, user);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
    }


}
