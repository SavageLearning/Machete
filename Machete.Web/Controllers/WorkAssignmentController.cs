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
        private readonly IWorkAssignmentService waServ;
        private readonly IWorkerService wkrServ;
        private readonly IWorkOrderService woServ;
        private readonly IWorkerSigninService wsiServ;
        private MacheteContext DB;
        private string culture {get; set;}
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentController", "");
        public WorkAssignmentController(IWorkAssignmentService workAssignmentService,
                                        IWorkerService workerService,
                                        IWorkOrderService workOrderService,
                                        IWorkerSigninService signinService)
        {
            this.waServ = workAssignmentService;
            this.wkrServ = workerService;
            this.woServ = workOrderService;
            this.wsiServ = signinService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            
            ViewBag.EmplrReferences = Lookups.emplrreference(CI.TwoLetterISOLanguageName);
            ViewBag.skillList = Lookups.skill(CI.TwoLetterISOLanguageName, false);
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
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            WorkAssignmentIndex _model = new WorkAssignmentIndex();
            //_model.todaysdate = DateTime.Today.ToShortDateString();
            _model.todaysdate = System.String.Format("{0:dddd, d MMMM yyyy}", DateTime.Today);

            return View(_model);
        }

        #endregion

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];

            ServiceIndexView<WorkAssignment> was = waServ.GetIndexView(new DispatchOptions {
                    CI = CI,
                    search = param.sSearch,
                    date = param.todaysdate == null ? null : (DateTime?)DateTime.Parse(param.todaysdate),
                    dwccardnum = Convert.ToInt32(param.dwccardnum),
                    woid = Convert.ToInt32(param.searchColName("WOID")),
                    orderDescending = param.sSortDir_0 == "asc" ? false : true,
                    displayStart = param.iDisplayStart,
                    displayLength = param.iDisplayLength,
                    sortColName=param.sortColName(),
                    wa_grouping = param.wa_grouping,
                    typeofwork_grouping = param.typeofwork_grouping,
                    status = param.status,
                    showPending = param.showPending
            });
            var result = from p in was.query select new { 
                            tabref = _getTabRef(p),
                            tablabel = _getTabLabel(p),
                            WOID = Convert.ToString(p.workOrderID),
                            WAID = Convert.ToString(p.ID),
                            recordid = Convert.ToString(p.ID),
                            pWAID = _getFullPseudoID(p), 
                            englishlevel = Convert.ToString(p.englishLevelID),
                            skill =  Lookups.byID(p.skillID, CI.TwoLetterISOLanguageName),
                            hourlywage = System.String.Format("${0:f2}", p.hourlyWage),
                            hours = Convert.ToString(p.hours),
                            days = Convert.ToString(p.days),
                            description = p.description,
                            dateupdated = Convert.ToString(p.dateupdated), 
                            updatedby = p.Updatedby,
                            dateTimeofWork = p.workOrder.dateTimeofWork.ToString(),
                            status = p.workOrder.status.ToString(),
                            earnings = System.String.Format("${0:f2}",(p.hourlyWage * p.hours * p.days)),
                            WSIID = p.workerSigninID ?? 0,
                            WID = p.workerAssignedID ?? 0,
                            assignedWorker = p.workerAssigned != null ? p.workerAssigned.dwccardnum + " " + p.workerAssigned.Person.fullName() : "",
                            requestedList = p.workOrder.workerRequests.Select(a => a.fullNameAndID).ToArray()
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
            assignment.workOrder = woServ.GetWorkOrder(assignment.workOrderID);
            assignment.workOrder.waPseudoIDCounter++;
            assignment.pseudoID = assignment.workOrder.waPseudoIDCounter;
            WorkAssignment newAssignment = waServ.Create(assignment, userName);

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
            WorkAssignment _assignment = waServ.Get(id);
            WorkAssignment newAssign = _assignment;
            newAssign.workOrder.waPseudoIDCounter++;
            newAssign.pseudoID = newAssign.workOrder.waPseudoIDCounter;
            newAssign.workerAssigned = null;
            newAssign.workerAssignedID = null;
            waServ.Create(newAssign, userName);
            return Json(new
            {
                sNewRef = _getTabRef(newAssign),
                sNewLabel = _getTabLabel(newAssign),
                iNewID = newAssign.ID
            },
            JsonRequestBehavior.AllowGet);

        }
        #endregion

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        #region Assign
        public ActionResult Assign(int waid, int wsiid, string userName)
        {
            string returnMsg = "";
            bool successful = true;
            WorkerSignin signin = wsiServ.GetWorkerSignin(wsiid);          
            WorkAssignment assignment = waServ.Get(waid);
            try
            {
                waServ.Assign(assignment, signin, userName);
            }
            catch (Exception e)
            {
                returnMsg = e.Message.ToString();
                successful = false;
            }
            return Json(new
            {
                resultMsg = returnMsg,
                result = successful
            }, JsonRequestBehavior.AllowGet);            
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]

        public ActionResult Unassign(int? waid, int? wsiid, string userName)
        {
            string returnMsg = "";
            bool successful = true;
            WorkerSignin signin = null;
            WorkAssignment assignment = null;
            if (wsiid != null)
            {
                signin = wsiServ.GetWorkerSignin((int)wsiid);
            }
            if (waid != null)
             assignment = waServ.Get((int)waid);
            try
            {
                waServ.Unassign(signin, assignment, userName);
            }
            catch (Exception e)
            {
                returnMsg = e.Message.ToString();
                successful = false;
            }
            return Json(new
            {
                resultMsg = returnMsg,
                result = successful
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
            WorkAssignment wa = waServ.Get(id);
            if (wa.workerAssignedID != null)
            {
                wa.workerAssigned = wkrServ.GetWorker((int)wa.workerAssignedID);
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
            WorkAssignment _assignment = waServ.Get(id);

            if (TryUpdateModel(_assignment))
            {
                waServ.Save(_assignment, userName);
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
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        #region View
        public ActionResult View(int id)
        {
            WorkAssignment workAssignment = waServ.Get(id);
            
            return View(workAssignment);
        }
        #endregion
        #region Delete
        //
        // POST: /WorkAssignment/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Delete(int id, FormCollection collection, string user)
        {
            waServ.Delete(id, user);
            string status = null;
            try
            {
                waServ.Delete(id, user);
            }
            catch (Exception e)
            {
                status = RootException.Get(e, "WorkAssignmentService");
            }

            return Json(new
            {
                status = status ?? "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
    }


}
