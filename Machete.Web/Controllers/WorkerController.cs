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
using Machete.Web.ViewModel;
using Microsoft.Web.Mvc;
using System.Web.Security;
using Elmah;
using NLog;
using Machete.Web.Helpers;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerController : Controller
    {
        private readonly IWorkerService workerService;
        //private readonly IPersonService personService;
        private readonly IImageService imageServ;
        private bool foo1;
        private bool foo2;
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerController", "");

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService,
                                IImageService  imageServ)
        {
            this.workerService = workerService;
            //this.personService = personService;
            this.imageServ = imageServ;
            ViewBag.races = Lookups.races;
            ViewBag.languages = Lookups.languages;
            ViewBag.neighborhoods = Lookups.neighborhoods;
            ViewBag.incomes = Lookups.incomes;
            ViewBag.maritalstatus = Lookups.maritalstatuses;
            ViewBag.Genders = Lookups.genders;
        }
        //
        // GET: /Worker/Index
        //
        #region index
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index()
        {
            ViewBag.show_inactive = false;
            var workers = workerService.GetWorkers(false);
            return View(workers);
        }
        [HttpPost]
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index(bool show_inactive)
        {
            var workers = workerService.GetWorkers(show_inactive);
            ViewBag.show_inactive = show_inactive;
            return View(workers);
        }
        #endregion
        //
        // GET: /Worker/Create
        //
        #region create
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create()
        {
            var _model = new WorkerViewModel();
            _model.person = new Person();
            _model.worker = new Worker();
            //Link person to work for EF to save
            _model.worker.Person = _model.person;
            _model.person.Worker = _model.worker;
            return View(_model);
        } 
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(WorkerViewModel _model, string userName)
        {
            if (!ModelState.IsValid)
            {
                levent.Level = LogLevel.Error; levent.Message = "ModelState invalid";
                //TODO will this log event work without record ID?
                //levent.Properties["RecordID"] = id; 
                log.Log(levent);
                return View(_model);
            }
            _model.worker.Person = _model.person;
            _model.person.Worker = _model.worker;

            workerService.CreateWorker(_model.worker, userName);
            return RedirectToAction("Index");
        }
        #endregion
        //
        // GET: /Worker/Edit/5
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker _worker = workerService.GetWorker(id);
            WorkerViewModel _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            if (_model.worker.ImageID != null)
            {
                _model.image = imageServ.GetImage((int)_worker.ImageID);
            }
            else
            {
                _model.image = new Image();
            }
            return View(_model);
        }
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")]
        public ActionResult Edit(int id, WorkerViewModel _model, string userName)
        {
            Worker worker = workerService.GetWorker(id);
            Person person = worker.Person;
            // TODO: catch exceptions, notify user
            foo1 = TryUpdateModel(worker);
            foo2 = TryUpdateModel(person);
            if (foo1 && foo2)
            {
                workerService.SaveWorker(worker, userName);
                return RedirectToAction("Index");
            }
            else 
            {
                levent.Level = LogLevel.Error; levent.Message = "ModelState invalid";
                levent.Properties["RecordID"] = id; log.Log(levent);
                return View(_model);
            }
        }
        //
        //GET: /Worker/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, User")]
        public ActionResult View(int id)
        {
            Worker _worker = workerService.GetWorker(id);
            WorkerViewModel _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            if (_model.worker.ImageID != null)
            {
                _model.image = imageServ.GetImage((int)_worker.ImageID);
            }
            else
            {
                _model.image = new Image();
            }
            return View(_model);
        }
        //
        // GET: /Worker/Delete/5
        #region delete
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var _worker = workerService.GetWorker(id);
            var _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            return View(_model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")] 
        public ActionResult Delete(int id, FormCollection collection)
        {
            workerService.DeleteWorker(id);
            levent.Level = LogLevel.Info; levent.Message = "Worker deleted";
            levent.Properties["RecordID"] = id; log.Log(levent);
            return RedirectToAction("Index");
        }
        #endregion
    }
}