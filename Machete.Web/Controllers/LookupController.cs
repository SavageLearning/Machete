#region COPYRIGHT
// File:     ConfigController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class LookupController : MacheteController
    {
        private readonly ILookupService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        System.Globalization.CultureInfo CI;
        public LookupController(ILookupService serv,
            IDefaults def,
            IMapper map)
        {
            this.serv = serv;
            this.map = map;
            this.def = def;
            ViewBag.configCategories = def.configCategories();
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
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            IEnumerable<DTO.LookupList> list = serv.GetIndexView(vo);
            var result = list
                .Select(
                    e => map.Map<DTO.LookupList, ViewModel.LookupList>(e)
                ).AsEnumerable();
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = serv.TotalCount(),
                iTotalDisplayRecords = serv.TotalCount(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            var m = map.Map<Domain.Lookup, ViewModel.Lookup>(new Lookup());
            m.def = def;
            return PartialView(m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(Lookup lookup, string userName)
        {
            //Lookup lookup = null;
            UpdateModel(lookup);
            lookup = serv.Create(lookup, userName);
            var result = map.Map<Domain.Lookup, ViewModel.Lookup>(lookup);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = result.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            var m = map.Map<Domain.Lookup, ViewModel.Lookup>(serv.Get(id));
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
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id, string userName)
        {
            Lookup lookup = serv.Get(id);
            UpdateModel(lookup);
            serv.Save(lookup, userName);
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
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult View(int id)
        {
            var m = map.Map<Domain.Lookup, ViewModel.Lookup>(serv.Get(id));
            m.def = def;
            return PartialView("Edit", m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator"), HandleError]
        public ActionResult Delete(int id, string user)
        {
            serv.Delete(id, user);

            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
