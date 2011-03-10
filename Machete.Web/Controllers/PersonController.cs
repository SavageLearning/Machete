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
    [HandleError]
    public class PersonController : Controller
    { 
        private readonly IPersonService personService;

        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }
        //
        // GET: /Person/
        //
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk, User")]
        public ActionResult Index()
        {
            var persons = personService.GetPersons();
            return View(persons);
        }
        //TODO: finish search functionality
        [HttpPost]
        [Authorize(Roles = "User, Manager, Administrator, Check-in, PhoneDesk, User")]
        public ActionResult Index(Person person)
        {

            return View(person);
        }

        //
        // GET: /Person/Create
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create()
        {
            var _model = new Person();
            ViewBag.Genders = Lookups.gender;
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
            person.datecreated = DateTime.Now;
            person.dateupdated = person.datecreated;
            person.Createdby = this.User.Identity.Name;
            person.Updatedby = this.User.Identity.Name;
            personService.CreatePerson(person);
            return RedirectToAction("Index");

        }
        //
        // GET: /Person/Edit/5
        //
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id)
        {
            Person person = personService.GetPerson(id);
            ViewBag.Genders = Lookups.gender;
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
            person.dateupdated = DateTime.Now;
            person.Updatedby = this.User.Identity.Name;
            if (TryUpdateModel(person))
            {
                personService.SavePerson();
                return RedirectToAction("Index");
               
            }
            else return View(person); 
        }
        //
        // GET: /Person/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var person = personService.GetPerson(id);
            ViewBag.Genders = Lookups.gender;
            return View(person);

        }

        //
        // POST: /Person/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")] 
        public ActionResult Delete(int id, FormCollection collection)
        {
            personService.DeletePerson(id);
            return RedirectToAction("Index");
        }
    }
}
