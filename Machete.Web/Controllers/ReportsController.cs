#region COPYRIGHT
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
using Machete.Service.shared;
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
        private readonly IReportService repServ;
        CultureInfo CI;

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

        #region PartialViews
        public ActionResult Orders()
        {
            return PartialView();
        }
        #endregion

        #region ExternalViews
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult GroupView(DateTime date, string typeOfReport)
        {
            if (typeOfReport == "monthly")
            {
                ReportPrintView<monthlyData> view = new ReportPrintView<monthlyData>();
//                view.report = repServ.monthlyView(date);
                return View(view);
            }
            else if (typeOfReport == "weekly")
            {
                ReportPrintView<weeklyData> view = new ReportPrintView<weeklyData>();
                return View(view);
            }
            else if (typeOfReport == "daily")
            {
                ReportPrintView<dailyData> view = new ReportPrintView<dailyData>();
                return View(view);
            }
            else if (typeOfReport == "jobsandzips")
            {
                ReportPrintView<jzcData> view = new ReportPrintView<jzcData>();
                return View(view);
            }
            else
            {
                return View();
            }
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
                    totalSignins = d.totalSignins,
                    noWeekJobs = d.noWeekJobs,
                    weekJobsSector = d.weekJobsSector,
                    weekEstDailyHours = d.weekEstDailyHours,
                    weekEstPayment = d.weekEstPayment,
                    weekHourlyWage = System.String.Format("{0:C}", d.weekHourlyWage)
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
            if (vo.date != null) mwdDate = DateTime.Parse(vo.date.ToString());
            else mwdDate = DateTime.Now;

            dataTableResult<monthlyData> mwd = repServ.monthlyView(mwdDate);

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

        public ActionResult AjaxJobsZipCodes(jQueryDataTableParam param)
        {
            DateTime jzcDate;

            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            if (vo.date != null) jzcDate = DateTime.Parse(vo.date.ToString());
            else jzcDate = DateTime.Now;

            dataTableResult<jzcData> jzc = repServ.jzcView(jzcDate);

            var result = from d in jzc.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             topZips = d.zips,
                             topJobs = d.jobs
                         };

            return Json(new{
                iTotalRecords = jzc.totalCount,
                iTotalDisplayRecords = jzc.filteredCount,
                sEcho = param.sEcho,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
