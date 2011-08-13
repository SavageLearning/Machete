using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
using Machete.Helpers;
using Machete.Web.ViewModel;
using Machete.Web.Helpers;
using NLog;
using Machete.Domain;
using Machete.Data;
using Microsoft.Reporting.WebForms;
using Machete.Web.Models;

namespace Machete.Web.Controllers
{
   [ElmahHandleError]
    public class WorkerSigninController : Controller
    {
        private readonly IWorkerSigninService _serv;
        
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerSigninController", "");
        public WorkerSigninController(IWorkerSigninService workerSigninService)
        {
            this._serv = workerSigninService;
        }

        //
        // GET: /WorkerSignin/
        // Initial page creation
        [Authorize(Roles = "Manager, Administrator, Check-in, PhoneDesk, User")]
        public ActionResult Index()
        {        
            WorkerSigninViewModel _model = new WorkerSigninViewModel();
            if (ViewBag.dateforsignin == null) _model.dateforsignin = DateTime.Today;
            else _model.dateforsignin = ViewBag.dateforsignin;
            _model.workersignins = _serv.getView(_model.dateforsignin);
            _model.last_chkin_image = new Image();
            return View(_model);
        }
        // subsequent page creations
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardentry, DateTime dateforsignin)
        {
            // Signin the card just swiped
            var _signin = new WorkerSignin();
            _signin.dwccardnum = dwccardentry;
            _signin.dateforsignin = dateforsignin;
            //TODO: Create config page that lets admin control whether unmatched card ids are recorded
            _serv.CreateWorkerSignin(_signin, this.User.Identity.Name);
            
            var _model = new WorkerSigninViewModel();
            
            //Get expiration date from checkin, show with next view
            _model.last_chkin_memberexpirationdate = _serv.getExpireDate(dwccardentry);         
            if (_model.last_chkin_memberexpirationdate < DateTime.Now) 
            {
                _model.memberexpired = true;
            } else {
                _model.memberexpired = false;
            }
            //Get picture from checkin, show with next view
            var checkin_image = _serv.getImage(dwccardentry);           
            
            if (checkin_image != null)
            {
                _model.last_chkin_image = checkin_image;
            }
            else
            {
                _model.last_chkin_image = new Image();
            }
            _model.dateforsignin = dateforsignin;                       //Pass date back for date checkin continuity
            ModelState.Remove("dwccardentry");                          // Clears previous entry from view for next iteration
            _model.workersignins = _serv.getView(_model.dateforsignin); //Get list of signins already checked in for the day
            return View(_model);
        }
        //
        // GET: /WorkerSignin/Delete/5
        public ActionResult Delete(int id)
        {
            _serv.DeleteWorkerSignin(id);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allWSI = _serv.GetWorkerSignins();
            IEnumerable<WorkerSignin> filteredWSI;
            IEnumerable<WorkerSignin> sortedWSI;
            //Search based on search-bar string
            if (!string.IsNullOrEmpty(param.todaysdate)) {
                allWSI = allWSI
                    .Where(jj => jj.dateforsignin.Date.Equals(Convert.ToDateTime(param.todaysdate)));
                      
            }
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredWSI = allWSI
                    .Where(p => p.dwccardnum.ToString().ContainsOIC(param.sSearch) ||
                                p.worker.Person.firstname1.ContainsOIC(param.sSearch) ||
                                p.worker.Person.firstname2.ContainsOIC(param.sSearch) ||
                                p.worker.Person.lastname1.ContainsOIC(param.sSearch) ||
                                p.worker.Person.lastname2.ContainsOIC(param.sSearch));
            }
            else
            {
                filteredWSI = allWSI;
            }                
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColName = param.sortColName();
            Func<WorkerSignin, string> orderingFunction = (p => sortColName == "dwccardnum" ? p.dwccardnum.ToString() :
                                                          sortColName == "firstname1" ? p.worker.Person.firstname1 :
                                                          sortColName == "firstname2" ? p.worker.Person.firstname2 :
                                                          sortColName == "lastname1" ? p.worker.Person.lastname1 :
                                                          sortColName == "lastname2" ? p.worker.Person.lastname2 :
                                                          sortColName == "dateforsignin" ? p.dateforsignin.ToString() :
                                                          p.dateupdated.ToString());
            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedWSI = filteredWSI.OrderBy(orderingFunction);
            else
                sortedWSI = filteredWSI.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayWSI = sortedWSI.Skip(param.iDisplayStart)
                                              .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayWSI
                         select new {  dwccardnum = p.dwccardnum,
                                       firstname1 = p.worker.Person.firstname1,
                                       firstname2 = p.worker.Person.firstname2,
                                       lastname1 = p.worker.Person.lastname1,
                                       lastname2 = p.worker.Person.lastname2, 
                                       dateforsignin = p.dateforsignin
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                //iTotalRecords = allWSI.Count(),
                //iTotalDisplayRecords = filteredWSI.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
