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
    public class ReportsController : MacheteController
    {
        // Initialize the following:
        // I am assuming for the moment that I'll only need what's in ReportService.cs
        private readonly IReportService repServ;
        // Oh, there's also this, which is the culture info setting:
        CultureInfo CI;

        // Right?
        public ReportsController(IReportService repServ)
        {
            this.repServ = repServ;
        }

        //Not entirely sure what we are initializing here, below.
        //The other controllers do not explain much.
        //However, they all have this.
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// MVC Controller for Machete Reports
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")] //others?
        #region Index 
        // A simple MVC index view
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        // Get ready to handle some AJAX, baby
        #region AjaxHandlers
        /// <summary>
        /// Provides json grid of first report 
        /// This is temporary -- we will need
        /// multiple Ajax handlers for  the
        /// other reports
        /// </summary>
        /// <param name="param">contains paramters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager")]
        //and then, all hell breaks loose
        public ActionResult AjaxMwd(DateTime mwdDate)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //
            //pass filter parameters to service level
            dataTableResult<mwdData> mvd = repServ.mwdView(mwdDate); //view date
            //
            //return what's left to datatables
            var result = from d in mvd.query
                         select new[] { System.String.Format("{0:MM/dd/yyyy}", d.date),
                                         d.date.ToString(),
                                         d.totalSignins > 0 ? d.totalSignins.ToString() : "0",
                                         d.totalDWCSignins > 0 ? d.totalDWCSignins.ToString() : "0",
                                         d.totalHHHSignins > 0 ? d.totalHHHSignins.ToString() : "0",
                                         d.dispatchedDWCSignins > 0 ? d.dispatchedDWCSignins.ToString() : "0",
                                         d.dispatchedHHHSignins > 0 ? d.dispatchedHHHSignins.ToString() : "0",
                                         d.totalHours > 0 ? d.totalHours.ToString() : "0",
                                         d.totalIncome > 0 ? d.totalIncome.ToString() : "0",
                                         d.avgIncomePerHour > 0 ? d.avgIncomePerHour.ToString() : "0"
                         };

            return Json(new
            {
                //sEcho = param.sEcho,
                iTotalRecords = mvd.totalCount,
                iTotalDisplayRecords = mvd.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
