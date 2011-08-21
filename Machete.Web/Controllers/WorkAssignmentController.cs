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
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkAssignmentController : Controller
    {
        private readonly IWorkAssignmentService _assServ;
        private readonly IWorkerService _wkrServ;
        private readonly IWorkOrderService _ordServ;
        private MacheteContext DB;
        private string culture {get; set;}
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentController", "");
        public WorkAssignmentController(IWorkAssignmentService workAssignmentService,
                                        IWorkerService workerService,
                                        IWorkOrderService workOrderService)
        {
            this._assServ = workAssignmentService;
            this._wkrServ = workerService;
            this._ordServ = workOrderService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            
            ViewBag.EmplrReferences = Lookups.emplrreference(CI.TwoLetterISOLanguageName);
            ViewBag.skillList = Lookups.skill(CI.TwoLetterISOLanguageName);
            ViewBag.TransportMethods = Lookups.transportmethod(CI.TwoLetterISOLanguageName);
            ViewBag.woStatuses = Lookups.orderstatus(CI.TwoLetterISOLanguageName);
            //ViewBag.languages = Lookups.language(CI.TwoLetterISOLanguageName);
            ViewBag.hoursList = Lookups.hours();
            ViewBag.daysList = Lookups.days();
            ViewBag.skillLevelList = Lookups.skillLevels();
            DB = new MacheteContext();
        }

        #region Index
        //
        // GET: /WorkAssignment/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult Index()
        {
            WorkAssignmentIndex _model = new WorkAssignmentIndex();
            _model.todaysdate = DateTime.Today.ToShortDateString();
            return View(_model);
        }

        #endregion

        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //var allAssignments = _assServ.GetMany();

            //return what's left to datatables
            ServiceIndexView<WorkAssignment> was = _assServ.GetIndexView(
                    CI,
                    param.sSearch,
                    param.todaysdate == null ? null : (DateTime?)DateTime.Parse(param.todaysdate),
                    Convert.ToInt32(param.dwccardnum),
                    Convert.ToInt32(param.searchColName("WOID")),
                    param.sSortDir_0 == "asc" ? false : true,
                    param.iDisplayStart,
                    param.iDisplayLength,
                    param.sortColName()
                );
            var result = from p in was.query
                         select new { tabref = _getTabRef(p),
                                      tablabel = _getTabLabel(p),
                                      WOID = Convert.ToString(p.workOrderID),
                                      WAID = Convert.ToString(p.ID),
                                      pWAID = _getFullPseudoID(p), 
                                      englishlevel = Convert.ToString(p.englishLevelID),
                                      skill =  Lookups.byID(p.skillID, culture),
                                      hourlywage = System.String.Format("${0:f2}", p.hourlyWage),
                                      hours = Convert.ToString(p.hours),
                                      days = Convert.ToString(p.days),
                                      description = p.description,
                                      dateupdated = Convert.ToString(p.dateupdated), 
                                      updatedby = p.Updatedby,
                                      dateTimeofWork = p.workOrder.dateTimeofWork.ToString(),
                                      earnings = System.String.Format("${0:f2}",(p.hourlyWage * p.hours * p.days))
                         };
 
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
        // _getTabRef
        //
        private string _getTabRef(WorkAssignment wa)
        {
            return "/WorkAssignment/Edit/" + Convert.ToString(wa.ID);
        }
        
        //
        // _getTabLabel
        //
        private string _getTabLabel(WorkAssignment wa)
        {
            return Machete.Web.Resources.WorkAssignments.tabprefix + _getFullPseudoID(wa);
        }
        private string _getFullPseudoID(WorkAssignment wa)
        {
            return (wa.workOrder.paperOrderNum.HasValue ? 
                        System.String.Format("{0,5:D5}", wa.workOrder.paperOrderNum) : 
                        System.String.Format("{0,5:D5}", wa.workOrderID))
                    + "-" + (wa.pseudoID.HasValue ? 
                        System.String.Format("{0,2:D2}", wa.pseudoID) : 
                        System.String.Format("{0,5:D5}", wa.ID));
        }
        
        
        //
        // GET: /WorkAssignment/Create
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Create
        public ActionResult Create(int WorkOrderID)
        {
            WorkAssignment _assignment = new WorkAssignment();
            _assignment.active = true;
            _assignment.workOrderID = WorkOrderID;
            _assignment.skillID = Lookups.skillDefault;
            _assignment.hours = Lookups.hoursDefault;
            _assignment.days = Lookups.daysDefault;
            _assignment.hourlyWage = Lookups.hourlyWageDefault;
            return PartialView(_assignment);
        }

        //
        // POST: /WorkAssignment/Create
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(WorkAssignment assignment, string userName)
        {
            if (!ModelState.IsValid)
            {
                //TODO: this may always blow up
                return PartialView("Create", assignment);
            }
            assignment.workOrder = _ordServ.GetWorkOrder(assignment.workOrderID);
            assignment.workOrder.waPseudoIDCounter++;
            assignment.pseudoID = assignment.workOrder.waPseudoIDCounter;
            WorkAssignment newAssignment = _assServ.Create(assignment, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newAssignment),
                sNewLabel = _getTabLabel(newAssignment),
                iNewID = newAssignment.ID
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        
        //
        // POST: /WorkAssignment/Edit/5
        // TODO: catch exceptions, notify user
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Duplicate
        public ActionResult Duplicate(int id, string userName)
        {
            WorkAssignment _assignment = _assServ.Get(id);
            WorkAssignment newAssign = _assignment;
            newAssign.workOrder.waPseudoIDCounter++;
            newAssign.pseudoID = newAssign.workOrder.waPseudoIDCounter;
            newAssign.workerAssigned = null;
            newAssign.workerAssignedID = null;
            _assServ.Create(newAssign, userName);
            return Json(new
            {
                sNewRef = _getTabRef(newAssign),
                sNewLabel = _getTabLabel(newAssign),
                iNewID = newAssign.ID
            },
            JsonRequestBehavior.AllowGet);

        }
        #endregion

        
        //
        // GET: /WorkAssignment/Edit/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Edit
        public ActionResult Edit(int id)
        {
            WorkAssignment wa = _assServ.Get(id);
            if (wa.workerAssignedID != null)
            {
                wa.workerAssigned = _wkrServ.GetWorker((int)wa.workerAssignedID);
            }
            //ViewBag.days2 = new SelectList(Lookups.days(), "Value", "Text", workAssignment.days);
            return PartialView(wa);
        }
        //
        // POST: /WorkAssignment/Edit/5
        // TODO: catch exceptions, notify user
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, FormCollection collection, string userName)
        {
            WorkAssignment _assignment = _assServ.Get(id);

            if (TryUpdateModel(_assignment))
            {
                _assServ.Save(_assignment, userName);
                //feeding parent work order to Index
                return PartialView(_assignment);
            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = _assignment.ID; log.Log(levent);
                return PartialView(_assignment);
            }
        }
        #endregion

        
        //
        //GET: /WorkAssignment/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, User")]
        #region View
        public ActionResult View(int id)
        {
            WorkAssignment workAssignment = _assServ.Get(id);
            
            return View(workAssignment);
        }
        #endregion

        
        //
        // GET: /WorkAssignment/Delete/5
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region Delete
        public ActionResult Delete(int id)
        {
            var workAssignment = _assServ.Get(id);

            return View(workAssignment);
        }

        //
        // POST: /WorkAssignment/Delete/5

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Delete(int id, FormCollection collection, string user)
        {
            var _assignment = _assServ.Get(id);
            var _ord = _ordServ.GetWorkOrder(_assignment.workOrderID);
            _assServ.Delete(id, user);
            return PartialView("Index", _ord);
        }
        #endregion


    }


}
