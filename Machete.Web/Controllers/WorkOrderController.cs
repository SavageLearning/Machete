using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
//using Machete.Helpers;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.ViewModel;
using System.Web.Routing;
using Machete.Web.Models;
using System.Text.RegularExpressions;
using System.Data.Objects;
using Machete.Service.Helpers;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkOrderController : MacheteController
    {
        private readonly IWorkOrderService woServ;
        private readonly IEmployerService _empServ;
        private readonly IWorkerService _reqServ;
        private readonly IWorkerRequestService _wrServ;
        private readonly IWorkAssignmentService _waServ;
        private static Regex isTimeSpecific = new Regex(@"^\s*\d{1,2}[\/-_]\d{1,2}[\/-_]\d{2,4}\s+\d{1,2}:\d{1,2}");
        private static Regex isDaySpecific = new Regex(@"^\s*\d{1,2}\/\d{1,2}\/\d{2,4}");
        private static Regex isMonthSpecific = new Regex(@"^\s*\d{1,2}\/\d{4,4}");
        
        public WorkOrderController(IWorkOrderService woServ, 
                                   IWorkAssignmentService workAssignmentService,
                                   IEmployerService employerService,
                                   IWorkerService workerService,
                                   IWorkerRequestService requestService)
        {
            this.woServ = woServ;
            this._empServ = employerService;
            this._reqServ = workerService;
            this._waServ = workAssignmentService;
            this._wrServ = requestService;
        }
 
        #region Index
        //
        // GET: /WorkOrder/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region AJaxSummary
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxSummary(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //Get all the records
            ServiceIndexView<WOWASummary> filteredSummary = 
                woServ.CombinedSummary(param.sSearch,
                    Request["sSortDir_0"] == "asc" ? false : true,
                    param.iDisplayStart,
                    param.iDisplayLength);
            //return what's left to datatables
            var result = from p in filteredSummary.query
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
                iTotalRecords = filteredSummary.totalCount,
                iTotalDisplayRecords = filteredSummary.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ajaxhandler
        /// <summary>
        /// Processes display requests from DataTables.Net javascript library. Called from /Index
        /// </summary>
        /// <param name="param">Datatables.Net parameters object</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            viewOptions opt = new viewOptions();
            opt.CI = (System.Globalization.CultureInfo)Session["Culture"];
            opt.search = param.sSearch;
            opt.EmployerID = string.IsNullOrEmpty(param.searchColName("EID")) ? (int?)null : Convert.ToInt32(param.searchColName("EID"));//employerID
            opt.status = string.IsNullOrEmpty(param.searchColName("status")) ? (int?)null : Convert.ToInt32(param.searchColName("status"));
            opt.orderDescending = param.sSortDir_0 == "asc" ? false : true;
            opt.displayStart = param.iDisplayStart;
            opt.displayLength = param.iDisplayLength;
            opt.sortColName = param.sortColName();
            opt.showOrdersWorkers = param.showOrdersWorkers;
            //Get all the records
            ServiceIndexView<WorkOrder> allWorkOrders = woServ.GetIndexView(opt);

            var result = from p in allWorkOrders.query
                         select new
                         {
                             tabref = p.getTabRef(),
                             tablabel = Machete.Web.Resources.WorkOrders.tabprefix + p.getTabLabel(),
                             EID = Convert.ToString(p.EmployerID),
                             WOID = System.String.Format("{0,5:D5}", p.paperOrderNum),
                             dateTimeofWork = p.dateTimeofWork.ToString(),
                             status = Lookups.byID(p.status, opt.CI.TwoLetterISOLanguageName),
                             WAcount = p.workAssignments.Count(a => a.workOrderID == p.ID).ToString(),
                             contactName = p.contactName,
                             workSiteAddress1 = p.workSiteAddress1,
                             dateupdated = System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", p.dateupdated),
                             updatedby = p.Updatedby,
                             transportMethod = Lookups.byID(p.transportMethodID, opt.CI.TwoLetterISOLanguageName),
                             displayState = _getDisplayState(p),
                             recordid = p.ID.ToString(),
                             workers = param.showOrdersWorkers ? 
                                        from w in p.workAssignments select new 
                                        { 
                                            WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
                                            name = w.workerAssigned != null ? w.workerAssigned.Person.fullName() : null,
                                            skill = Lookups.byID(w.skillID, opt.CI.TwoLetterISOLanguageName),
                                            hours = w.hours,
                                            wage = w.hourlyWage
                                        } : null
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allWorkOrders.totalCount,
                iTotalDisplayRecords = allWorkOrders.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        } 


        private string _getDisplayState(WorkOrder wo)
        {
            string status = Lookups.byID(wo.status, "en");
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
            _wo.transportMethodID = Lookups.transportmethodDefault;
            _wo.typeOfWorkID = Lookups.typesOfWorkDefault;
            _wo.status = Lookups.woStatusDefault;
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
            WorkOrder neworder = woServ.CreateWorkOrder(_wo, userName);           
            //
            //New requests to add
            foreach (var add in workerRequests2)
            {
                add.workOrder = neworder;
                add.workerRequested = _reqServ.GetWorker(add.WorkerID);
                add.updatedby(userName);
                add.createdby(userName);
                neworder.workerRequests.Add(add);
            }
            woServ.SaveWorkOrder(neworder, userName);
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
            WorkOrder workOrder = woServ.GetWorkOrder(id);
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
        // TODO: de-duplicate these actions
        //[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, FormCollection collection, string userName, List<WorkerRequest> workerRequests2)
        {
            WorkOrder workOrder = woServ.GetWorkOrder(id);
            UpdateModel(workOrder);
            //Stale requests to remove
            foreach (var rem in workOrder.workerRequests.Except<WorkerRequest>(workerRequests2, new WorkerRequestComparer()).ToArray())
            {
                var request = _wrServ.GetWorkerRequestsByNum(workOrder.ID, rem.WorkerID);
                _wrServ.DeleteWorkerRequest(request.ID, userName);
                workOrder.workerRequests.Remove(rem);
            }
            //New requests to add
            foreach (var add in workerRequests2.Except<WorkerRequest>(workOrder.workerRequests, new WorkerRequestComparer()))
            {
                add.workOrder = workOrder;
                add.workerRequested = _reqServ.GetWorker(add.WorkerID);
                add.updatedby(userName);
                add.createdby(userName);
                workOrder.workerRequests.Add(add);
            }

            woServ.SaveWorkOrder(workOrder, userName);
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
            WorkOrder workOrder = woServ.GetWorkOrder(id);
            return View(workOrder);
        }
        //
        // GroupView -- Creates the view to print all orders for a given day
        //              assignedOnly: only shows orders that are fully assigned
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult GroupView(DateTime date, bool? assignedOnly)
        {
            WorkOrderGroupPrintView view = new WorkOrderGroupPrintView();
            if (assignedOnly == true) 
                view.orders = woServ.GetActiveOrders(date, true );
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
            woServ.DeleteWorkOrder(id, user);
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
            var workOrder = woServ.GetWorkOrder(id);
            // lookup int value for status active
            workOrder.status = Lookups.getSingleEN("orderstatus","Active");
            woServ.SaveWorkOrder(workOrder, userName);         
            return Json(new
            {
                status = "activated"
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
