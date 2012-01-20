using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Helpers;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.ViewModel;
using System.Web.Routing;
using Machete.Web.Models;
using System.Text.RegularExpressions;
using System.Data.Objects;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkOrderController : Controller
    {
        private readonly IWorkOrderService woServ;
        private readonly IEmployerService _empServ;
        private readonly IWorkerService _reqServ;
        private readonly IWorkerRequestService _wrServ;
        private readonly IWorkAssignmentService _waServ;
        private static Regex isTimeSpecific = new Regex(@"^\s*\d{1,2}[\/-_]\d{1,2}[\/-_]\d{2,4}\s+\d{1,2}:\d{1,2}");
        private static Regex isDaySpecific = new Regex(@"^\s*\d{1,2}\/\d{1,2}\/\d{2,4}");
        private static Regex isMonthSpecific = new Regex(@"^\s*\d{1,2}\/\d{4,4}");
        
        private string culture {get; set;}
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkOrderController", "");
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
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);

        //    System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
        //    ViewBag.EmplrReferences = Lookups.emplrreference(CI.TwoLetterISOLanguageName);
        //    ViewBag.TypesOfWork = Lookups.typeOfWork(CI.TwoLetterISOLanguageName);
        //    ViewBag.TransportMethods = Lookups.transportmethod(CI.TwoLetterISOLanguageName);
        //    ViewBag.woStatuses = Lookups.orderstatus(CI.TwoLetterISOLanguageName);
        //    ViewBag.languages = Lookups.language(CI.TwoLetterISOLanguageName);
        //}
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
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];            
            //Get all the records            
            ServiceIndexView<WorkOrder> allWorkOrders = woServ.GetIndexView(
                CI,
                param.sSearch,
                string.IsNullOrEmpty(param.sSearch_2) ? (int?)null : Convert.ToInt32(param.sSearch_2),
                string.IsNullOrEmpty(param.sSearch_5) ? (int?)null : Convert.ToInt32(param.sSearch_5),
                param.sSortDir_0 == "asc" ? false : true,
                param.iDisplayStart,
                param.iDisplayLength,
                param.sortColName()
                );


            var result = from p in allWorkOrders.query
                         select new { tabref = p.getTabRef(),
                                      tablabel = Machete.Web.Resources.WorkOrders.tabprefix + p.getTabLabel(),
                                      EID = Convert.ToString(p.EmployerID),
                                      WOID = System.String.Format("{0,5:D5}", p.paperOrderNum),
                                      dateTimeofWork =  p.dateTimeofWork.ToString(),
                                      status = Lookups.byID(p.status, CI.TwoLetterISOLanguageName),
                                      WAcount = p.workAssignments.Count(a => a.workOrderID == p.ID).ToString(),
                                      contactName =  p.contactName, 
                                      workSiteAddress1 =  p.workSiteAddress1,                                        
                                      dateupdated = System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", p.dateupdated), 
                                      updatedby = p.Updatedby,
                                      transportMethod = Lookups.byID(p.transportMethodID, CI.TwoLetterISOLanguageName),
                                      displayState = _getDisplayState(p),
                                      recordid = p.ID.ToString()
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

        private void _setCreateDefaults(WorkOrderEditor _model)
        {
            _model.order.transportMethodID = Lookups.transportmethodDefault;
            _model.order.typeOfWorkID = Lookups.typesOfWorkDefault;
            _model.order.status = Lookups.woStatusDefault;
        }

        #region Create
        /// <summary>
        /// HTTP GET /WorkOrder/Create
        /// </summary>
        /// <param name="EmployerID">Parent record ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(int EmployerID)
        {
            WorkOrder _model = new WorkOrder();
            _model.EmployerID = EmployerID;
            _model.dateTimeofWork = DateTime.Today;
            _model.transportMethodID = Lookups.transportmethodDefault;
            _model.typeOfWorkID = Lookups.typesOfWorkDefault;
            _model.status = Lookups.woStatusDefault;
            ViewBag.workerRequests = new List<SelectListItem> {};
            return PartialView("Create", _model);
        }
        /// <summary>
        /// POST: /WorkOrder/Create
        /// </summary>
        /// <param name="_model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(WorkOrder _model, string userName, List<WorkerRequest> workerRequests2)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Create", _model);
            }
            WorkOrder neworder = woServ.CreateWorkOrder(_model, userName);           
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
        // TODO: catch exceptions, notify user
        // TODO: de-duplicate these actions
        //[Bind(Exclude = "workerRequests")]
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, FormCollection collection, string userName, List<WorkerRequest> workerRequests2)
        {
            WorkOrder workOrder = woServ.GetWorkOrder(id);
            TryUpdateModel(workOrder);
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
            if (ModelState.IsValid)
            {
                woServ.SaveWorkOrder(workOrder, userName);
                return Json(new
                {
                    status = "OK",
                    editedID = id
                },
                JsonRequestBehavior.AllowGet);
            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = workOrder.ID; log.Log(levent);
                return Json(new
                {
                    status = "ERROR",
                    editedID = id
                },
                JsonRequestBehavior.AllowGet);
            }
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
        //
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
            string status = null;
            try
            {
                woServ.DeleteWorkOrder(id, user);
            }
            catch (Exception e)
            {
                status = RootException.Get(e, "WorkOrderService");
            }

            return Json(new
            {
                status = status ?? "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Activate
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Activate(int id, FormCollection collection, string userName)
        {
            var workOrder = woServ.GetWorkOrder(id);
            // lookup int value for status active
            workOrder.status = Lookups.getSingleEN("orderstatus","Active");
            woServ.SaveWorkOrder(workOrder, userName);
         
            //return PartialView("Edit", workOrder);
            return Json(new
            {
                status = "activated"
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
