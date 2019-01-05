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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class LookupController : MacheteController
    {
        private readonly ILookupService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;
        public LookupController(ILookupService serv,
            IDefaults def,
            IMapper map)
        {
            this.serv = serv;
            this.map = map;
            this.def = def;
            ViewBag.configCategories = def.configCategories();
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View("~/Views/Config/Index.cshtml");
        }
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            IEnumerable<LookupList> list = serv.GetIndexView(vo);
            var result = list
                .Select(
                    e => map.Map<LookupList, ViewModel.LookupList>(e)
                ).AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = serv.TotalCount(),
                iTotalDisplayRecords = serv.TotalCount(),
                aaData = result
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            var m = map.Map<Lookup, ViewModel.Lookup>(new Lookup());
            m.def = def;
            return PartialView("~/Views/Config/Create.cshtml", m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookup">The model being created.</param>
        /// <param name="userName">Automatically generated.</param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Create(Lookup lookup, string userName)
        {
            //Lookup lookup = null;
            if(await TryUpdateModelAsync(lookup)) {
                lookup = serv.Create(lookup, userName);
                var result = map.Map<Lookup, ViewModel.Lookup>(lookup);
                return Json(new
                {
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            var m = map.Map<Lookup, ViewModel.Lookup>(serv.Get(id));
            m.def = def;
            return PartialView("~/Views/Config/Edit.cshtml", m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> Edit(int id, string userName)
        {
            var lookup = serv.Get(id);
            if (await TryUpdateModelAsync(lookup)) {
                serv.Save(lookup, userName);
                return Json(new
                {
                     status = "OK"
                });
            } else {
                return Json(new { status = "Not OK" }); // TODO Chaim plz
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult View(int id)
        {
            var m = map.Map<Lookup, ViewModel.Lookup>(serv.Get(id));
            m.def = def;
            return PartialView("~/Views/Config/Edit.cshtml", m);
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
            });
        }
    }

    public class HandleErrorAttribute : ExceptionFilterAttribute
    {
        // TODO http://www.binaryintellect.net/articles/5df6e275-1148-45a1-a8b3-0ba2c7c9cea1.aspx
        public override void OnException(ExceptionContext context)
        {
            var result = new ViewResult { ViewName = "Error" };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(
                modelMetadata, context.ModelState);
            result.ViewData.Add("HandleException", 
                context.Exception);
            context.Result = result;
            context.ExceptionHandled = true;
        }     
    }
}
