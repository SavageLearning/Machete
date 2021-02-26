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
using Machete.Data;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkerRequest = Machete.Domain.WorkerRequest;
using WorkOrdersList = Machete.Service.DTO.WorkOrdersList;

namespace Machete.Web.Controllers
{
        public class WorkOrderController : MacheteController
    {
        private readonly IWorkOrderService _woServ;
        private readonly IWorkerRequestService _reqServ;
        private readonly IMapper _map;
        private readonly IDefaults _defaults;
        private readonly IModelBindingAdaptor _adaptor;
        private readonly TimeZoneInfo _clientTimeZoneInfo;

        /// <summary>
        /// The Work Order controller is responsible for handling all REST actions related to the
        /// creation, modification, processing and retaining of Work Orders created by staff or
        /// employers (hirers/2.0).
        /// </summary>
        /// <param name="woServ">Work Order service</param>
        /// <param name="reqServ">Req serv</param>
        /// <param name="defaults">Default config values</param>
        /// <param name="map">AutoMapper service</param>
        /// <param name="adaptor"></param>
        /// <param name="tenantService"></param>
        public WorkOrderController(
            IWorkOrderService woServ,
            IWorkerRequestService reqServ,
            IDefaults defaults,
            IMapper map,
            IModelBindingAdaptor adaptor,
            ITenantService tenantService
        )
        {
            _woServ = woServ;
            _reqServ = reqServ;
            _map = map;
            _adaptor = adaptor;
            _defaults = defaults;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }
        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.def = _defaults;
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
            if (param.todaysdate != null) {
                var clientDate = DateTime.Parse(param.todaysdate);
                var utcDate = TimeZoneInfo.ConvertTimeToUtc(clientDate, _clientTimeZoneInfo);
                param.todaysdate = utcDate.ToString(CultureInfo.InvariantCulture);
            }

            // Retrieve WO/WA Summary based on parameters
            dataTableResult<WOWASummary> dtr = 
                _woServ.CombinedSummary(param.sSearch,
                    Request.Query["sSortDir_0"] != "asc",
                    param.iDisplayStart,
                    param.iDisplayLength);
            //
            //return what's left to datatables
            var result = from p in dtr.query
                         select new[] {
                             p.date,
                             p.weekday,
                             p.PendingWO > 0 ? p.PendingWO.ToString(): null,
                             p.PendingWA > 0 ? p.PendingWA.ToString(): null,
                             p.ActiveWO > 0 ? p.ActiveWO.ToString(): null,
                             p.ActiveWA > 0 ? p.ActiveWA.ToString(): null,
                             p.CompletedWO > 0 ? p.CompletedWO.ToString(): null,
                             p.CompletedWA > 0 ? p.CompletedWA.ToString(): null,
                             p.CancelledWO > 0 ? p.CancelledWO.ToString(): null,
                             p.CancelledWA > 0 ? p.CancelledWA.ToString(): null,
                             p.ExpiredWO > 0 ? p.ExpiredWO.ToString(): null,
                             p.ExpiredWA > 0 ? p.ExpiredWA.ToString(): null
                         }.ToList();

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
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;

            var vo = _map.Map<jQueryDataTableParam, viewOptions>(param);
            //Get all the records
            var dataTableResult = _woServ.GetIndexView(vo);

            var result = dataTableResult.query
                .Select(
                    e => _map.Map<WorkOrdersList, ViewModel.WorkOrdersList>(e)
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
            var utcNow = DateTime.UtcNow;

            var workOrder = new WorkOrder
            {
                EmployerID = employerId,
                dateTimeofWork = utcNow,
                transportMethodID = _defaults.getDefaultID(LCategory.transportmethod),
                typeOfWorkID = _defaults.getDefaultID(LCategory.worktype),
                statusID = _defaults.getDefaultID(LCategory.orderstatus),
                timeFlexible = true,
                workerRequestsDDD = new List<WorkerRequest>()
            };
            ViewBag.workerRequests = workOrder.workerRequestsDDD?.Select(a => 
                new SelectListItem
                {
                    Value = a.WorkerID.ToString(), 
                    Text = a.workerRequested.dwccardnum.ToString() + ' ' + 
                           a.workerRequested.Person.firstname1 + ' ' + 
                           a.workerRequested.Person.lastname1 
                }
            );

            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            var wo = _map.Map<WorkOrder, ViewModel.WorkOrderMVC>(workOrder);

            return PartialView("Create", wo);
        }

        /// <summary>
        /// POST: /WorkOrder/Create
        /// </summary>
        /// <param name="wo">WorkOrder to create</param>
        /// <param name="userName">User performing action</param>
        /// <param name="workerRequests"></param>
        /// <returns>JSON Object representing new Work Order</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Create(WorkOrder wo, string userName, [FromForm] List<int> workerRequestsAAA)
        {
            ModelState.ThrowIfInvalid();
            var modelUpdated = await _adaptor.TryUpdateModelAsync(this, wo);
            if (!modelUpdated) return StatusCode(500);
            
            List<Domain.WorkerRequest> wRequests = new List<Domain.WorkerRequest>();

            foreach (var workerID in workerRequestsAAA)
            {
                wRequests.Add(new WorkerRequest { WorkerID = workerID });
            }

            //wo.workerRequests = wRequests;

            wo.dateTimeofWork = TimeZoneInfo.ConvertTimeToUtc(wo.dateTimeofWork, _clientTimeZoneInfo);

            var workOrder = _woServ.Create(wo, wRequests, userName);

            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            var result = _map.Map<WorkOrder, ViewModel.WorkOrderMVC>(workOrder);
            return Json(new {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID
            });
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
            WorkOrder workOrder = _woServ.Get(id);
            
            // Retrieve Worker Requests associated with Work Order
            var workerRequests = workOrder.workerRequestsDDD;
            var selectListItems = workerRequests?.Select(a => 
                new SelectListItem
                {
                    Value = a.WorkerID.ToString(), 
                    Text = a.workerRequested.dwccardnum.ToString() + ' ' + 
                           a.workerRequested.Person.firstname1 + ' ' + 
                           a.workerRequested.Person.lastname1 
                });
            ViewBag.workerRequests = selectListItems;
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            var m = _map.Map<WorkOrder, ViewModel.WorkOrderMVC>(workOrder);

            return PartialView("Edit", m);
        }
        /// <summary>
        /// POST: /WorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="userName">UserName performing action</param>
        /// <param name="workerRequestList">List of workers requested</param>
        /// <param name="workerRequests">List of workers requested</param>
        /// <returns>MVC Action Result</returns>
        //[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Edit(int id, string userName, List<int> workerRequestsAAA)
        {
            ModelState.ThrowIfInvalid();
            
            var workOrder = _woServ.Get(id);
            var modelUpdated = await _adaptor.TryUpdateModelAsync(this, workOrder);
            if (!modelUpdated) return StatusCode(500);
            
            var workerRequestList = _reqServ.GetAllByWorkOrderID(workOrder.ID).ToList();

            for (var i = workerRequestList.Count - 1; i >= 0; i--)
                if (!workerRequestsAAA.Contains(workerRequestList[i].WorkerID))
                    workerRequestList.RemoveAt(i);

            foreach (var workerID in workerRequestsAAA)
                if (!workerRequestList.Any(workerRequest => workerRequest.WorkerID == workerID))
                    workerRequestList.Add(new WorkerRequest {WorkerID = workerID});

            workOrder.dateTimeofWork = TimeZoneInfo.ConvertTimeToUtc(workOrder.dateTimeofWork, _clientTimeZoneInfo);
            
            _woServ.Save(workOrder, workerRequestList, userName);
            
            return Json(new {
                status = "OK",
                editedID = id
            });
        }
        /// <summary>
        /// GET: /WorkOrder/View/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            WorkOrder workOrder = _woServ.Get(id);
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            var m = _map.Map<WorkOrder, ViewModel.WorkOrderMVC>(workOrder);

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
            WorkOrder workOrder = _woServ.Get(id);
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            var m = _map.Map<WorkOrder, ViewModel.WorkOrderMVC>(workOrder);

            ViewBag.OrganizationName = _defaults.getConfig("OrganizationName");
            return PartialView(m);
        }
        /// <summary>
        /// Creates the view to print all orders for a given day
        /// </summary>
        /// <param name="targetDate">Date to perform action</param>
        /// <param name="assignedOnly">Optional flag: if True, only shows orders that are fully assigned</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult GroupView(DateTime targetDate, bool? assignedOnly)
        {
            var utcDate = TimeZoneInfo.ConvertTimeToUtc(targetDate, _clientTimeZoneInfo);

            var v = _woServ.GetActiveOrders(utcDate, assignedOnly ?? false).ToList();
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            MapperHelpers.Defaults = _defaults;
            List<ViewModel.WorkOrderMVC> foo = new List<ViewModel.WorkOrderMVC>();
            foreach (var item in v)
            {
                foo.Add(_map.Map<Domain.WorkOrder, ViewModel.WorkOrderMVC>(item));
            }
            //var erders = v.Select(e => _map.Map<WorkOrder, ViewModel.WorkOrder>(e)).ToList();
            var view = new WorkOrderGroupPrintView
            {
                orders = foo
            };

            return View(view);
        }
        /// <summary>
        /// Completes all orders for a given day
        /// </summary>
        /// <param name="targetDate">Date to perform action</param>
        /// <param name="userName">UserName performing action</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult CompleteOrders(DateTime targetDate, string userName)
        {
            int count = _woServ.CompleteActiveOrders(targetDate, userName);
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
            _woServ.Delete(id, user);
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
            var workOrder = _woServ.Get(id);
            // lookup int value for status active
            workOrder.statusID = WorkOrder.iActive;
            _woServ.Save(workOrder, userName);         
            return Json(new
            {
                status = "activated"
            });
        }
    }
}
