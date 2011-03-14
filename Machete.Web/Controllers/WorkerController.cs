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

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerController : Controller
    {
        private readonly IWorkerService workerService;
        private readonly IPersonService personService;

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService)
        {
            this.workerService = workerService;
            this.personService = personService;
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
            _model.person.Createdby = this.User.Identity.Name;
            _model.person.Updatedby = this.User.Identity.Name;
            _model.worker.Createdby = this.User.Identity.Name;
            _model.worker.Updatedby = this.User.Identity.Name;
            
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
            return View(_model);
        }
        //
        // POST: /Worker/Edit/5
        // TODO: catch exceptions, notify user
        // TODO: disable button
        //
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator")] 
        public ActionResult Edit(int id, FormCollection collection)
        {
            var worker = workerService.GetWorker(id);
            worker.dateupdated = DateTime.Now;
            worker.Updatedby = this.User.Identity.Name;
            if (TryUpdateModel(worker))
            {
                workerService.SaveWorker();
                return RedirectToAction("Index");
               
            }
            else return View(worker); 
        }
        //
        // GET: /Worker/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var _worker = workerService.GetWorker(id);
            var _model = new WorkerViewModel();
            _model.worker = _worker;
            _model.person = _worker.Person;
            return View(_model);

        }

        //
        // POST: /Worker/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")] 
        public ActionResult Delete(int id, FormCollection collection)
        {
            workerService.DeleteWorker(id);
            return RedirectToAction("Index");
        }
    }
}