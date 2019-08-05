#region COPYRIGHT
// File:     EmployerController.cs
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
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Employer = Machete.Domain.Employer;

namespace Machete.Web.Controllers
{

        public class EmployerController : MacheteController
    {
        private readonly IEmployerService _serv;
        private readonly IDefaults _defaults;
        private readonly IMapper _map;
        private readonly IModelBindingAdaptor _adaptor;

        public EmployerController(
            IEmployerService employerService, 
            IDefaults defaults,
            IMapper map,
            IModelBindingAdaptor adaptor) {
            _serv = employerService;
            _map = map;
            _adaptor = adaptor;
            _defaults = defaults;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.idPrefix = "employer";
        }

        // GET: /Employer/Index
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() => View());
        }
        
        // GET: /Employer/AjaxHandler
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            dataTableResult<EmployersList> list;

            try {
                var vo = _map.Map<jQueryDataTableParam, viewOptions>(param);
                list = _serv.GetIndexView(vo);
            }
            catch (Exception ex) {
                throw ex; // TODO Chaim plz
            }
            //return what's left to datatables
            var result = list.query
                .Select(e => _map.Map<Service.DTO.EmployersList, ViewModel.EmployerList>(e))
                .AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }

        // GET: /Employer/Create
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Create()
        {
            var model = _map.Map<Employer, ViewModel.Employer>(new Employer
            {
                active = true,
                blogparticipate = false,
                referredby = _defaults.getDefaultID(LCategory.emplrreference)
            });
            model.def = _defaults;
            return await Task.Run(() => PartialView("Create", model));
        }

        // POST: /Employer/Create
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<JsonResult> Create(Employer employer, string userName)
        {
            ModelState.ThrowIfInvalid();
            
            var modelIsValid = await _adaptor.TryUpdateModelAsync(this, employer);
            if (modelIsValid) {
                var saved = _serv.Create(employer, userName);
                var result = _map.Map<Employer, ViewModel.Employer>(saved);
                return Json(new {
                    sNewRef = result.tabref,
                    sNewLabel = result.tablabel,
                    iNewID = result.ID,
                    jobSuccess = true
                });
            } else {
                return Json(new { jobSuccess = false });
            }
        }

        /// <summary>
        /// GET: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<ActionResult> Edit(int id)
        {
            var e = _serv.Get(id);
            var m = _map.Map<Employer, ViewModel.Employer>(e);
            m.def = _defaults;
            return await Task.Run(() => PartialView("Edit", m));
        }

        /// <summary>
        /// POST: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public async Task<JsonResult> Edit(int id, string userName)
        {
            ModelState.ThrowIfInvalid();

            var employer = _serv.Get(id);

            var modelIsValid = await _adaptor.TryUpdateModelAsync(this, employer);
            if (modelIsValid) {
                _serv.Save(employer, userName);
                return Json(new { jobSuccess = true });
            } else {
                return Json(new { jobSuccess = false });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator")]
        public JsonResult Delete(int id, string userName)
        {
            _serv.Delete(id, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            });
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult GetDuplicates(string name, string address, string phone, string city, string zipcode)
        {
            var name1 = name;
            var address1 = address;
            var phone1 = phone;
            var city1 = city;
            var zipcode1 = zipcode;
            //Get all the records            
            var list = _serv.GetAll();
            var employersFound = new List<Dictionary<string, string>>();
            
            name1 = name1.Replace(" ", "");
            address1 = address1.Replace(" ", "");
            phone1 = string.IsNullOrEmpty(phone1) ? "nonMatchingValue" : phone1;
            city1 = city1.Replace(" ","");
            zipcode1 = zipcode1.Replace(" ","");

            foreach (var employer in list)
            {
                var employerName = employer.name.Replace(" ", "");
                var employerAddress = employer.address1.Replace(" ", "");
                var employerPhone = string.IsNullOrEmpty(employer.phone) ? string.Empty : employer.phone;
                var employerCity = employer.city.Replace(" ","");
                var employerZipcode = employer.zipcode.Replace(" ","");

                //checking if person already exists in database
                var matchCount = 0;
                if (employerName.Equals(name1, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employerAddress.Equals(address1, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employerPhone.Equals(phone1, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employerZipcode.Equals(zipcode1, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employerCity.Equals(city1, StringComparison.CurrentCultureIgnoreCase)) matchCount++;


                if (matchCount < 3) continue;
                var employerFound = new Dictionary<string, string> {
                    { "Name", employer.name },
                    { "Address", employer.address1 },
                    { "Phone", employer.phone },
                    { "City", employer.city },
                    { "ZipCode", employer.zipcode },
                    { "ID", employer.ID.ToString() }
                };

                employersFound.Add(employerFound);
            }

            return Json(new {
                duplicates = employersFound
            });
        }
    }
}
