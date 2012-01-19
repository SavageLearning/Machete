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
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //Get all the records            
            ServiceIndexView<Person> personView = personService.GetIndexView(
                CI,
                param.sSearch,
                string.IsNullOrEmpty(param.sSearch_2) ? (int?)null : Convert.ToInt32(param.sSearch_2),
                string.IsNullOrEmpty(param.sSearch_5) ? (int?)null : Convert.ToInt32(param.sSearch_5),
                param.sSortDir_0 == "asc" ? false : true,
                param.iDisplayStart,
                param.iDisplayLength,
                param.sortColName()
                );

            var result = from p in personView.query
                         select new
                         {
                             tabref = "/Person/Edit/" + Convert.ToString(p.ID),
                             tablabel = p.firstname1 + ' ' + p.lastname1,
                             active = Convert.ToString(p.active),
                             firstname1 = p.firstname1,
                             firstname2 = p.firstname2,
                             lastname1 = p.lastname1,
                             lastname2 = p.lastname2,
                             phone = p.phone,
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby,
                             recordid = Convert.ToString(p.ID)
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = personView.totalCount,
                iTotalDisplayRecords = personView.filteredCount,
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
            Person newperson = null;
            if (ModelState.IsValid)
            {
                newperson = personService.CreatePerson(person, userName);
            }
            
            return Json(new
            {
                sNewRef = _getTabRef(newperson),
                sNewLabel = _getTabLabel(newperson),
                iNewID = (newperson == null ? 0 : newperson.ID)
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
        private string _getTabRef(Person per)
        {
            if (per != null) return "/Person/Edit/" + Convert.ToString(per.ID);           
            else return null;            
        }

        private string _getTabLabel(Person per)
        {
            if (per != null) return per.fullName();
            else return null;
        }
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
            string status = null;
            
            if (TryUpdateModel(person))
            {
                try
                {
                    personService.SavePerson(person, userName);
                }
                catch (Exception e)
                {
                    status = RootException.Get(e, "WorkerService");
                }
            }
            else
            {
                status = "Controller UpdateModel failure on recordtype: person";
                levent.Level = LogLevel.Error; levent.Message = status;
                levent.Properties["RecordID"] = person.ID; log.Log(levent);
            }
            return Json(new
            {
                status = status ?? "OK"
            },
            JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region View
        //
        //GET: /Person/View/5
        //
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
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
            string status = null;
            try
            {
                personService.DeletePerson(id, user);
            }
            catch (Exception e)
            {
                status = RootException.Get(e, "WorkerService");
            }
            return Json(new
            {
                status = status ?? "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
