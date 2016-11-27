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
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using Machete.Web.Resources;
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
    public class WorkOrderController : MacheteController
    {
        private readonly IWorkOrderService woServ;
        private readonly IEmployerService eServ;
        private readonly IWorkerService wServ;
        private readonly IWorkerRequestService wrServ;
        private readonly IWorkAssignmentService waServ;
        private readonly ILookupCache lcache;
        private readonly IMapper map;
        private readonly IDefaults def;
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
        public WorkOrderController(IWorkOrderService woServ,
            IWorkAssignmentService waServ,
            IEmployerService eServ,
            IWorkerService wServ,
            IWorkerRequestService wrServ,
            ILookupCache lcache,
            IDefaults def,
            IMapper map)
        {
            this.woServ = woServ;
            this.eServ = eServ;
            this.wServ = wServ;
            this.waServ = waServ;
            this.wrServ = wrServ;
            this.lcache = lcache;
            this.map = map;
            this.def = def;
        }

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.CI = (CultureInfo)Session["Culture"];
            ViewBag.orderstatuses = def.getSelectList(LCategory.orderstatus);
            ViewBag.transportmethod = def.getSelectList(LCategory.transportmethod);
            ViewBag.transporttransacttype = def.getSelectList(LCategory.transportTransactType);
        }

        #region Index
        /// <summary>
        /// HTTP GET /WorkOrder/Index
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region AJaxSummary
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

        // TODO: investigate why the following columns aren't properly sortable: Weekday, Completed Assignment
        // TODO: investigate why the following work orders aren't appearing in the summary results: active & cancelled - they only appear once a WA is created!!!

        #region Ajaxhandler
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
            dataTableResult<DTO.WorkOrder> dtr = woServ.GetIndexView(vo);
            var result = dtr.query
                .Select(
                    e => map.Map<Domain.WorkOrder, ViewModel.WorkOrder>(e)
                ).AsEnumerable();
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = dtr.totalCount,
                iTotalDisplayRecords = dtr.filteredCount,
                aaData = result
                //aaData = from p in dtr.query
                //         select dtResponse( p, param.showOrdersWorkers)
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Returns Work Order object - presented on the WorkOrder Details tab
        /// </summary>
        /// <param name="wo">WorkOrder</param>
        /// <param name="showWorkers">bool flag determining whether the workers associated with the WorkOrder should be retrieved</param>
        /// <returns>Work Order </returns>
        public object dtResponse( ViewModel.WorkOrder wo, bool showWorkers)
        {
            int ID = wo.ID;
            return new 
            {
                //tabref = wo.getTabRef(),
                //tablabel = Machete.Web.Resources.WorkOrders.tabprefix + wo.getTabLabel(),
                //EID = Convert.ToString(wo.EmployerID), // Note: Employer ID appears to be unused
                //WOID = System.String.Format("{0,5:D5}", wo.paperOrderNum), // TODO: investigate why PaperOrderNum is used - shouldn't this be the Order # from the WO Table?
                //dateTimeofWork = wo.dateTimeofWork.ToString(),
                //TODO: status = lcache.textByID(wo.status, CI.TwoLetterISOLanguageName),
                //TODO: WAcount = wo.workAssignments.Count(a => a.workOrderID == ID).ToString(),
                //contactName = wo.contactName,
                //workSiteAddress1 = wo.workSiteAddress1,
                //zipcode = wo.zipcode,
                //dateupdated = System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", wo.dateupdated), // Note: Date Updated appears to be unused
                //updatedby = wo.updatedby,
                //TODO: transportMethod = lcache.textByID(wo.transportMethodID, CI.TwoLetterISOLanguageName), // Used in Hirer a lot
                //TODO: displayState = _getDisplayState(wo), // Used to determine row color on /WorkOrder/Index.cshtml
                //onlineSource = wo.onlineSource ? Shared.True : Shared.False,
                //TODO: emailSentCount = wo.Emails.Where(e => e.statusID == Email.iSent || e.statusID == Email.iReadyToSend).Count(),
                //TODO: emailErrorCount = wo.Emails.Where(e => e.statusID == Email.iTransmitError).Count(),
                //recordid = wo.ID.ToString(), 
                //TODO: workers = showWorkers ? // Used in Datatables details
                //        from w in wo.workAssignments
                //        select new
                //        {
                //            WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
                //            name = w.workerAssigned != null ? w.workerAssigned.Person.fullName() : null,
                //            skill = lcache.textByID(w.skillID, CI.TwoLetterISOLanguageName),
                //            hours = w.hours,
                //            wage = w.hourlyWage
                //        } : null
            };
        }


        /// <summary>
        /// determines displayState value in WorkOrder/AjaxHandler
        /// </summary>
        /// <param name="wo">WorkOrder</param>
        /// <returns>status string</returns>
        private string _getDisplayState(Domain.WorkOrder wo)
        {
            string status = lcache.textByID(wo.status, "en");

            if (wo.status == Domain.WorkOrder.iCompleted)
            {
                // If WO is completed, but 1 (or more) WA aren't assigned - the WO is still Unassigned
                if (wo.workAssignments.Count(wa => wa.workerAssignedID == null) > 0) return "Unassigned";
                // If WO is completed, but 1 (or more) Assigned Worker(s) never signed in, then the WO has been Orphaned
                if (wo.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null) > 0) return "Orphaned";                                 
            }
            return status;
        }
        #endregion

        #region Create
        /// <summary>
        /// HTTP GET /WorkOrder/Create
        /// </summary>
        /// <param name="employerID">Employer ID associated with Work Order (Parent Object)</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(int EmployerID)
        {
            var wo = map.Map<Domain.WorkOrder, ViewModel.WorkOrder>(new Domain.WorkOrder()
            {
                EmployerID = EmployerID,
                dateTimeofWork = DateTime.Today,
                transportMethodID = def.getDefaultID(LCategory.transportmethod),
                typeOfWorkID = def.getDefaultID(LCategory.worktype),
                status = def.getDefaultID(LCategory.orderstatus),
                timeFlexible = true
            });
            wo.def = def;
            ViewBag.workerRequests = new List<SelectListItem> {};
            return PartialView("Create", wo);
        }

        /// <summary>
        /// POST: /WorkOrder/Create
        /// </summary>
        /// <param name="wo">WorkOrder to create</param>
        /// <param name="userName">User performing action</param>
        /// <param name="workerRequestList">List of workers requested</param>
        /// <returns>JSON Object representing new Work Order</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(Domain.WorkOrder wo, string userName, List<WorkerRequest> workerRequestList)
        {
            UpdateModel(wo);
            Domain.WorkOrder neworder = woServ.Create(wo, userName);           

            // New Worker Requests to add
            foreach (var workerRequest in workerRequestList)
            {
                workerRequest.workOrder = neworder;
                workerRequest.workerRequested = wServ.Get(workerRequest.WorkerID);
                workerRequest.updatedByUser(userName);
                workerRequest.createdByUser(userName);
                neworder.workerRequests.Add(workerRequest);
            }

            woServ.Save(neworder, userName);

            // JSON object with new work order data
            var result = map.Map<Domain.WorkOrder, ViewModel.WorkOrder>(neworder);
            return Json(new 
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID
            }, 
            JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        /// <summary>
        /// GET: /WorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            // Retrieve Work Order
            Domain.WorkOrder workOrder = woServ.Get(id);
            
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
        /// POST: /WorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="collection">FormCollection</param>
        /// <param name="userName">UserName performing action</param>
        /// <param name="workerRequestList">List of workers requested</param>
        /// <returns>MVC Action Result</returns>
        //[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, FormCollection collection, string userName, List<WorkerRequest> workerRequestList)
        {
            Domain.WorkOrder workOrder = woServ.Get(id);
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
                add.updatedByUser(userName);
                add.createdByUser(userName);
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
        /// GET: /WorkOrder/View/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            Domain.WorkOrder workOrder = woServ.Get(id);
            ViewBag.lunchsupplied = def.getBool(workOrder.lunchSupplied);
            ViewBag.transportmethod = def.byID(workOrder.transportMethodID);

            return View(workOrder);
        }

        /// <summary>
        /// Creates the view for email
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult ViewForEmail(int id)
        {
            Domain.WorkOrder workOrder = woServ.Get(id);
            ViewBag.lunchsupplied = def.getBool(workOrder.lunchSupplied);
            ViewBag.transportmethod = def.byID(workOrder.transportMethodID);

            return PartialView(workOrder);
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
            IEnumerable<Domain.WorkOrder> v;
            if (assignedOnly == true) v = woServ.GetActiveOrders(date, true );
            else v = woServ.GetActiveOrders(date, false);

            // TODO2016: map v into view.orders
            return View(view);
        }
        #endregion

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
            },
            JsonRequestBehavior.AllowGet);
        }

        #region Delete
        /// <summary>
        /// POST: /WorkOrder/Delete/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="user">User performing action</param>
        /// <returns>MVC Action Result</returns>
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
        /// <summary>
        /// POST: /WorkOrder/Activate/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <param name="user">User performing action</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Activate(int id, string userName)
        {
            var workOrder = woServ.Get(id);
            // lookup int value for status active
            workOrder.status = Domain.WorkOrder.iActive;
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
