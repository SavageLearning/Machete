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
using NLog;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class PersonController : Controller
    { 
        private readonly IPersonService personService;
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "PersonController", "");
        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }
        //
        // GET: /Person/
        //
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index()
        {
            var persons = personService.GetPersons();
            return View(persons);
        }
        
        [HttpPost]
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk")]
        public ActionResult Index(Person person)
        {
            //TODO: finish search functionality
            return View(person);
        }

        //
        // GET: /Person/Create
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create()
        {
            var _model = new Person();
            ViewBag.Genders = Lookups.genders;
            return View(_model);
        } 

        //
        // POST: /Person/Create
        //
        [HttpPost]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(Person person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            person.createdby(this.User.Identity.Name);
            personService.CreatePerson(person);

            //log.Info("New Person created, ID={0}", person.ID);
            levent.Level = LogLevel.Info; levent.Message = "New Person created";
            levent.Properties["RecordID"] = person.ID; log.Log(levent);
            return RedirectToAction("Index");

        }
        //
        // GET: /Person/Edit/5
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Person person = personService.GetPerson(id);
            ViewBag.Genders = Lookups.genders;
            return View(person);
        }
        //
        // POST: /Person/Edit/5
        // TODO: catch exceptions, notify user
        //
        [HttpPost]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id, FormCollection collection)
        {
            var person = personService.GetPerson(id);
            
            if (TryUpdateModel(person))
            {
                person.updatedby(this.User.Identity.Name);
                personService.SavePerson();
                levent.Level = LogLevel.Info; levent.Message = "Person edited";
                levent.Properties["RecordID"] = person.ID; log.Log(levent);
                return RedirectToAction("Index");

            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = person.ID; log.Log(levent);
                return View(person);
            }
        }
        //
        // GET: /Person/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var person = personService.GetPerson(id);
            ViewBag.Genders = Lookups.genders;
            return View(person);

        }

        //
        // POST: /Person/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")] 
        public ActionResult Delete(int id, FormCollection collection)
        {
            personService.DeletePerson(id);
            levent.Level = LogLevel.Info; levent.Message = "Person deleted";
            levent.Properties["RecordID"] = id; log.Log(levent);
            return RedirectToAction("Index");
        }
    }
}
