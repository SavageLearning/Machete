#region COPYRIGHT
// File:     PersonController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Models;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class PersonController : MacheteController
    { 
        private readonly IPersonService personService;
        private readonly ILookupCache lcache;
        System.Globalization.CultureInfo CI;

        public PersonController(IPersonService personService, ILookupCache _lcache)
        {
            this.personService = personService;
            this.lcache = _lcache;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records            
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Person> list = personService.GetIndexView(vo); 

            var result = from p in list.query select new
            {
                tabref = "/Person/Edit/" + Convert.ToString(p.ID),
                tablabel = p.firstname1 + ' ' + p.lastname1,
                dwccardnum = p.Worker == null ? "" : p.Worker.dwccardnum.ToString(),
                active = p.active ? Shared.True : Shared.False,
                status = p.active,
                workerStatus = p.Worker == null ? "Not a worker" : lcache.textByID(p.Worker.memberStatus, CI.TwoLetterISOLanguageName),
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
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")] 
        public ActionResult Create()
        {
            var _model = new Person();
            _model.gender = Lookups.getDefaultID(LCategory.gender);
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
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
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")] 
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
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")] 
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
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
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

 

        private List<Dictionary<string, string>> DuplicatePersons(string firstname, string lastname, string phone)
        {
            //Get all the records            
            IEnumerable<Person> list = personService.GetAll();
            var peopleFound = new List<Dictionary<string, string>>();
            firstname = firstname.Replace(" ", "");
            lastname = lastname.Replace(" ", "");
            phone = string.IsNullOrEmpty(phone) ? "x" : phone;
 
            foreach (var person in list)
            {
                var person_FirstName = person.firstname1.Replace(" ", "");
                var person_LastName = person.lastname1.Replace(" ", "");
                var person_Phone = string.IsNullOrEmpty(person.phone) ? "y" : person.phone;

               //checking if person already exists in dbase
                if ((person_FirstName.Equals(firstname, StringComparison.CurrentCultureIgnoreCase)
                    && person_LastName.Equals(lastname, StringComparison.CurrentCultureIgnoreCase))
                    || (person_FirstName.Equals(firstname, StringComparison.CurrentCultureIgnoreCase)
                        && person.phone == phone)
                    || (person_LastName.Equals(lastname, StringComparison.CurrentCultureIgnoreCase)
                        && person.phone == phone))
                {
                    var personFound = new Dictionary<string, string>();
                    personFound.Add("First Name", person.firstname1);
                    personFound.Add("Last Name", person.lastname1);
                    personFound.Add("Phone", person.phone);
                   
                    peopleFound.Add(personFound);
                }
            }
            return peopleFound;
        }

        [AllowAnonymous]
        public JsonResult GetDuplicates(string firstname, string lastname, string phone)
        {
            var duplicateFound = DuplicatePersons(firstname, lastname, phone);
            return Json(new { duplicates = duplicateFound }, JsonRequestBehavior.AllowGet);
        }
    }
}
