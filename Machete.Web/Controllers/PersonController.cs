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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class PersonController : MacheteController
    {
        private readonly IPersonService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        private readonly IModelBindingAdaptor _adaptor;

        public PersonController(
            IPersonService pServ,
            IDefaults def,
            IMapper map,
            IModelBindingAdaptor adaptor)
        {
            serv = pServ;
            this.map = map;
            _adaptor = adaptor;
            this.def = def;
        }

        // GET /Person/Index
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records            
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            dataTableResult<PersonList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(e => map.Map<PersonList, ViewModel.PersonList>(e))
                .AsEnumerable();
            return Json(new {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }

        // GET /Person/Create
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public async Task<ActionResult> Create()
        {
            var p = map.Map<Person, ViewModel.Person>(new Person {
                gender = def.getDefaultID(LCategory.gender),
                active = true
            });
            p.def = def;
            return await Task.Run(() => PartialView("Create", p));
        }

        // POST /Person/Create
        [HttpPost, UserNameFilter]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public async Task<ActionResult> Create(Person person, string userName)
        {
            ModelState.ThrowIfInvalid();
            
            if (await _adaptor.TryUpdateModelAsync(this, person)) {
                var newperson = serv.Create(person, userName);
                var result = map.Map<Person, ViewModel.Person>(newperson);
                return Json(new {
                    sNewRef = result.tabref,
                    sNewLabel = result.tablabel,
                    iNewID = result.ID
                });
            } else {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")] 
        public async Task<ActionResult> Edit(int id)
        {
            var person = serv.Get(id);
            var model = map.Map<Person, ViewModel.Person>(person);
            model.def = def;
            return await Task.Run(() => PartialView("Edit", model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public async Task<ActionResult> Edit(int id, string userName)
        {
            ModelState.ThrowIfInvalid();

            var person = serv.Get(id);
            
            var modelIsValid = await _adaptor.TryUpdateModelAsync(this, person);
            if (modelIsValid) {
                serv.Save(person, userName);
                return Json(new {
                    status = "OK"
                });
            } else {
                return Json(new {status = "not OK"});
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public ActionResult View(int id)
        {
            Person person = serv.Get(id);
            var m = map.Map<Person, ViewModel.Person>(person);
            m.def = def;
            return View(m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator")] 
        public async Task<ActionResult> Delete(int id, string user)
        {
            serv.Delete(id, user);

            return await Task.Run(() => Json(new
            {
                status = "OK",
                deletedID = id
            }));
        }

        private List<Dictionary<string, string>> DuplicatePersons(string firstname, string lastname, string phone)
        {
            //Get all the records            
            IEnumerable<Person> list = serv.GetAll();
            var peopleFound = new List<Dictionary<string, string>>();
            firstname = firstname.Replace(" ", "");
            lastname = lastname.Replace(" ", "");
            phone = string.IsNullOrEmpty(phone) ? "x" : phone;
 
            foreach (var person in list)
            {
                var personFirstName = person.firstname1.Replace(" ", "");
                var personLastName = person.lastname1.Replace(" ", "");
                var personPhone = string.IsNullOrEmpty(person.phone) ? "y" : person.phone;

               //checking if person already exists in dbase
                if ((personFirstName.Equals(firstname, StringComparison.CurrentCultureIgnoreCase)
                    && personLastName.Equals(lastname, StringComparison.CurrentCultureIgnoreCase))
                    || (personFirstName.Equals(firstname, StringComparison.CurrentCultureIgnoreCase)
                        && person.phone == phone)
                    || (personLastName.Equals(lastname, StringComparison.CurrentCultureIgnoreCase)
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
            return Json(new { duplicates = duplicateFound });
        }
    }
}
