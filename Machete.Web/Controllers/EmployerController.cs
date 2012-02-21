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
    public class MacheteController : Controller
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "EmployerController", "");

        protected override void OnException(ExceptionContext filterContext)
        {
            string returnMsg = "";
            string modelerrors = null;
            bool success = true;
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            returnMsg = RootException.Get(filterContext.Exception, this.ToString());
            modelerrors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            levent.Level = LogLevel.Error; levent.Message = "waServ Edit failed. " + returnMsg;
            levent.Properties["RecordID"] = "1"; log.Log(levent);
            success = false;
            filterContext.Result = Json(new
            {
                rtnMessage = returnMsg,
                modelErrors = modelerrors,
                jobSuccess = success
            }, JsonRequestBehavior.AllowGet);
            filterContext.ExceptionHandled = true;
        }
    }

    [ElmahHandleError]
    public class EmployerController : MacheteController
    {
        private readonly IEmployerService eServ;

        public EmployerController(IEmployerService employerService)
        {
            this.eServ = employerService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        #region Index
        //
        // GET: /Employer/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        /// <summary>
        /// GET: /Employer/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allEmployers = eServ.GetEmployers(true);
            IEnumerable<Employer> filteredEmployers;
            IEnumerable<Employer> sortedEmployers;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredEmployers = eServ.GetEmployers(true)
                    .Where(p => p.active.ToString().ContainsOIC(param.sSearch) ||
                                p.name.ContainsOIC(param.sSearch) ||
                                p.address1.ContainsOIC(param.sSearch) ||
                                p.phone.ContainsOIC(param.sSearch) ||
                                p.city.ContainsOIC(param.sSearch));
            }
            else
            {
                filteredEmployers = allEmployers;
            }
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Employer, string> orderingFunction = (p => sortColIdx == 2 ? p.active.ToString() :
                                                          sortColIdx == 3 ? p.name :
                                                          sortColIdx == 4 ? p.address1 :
                                                          sortColIdx == 5 ? p.city :
                                                          sortColIdx == 6 ? p.phone :
                                                          p.dateupdated.ToBinary().ToString());
                                                          
            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedEmployers = filteredEmployers.OrderBy(orderingFunction);
            else
                sortedEmployers = filteredEmployers.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayEmployers = sortedEmployers.Skip(param.iDisplayStart)
                                                .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayEmployers
                         select new { tabref = _getTabRef(p),
                                      tablabel = _getTabLabel(p),
                                      active = Convert.ToString(p.active),
                                      EID = Convert.ToString(p.ID),
                                      recordid = Convert.ToString(p.ID),
                                      name = p.name, 
                                      address1 = p.address1, 
                                      city = p.city, 
                                      phone =  p.phone, 
                                      dateupdated = Convert.ToString(p.dateupdated),
                                      Updatedby = p.Updatedby 
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allEmployers.Count(),
                iTotalDisplayRecords = filteredEmployers.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        private string _getTabRef(Employer emp)
        {
            if (emp == null) return null;
            return "/Employer/Edit/" + Convert.ToString(emp.ID);
        }

        private string _getTabLabel(Employer emp)
        {
            if (emp == null) return null;
            return emp.name;
        }

        #region Create
        /// <summary>
        /// GET: /Employer/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create()
        {
            var _model = new Employer();
            _model.active = true;
            _model.city = "Seattle";
            _model.state = "WA";
            _model.blogparticipate = false;
            _model.referredby = Lookups.emplrreferenceDefault;
            return PartialView("Create", _model);
        }

        //
        // POST: /Employer/Create
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Create(Employer employer, string userName)
        {
            string returnMsg = "";
            string modelerrors = null;
            bool success = true;
            Employer newEmployer = null;
            try
            {
                UpdateModel(employer);
                newEmployer = eServ.CreateEmployer(employer, userName);              
            }
            catch (Exception e)
            {
                returnMsg = RootException.Get(e, "EmployerService");
                modelerrors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                levent.Level = LogLevel.Error; levent.Message = "waServ Edit failed. " + returnMsg;
                levent.Properties["RecordID"] = employer.ID; log.Log(levent);
                success = false;
            }            

            return Json(new
            {
                sNewRef = _getTabRef(newEmployer),
                sNewLabel = _getTabLabel(newEmployer),
                iNewID = newEmployer != null ? newEmployer.ID : 0,
                modelErrors = modelerrors,
                jobSuccess = success
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit
        //
        // GET: /Employer/Edit/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            Employer employer = eServ.GetEmployer(id);
            return PartialView("Edit", employer);
        }
        //
        // POST: /Employer/Edit/5
        // TODO: catch exceptions, notify user
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Edit(int id, FormCollection collection, string userName)
        {
            Employer employer = eServ.GetEmployer(id);
            string returnMsg = "";
            string modelerrors = null;
            bool success = true;
            try
            {
                UpdateModel(employer);
                eServ.SaveEmployer(employer, userName);                
            }
            catch (Exception e)
            {
                returnMsg = RootException.Get(e, "WorkAssignmentService");
                modelerrors = string.Join("; ", ModelState.Values
                                                .SelectMany(x => x.Errors)
                                                .Select(x => x.ErrorMessage));
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed: " + modelerrors;
                levent.Properties["RecordID"] = employer.ID; log.Log(levent);
                success = false;
            }
            return Json(new
            {
                rtnMessage = returnMsg,
                modelErrors = modelerrors,
                jobSuccess = success
            }, JsonRequestBehavior.AllowGet);
            
        }
        #endregion
        #region View
        //
        //GET: /Employer/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            EmployerViewModel _vm = new EmployerViewModel();
            _vm.employer = eServ.GetEmployer(id);
            _vm.orders = eServ.GetOrders(id);
            return View(_vm);
        }
        #endregion

        #region Delete
        //
        // POST: /Employer/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Delete(int id, FormCollection collection, string user)
        {
            string status = null;
            bool jobSuccess = true;
            try
            {
                eServ.DeleteEmployer(id, user);
            }
            catch (Exception e)
            {
                status = RootException.Get(e, "EmployerService");
                jobSuccess = false;
            }
            return Json(new
            {
                status = status ?? "OK",
                jobSuccess = jobSuccess,
                rtnMessage = status,
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
