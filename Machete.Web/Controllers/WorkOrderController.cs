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

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkOrderController : Controller
    {
        private readonly IWorkOrderService workOrderService;
        private readonly IEmployerService _empServ;
        private readonly IWorkerService _reqServ;
        private readonly IWorkerRequestService _wrServ;
        private readonly IWorkAssignmentService _waServ;
        private string culture {get; set;}
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkOrderController", "");
        public WorkOrderController(IWorkOrderService workOrderService, 
                                   IWorkAssignmentService workAssignmentService,
                                   IEmployerService employerService,
                                   IWorkerService workerService,
                                   IWorkerRequestService requestService)
        {
            this.workOrderService = workOrderService;
            this._empServ = employerService;
            this._reqServ = workerService;
            this._waServ = workAssignmentService;
            this._wrServ = requestService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            ViewBag.EmplrReferences = Lookups.emplrreference(CI.TwoLetterISOLanguageName);
            ViewBag.TypesOfWork = Lookups.typeOfWork(CI.TwoLetterISOLanguageName);
            ViewBag.TransportMethods = Lookups.transportmethod(CI.TwoLetterISOLanguageName);
            ViewBag.woStatuses = Lookups.orderstatus(CI.TwoLetterISOLanguageName);
            ViewBag.languages = Lookups.language(CI.TwoLetterISOLanguageName);
        }
        #region Index
        //
        // GET: /WorkOrder/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        //[Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        //public ActionResult Dispatch()
        //{
        //    return View(_getSummary());
        //}
        #region AJaxSummary
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult AjaxSummary(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //Get all the records
            IEnumerable<OrderSummary> allSummary = _getSummary();
            IEnumerable<OrderSummary> filteredSummary;
            IEnumerable<OrderSummary> sortedSummary;
            //Search based on search-bar string 

            if (!string.IsNullOrEmpty(param.sSearch))            
                filteredSummary = allSummary.Where(p => p.date.ContainsOIC(param.sSearch));            
            else
                filteredSummary = allSummary;

            //Sort the order  based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<OrderSummary, string> orderingFunction = (p => p.date);

            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedSummary = filteredSummary.OrderBy(orderingFunction);
            else
                sortedSummary = filteredSummary.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayedSummary = sortedSummary.Skip(param.iDisplayStart)
                                                .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayedSummary
                         select  new[] { p.date,
                                         p.weekday,
                                         p.pending_wo.ToString(),
                                         p.pending_wa.ToString(),
                                         p.active_wo.ToString(),
                                         p.active_wa.ToString(),
                                         p.completed_wo.ToString(),
                                         p.completed_wa.ToString(),
                                         p.cancelled_wo.ToString(),
                                         p.cancelled_wa.ToString(),
                                         p.expired_wo.ToString(),
                                         p.expired_wa.ToString()
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = filteredSummary.Count(),
                iTotalDisplayRecords = filteredSummary.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region addrequest/removerequeset
        /// <summary>
        /// Adds workerRequest
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workerID"></param>
        /// <param name="collection"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        //[HttpPost, UserNameFilter]
        //[Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        //public bool  AddRequest(int id, int workerID, FormCollection collection, string user)
        //{
        //    WorkOrder order = workOrderService.GetWorkOrder(id);
        //    if (order.workerRequests != null)
        //    {
        //        foreach (WorkerRequest req in order.workerRequests)
        //        {
        //            if (req.WorkerID == workerID) return false;
        //        }
        //    }
        //    WorkerRequest request = new WorkerRequest();
        //    request.WorkOrderID = id;
        //    request.WorkerID = workerID;
        //    _reqServ.CreateWorkerRequest(request, user);
        //    return true;
        //}

        //[HttpPost, UserNameFilter]
        //[Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        //public void RemoveRequest(int id, FormCollection collection, string user)
        //{
        //    _reqServ.DeleteWorkerRequest(id, user);
        //}

        //[Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        //public JsonResult GetRequests(int id)
        //{
        //    WorkOrder workOrder = workOrderService.GetWorkOrder(id);
        //    List<SelectListItem> list;
        //    try
        //    {
        //        list = new List<SelectListItem>(workOrder.workerRequests.ToList()
        //               .Select(x => new SelectListItem
        //               {
        //                   Text = x.workerRequested.Person.fullName(),
        //                   Value = x.ID.ToString()
        //               }));
        //    }
        //    catch (Exception e)
        //    {
        //        list = null;
        //    }


        //    return Json(list, JsonRequestBehavior.AllowGet);

        //}
        #endregion
        #region Ajaxhandler
        /// <summary>
        /// Processes display requests from DataTables.Net javascript library. Called from /Index
        /// </summary>
        /// <param name="param">Datatables.Net parameters object</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];            
            //Get all the records            
            ServiceIndexView<WorkOrder> allWorkOrders = workOrderService.GetIndexView(
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
                         select new { tabref = _getTabRef(p),
                                      tablabel =  _getTabLabel(p),
                                      EID = Convert.ToString(p.EmployerID),
                                      WOID = System.String.Format("{0,5:D5}", p.paperOrderNum),
                                      dateTimeofWork = System.String.Format("{0:MM/dd/yyyy}", p.dateTimeofWork),
                                      status = Lookups.byID(p.status, CI.TwoLetterISOLanguageName),
                                      WAcount = p.workAssignments.Count(a => a.workOrderID == p.ID).ToString(),
                                      contactName =  p.contactName, 
                                      workSiteAddress1 =  p.workSiteAddress1,                                        
                                      dateupdated = System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", p.dateupdated), 
                                      updatedby = p.Updatedby
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

        private string _getTabRef(WorkOrder wo) {
            return "/WorkOrder/Edit/" + Convert.ToString(wo.ID);
        }

        private string _getTabLabel(WorkOrder wo)
        {
            return Machete.Web.Resources.WorkOrders.tabprefix + wo.getPseudoWOID() + " @ " + wo.workSiteAddress1;
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
        public ActionResult Create(WorkOrder _model, string userName)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Create", _model);
            }
            WorkOrder neworder = workOrderService.CreateWorkOrder(_model, userName);
            //return PartialView("Index", neworder);
            return Json(new 
            {
                sNewRef = _getTabRef(neworder),
                sNewLabel = _getTabLabel(neworder),
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
            WorkOrder workOrder = workOrderService.GetWorkOrder(id);
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
            WorkOrder workOrder = workOrderService.GetWorkOrder(id);
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
                workOrderService.SaveWorkOrder(workOrder, userName);
                return RedirectToAction("Index", new { EmployerID = workOrder.EmployerID });
            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = workOrder.ID; log.Log(levent);
                return PartialView("Edit", workOrder);
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
            WorkOrder workOrder = workOrderService.GetWorkOrder(id);
            return View(workOrder);
        }
        #endregion
        #region Delete
        //
        // GET: /WorkOrder/Delete/5
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Delete(int id)
        {
            var workOrder = workOrderService.GetWorkOrder(id);
            return View(workOrder);
        }
        //
        // POST: /WorkOrder/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Delete(int id, FormCollection collection, string user)
        {
            var workOrder = workOrderService.GetWorkOrder(id);
            workOrderService.DeleteWorkOrder(id, user);
            return RedirectToAction("Index", new { EmployerID = workOrder.EmployerID });
        }
        #endregion
        #region Activate
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Activate(int id, FormCollection collection, string userName)
        {
            var workOrder = workOrderService.GetWorkOrder(id);
            // lookup int value for status active
            workOrder.status = Lookups.getSingleEN("orderstatus","Active");
            workOrderService.SaveWorkOrder(workOrder, userName);
            //workOrderService.DeleteWorkOrder(id, user);
            return PartialView("Edit", workOrder);
        }
        #endregion
        private IEnumerable<OrderSummary> _getSummary()
        {

            IEnumerable<OrderSummary> result;

            result = workOrderService.GetSummary().Join(_waServ.GetSummary(),
                            wo => new { wo.date, wo.status },
                            wa => new { wa.date, wa.status },
                            (wo, wa) => new
                            {
                                wo.date,
                                wo.status,
                                wo_count = wo.count,
                                wa_count = wa.count
                            }).OrderBy(a => a.date)
            .GroupBy(gb => gb.date)
            .Select(g => new OrderSummary
            {
                date = g.Key,
                weekday = Convert.ToDateTime(g.Key).ToString("dddd"),
                pending_wo = g.Where(c => c.status == 43).Sum(d => d.wo_count),
                pending_wa = g.Where(c => c.status == 43).Sum(d => d.wa_count),
                active_wo = g.Where(c => c.status == 42).Sum(d => d.wo_count),
                active_wa = g.Where(c => c.status == 42).Sum(d => d.wa_count),
                completed_wo = g.Where(c => c.status == 44).Sum(d => d.wo_count),
                completed_wa = g.Where(c => c.status == 44).Sum(d => d.wa_count),
                cancelled_wo = g.Where(c => c.status == 45).Sum(d => d.wo_count),
                cancelled_wa = g.Where(c => c.status == 45).Sum(d => d.wa_count),
                expired_wo = g.Where(c => c.status == 46).Sum(d => d.wo_count),
                expired_wa = g.Where(c => c.status == 46).Sum(d => d.wa_count)
            });
            return result;
        }
    }
}
