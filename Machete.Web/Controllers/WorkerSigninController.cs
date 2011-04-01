using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
using Machete.Helpers;
using Machete.Web.ViewModel;
using NLog;
using Machete.Domain;
using Machete.Data;

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
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index()
        {        
            WorkerSigninViewModel _model = new WorkerSigninViewModel();
            if (ViewBag.dateforsignin == null) _model.dateforsignin = DateTime.Today;
            else _model.dateforsignin = ViewBag.dateforsignin;
            _model.workersignins = _serv.getView(_model.dateforsignin);
            _model.last_chkin_image = new Image();
            return View(_model);
        }

        [HttpPost]
        public ActionResult Index(int dwccardentry, DateTime dateforsignin)
        {

            var _signin = new WorkerSignin();
            _signin.dwccardnum = dwccardentry;
            _signin.dateforsignin = dateforsignin;
            _serv.CreateWorkerSignin(_signin, this.User.Identity.Name);

            try
            {
                //Get picture from checkin, show with next view
                var checkin_image = _serv.getImage(dwccardentry);
                var _model = new WorkerSigninViewModel();
                if (checkin_image != null)
                {
                    _model.last_chkin_image = checkin_image;
                }
                else
                {
                    _model.last_chkin_image = new Image();
                }
                _model.dateforsignin = dateforsignin;
                ModelState.Remove("dwccardentry"); // Clears previous entry from view for next iteration
                _model.workersignins = _serv.getView(_model.dateforsignin);
                return View(_model);
            }
            catch
            {
                return View();
            }
        }
        //
        // GET: /WorkerSignin/Delete/5
 
        public ActionResult Delete(int id)
        {
            _serv.DeleteWorkerSignin(id);
            return RedirectToAction("Index");
        }
    }
}
