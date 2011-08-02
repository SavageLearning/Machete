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
                iTotalRecords = allWSI.Count(),
                iTotalDisplayRecords = filteredWSI.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        //[Authorize(Roles = "Manager, Administrator, Check-in")]
        //public ActionResult Report()
        //{
        //    var ReportPath = Server.MapPath("~/RDLC/WorkerSigninsDaily.rdlc");

        //    object Model =  _serv.getView(DateTime.Today);

        //    RenderReport(ReportPath, Model);

        //    return View();
        //}

        //private void RenderReport(string ReportPath, object Model)
        //{

        //    var localReport = new LocalReport { ReportPath = ReportPath };

        //    //Give the collection a name (EmployeeCollection) so that we can reference it in our report designer
        //    var reportDataSource = new ReportDataSource("SigninDaily", Model);
        //    localReport.DataSources.Add(reportDataSource);

        //    var reportType = "PDF";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    //The DeviceInfo settings should be changed based on the reportType
        //    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
        //    var deviceInfo =
        //        string.Format("<DeviceInfo><OutputFormat>{0}</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight><MarginTop>0.5in</MarginTop><MarginLeft>1in</MarginLeft><MarginRight>1in</MarginRight><MarginBottom>0.5in</MarginBottom></DeviceInfo>", reportType);

        //    Warning[] warnings;
        //    string[] streams;

        //    //Render the report
        //    var renderedBytes = localReport.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);

        //    //Clear the response stream and write the bytes to the outputstream
        //    //Set content-disposition to "attachment" so that user is prompted to take an action
        //    //on the file (open or save)
        //    Response.Clear();
        //    Response.ContentType = mimeType;
        //    Response.AddHeader("content-disposition", "attachment; filename=foo." + fileNameExtension);
        //    Response.BinaryWrite(renderedBytes);
        //    Response.End();
        //}




    }
}
