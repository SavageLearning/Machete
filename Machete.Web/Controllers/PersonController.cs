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
using Machete.Web.ViewModel;
using Machete.Web.Models;
using System.Web.Routing;
using System.Data.Entity.Infrastructure;

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
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            ViewBag.Genders = Lookups.gender(CI.TwoLetterISOLanguageName);
        }
        #region Index
        //
        // GET: /Person/
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk, Check-in, User")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allpersons = personService.GetPersons(true);
            IEnumerable<Person> filteredPersons;
            IEnumerable<Person> sortedPersons;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPersons = personService.GetPersons(true)
                    .Where(p => p.active.ToString().ContainsOIC(param.sSearch) ||
                                p.firstname1.ContainsOIC(param.sSearch) ||
                                p.firstname2.ContainsOIC(param.sSearch) ||
                                p.lastname1.ContainsOIC(param.sSearch) ||
                                p.lastname2.ContainsOIC(param.sSearch));
            }
            else
            {
                filteredPersons = allpersons;
            }
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Person, string> orderingFunction = (p => sortColIdx == 2 ? p.active.ToString() : 
                                                          sortColIdx == 3 ? p.firstname1 :
                                                          sortColIdx == 4 ? p.firstname2 :
                                                          sortColIdx == 5 ? p.lastname1 :
                                                          sortColIdx == 6 ? p.lastname2 :
                                                          sortColIdx == 7 ? p.phone :
                                                          sortColIdx == 8 ? p.dateupdated.ToString() :
                                                          p.Updatedby);
            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedPersons = filteredPersons.OrderBy(orderingFunction);
            else
                sortedPersons = filteredPersons.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayPersons = sortedPersons.Skip(param.iDisplayStart)
                                              .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayPersons
                         select new[] { "/Person/Edit/" + Convert.ToString(p.ID),
                                        p.firstname1 + ' ' + p.lastname1,
                                        Convert.ToString(p.active), 
                                        p.firstname1, 
                                        p.firstname2, 
                                        p.lastname1, 
                                        p.lastname2, 
                                        p.phone, 
                                        Convert.ToString(p.dateupdated), 
                                        p.Updatedby 
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allpersons.Count(),
                iTotalDisplayRecords = filteredPersons.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Create
        //
        // GET: /Person/Create
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")] 
        public ActionResult Create()
        {
            var _model = new Person();
            _model.gender = Lookups.genderDefault;
            _model.active = true;
            return PartialView(_model);
        }
        //
        // POST: /Person/Create
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(Person person, string userName)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(person);
            }
            personService.CreatePerson(person, userName);
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        //
        // GET: /Person/Edit/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")] 
        public ActionResult Edit(int id)
        {
            Person person = personService.GetPerson(id);
            return PartialView(person);
        }
        //
        // POST: /Person/Edit/5
        // TODO: catch exceptions, notify user
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")] 
        public ActionResult Edit(int id, FormCollection collection, string userName)
        {
            Person person = personService.GetPerson(id);
            
            try {
                if (TryUpdateModel(person))
                {
                    personService.SavePerson(person, userName);
                    return RedirectToAction("Index");
                }
                else
                {
                    levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                    levent.Properties["RecordID"] = person.ID; log.Log(levent);
                    return PartialView(person);
                }
            }
            //catch (DbUpdateConcurrencyException ex)
            catch
            {

                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = person.ID; log.Log(levent);
                return PartialView(person);
            }
        }
        #endregion
        #region View
        //
        //GET: /Person/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk, User")]
        public ActionResult View(int id)
        {
            Person person = personService.GetPerson(id);
            return View(person);
        }
        #endregion

        #region Delete
        //
        // GET: /Person/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var person = personService.GetPerson(id);
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
