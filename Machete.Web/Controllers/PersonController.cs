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
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class PersonController : MacheteController
    { 
        private readonly IPersonService personService;
        private readonly ILookupCache lcache;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;

        public PersonController(
            IPersonService personService, 
            ILookupCache _lcache,
            IDefaults def,
            IMapper map)
        {
            this.personService = personService;
            this.lcache = _lcache;
            this.map = map;
            this.def = def;
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
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<DTO.PersonList> list = personService.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.PersonList, ViewModel.PersonList>(e)
                ).AsEnumerable();
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
            var p = map.Map<Domain.Person, ViewModel.Person>(new Domain.Person()
            {
                gender = def.getDefaultID(LCategory.gender),
                active = true
            });
            p.def = def;
            return PartialView("Create", p);
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
            UpdateModel(person);
            var newperson = personService.Create(person, userName);
            var result = map.Map<Domain.Person, ViewModel.Person>(newperson);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")] 
        public ActionResult Edit(int id)
        {
            var p = personService.Get(id);
            var m = map.Map<Domain.Person, ViewModel.Person>(p);
            m.def = def;
            return PartialView("Edit", m);
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
                    personFound.Add("ID", person.ID.ToString()); 
                   
                    peopleFound.Add(personFound);
                }
            }
            return peopleFound;
        }

        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public JsonResult GetDuplicates(string firstname, string lastname, string phone)
        {
            var duplicateFound = DuplicatePersons(firstname, lastname, phone);
            return Json(new { duplicates = duplicateFound }, JsonRequestBehavior.AllowGet);
        }
    }
}
