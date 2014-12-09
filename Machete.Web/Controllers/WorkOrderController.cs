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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.ViewModel;
using System.Web.Routing;
using Machete.Web.Models;
using System.Text.RegularExpressions;
using AutoMapper;
using System.Globalization;
using Machete.Web.Resources;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkOrderController : MacheteController
    {
        private readonly IWorkOrderService woServ;
        private readonly IEmployerService eServ;
        private readonly IWorkerService wServ;
        private readonly IWorkerRequestService wrServ;
        private readonly IWorkAssignmentService waServ;
        private readonly ILookupCache lcache;
        CultureInfo CI;
        
        public WorkOrderController(IWorkOrderService woServ, 
                                   IWorkAssignmentService workAssignmentService,
                                   IEmployerService employerService,
                                   IWorkerService workerService,
                                   IWorkerRequestService requestService, 
                                   ILookupCache lc)
        {
            this.woServ = woServ;
            this.eServ = employerService;
            this.wServ = workerService;
            this.waServ = workAssignmentService;
            this.wrServ = requestService;
            this.lcache = lc;
        }
        protected override void Initialize(RequestContext requestContext)
                {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }

        #region Index
        /// <summary>
        /// Returns WorkOrder default page
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region AJaxSummary
        /// <summary>
        /// Provides json grid of order summary and their statuses
        /// </summary>
        /// <param name="param">contains paramters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxSummary(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //
            //pass filter parameters to service level
            dataTableResult<WOWASummary> dtr = 
                woServ.CombinedSummary(param.sSearch,
                    Request["sSortDir_0"] == "asc" ? false : true,
                    param.iDisplayStart,
                    param.iDisplayLength);
            //
            //return what's left to datatables
            var result = from p in dtr.query
                         select new[] { System.String.Format("{0:MM/dd/yyyy}", p.date),
                                         p.weekday.ToString(),
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
                sEcho = param.sEcho,
                iTotalRecords = dtr.totalCount,
                iTotalDisplayRecords = dtr.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ajaxhandler
        /// <summary>
        /// Provides json grid of orders
        /// </summary>
        /// <param name="param">contains parameters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI =  CI;
            //Get all the records
            dataTableResult<WorkOrder> dtr = woServ.GetIndexView(vo);

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
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="showWorkers"></param>
        /// <returns></returns>
        public object dtResponse(ref WorkOrder p, bool showWorkers)
        {
            int ID = p.ID;
            return new 
            {
                tabref = p.getTabRef(),
                tablabel = Machete.Web.Resources.WorkOrders.tabprefix + p.getTabLabel(),
                EID = Convert.ToString(p.EmployerID),
                WOID = System.String.Format("{0,5:D5}", p.paperOrderNum),
                dateTimeofWork = p.dateTimeofWork.ToString(),
                status = lcache.textByID(p.status, CI.TwoLetterISOLanguageName),
                WAcount = p.workAssignments.Count(a => a.workOrderID == ID).ToString(),
                contactName = p.contactName,
                workSiteAddress1 = p.workSiteAddress1,
                zipcode = p.zipcode,
                dateupdated = System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", p.dateupdated),
                updatedby = p.Updatedby,
                transportMethod = lcache.textByID(p.transportMethodID, CI.TwoLetterISOLanguageName),
                displayState = _getDisplayState(p),
                onlineSource = p.onlineSource ? Shared.True : Shared.False,
                emailSentCount = p.Emails.Where(e => e.statusID == Email.iSent || e.statusID == Email.iReadyToSend).Count(),
                emailErrorCount = p.Emails.Where(e => e.statusID == Email.iTransmitError).Count(),
                recordid = p.ID.ToString(),
                workers = showWorkers ?
                        from w in p.workAssignments
                        select new
                        {
                            WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
                            name = w.workerAssigned != null ? w.workerAssigned.Person.fullName() : null,
                            skill = lcache.textByID(w.skillID, CI.TwoLetterISOLanguageName),
                            hours = w.hours,
                            wage = w.hourlyWage
                        } : null
            };
        }


        /// <summary>
        /// determines displayState value in WorkOrder/AjaxHandler
        /// </summary>
        /// <param name="wo">WorkOrder</param>
        /// <returns>status string</returns>
        private string _getDisplayState(WorkOrder wo)
        {
            string status = lcache.textByID(wo.status, "en");
            //TODO: Pull out WorkOrder status strings, use WorkOrder object reference
            if (status == "Completed")
            {
                if (wo.workAssignments.Count(wa => wa.workerAssignedID == null) > 0) return "Unassigned";
                if (wo.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null) > 0) return "Orphaned";                                 
            }
            return status;
        }
        #endregion

        #region Create
        /// <summary>
        /// HTTP GET /WorkOrder/Create
        /// </summary>
        /// <param name="EmployerID">Parent record ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(int EmployerID)
        {
            WorkOrder _wo = new WorkOrder();
            _wo.EmployerID = EmployerID;
            _wo.dateTimeofWork = DateTime.Today;
            _wo.transportMethodID = Lookups.getDefaultID(LCategory.transportmethod);
            _wo.typeOfWorkID = Lookups.getDefaultID(LCategory.worktype);
            _wo.status = Lookups.getDefaultID(LCategory.orderstatus);
            _wo.timeFlexible = true;
            ViewBag.workerRequests = new List<SelectListItem> {};
            return PartialView("Create", _wo);
        }
        /// <summary>
        /// POST: /WorkOrder/Create
        /// </summary>
        /// <param name="_model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(WorkOrder _wo, string userName, List<WorkerRequest> workerRequests2)
        {
            UpdateModel(_wo);
            WorkOrder neworder = woServ.Create(_wo, userName);           
            //
            //New requests to add
            foreach (var add in workerRequests2)
            {
                add.workOrder = neworder;
                add.workerRequested = wServ.Get(add.WorkerID);
                add.updatedby(userName);
                add.createdby(userName);
                neworder.workerRequests.Add(add);
            }
            woServ.Save(neworder, userName);
            //
            //
            return Json(new 
            {
                sNewRef = neworder.getTabRef(),
                sNewLabel = Machete.Web.Resources.WorkOrders.tabprefix + neworder.getTabLabel(),
                iNewID = neworder.ID
            }, 
            JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Edit
        //
        // GET: /WorkOrder/Edit/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
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
        //
        // POST: /WorkOrder/Edit/5
        //[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, FormCollection collection, string userName, List<WorkerRequest> workerRequests2)
        {
            WorkOrder workOrder = woServ.Get(id);
            UpdateModel(workOrder);
            //Stale requests to remove
            foreach (var rem in workOrder.workerRequests.Except<WorkerRequest>(workerRequests2, new WorkerRequestComparer()).ToArray())
            {
                var request = wrServ.GetWorkerRequestsByNum(workOrder.ID, rem.WorkerID);
                wrServ.Delete(request.ID, userName);
                workOrder.workerRequests.Remove(rem);
            }
            //New requests to add
            foreach (var add in workerRequests2.Except<WorkerRequest>(workOrder.workerRequests, new WorkerRequestComparer()))
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
        //
        //GET: /WorkOrder/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
            return View(workOrder);
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult ViewForEmail(int id)
        {
            WorkOrder workOrder = woServ.Get(id);
            return PartialView(workOrder);
        }
        /// <summary>
        /// Creates the view to print all orders for a given day
        /// assignedOnly: only shows orders that are fully assigned
        /// </summary>
        /// <param name="date"></param>
        /// <param name="assignedOnly"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult GroupView(DateTime date, bool? assignedOnly)
        {
            WorkOrderGroupPrintView view = new WorkOrderGroupPrintView();
            if (assignedOnly == true) view.orders = woServ.GetActiveOrders(date, true );
            else view.orders = woServ.GetActiveOrders(date, false);
            return View(view);
        }
        //
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult CompleteOrders(DateTime date, string userName)
        {
            int count = woServ.CompleteActiveOrders(date, userName);
            return Json(new
            {
                completedCount = count
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Delete
        //
        // POST: /WorkOrder/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
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
        #region Activate
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Activate(int id, string userName)
        {
            var workOrder = woServ.Get(id);
            // lookup int value for status active
            workOrder.status = WorkOrder.iActive;
            woServ.Save(workOrder, userName);         
            return Json(new
            {
                status = "activated"
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
