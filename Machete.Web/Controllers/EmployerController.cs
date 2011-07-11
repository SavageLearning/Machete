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
    public class EmployerController : Controller
    {
        private readonly IEmployerService employerService;
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "EmployerController", "");
        public EmployerController(IEmployerService employerService)
        {
            this.employerService = employerService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            ViewBag.emplrReferences = Lookups.emplrreference(CI.TwoLetterISOLanguageName);
        }
        #region Index
        //
        // GET: /Employer/
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
            var allEmployers = employerService.GetEmployers(true);
            IEnumerable<Employer> filteredEmployers;
            IEnumerable<Employer> sortedEmployers;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredEmployers = employerService.GetEmployers(true)
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
                         select new[] { _getTabRef(p),
                                        _getTabLabel(p),
                                        Convert.ToString(p.active), 
                                        p.name, 
                                        p.address1, 
                                        p.city, 
                                        p.phone, 
                                        Convert.ToString(p.dateupdated), 
                                        p.Updatedby 
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
            return "/Employer/Edit/" + Convert.ToString(emp.ID);
        }

        private string _getTabLabel(Employer emp)
        {
            return emp.name;
        }
        //private string _getLabel(Employer record)
        //{
        //    return record.name;
        //}
        //private void _setLabel(Employer record)
        //{
        //    record.tablabel = _getLabel(record);
        //}

        #region Create
        //
        // GET: /Employer/Create
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create()
        {
            var _model = new Employer();
            _model.active = true;
            _model.city = "Seattle";
            _model.state = "WA";
            _model.referredby = Lookups.emplrreferenceDefault;
            return PartialView("Create", _model);
        }

        //
        // POST: /Employer/Create
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(Employer employer, string userName)
        {
            if (!ModelState.IsValid)
            {
                //TODO: This probably wont work for tabs & partials
                return PartialView("Create", employer);
            }
            Employer newEmployer = employerService.CreateEmployer(employer, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newEmployer),
                sNewLabel = _getTabLabel(newEmployer),
                iNewID = newEmployer.ID
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
            Employer employer = employerService.GetEmployer(id);
            return PartialView("Edit", employer);
        }
        //
        // POST: /Employer/Edit/5
        // TODO: catch exceptions, notify user
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, FormCollection collection, string userName)
        {
            Employer employer = employerService.GetEmployer(id);

            if (TryUpdateModel(employer))
            {
                employerService.SaveEmployer(employer, userName);
                //_setLabel(employer);
                return PartialView("Edit", employer);
            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = employer.ID; log.Log(levent);
                return PartialView("Edit", employer);
            }
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
            _vm.employer = employerService.GetEmployer(id);
            _vm.orders = employerService.GetOrders(id);
            return View(_vm);
        }
        #endregion

        #region Delete
        //
        // GET: /Employer/Delete/5
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Delete(int id)
        {
            var employer = employerService.GetEmployer(id);
            return View(employer);
        }

        //
        // POST: /Employer/Delete/5

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Delete(int id, FormCollection collection, string user)
        {
            employerService.DeleteEmployer(id, user);
            return PartialView("Index");
        }
        #endregion
    }
}
