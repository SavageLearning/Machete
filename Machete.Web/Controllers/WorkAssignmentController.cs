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
    public class WorkAssignmentController : Controller
    {
        private readonly IWorkAssignmentService _assServ;
        private readonly IEmployerService _empServ;
        private readonly IWorkOrderService _ordServ;
        private string culture {get; set;}
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentController", "");
        public WorkAssignmentController(IWorkAssignmentService workAssignmentService,
                                        IEmployerService employerService,
                                        IWorkOrderService workOrderService)
        {
            this._assServ = workAssignmentService;
            this._empServ = employerService;
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
        }

        #region Index
        //
        // GET: /WorkAssignment/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            var allAssignments = _assServ.GetMany(true);
            IEnumerable<WorkAssignment> emplrfilteredAssignments;
            IEnumerable<WorkAssignment> filteredAssignments;
            IEnumerable<WorkAssignment> sortedAssignments;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch_2))
            {
                emplrfilteredAssignments = _assServ.GetMany(true)
                    .Where(p => p.workOrderID.Equals(Convert.ToInt32(param.sSearch_2)));
            }
            else
            {
                emplrfilteredAssignments = allAssignments;
            }
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredAssignments = emplrfilteredAssignments
                    .Where(p => p.ID.ToString().ContainsOIC(param.sSearch) ||
                                p.active.ToString().ContainsOIC(param.sSearch) ||
                                //p.dateTimeofWork.ToString().ContainsOIC(param.sSearch) ||
                                p.description.ContainsOIC(param.sSearch) ||
                                p.englishLevelID.ToString().ContainsOIC(param.sSearch) ||
                                Lookups.byID(p.skillID, CI.TwoLetterISOLanguageName).ContainsOIC(param.sSearch) ||
                                p.dateupdated.ToString().ContainsOIC(param.sSearch) ||
                                p.Updatedby.ContainsOIC(param.sSearch));
            }
            else
            {
                filteredAssignments = emplrfilteredAssignments;
            }
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<WorkAssignment, string> orderingFunction = (p => sortColIdx == 2 ? p.ID.ToString() :
                                                          sortColIdx == 3 ? p.active.ToString() :
                                                          //sortColIdx == 4 ? p.dateTimeofWork.ToBinary().ToString() :
                                                          sortColIdx == 4 ? p.englishLevelID.ToString() :
                                                          sortColIdx == 5 ? Lookups.byID(p.skillID, culture) :
                                                          sortColIdx == 6 ? p.Updatedby :
                                                          p.dateupdated.ToBinary().ToString());

            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedAssignments = filteredAssignments.OrderBy(orderingFunction);
            else
                sortedAssignments = filteredAssignments.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayAssignments = sortedAssignments.Skip(param.iDisplayStart)
                                                .Take(param.iDisplayLength);

            //return what's left to datatables
            
            var result = from p in displayAssignments
                         select new { tabref = _getTabRef(p),
                                      tablabel = _getTabLabel(p),
                                      WOID = Convert.ToString(p.workOrderID),
                                      WAID = Convert.ToString(p.ID),
                                      englishlevel = Convert.ToString(p.englishLevelID),
                                      skill =  Lookups.byID_noBrackets(p.skillID, culture),
                                      hourlywage = Convert.ToString(p.hourlyWage),
                                      hours = Convert.ToString(p.hours),
                                      days = Convert.ToString(p.days),
                                      description = p.description,
                                      dateupdated = Convert.ToString(p.dateupdated), 
                                      updatedby = p.Updatedby,
                                      dateTimeofWork = p.workOrder.dateTimeofWork.ToString()
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = filteredAssignments.Count(),
                iTotalDisplayRecords = filteredAssignments.Count(),
                aaData = result//,
                //sColumns = "refstring" + "," +
                //           "label" + "," +
                //           "WOID" + "," +
                //           "WAID" + "," +
                //           "englishlevel" + "," +
                //           "skill" + "," +
                //           "hourlyWage" + "," +
                //           "hours" + "," +
                //           "days" + "," +
                //           "description" + "," +
                //           "dateupdated" + "," +
                //           "Updatedby" + ","
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
            return Machete.Web.Resources.WorkAssignments.tabprefix + wa.ID;
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
            //_assignment.dateTimeofWork = DateTime.Today;
            _assignment.skillID = Lookups.skillDefault;
            //_assignment.skillLevel = Lookups.skillLevelDefault;
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
            WorkAssignment workAssignment = _assServ.Get(id);
            //ViewBag.days2 = new SelectList(Lookups.days(), "Value", "Text", workAssignment.days);
            return PartialView(workAssignment);
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
