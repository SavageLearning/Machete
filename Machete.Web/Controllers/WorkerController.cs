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

namespace Machete.Web.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkerService workerService;
        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }
        //
        // GET: /Worker/
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk, User")]
        public ActionResult Index()
        {
            var workers = workerService.GetWorkers();
            return View(workers);
        }
        //
        // GET: /Worker/Create
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create()
        {
            // TODO: ViewBag.Genders
            return View();
        } 

        //
        // POST: /Worker/Create
        //
        [HttpPost]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(Worker worker)
        {
            if (!ModelState.IsValid)
            {
                return View(worker);
            }
            worker.datecreated = DateTime.Now;
            worker.dateupdated = worker.datecreated;
            workerService.CreateWorker(worker);
            return RedirectToAction("Index");

        }
        //
        // GET: /Worker/Edit/5
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker worker = workerService.GetWorker(id);
            return View(worker);
        }
        //
        // POST: /Worker/Edit/5
        // TODO: catch exceptions, notify user
        // TODO: disable button
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