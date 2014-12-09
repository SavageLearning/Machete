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
using AutoMapper;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ConfigController : MacheteController
    {
        private readonly ILookupService serv;
        System.Globalization.CultureInfo CI;
        public ConfigController(ILookupService serv)
        {
            this.serv = serv;
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
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            IEnumerable<Lookup> list = serv.GetIndexView(vo);
            var result = from p in list
                         select new
                         {
                             tabref = "/Config/Edit/" + Convert.ToString(p.ID),
                             tablabel = p.category + ' ' + p.text_EN,
                             category = p.category,
                             selected = p.selected,
                             text_EN = p.text_EN,
                             text_ES = p.text_ES,
                             subcategory = p.subcategory,
                             level = p.level,
                             //wage = p.wage,
                             //minHour = p.minHour,
                             //fixedJob = p.fixedJob,
                             //sortorder = p.sortorder,
                             //typeOfWorkID = p.typeOfWorkID,
                             //specialtiy = p.speciality,
                             ltrCode = p.ltrCode,
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby,
                             recordid = Convert.ToString(p.ID)
                         };
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
            var _model = new Lookup();
            return PartialView(_model);
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

            return Json(new
            {
                sNewRef = _getTabRef(lookup),
                sNewLabel = _getTabLabel(lookup),
                iNewID = (lookup == null ? 0 : lookup.ID)
            },
            JsonRequestBehavior.AllowGet);
        }
        private string _getTabRef(Lookup per)
        {
            if (per != null) return "/Config/Edit/" + Convert.ToString(per.ID);
            else return null;
        }
        private string _getTabLabel(Lookup per)
        {
            if (per != null) return per.text_EN;
            else return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            Lookup lookup = serv.Get(id);
            return PartialView(lookup);
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
            Lookup lookup = serv.Get(id);
            return View(lookup);
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
