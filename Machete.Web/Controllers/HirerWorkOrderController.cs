#region COPYRIGHT
// File:     HirerWorkOrderController.cs
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
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Models;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class HirerWorkOrderController : MacheteController
    {
        private readonly IWorkOrderService woServ;
        private readonly IEmployerService eServ;
        private readonly IWorkerService wServ;
        private readonly IWorkerRequestService wrServ;
        private readonly IWorkAssignmentService waServ;
        private readonly ILookupCache lcache;
        CultureInfo CI;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="woServ">Work Order service</param>
        /// <param name="waServ">Work Assignment service</param>
        /// <param name="eServ">Employer service</param>
        /// <param name="wServ">Worker service</param>
        /// <param name="wrServ">Worker request service</param>
        /// <param name="lcache">Lookup cache</param>
        public HirerWorkOrderController(IWorkOrderService woServ,
                                   IWorkAssignmentService waServ,
                                   IEmployerService eServ,
                                   IWorkerService wServ,
                                   IWorkerRequestService wrServ,
                                   ILookupCache lcache)
        {
            this.woServ = woServ;
            this.eServ = eServ;
            this.wServ = wServ;
            this.waServ = waServ;
            this.wrServ = wrServ;
            this.lcache = lcache;
        }

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.CI = (CultureInfo)Session["Culture"];
        }

        #region Index
        /// <summary>
        /// HTTP GET /HirerWorkOrder/Index
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Ajaxhandler
        /// <summary>
        /// Provides json grid of work orders -- used with the hirerworkorders/index view
        /// </summary>
        /// <param name="param">contains parameters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = this.CI;

            // TODO: filter by current employer
            vo.EmployerID = 1;

            //Get all the records
            dataTableResult<WorkOrder> dtr = woServ.GetIndexView(vo);

            // TODO: investigate this
            param.showOrdersWorkers = true;

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = dtr.totalCount,
                iTotalDisplayRecords = dtr.filteredCount,
                aaData = from p in dtr.query
                         select dtResponse(ref p, param.showOrdersWorkers)
            },
            JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns Work Order object to AjaxHandler - presented on the WorkOrder Details tab
        /// </summary>
        /// <param name="wo">WorkOrder</param>
        /// <param name="showWorkers">bool flag determining whether the workers associated with the WorkOrder should be retrieved</param>
        /// <returns>Work Order </returns>
        public object dtResponse(ref WorkOrder wo, bool showWorkers)
        {
            // tabref = "/HirerWorkOrder/Edit" + Convert.ToString(wo.ID),
            int ID = wo.ID;
            return new
            {
                tabref = "/HirerWorkOrder/Edit/" + Convert.ToString(wo.ID),
                tablabel = Machete.Web.Resources.WorkOrders.tabprefix + wo.getTabLabel(),
                EID = Convert.ToString(wo.EmployerID),
                WOID = System.String.Format("{0,5:D5}", wo.paperOrderNum), // Note: paperOrderNum defaults to the value of the WO when a paperOrderNum is not provided
                dateTimeofWork = wo.dateTimeofWork.ToString(),
                status = lcache.textByID(wo.status, CI.TwoLetterISOLanguageName),
                WAcount = wo.workAssignments.Count(a => a.workOrderID == ID).ToString(),
                contactName = wo.contactName,
                workSiteAddress1 = wo.workSiteAddress1,
                zipcode = wo.zipcode,
                transportMethod = lcache.textByID(wo.transportMethodID, CI.TwoLetterISOLanguageName),
                displayState = _getDisplayState(wo), // State is used to provide color highlighting to records based on state
                onlineSource = wo.onlineSource ? Shared.True : Shared.False,
                workers = showWorkers ? // Workers is only loaded when showWorkers parameter set to TRUE
                        from w in wo.workAssignments
                        select new
                        {
                            WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
                            name = w.workerAssigned != null ? w.workerAssigned.Person.firstname1 : null, // Note: hirers should only have access to the workers first name
                            skill = lcache.textByID(w.skillID, CI.TwoLetterISOLanguageName),
                            hours = w.hours,
                            wage = w.hourlyWage
                        } : null

            };
        }


        /// <summary>
        /// Determines displayState value in WorkOrder/AjaxHandler. Display state is used to provide color highlighting to records based on state.
        /// The displayState is not presented to the user, so don't have to provide internationalization text.
        /// </summary>
        /// <param name="wo">WorkOrder</param>
        /// <returns>status string</returns>
        private string _getDisplayState(WorkOrder wo)
        {
            string status = lcache.textByID(wo.status, "en");

            if (wo.status == WorkOrder.iCompleted)
            {
                // If WO is completed, but 1 (or more) WA aren't assigned - the WO is still Unassigned
                if (wo.workAssignments.Count(wa => wa.workerAssignedID == null) > 0)
                {
                    return "Unassigned";
                }
                // If WO is completed, but 1 (or more) Assigned Worker(s) never signed in, then the WO has been Orphaned
                if (wo.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null) > 0)
                {
                    return "Orphaned";
                }
            }
            return status;
        }
        #endregion

        #region Create

        /// <summary>
        /// HTTP GET /HirerWorkOrder/Create
        /// </summary>
        /// <param name="employerID">Employer ID associated with Work Order (Parent Object)</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult Create()
        {
            WorkOrder wo = new WorkOrder();

            // Set default values
            wo.EmployerID = 1;
            wo.dateTimeofWork = DateTime.Today;
            wo.transportMethodID = Lookups.getDefaultID(LCategory.transportmethod);
            wo.typeOfWorkID = Lookups.getDefaultID(LCategory.worktype);
            wo.status = Lookups.getDefaultID(LCategory.orderstatus);
            wo.timeFlexible = true;
            wo.onlineSource = true;
            ViewBag.workerRequests = new List<SelectListItem> { };
            return PartialView("Create", wo);
        }

        /// <summary>
        /// POST: /HirerWorkOrder/Create
        /// </summary>
        /// <param name="wo">WorkOrder to create</param>
        /// <param name="userName">User performing action</param>
        /// <param name="workerRequestList">List of workers requested</param>
        /// <returns>JSON Object representing new Work Order</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult Create(WorkOrder wo, string userName, List<WorkerRequest> workerRequestList)
        {
            UpdateModel(wo);
            WorkOrder neworder = woServ.Create(wo, userName);

            // New Worker Requests to add
            foreach (var workerRequest in workerRequestList)
            {
                workerRequest.workOrder = neworder;
                workerRequest.workerRequested = wServ.Get(workerRequest.WorkerID);
                workerRequest.updatedby(userName);
                workerRequest.createdby(userName);
                neworder.workerRequests.Add(workerRequest);
            }
            woServ.Save(neworder, userName);

            // JSON object with new work order data
            return Json(new
            {
                sNewRef = neworder.getTabRef(),
                sNewLabel = "cc" + Machete.Web.Resources.WorkOrders.tabprefix + neworder.getTabLabel(),
                iNewID = neworder.ID
            },
            JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        /// <summary>
        /// GET: /HirerWorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult Edit(int id)
        {
            // Retrieve Work Order
            WorkOrder workOrder = woServ.Get(id);

            // Retrieve Worker Requests associated with Work Order
            ViewBag.workerRequests = workOrder.workerRequests.Select(a =>
                new SelectListItem
                {
                    Value = a.WorkerID.ToString(),
                    Text = a.workerRequested.dwccardnum.ToString() + ' ' +
                    a.workerRequested.Person.firstname1 + ' ' +
                    a.workerRequested.Person.lastname1
                });

            return PartialView("Edit", workOrder);
        }

        /// <summary>
        /// POST: /HirerWorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="collection">FormCollection</param>
        /// <param name="userName">UserName performing action</param>
        /// <param name="workerRequestList">List of workers requested</param>
        /// <returns>MVC Action Result</returns>
        ///[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult Edit(int id, FormCollection collection, string userName, List<WorkerRequest> workerRequestList)
        {
            WorkOrder workOrder = woServ.Get(id);
            UpdateModel(workOrder);

            // Stale requests to remove
            foreach (var rem in workOrder.workerRequests.Except<WorkerRequest>(workerRequestList, new WorkerRequestComparer()).ToArray())
            {
                var request = wrServ.GetWorkerRequestsByNum(workOrder.ID, rem.WorkerID);
                wrServ.Delete(request.ID, userName);
                workOrder.workerRequests.Remove(rem);
            }

            // New requests to add
            foreach (var add in workerRequestList.Except<WorkerRequest>(workOrder.workerRequests, new WorkerRequestComparer()))
            {
                add.workOrder = workOrder;
                add.workerRequested = wServ.Get(add.WorkerID);
                add.updatedby(userName);
                add.createdby(userName);
                workOrder.workerRequests.Add(add);
            }

            woServ.Save(workOrder, userName);
            return Json(new
            {
                status = "OK",
                editedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View

        /// <summary>
        /// GET: /HirerWorkOrder/View/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult View(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
            return View(workOrder);
        }

        /// <summary>
        /// Creates the view for email
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult ViewForEmail(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
            return PartialView(workOrder);
        }
        #endregion

        // TODO: Consider allowing an employer to cancel a work order
        #region Delete
        /// <summary>
        /// POST: /HirerWorkOrder/Delete/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="user">User performing action</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult Delete(int id, string user)
        {
            woServ.Delete(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
