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
        private readonly IWorkerSigninService workerSigninService;
        private MacheteContext DB;
        
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerSigninController", "");


        public WorkerSigninController(IWorkerSigninService workerSigninService)
        {
            this.workerSigninService = workerSigninService;
            DB = new MacheteContext();
        }

        private IEnumerable<WorkerSigninView> getView(DateTime date)
        {
            return from s in DB.WorkerSignins.AsEnumerable()
                   join w in DB.Workers.AsEnumerable() on s.dwccardnum equals w.dwccardnum into outer
                   from o in outer.DefaultIfEmpty()
                   join p in DB.Persons.AsEnumerable() on o.ID equals p.ID
                   where s.dateforsignin.DayOfYear == date.DayOfYear
                   select new WorkerSigninView() { person = p, signin = s };
        }
        //
        // GET: /WorkerSignin/

        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index()
        {        
            var _model = new WorkerSigninViewModel();
            if (ViewBag.dateforsignin == null) _model.dateforsignin = DateTime.Today;
            else _model.dateforsignin = ViewBag.dateforsignin;
            _model.workersignins = getView(_model.dateforsignin);
            return View(_model);
        }

        [HttpPost]
        public ActionResult Index(int dwccardentry, DateTime dateforsignin)
        {

            var _signin = new WorkerSignin();
            _signin.dwccardnum = dwccardentry;
            _signin.dateforsignin = dateforsignin;
            _signin.createdby(this.User.Identity.Name);
            if (DB.WorkerSignins.AsEnumerable().Count(s => s.dateforsignin == dateforsignin && s.dwccardnum == dwccardentry) == 0)
            {
                DB.WorkerSignins.Add(_signin);
                DB.SaveChanges();
            }   
            //try
            //{


                var _model = new WorkerSigninViewModel();
                _model.dateforsignin = dateforsignin;
                ModelState.Remove("dwccardentry");
                _model.workersignins = getView(_model.dateforsignin);
                return View(_model);
            //}
            //catch
            //{
            //    return View();
            //}
        }
        

        //
        // GET: /WorkerSignin/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /WorkerSignin/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /WorkerSignin/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /WorkerSignin/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /WorkerSignin/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
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
            var del = DB.WorkerSignins.Find(id);
            DB.WorkerSignins.Remove(del);
            DB.SaveChanges();
            //workerSigninService.DeleteWorkerSignin(id);
            return RedirectToAction("Index");
        }

        //
        // POST: /WorkerSignin/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
