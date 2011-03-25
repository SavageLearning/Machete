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
using Machete.Web.Helpers;
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
        #region Index
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
            throw new NotImplementedException();
            //return View(person);
        }
        #endregion

        #region Create
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
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Create(Person person, string userName)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            personService.CreatePerson(person, userName);
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
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
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Administrator")] 
        public ActionResult Edit(int id, FormCollection collection, string userName)
        {
            var person = personService.GetPerson(id);
            
            if (TryUpdateModel(person))
            {
                personService.SavePerson(person, userName);
                return RedirectToAction("Index");
            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = person.ID; log.Log(levent);
                return View(person);
            }
        }
        #endregion

        #region Delete
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

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator")] 
        public ActionResult Delete(int id, FormCollection collection, string user)
        {
            personService.DeletePerson(id, user);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
