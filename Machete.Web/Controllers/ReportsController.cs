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
using NLog;
using Machete.Web.ViewModel;
using Machete.Web.Models;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Data;
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
        public ActionResult Summary()
        {
            return PartialView();
        }
        
        public ActionResult Orders()
        {
            return PartialView();
        }
        
        public ActionResult Workers()
        {
            return PartialView();
        }
        public ActionResult SSRS()
        {
            return PartialView();
        }
       
        #endregion

        #region ExternalViews

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult PrintView(DateTime date, string typeOfReport)
        {
            ReportPrintView view = new ReportPrintView();
            view.date = date;
            view.typeOfReport = typeOfReport;
            return View(view);
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

            dclDate = voDate(param);
            // pass filter parameters to service level
            // Call view model from service layer:
            dataTableResult<dailyData> dcl = DailyView(dclDate);
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
                            uniqueSignins = d.uniqueSignins,
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
            wecDate = voDate(param);

            dataTableResult<weeklyData> wec = WeeklyView(wecDate);
            //
            //return what's left to datatables
            var result = from d in wec.query
                         select new
                         {
                             weekday = d.dayofweek.ToString(),
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             totalSignins = d.totalSignins,
                             totalAssignments = d.noWeekJobs,
                             weekEstDailyHours = d.weekEstDailyHours,
                             weekEstPayment = System.String.Format("{0:C}", d.weekEstPayment),
                             weekHourlyWage = System.String.Format("{0:C}", d.weekHourlyWage),
                             topJobs = from j in d.topJobs
                                       select new
                                       {
                                           date = System.String.Format("{0:MM/dd/yyyy}", j.date),
                                           skill = j.info,
                                           count = j.count
                                       }
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
        public ActionResult AjaxMwd(jQueryDataTableParam param)
        {
            DateTime mwdDate;
            dataTableResult<monthlyData> mwd;

            mwdDate = voDate(param);
            mwd = monthlyView(mwdDate);

            var result = from d in mwd.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             stillHere = d.stillHere.ToString(),
                             totalSignins = d.totalSignins.ToString(),
                             dispatched = d.dispatched.ToString(),
                             peopleWhoWentToClass = d.peopleWhoWentToClass.ToString(),
                             totalHours = d.totalHours.ToString(),
                             totalIncome = d.totalIncome.ToString(),
                             avgIncomePerHour = System.String.Format("{0:C}", d.avgIncomePerHour),
                             drilldown = new
                                         {
                                             newlyEnrolled = d.newlyEnrolled.ToString(), //dd
                                             peopleWhoLeft = d.peopleWhoLeft.ToString(), //dd
                                             uniqueSignins = d.uniqueSignins.ToString(), //dd
                                             tempDispatched = d.tempDispatched.ToString(), //dd
                                             permanentPlacements = d.permanentPlacements.ToString(), //dd
                                             undupDispatched = d.undupDispatched.ToString(), //dd
                                         }
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

        public ActionResult AjaxYear(jQueryDataTableParam param)
        {
            DateTime yearDate;
            dataTableResult<yearSumData> year;

            yearDate = voDate(param);
            year = yearlyView(yearDate);

            var result = from d in year.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             temporaryPlacements = d.temporaryPlacements.ToString(),
                             safetyTrainees = d.safetyTrainees.ToString(),
                             skillsTrainees = d.skillsTrainees.ToString(),
                             eslAssessed = d.eslAssessed.ToString(),
                             basicGardenTrainees = d.basicGardenTrainees.ToString(),
                             advGardenTrainees = d.advGardenTrainees.ToString(),
                             finTrainees = d.finTrainees.ToString(),
                         };

            return Json(new
            {
                iTotalRecords = year.totalCount, //total records, before filtering
                iTotalDisplayRecords = year.filteredCount, //total records, after filtering
                sEcho = param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxJzc(jQueryDataTableParam param)
        {
            DateTime jzcDate;

            jzcDate = voDate(param);

            dataTableResult<jzcData> jzc = jzcView(jzcDate);

            var result = from d in jzc.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             topZips = d.zips.ToString(),
                             topZipsCount = d.zipsCount.ToString(),
                             topJobs = d.jobs.ToString(),
                             topJobsCount = d.jobsCount.ToString()
                         };

            return Json(new{
                iTotalRecords = jzc.totalCount,
                iTotalDisplayRecords = jzc.filteredCount,
                sEcho = param.sEcho,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxNewWkr(jQueryDataTableParam param)
        {
            DateTime newDate;

            newDate = voDate(param);

            dataTableResult<newWorkerData> newWkr = newWorkerView(newDate);

            var result = from d in newWkr.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             singleAdults = d.singleAdults.ToString(),
                             familyHouseholds = d.familyHouseholds.ToString(),
                             newSingleAdults = d.newSingleAdults.ToString(),
                             newFamilyHouseholds = d.newFamilyHouseholds.ToString(),
                             zipCodeCompleteness = d.zipCodeCompleteness.ToString()
                         };

            return Json(new{
                iTotalRecords = newWkr.totalCount,
                iTotalDisplayRecords = newWkr.filteredCount,
                sEcho = param.sEcho,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public DateTime voDate(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            if (vo.date != null) return DateTime.Parse(vo.date.ToString());
            else return DateTime.Now;
        }
        #endregion

        #region DataTablesStuff
        // The following methods organize the above service-layer views for return to Ajax/DataTables and the GUI.
        // TODO: These are views, should contain no operating logic, and ideally would not be at the MVC layer....
        public dataTableResult<dailyData> DailyView(DateTime date)
        {
            IEnumerable<dailyData> query;

            query = repServ.DailyController(date);

            var result = GetDataTableResult<dailyData>(query);

            return result; // ...for DataTables.
        }

        public dataTableResult<weeklyData> WeeklyView(DateTime weekDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<weeklyData> query;

            beginDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 0, 0, 0).AddDays(-6);
            endDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 23, 59, 59);

            query = repServ.WeeklyController(beginDate, endDate);

            var result = GetDataTableResult<weeklyData>(query);

            return result;
        }

        public dataTableResult<monthlyData> monthlyView(DateTime monthDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<monthlyData> query;

            beginDate = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(monthDate.Year, monthDate.Month, System.DateTime.DaysInMonth(monthDate.Year, monthDate.Month));

            query = repServ.MonthlySummaryController(beginDate, endDate);

            var result = GetDataTableResult<monthlyData>(query);

            return result;
        }

        public dataTableResult<yearSumData> yearlyView(DateTime yDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<yearSumData> query;

            beginDate = new DateTime(yDate.Year, yDate.Month, 1, 0, 0, 0).AddMonths(-12);
            endDate = new DateTime(yDate.Year, yDate.Month, System.DateTime.DaysInMonth(yDate.Year, yDate.Month), 23, 59, 59);

            query = repServ.YearlyController(beginDate, endDate);

            var result = GetDataTableResult<yearSumData>(query);

            return result;
        }

        public dataTableResult<jzcData> jzcView(DateTime jzcDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<jzcData> query;

            beginDate = new DateTime(jzcDate.Year, jzcDate.Month, jzcDate.Day, 0, 0, 0);
            endDate = new DateTime(jzcDate.Year, jzcDate.Month, jzcDate.Day, 23, 59, 59);

            query = repServ.jzcController(beginDate, endDate);

            var result = GetDataTableResult<jzcData>(query);

            return result;
        }

        public dataTableResult<newWorkerData> newWorkerView(DateTime newDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<newWorkerData> query;

            beginDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, 0, 0, 0);
            endDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, 23, 59, 59);
            query = repServ.NewWorkerController(beginDate, endDate);

            var result = GetDataTableResult<newWorkerData>(query);

            return result;
        }

        public dataTableResult<T> GetDataTableResult<T>(IEnumerable<T> query)
        {
            var result = new dataTableResult<T>();

            result.filteredCount = query.Count();
            result.query = query;
            result.totalCount = query.Count();
            return result;
        }

        #endregion
    }
}