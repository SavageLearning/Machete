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
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class EmployerController : MacheteController
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;
        private System.Globalization.CultureInfo CI;

        public EmployerController(IEmployerService employerService, IWorkOrderService workorderService)
        {
            this.serv = employerService;
            this.woServ = workorderService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
            ViewBag.idPrefix = "employer"; //TODO: integration with mUIExtension, feed idPrefix
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
        /// <summary>
        /// GET: /Employer/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Employer> list = serv.GetIndexView(vo);
            //return what's left to datatables
            var result = from p in list.query select new 
            { 
                tabref = _getTabRef(p),
                tablabel = _getTabLabel(p),
                active = Convert.ToString(p.active),
                EID = Convert.ToString(p.ID),
                recordid = Convert.ToString(p.ID),
                name = p.name, 
                address1 = p.address1, 
                city = p.city, 
                phone =  p.phone, 
                driverslicense = p.driverslicense,
                licenseplate = p.licenseplate,
                dateupdated = Convert.ToString(p.dateupdated),
                Updatedby = p.Updatedby,
                onlineSource = p.onlineSource ? Shared.True : Shared.False
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
        private string _getTabRef(Employer emp)
        {
            if (emp == null) return null;
            return "/Employer/Edit/" + Convert.ToString(emp.ID);
        }
        private string _getTabLabel(Employer emp)
        {
            if (emp == null) return null;
            return emp.name;
        }
        /// <summary>
        /// GET: /Employer/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create()
        {
            var _model = new Employer();
            _model.active = true;
        //    _model.city = "Seattle"; // no null types allowed in var
        //    _model.state = "WA";     // no null types allowed in var
            _model.blogparticipate = false;
            _model.referredby = Lookups.getDefaultID(LCategory.emplrreference);
            return PartialView("Create", _model);
        }
        /// <summary>
        /// POST: /Employer/Create
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Create(Employer employer, string userName)
        {
            UpdateModel(employer);
            Employer newEmployer = serv.Create(employer, userName);                          

            return Json(new
            {
                sNewRef = _getTabRef(newEmployer),
                sNewLabel = _getTabLabel(newEmployer),
                iNewID = newEmployer.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult CreateCombined()
        {
            var model = new EmployerWoCombined();
            return PartialView(model);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult CreateCombined(EmployerWoCombined combined, string userName)
        {
            //UpdateModel(combined);
            //split the combined model into domain models
            Employer mappedEmployer = Mapper.Map<EmployerWoCombined, Employer>(combined);
            WorkOrder mappedWO = Mapper.Map<EmployerWoCombined, WorkOrder>(combined);
            mappedEmployer.onlineSource = true;
            mappedWO.onlineSource = true;
            //update domain
            Employer newEmployer = serv.Create(mappedEmployer, userName);
            mappedWO.EmployerID = newEmployer.ID;
            mappedWO.status = WorkOrder.iPending;
            WorkOrder newWO = woServ.Create(mappedWO, userName);
            // return 
            return Json(new
            {
                iEmployerID = newEmployer.ID,
                iWorkOrderID = newWO.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            Employer employer = serv.Get(id);
            return PartialView("Edit", employer);
        }
        /// <summary>
        /// POST: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Edit(int id, FormCollection collection, string userName)
        {
            Employer employer = serv.Get(id);
            UpdateModel(employer);
            serv.Save(employer, userName);                            
            return Json(new
            {
                jobSuccess = true
            }, JsonRequestBehavior.AllowGet);            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Delete(int id, string userName)
        {
            serv.Delete(id, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }

        private List<Dictionary<string, string>> DuplicateEmployers(string name, string address, 
            string phone, string city, string zipcode)
        {
            //Get all the records            
            IEnumerable<Employer> list = serv.GetAll();
            var employersFound = new List<Dictionary<string, string>>();
            name = name.Replace(" ", "");
            address = address.Replace(" ", "");
            phone = string.IsNullOrEmpty(phone) ? "x" : phone;
            city = city.Replace(" ","");
            zipcode = zipcode.Replace(" ","");

            foreach (var employer in list)
            {
                var employer_Name = employer.name.Replace(" ", "");
                var employer_Address = employer.address1.Replace(" ", "");
                var employer_Phone = string.IsNullOrEmpty(employer.phone) ? "y" : employer.phone;
                var employer_City = employer.city.Replace(" ","");
                var employer_Zipcode = employer.zipcode.Replace(" ","");

                //checking if person already exists in dbase
                var matchCount = 0;
                if (employer_Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_Address.Equals(address, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_Phone.Equals(phone, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_Zipcode.Equals(zipcode, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_City.Equals(city, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                

                if (matchCount >= 3)
                {
                    var employerFound = new Dictionary<string, string>();
                    employerFound.Add("Name", employer.name);
                    employerFound.Add("Address", employer.address1);
                    employerFound.Add("Phone", employer.phone);
                    employerFound.Add("City", employer.city);
                    employerFound.Add("ZipCode", employer.zipcode);
                    employerFound.Add("ID", employer.ID.ToString());

                    employersFound.Add(employerFound);
                }
            }

            return employersFound;
        }

        [AllowAnonymous]
        public JsonResult GetDuplicates(string name, string address,
            string phone, string city, string zipcode )
        {
            var duplicateFound = DuplicateEmployers(name, address, phone, city, zipcode);
            return Json(new { duplicates = duplicateFound }, JsonRequestBehavior.AllowGet);
        }
    }
}
