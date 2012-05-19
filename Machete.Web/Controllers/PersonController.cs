using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
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
    public class PersonController : MacheteController
    { 
        private readonly IPersonService personService;
        System.Globalization.CultureInfo CI;
        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records            
            dTableList<Person> personView = personService.GetIndexView(new viewOptions() {
                CI = CI,
                search = param.sSearch,
                status = string.IsNullOrEmpty(param.searchColName("status")) ? (int?)null : Convert.ToInt32(param.searchColName("status")),
                orderDescending = param.sSortDir_0 == "asc" ? false : true,
                displayStart = param.iDisplayStart,
                displayLength = param.iDisplayLength,
                sortColName = param.sortColName()
            });

            var result = from p in personView.query select new
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")] 
        public ActionResult Create()
        {
            var _model = new Person();
            _model.gender = Lookups.genderDefault;
            _model.active = true;
            return PartialView(_model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(Person person, string userName)
        {
            Person newperson = null;
            UpdateModel(person);
            newperson = personService.Create(person, userName);
            
            return Json(new
            {
                sNewRef = _getTabRef(newperson),
                sNewLabel = _getTabLabel(newperson),
                iNewID = (newperson == null ? 0 : newperson.ID)
            },
            JsonRequestBehavior.AllowGet);
        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")] 
        public ActionResult Edit(int id)
        {
            Person person = personService.Get(id);
            return PartialView(person);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")] 
        public ActionResult Edit(int id, string userName)
        {
            Person person = personService.Get(id);          
            UpdateModel(person);
            personService.Save(person, userName);
            return Json(new
            {
                status = "OK"
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            Person person = personService.Get(id);
            return View(person);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator")] 
        public ActionResult Delete(int id, string user)
        {
            personService.Delete(id, user);

            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
