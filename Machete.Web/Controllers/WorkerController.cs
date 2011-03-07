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

namespace Machete.Web.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkerService workerService;
        private readonly IRaceService raceService;
        private readonly IPersonService personService;
        private readonly ILangService langService;
        private readonly IHoodService hoodService;
        private readonly IIncomeService incomeService;
        private SelectListItem[] maritalstatuslist = new[]
            {
                new SelectListItem {Value = "S", Text = "Single", Selected=true},
                new SelectListItem {Value = "M", Text = "married"},
                new SelectListItem {Value = "D", Text = "Divorced"}
            }; 

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService, 
                                IRaceService raceService,
                                ILangService langService,
                                IHoodService hoodService,
                                IIncomeService incomeService
            )
        {
            this.workerService = workerService;
            this.raceService = raceService;
            this.personService = personService;
            this.langService = langService;
            this.hoodService = hoodService;
            this.incomeService = incomeService;
        }
        //
        // GET: /Worker/Index
        //
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index()
        {
            var workers = workerService.GetWorkers();
            return View(workers);
        }
        //
        // GET: /Worker/Create
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create()
        {
            var _model = new WorkerViewModel();
            _model.person = new Person();
            _model.worker = new Worker();
            //Link person to work for EF to save
            _model.worker.Person = _model.person;
            _model.person.Worker = _model.worker;
            ViewBag.races = raceService.Lookup();
            ViewBag.languages = langService.Lookup();
            ViewBag.neighborhoods = hoodService.Lookup();
            ViewBag.incomes = incomeService.Lookup();
            ViewBag.maritalstatus = maritalstatuslist;
            return View(_model);
        } 

        //
        // POST: /Worker/Create
        //
        [HttpPost]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(WorkerViewModel _model)
        {
            if (!ModelState.IsValid)
            {
                return View(_model);
            }
            _model.worker.Person = _model.person;
            _model.person.Worker = _model.worker;
            DateTime rightnow = DateTime.Now;
            _model.person.datecreated = rightnow;
            _model.person.dateupdated = rightnow;
            _model.worker.datecreated = rightnow;
            _model.worker.dateupdated = rightnow;
            //TODO: get user Guid
            _model.person.Createdby = Guid.Empty;
            _model.person.Updatedby = Guid.Empty;
            _model.worker.Createdby = Guid.Empty;
            _model.worker.Updatedby = Guid.Empty;
            
            workerService.CreateWorker(_model.worker);
            return RedirectToAction("Index");
        }
        //
        // GET: /Worker/Edit/5
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker _worker = workerService.GetWorker(id);
            var _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            ViewBag.races = raceService.Lookup();
            ViewBag.languages = langService.Lookup();
            ViewBag.neighborhoods = hoodService.Lookup();
            ViewBag.incomes = incomeService.Lookup();
            ViewBag.maritalstatus = new[]
            {
                new SelectListItem {Value = "S", Text = "Single", Selected=true},
                new SelectListItem {Value = "M", Text = "married"},
                new SelectListItem {Value = "D", Text = "Divorced"}
            };
            return View(_model);
        }
        //
        // POST: /Worker/Edit/5
        // TODO: catch exceptions, notify user
        // TODO: disable button
        // TODO: update edit-post
        //
        [HttpPost]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id, FormCollection collection)
        {
            var worker = workerService.GetWorker(id);
            worker.dateupdated = DateTime.Now;
            if (TryUpdateModel(worker))
            {
                workerService.SaveWorker();
                return RedirectToAction("Index");
               
            }
            else return View(worker); 
        }
        //
        // GET: /Worker/Delete/5
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Delete(int id)
        {
            var worker = workerService.GetWorker(id);
            return View(worker);

        }

        //
        // POST: /Worker/Delete/5

        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")] 
        public ActionResult Delete(int id, FormCollection collection)
        {
            //TODO: privilege check
            workerService.DeleteWorker(id);
            return RedirectToAction("Index");
        }
    }
}