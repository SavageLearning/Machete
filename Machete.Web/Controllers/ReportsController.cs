#region COPYRIGHT
// File:     ReportsController.cs
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

// how did this get so unbelievably huge?

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Web.Helpers;
using Elmah;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using NLog;
using Machete.Web.ViewModel;
using Machete.Web.Models;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Data.Objects;
using AutoMapper;
using System.Globalization;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ReportsController : Controller
    {
        /// <summary>
        /// MVC Controller for Machete Reports
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        #region Index 
        // how do you declare two versions of this?
        public ActionResult Index()
        {
            //the wrong way; get these from View layer
            DateTime reportDate = new DateTime(2012, 07, 01);
            DateTime beginDate = new DateTime(reportDate.Year, reportDate.Month, 01);
            DateTime endDate = new DateTime(reportDate.Year, reportDate.Month, System.DateTime.DaysInMonth(reportDate.Year, reportDate.Month));

            //ACK fix this
            var result = Machete.Models.ReportModel.MonthlyWithDetail(beginDate, endDate);

            return View();
            //return Json(result,
            //JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
