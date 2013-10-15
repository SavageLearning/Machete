﻿#region COPYRIGHT
// File:     ReportsController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// Some portions 2013 Life Computer Upgrades, LLC, all rights reserved.
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
// forked at http://www.github.com/chaim1221/machete/
// 
#endregion
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

        /// <summary>
        /// Initialize the controller context for the reports.
        /// </summary>
        /// <param name="requestContext">RequestContext</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }

        /// <summary>
        /// MVC Controller for Machete Reports
        /// </summary>
        /// <returns>View()</returns>
        [Authorize(Roles = "Administrator, Manager")] 
        #region Index 

        // A simple MVC index view
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region AjaxHandlers
        /// <summary>
        /// Daily, Casa Latina == dcl. This is a daily report for Machete, different than the Work Order
        /// Status summary, and part of the summary reports.
        /// </summary>
        /// <param name="param">jQueryDataTableParam</param>
        /// <returns>json object for use with datatables, etc.</returns>
        public ActionResult AjaxDcl(jQueryDataTableParam param)
        {
            DateTime dclDate;

            // jQuery passes in parameters that must be mapped to viewOptions
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);

            // Take the date from the view and assign it to this.mwdDate
            if (vo.date != null) dclDate = DateTime.Parse(vo.date.ToString());
            else dclDate = DateTime.Now;
            // pass filter parameters to service level
            // Call view model from service layer:
            dataTableResult<dailyData> dcl = repServ.DailyView(dclDate);
            //
            //return what's left to datatables
            var result = from d in dcl.query
                         select new
                         {
                            date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                            dwcList = d.dwcList,
                            dwcPropio = d.dwcPropio,
                            hhhList = d.hhhList,
                            hhhPropio = d.hhhPropio,
                            totalSignins = d.totalSignins,
                            totalAssignments = d.totalAssignments,
                            cancelledJobs = d.cancelledJobs
                         };

            return Json(new
            {
                iTotalRecords = dcl.totalCount, //total records, before filtering
                iTotalDisplayRecords = dcl.filteredCount, //total records, after filtering
                sEcho = param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 'WEC' is 'Weekly, El Centro' -- a weekly report for Machete that comes from
        /// El Centro's requirements for weekly summaries (Staten Is., N.Y.); AjaxWec is
        /// the weekly report controller.
        /// </summary>
        /// <param name="param">jQueryDataTableParam</param>
        /// <returns>json object for use with datatables, etc.</returns>
        public ActionResult AjaxWec(jQueryDataTableParam param)
        {
            DateTime wecDate;
            // jQuery passes in parameters that must be mapped to viewOptions
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            // Take the date from the view and assign it to this.mwdDate
            if (vo.date != null) wecDate = DateTime.Parse(vo.date.ToString());
            else wecDate = DateTime.Now;
            //pass filter parameters to service level
            // Call view model from service layer:
            dataTableResult<weeklyData> wec = repServ.WeeklyView(wecDate);
            //
            //return what's left to datatables
            var result = from d in wec.query
                select new
                {
                    weekday = d.dayofweek.ToString(),
                    date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                    totalSignins = d.totalSignins > 0 ? d.totalSignins.ToString() : "0",
                    noWeekJobs = d.noWeekJobs > 0 ? d.noWeekJobs.ToString() : "0",
                    weekJobsSector = d.weekJobsSector == null ? "None" : d.weekJobsSector,
                    weekEstDailyHours = d.weekEstDailyHours > 0 ? d.weekEstDailyHours.ToString() : "0",
                    weekEstPayment = d.weekEstPayment > 0 ? d.weekEstPayment.ToString() : "0",
                    weekHourlyWage = d.weekHourlyWage > 0 ? d.weekHourlyWage.ToString() : "0"
                };

            return Json(new
            {
                iTotalRecords = wec.totalCount, //total records, before filtering
                iTotalDisplayRecords = wec.filteredCount, //total records, after filtering
                sEcho = param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

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
        public ActionResult AjaxMwd(jQueryDataTableParam param)
        {
            DateTime mwdDate;
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param); 
            // jQuery passes in parameters that must be mapped to viewOptions
            // following the format of the other files here;
            // The only thing we're using it for here is the date, but this could
            // be extended to other features.
            // Set culture setting to whatever current session setting is
            //vo.CI = (System.Globalization.CultureInfo)Session["Culture"]; 
            // Commented out because this does not seem to be needed beyond
            // the view layer.
            // Take the date from the view and assign it to this.mwdDate
            if (vo.date != null) mwdDate = DateTime.Parse(vo.date.ToString());
            else mwdDate = DateTime.Now;
            //pass filter parameters to service level
            // Call view model from service layer:
            dataTableResult<monthlyData> mwd = repServ.monthlyView(mwdDate);
            //
            //return what's left to datatables
            var result = from d in mwd.query
                         select new { 
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             totalSignins = d.totalSignins.ToString(),
                             totalDWCSignins =  d.totalDWCSignins.ToString(),
                             totalHHHSignins =  d.totalHHHSignins.ToString(),
                             dispatchedDWCSignins = d.dispatchedDWCSignins.ToString(),
                             dispatchedHHHSignins = d.dispatchedHHHSignins.ToString(),
                             totalHours = d.totalHours.ToString(),
                             totalIncome = d.totalIncome.ToString(),
                             avgIncomePerHour = d.avgIncomePerHour.ToString()
                         };

            return Json(new
            {
                iTotalRecords = mwd.totalCount, //total records, before filtering
                iTotalDisplayRecords = mwd.filteredCount, //total records, after filtering
                sEcho = param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
