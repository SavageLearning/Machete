#region COPYRIGHT
// File:     WorkOrderController.cs
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
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkerRequest = Machete.Domain.WorkerRequest;
using WorkOrder = Machete.Domain.WorkOrder;
using WorkOrdersList = Machete.Service.DTO.WorkOrdersList;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkOrderController : MacheteController
    {
        private readonly IWorkOrderService woServ;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;

        /// <summary>
        /// The Work Order controller is responsible for handling all REST actions related to the
        /// creation, modification, processing and retaining of Work Orders created by staff or
        /// employers (hirers/2.0).
        /// </summary>
        /// <param name="woServ">Work Order service</param>
        /// <param name="def">Default config values</param>
        /// <param name="map">AutoMapper service</param>
        public WorkOrderController(IWorkOrderService woServ,
            IDefaults def,
            IMapper map)
        {
            this.woServ = woServ;
            this.map = map;
            this.def = def;
        }
        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
            ViewBag.def = def;
        }
        /// <summary>
        /// HTTP GET /WorkOrder/Index
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Provides json grid of wo/wa summaries and their statuses
        /// </summary>
        /// <param name="param">contains parameters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxSummary(jQueryDataTableParam param)
        {
            // TODO: investigate if this can be removed
            // System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];

            // Retrieve WO/WA Summary based on parameters
            dataTableResult<WOWASummary> dtr = 
                woServ.CombinedSummary(param.sSearch,
                    Request.Form["sSortDir_0"] != "asc",
                    param.iDisplayStart,
                    param.iDisplayLength);
            //
            //return what's left to datatables
            var result = from p in dtr.query
                         select new[] {
                             $"{p.date:MM/dd/yyyy}",
                             p.weekday,
                             p.pending_wo > 0 ? p.pending_wo.ToString(): null,
                             p.pending_wa > 0 ? p.pending_wa.ToString(): null,
                             p.active_wo > 0 ? p.active_wo.ToString(): null,
                             p.active_wa > 0 ? p.active_wa.ToString(): null,
                             p.completed_wo > 0 ? p.completed_wo.ToString(): null,
                             p.completed_wa > 0 ? p.completed_wa.ToString(): null,
                             p.cancelled_wo > 0 ? p.cancelled_wo.ToString(): null,
                             p.cancelled_wa > 0 ? p.cancelled_wa.ToString(): null,
                             p.expired_wo > 0 ? p.expired_wo.ToString(): null,
                             p.expired_wa > 0 ? p.expired_wa.ToString(): null
                         };

            return Json(new
            {
                param.sEcho,
                iTotalRecords = dtr.totalCount,
                iTotalDisplayRecords = dtr.filteredCount,
                aaData = result
            });
        }
        // TODO: investigate why the following columns aren't properly sortable: Weekday, Completed Assignment
        // TODO: investigate why the following work orders aren't appearing in the summary results: active & cancelled - they only appear once a WA is created!!!

        /// <summary>
        /// Provides json grid of orders
        /// </summary>
        /// <param name="param">contains parameters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI =  CI;
            //Get all the records
            var dataTableResult = woServ.GetIndexView(vo);
            var result = dataTableResult.query
                .Select(
                    e => map.Map<WorkOrdersList, ViewModel.WorkOrdersList>(e)
                ).AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = dataTableResult.totalCount,
                iTotalDisplayRecords = dataTableResult.filteredCount,
                aaData = result
            });
        }
        /// <summary>
        /// GET: /WorkOrder/Create
        /// </summary>
        /// <param name="employerId">Employer ID associated with Work Order (Parent Object)</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(int employerId)
        {
            var wo = map.Map<WorkOrder, ViewModel.WorkOrder>(new WorkOrder
            {
                EmployerID = employerId,
                dateTimeofWork = DateTime.Today,
                transportMethodID = def.getDefaultID(LCategory.transportmethod),
                typeOfWorkID = def.getDefaultID(LCategory.worktype),
                statusID = def.getDefaultID(LCategory.orderstatus),
                timeFlexible = true
            });
            wo.def = def;
            ViewBag.workerRequests = new List<SelectListItem>();
            return PartialView("Create", wo);
        }
        /// <summary>
        /// POST: /WorkOrder/Create
        /// </summary>
        /// <param name="wo">WorkOrder to create</param>
        /// <param name="userName">User performing action</param>
        /// <returns>JSON Object representing new Work Order</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Create(WorkOrder wo, string userName)
        {
            if (await TryUpdateModelAsync(wo)) {
                var workOrder = woServ.Create(wo, userName);

                // JSON object with new work order data
                var result = map.Map<WorkOrder, ViewModel.WorkOrder>(workOrder);
                return Json(new {
                    sNewRef = result.tabref,
                    sNewLabel = result.tablabel,
                    iNewID = result.ID
                });
            } else { return Json(new { status = "Not OK" }); } // TODO Chaim plz
        }
        /// <summary>
        /// GET: /WorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            // Retrieve Work Order
            WorkOrder workOrder = woServ.Get(id);
            
            // Retrieve Worker Requests associated with Work Order
            var workerRequests = workOrder.workerRequests;
            var selectListItems = workerRequests?.Select(a => 
                new SelectListItem
                {
                    Value = a.WorkerID.ToString(), 
                    Text = a.workerRequested.dwccardnum.ToString() + ' ' + 
                           a.workerRequested.Person.firstname1 + ' ' + 
                           a.workerRequested.Person.lastname1 
                });
            ViewBag.workerRequests = selectListItems;
            
            var m = map.Map<WorkOrder, ViewModel.WorkOrder>(workOrder);
            m.def = def;
            return PartialView("Edit", m);
        }
        /// <summary>
        /// POST: /WorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="userName">UserName performing action</param>
        /// <param name="workerRequestList">List of workers requested</param>
        /// <returns>MVC Action Result</returns>
        //[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Edit(int id, string userName, List<WorkerRequest> workerRequestList)
        {
            WorkOrder workOrder = woServ.Get(id);
            if (await TryUpdateModelAsync(workOrder)) {

            woServ.Save(workOrder, workerRequestList, userName);
            return Json(new
            {
                status = "OK",
                editedID = id
            });
            } else { return Json(new { status = "Not OK" }); } // TODO Chaim plz
        }
        /// <summary>
        /// GET: /WorkOrder/View/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
            var m = map.Map<WorkOrder, ViewModel.WorkOrder>(workOrder);
            m.def = def;
            return View(m);
        }
        /// <summary>
        /// Creates the view for email
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult ViewForEmail(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
            var m = map.Map<WorkOrder, ViewModel.WorkOrder>(workOrder);
            m.def = def;
            ViewBag.OrganizationName = def.getConfig("OrganizationName");
            return PartialView(m);
        }
        /// <summary>
        /// Creates the view to print all orders for a given day
        /// </summary>
        /// <param name="date">Date to perform action</param>
        /// <param name="assignedOnly">Optional flag: if True, only shows orders that are fully assigned</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult GroupView(DateTime date, bool? assignedOnly)
        {
            WorkOrderGroupPrintView view = new WorkOrderGroupPrintView();
            var v = woServ.GetActiveOrders(date, assignedOnly ?? false);
            view.orders = v.Select(e => map.Map<WorkOrder, ViewModel.WorkOrder>(e)).ToList();
            foreach (var i in view.orders) // inelegant, but functional
            {
                i.def = def;
            }
            
            return View(view);
        }
        /// <summary>
        /// Completes all orders for a given day
        /// </summary>
        /// <param name="date">Date to perform action</param>
        /// <param name="userName">UserName performing action</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult CompleteOrders(DateTime date, string userName)
        {
            int count = woServ.CompleteActiveOrders(date, userName);
            return Json(new
            {
                completedCount = count
            });
        }
        /// <summary>
        /// POST: /WorkOrder/Delete/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="user">User performing action</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {
            woServ.Delete(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            });
        }
        /// <summary>
        /// POST: /WorkOrder/Activate/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="userName">User performing action</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Activate(int id, string userName)
        {
            var workOrder = woServ.Get(id);
            // lookup int value for status active
            workOrder.statusID = WorkOrder.iActive;
            woServ.Save(workOrder, userName);         
            return Json(new
            {
                status = "activated"
            });
        }
    }
}
