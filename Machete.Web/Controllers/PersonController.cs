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
    public class PersonController : Controller
    {
        private readonly IPersonService personService;

        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }
        //
        // GET: /Person/

        public ActionResult Index()
        {
            var persons = personService.GetPersons();
            return View(persons);
        }



        //
        // GET: /Person/Create

        public ActionResult Create()
        {
            // TODO: ViewBag.Genders
            return View();
        } 

        //
        // POST: /Person/Create

        [HttpPost]
        public ActionResult Create(Person person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            person.datecreated = DateTime.Now;
            person.dateupdated = person.datecreated;
            personService.CreatePerson(person);
            return RedirectToAction("Index");

        }
        
        //
        // GET: /Person/Edit/5
 
        public ActionResult Edit(int id)
        {
            Person person = personService.GetPerson(id);
            return View(person);
        }

        //
        // POST: /Person/Edit/5
        // TODO: catch exceptions, notify user

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var person = personService.GetPerson(id);
            if (TryUpdateModel(person))
            {
                personService.SavePerson();
                return RedirectToAction("Index");
            }
            else return View(person); 
        }
        //
        // GET: /Person/Delete/5
        public ActionResult Delete(int id)
        {
            var person = personService.GetPerson(id);
            return View(person);

        }

        //
        // POST: /Person/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //TODO: privilege check
            personService.DeletePerson(id);
            //var persons = personService.GetPersons();
            // return PartialView("CategoryList", categories);
            return RedirectToAction("Index");
        }
    }
}
