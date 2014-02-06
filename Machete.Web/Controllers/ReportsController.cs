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
    #region public class ReportsController...
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

    #endregion
        #region Index

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region PartialViews
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Summary()
        {
            return PartialView();
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Orders()
        {
            return PartialView();
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Employers()
        {
            return PartialView();
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Workers()
        {
            return PartialView();
        }

        [Authorize(Roles = "Administrator, Manager")]
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
        public JsonResult AjaxDcl(jQueryDataTableParam param)
        {
            DateTime dclDate;

            dclDate = voDate(param);
            // pass filter parameters to service level
            // Call view model from service layer:
            dataTableResult<dailyData> dcl = DaySumView(dclDate);
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
        public JsonResult AjaxWec(jQueryDataTableParam param)
        {
            DateTime wecDate;
            // jQuery passes in parameters that must be mapped to viewOptions
            wecDate = voDate(param);

            dataTableResult<weeklyData> wec = WeekSumView(wecDate);
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
        public JsonResult AjaxMwd(jQueryDataTableParam param)
        {
            DateTime mwdDate;
            dataTableResult<monthlyData> mwd;

            mwdDate = voDate(param);
            mwd = monthSumView(mwdDate);

            var result = from d in mwd.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             stillHere = d.stillHere.ToString(),
                             totalSignins = d.totalSignins.ToString(),
                             wentToClass = d.peopleWhoWentToClass.ToString(),
                             dispatched = d.dispatched.ToString(),
                             totalHours = d.totalHours.ToString(),
                             totalIncome = System.String.Format("{0:C}", d.totalIncome),
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

        /// <summary>
        /// Provides json grid of first report 
        /// This is temporary -- we will need
        /// multiple Ajax handlers for  the
        /// other reports
        /// </summary>
        /// <param name="param">contains paramters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxYwd(jQueryDataTableParam param)
        {
            DateTime ywdDate;
            dataTableResult<yearSumData> ywd;

            ywdDate = voDate(param);
            ywd = yearSumView(ywdDate);

            var result = from d in ywd.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             stillHere = d.stillHere.ToString(),
                             totalSignins = d.totalSignins.ToString(),
                             wentToClass = d.peopleWhoWentToClass.ToString(),
                             dispatched = d.dispatched.ToString(),
                             totalHours = d.totalHours.ToString(),
                             totalIncome = System.String.Format("{0:C}", d.totalIncome),
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
                iTotalRecords = ywd.totalCount, //total records, before filtering
                iTotalDisplayRecords = ywd.filteredCount, //total records, after filtering
                sEcho = param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult AjaxYearAct(jQueryDataTableParam param)
        {
            DateTime yearDate;
            dataTableResult<yearActData> year;

            yearDate = voDate(param);
            year = yearActView(yearDate);

            var result = from d in year.query
                         select new
                         {
                             date = System.String.Format("{0:MM/dd/yyyy}", d.date),
                             eslAssessed = d.eslAssessed.Count().ToString(),
                             eslHours = (((int)d.eslAssessed.Sum(z => z.minutesInClass) / 60)).ToString(), //dd?
                             safetyTrainees = d.safetyTrainees.Sum(z => z.count).ToString(),
                             skillsTrainees = d.skillsTrainees.Sum(z => z.count).ToString(),
                             drilldownData1 = from x in d.safetyTrainees
                                              group x by x.info into y
                                                select new {
                                                    safetyName = y.Key,
                                                    safetyCount = y.Sum(z => z.count).ToString()
                                                },
                             drilldownData2 = from x in d.skillsTrainees
                                              group x by x.info into y
                                                select new {
                                                    skillsName = y.Key,
                                                    skillsCount = y.Sum(z => z.count).ToString()
                                                }, //assumption is this will include information about gardening and financial...
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

        //public ActionResult AjaxNewWkr(jQueryDataTableParam param)
        //{
        //    DateTime newDate;

        //    newDate = voDate(param);

        //    dataTableResult<newWorkerData> newWkr = newWorkerView(newDate);

        //    var result = from d in newWkr.query
        //                 select new
        //                 {
        //                     date = System.String.Format("{0:MM/dd/yyyy}", d.date),
        //                     singleAdults = d.singleAdults.ToString(),
        //                     familyHouseholds = d.familyHouseholds.ToString(),
        //                     newSingleAdults = d.newSingleAdults.ToString(),
        //                     newFamilyHouseholds = d.newFamilyHouseholds.ToString(),
        //                     zipCodeCompleteness = d.zipCodeCompleteness.ToString()
        //                 };

        //    return Json(new{
        //        iTotalRecords = newWkr.totalCount,
        //        iTotalDisplayRecords = newWkr.filteredCount,
        //        sEcho = param.sEcho,
        //        aaData = result
        //    },
        //    JsonRequestBehavior.AllowGet);
        //}

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
        public dataTableResult<dailyData> DaySumView(DateTime date)
        {
            IEnumerable<dailyData> query;

            query = repServ.DailySumController(date);

            var result = GetDataTableResult<dailyData>(query);

            return result; // ...for DataTables.
        }

        public dataTableResult<weeklyData> WeekSumView(DateTime weekDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<weeklyData> query;

            beginDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 0, 0, 0).AddDays(-6);
            endDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 23, 59, 59);

            query = repServ.WeeklySumController(beginDate, endDate);

            var result = GetDataTableResult<weeklyData>(query);

            return result;
        }

        public dataTableResult<monthlyData> monthSumView(DateTime monthDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<monthlyData> query;

            beginDate = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(monthDate.Year, monthDate.Month, System.DateTime.DaysInMonth(monthDate.Year, monthDate.Month));

            query = repServ.MonthlySumController(beginDate, endDate);

            var result = GetDataTableResult<monthlyData>(query);

            return result;
        }

        public dataTableResult<yearSumData> yearSumView(DateTime yDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<yearSumData> query;

            beginDate = new DateTime(yDate.Year, yDate.Month, 1, 0, 0, 0).AddMonths(-12);
            endDate = new DateTime(yDate.Year, yDate.Month, System.DateTime.DaysInMonth(yDate.Year, yDate.Month), 23, 59, 59);

            query = repServ.YearlySumController(beginDate, endDate);

            var result = GetDataTableResult<yearSumData>(query);

            return result;
        }

        private dataTableResult<yearActData> yearActView(DateTime yearDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<yearActData> query;

            beginDate = new DateTime(yearDate.Year, yearDate.Month, 1, 0, 0, 0).AddMonths(-12);
            endDate = new DateTime(yearDate.Year, yearDate.Month, System.DateTime.DaysInMonth(yearDate.Year, yearDate.Month), 23, 59, 59);

            query = repServ.YearlyActController(beginDate, endDate);

            var result = GetDataTableResult<yearActData>(query);

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