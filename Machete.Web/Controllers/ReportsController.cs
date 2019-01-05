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
using System.Globalization;
using System.Linq;
using AutoMapper;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using String = System.String;

namespace Machete.Web.Controllers
{
    #region public class ReportsController...
    [ElmahHandleError]
    public class ReportsController : MacheteController
    {
        private readonly IReportService repServ;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;

        public ReportsController(IReportService repServ,
            IDefaults def,
            IMapper map)
        {
            this.repServ = repServ;
            this.map = map;
            this.def = def;
        }

        /// <summary>
        /// Initialize the controller context for the reports.
        /// </summary>
        /// <param name="requestContext">RequestContext</param>
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
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

        // TODO where is this view?
//        [Authorize(Roles = "Administrator, Manager")]
//        public ActionResult Orders()
//        {
//            return PartialView();
//        }

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
        #region Summary (Orders) Reports
        /// <summary>
        /// Daily, Casa Latina == dcl. This is a daily report for Machete, different than the Work Order
        /// Status summary, and part of the summary reports.
        /// </summary>
        /// <param name="param">jQueryDataTableParam</param>
        /// <returns>json object for use with datatables, etc.</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxDcl(jQueryDataTableParam param)
        {
            DateTime dclDate;

            dclDate = voDate(param);
            // pass filter parameters to service level
            // Call view model from service layer:
            dataTableResult<DailySumData> dcl = DaySumView(dclDate);
            //
            //return what's left to datatables
            var result = from d in dcl.query
                         select new
                         {
                            date = String.Format("{0:MM/dd/yyyy}", d.date),
                            d.dwcList,
                            d.dwcPropio,
                            d.hhhList,
                            d.hhhPropio,
                            d.uniqueSignins,
                            d.totalSignins,
                            d.totalAssignments,
                            d.cancelledJobs
                         };

            return Json(new
            {
                iTotalRecords = dcl.totalCount, //total records, before filtering
                iTotalDisplayRecords = dcl.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }
        /// <summary>
        /// 'WEC' is 'Weekly, El Centro' -- a weekly report for Machete that comes from
        /// El Centro's requirements for weekly summaries (Staten Is., N.Y.); AjaxWec is
        /// the weekly report controller.
        /// </summary>
        /// <param name="param">jQueryDataTableParam</param>
        /// <returns>json object for use with datatables, etc.</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxWec(jQueryDataTableParam param)
        {
            DateTime weekDate;
            DateTime beginDate;
            DateTime endDate;
            dataTableResult<WeeklySumData> wec;

            // jQuery passes in parameters that must be mapped to viewOptions
            weekDate = voDate(param);

            beginDate = weekDate.AddDays(-6).Date;
            endDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 23, 59, 59);

            wec = WeekSumView(beginDate, endDate);

            var result = from d in wec.query
                         select new
                         {
                             weekday = d.dayofweek.ToString(),
                             date = String.Format("{0:MM/dd/yyyy}", d.date),
                             d.totalSignins,
                             totalAssignments = d.noWeekJobs,
                             d.weekEstDailyHours,
                             weekEstPayment = String.Format("{0:C}", d.weekEstPayment),
                             weekHourlyWage = String.Format("{0:C}", d.weekHourlyWage),
                             drilldown = from j in d.topJobs
                                       select new
                                       {
//                                           date = System.String.Format("{0:MM/dd/yyyy}", j.date),
                                           skill = j.info, j.count
                                       }
                         };

            return Json(new
            {
                iTotalRecords = wec.totalCount, //total records, before filtering
                iTotalDisplayRecords = wec.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }

        /// <summary>
        /// Provides json grid of monthly summary report
        /// </summary>
        /// <param name="param">contains paramters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxMwd(jQueryDataTableParam param)
        {
            DateTime monthDate;
            DateTime beginDate;
            DateTime endDate;
            dataTableResult<MonthlySumData> mwd;

            monthDate = voDate(param);
            beginDate = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));

            mwd = monthSumView(beginDate, endDate);

            var result = from d in mwd.query
                         select new
                         {
                             date = String.Format("{0:MM/dd/yyyy}", d.dateStart),
                             datestring = String.Format("{0:MMMM d}", d.dateStart),
                             stillHere = d.stillHere.ToString(),
                             totalSignins = d.totalSignins.ToString(),
                             wentToClass = d.peopleWhoWentToClass.ToString(),
                             dispatched = d.dispatched.ToString(),
                             totalHours = d.totalHours.ToString(),
                             totalIncome = String.Format("{0:C}", d.totalIncome),
                             avgIncomePerHour = String.Format("{0:C}", d.avgIncomePerHour),
                             drilldown = new
                                         {
                                             newlyEnrolled = d.newlyEnrolled.ToString(), //dd
                                             peopleWhoLeft = d.peopleWhoLeft.ToString(), //dd
                                             uniqueSignins = d.uniqueSignins.ToString(), //dd
                                             tempDispatched = d.tempDispatched.ToString(), //dd
                                             permanentPlacements = d.permanentPlacements.ToString(), //dd
                                             undupDispatched = d.undupDispatched.ToString() //dd
                                         }
                         };

            return Json(new
            {
                iTotalRecords = mwd.totalCount, //total records, before filtering
                iTotalDisplayRecords = mwd.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }

        /// <summary>
        /// Provides json grid of yearly report 
        /// </summary>
        /// <param name="param">contains paramters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxYwd(jQueryDataTableParam param)
        {
            DateTime yDate;
            DateTime beginDate;
            DateTime endDate;
            dataTableResult<YearSumData> ywd;

            yDate = voDate(param);

            beginDate = yDate.AddMonths(-12).Date;
            endDate = new DateTime(yDate.Year, yDate.Month, yDate.Day, 23, 59, 59);

            ywd = yearSumView(beginDate, endDate);

            var result = from d in ywd.query
                         select new
                         {
                             date = String.Format("{0:MM/dd/yyyy}", d.dateEnd),
                             datestring = "Quarter beginning " + String.Format("{0:MM/dd/yyyy}", d.dateStart) + " and ending " + String.Format("{0:MM/dd/yyyy}", d.dateEnd),
                             stillHere = d.stillHere.ToString(),
                             totalSignins = d.totalSignins.ToString(),
                             wentToClass = d.peopleWhoWentToClass.ToString(),
                             dispatched = d.dispatched.ToString(),
                             totalHours = d.totalHours.ToString(),
                             totalIncome = String.Format("{0:C}", d.totalIncome),
                             avgIncomePerHour = String.Format("{0:C}", d.avgIncomePerHour),
                             drilldown = new
                             {
                                 newlyEnrolled = d.newlyEnrolled.ToString(), //dd
                                 peopleWhoLeft = d.peopleWhoLeft.ToString(), //dd
                                 uniqueSignins = d.uniqueSignins.ToString(), //dd
                                 tempDispatched = d.tempDispatched.ToString(), //dd
                                 permanentPlacements = d.permanentPlacements.ToString(), //dd
                                 undupDispatched = d.undupDispatched.ToString() //dd
                             }
                         };

            return Json(new
            {
                iTotalRecords = ywd.totalCount, //total records, before filtering
                iTotalDisplayRecords = ywd.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }
        #endregion

        #region Activities Reports
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxWeekAct(jQueryDataTableParam param)
        {
            DateTime weekDate;
            DateTime beginDate;
            DateTime endDate;

            dataTableResult<ActivityData> year;

            weekDate = voDate(param);

            beginDate = weekDate.AddDays(-6).Date;
            endDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 23, 59, 59);

            year = ActivityView(beginDate, endDate, "monthly");

            var result = from d in year.query
                         select new
                         {
                             date = String.Format("{0:MMMM d, yyyy}", d.dateStart),
                             safety = d.safety.ToString(),
                             skills = d.skills.ToString(),
                             esl = d.esl.ToString(),
                             basGarden = d.basGarden.ToString(),
                             advGarden = d.advGarden.ToString(),
                             finEd = d.finEd.ToString(),
                             osha = d.osha.ToString(),
                             drilldown = from g in d.drilldown
                                         group g by new { g.info, g.activityType } into x
                                         select new
                                         {
                                             name = x.Key.info,
                                             type = x.Key.activityType,
                                             count = x.Sum(y => y.count)
                                         }
                         };

            return Json(new
            {
                iTotalRecords = year.totalCount, //total records, before filtering
                iTotalDisplayRecords = year.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxMonthAct(jQueryDataTableParam param)
        {
            DateTime monthDate;
            DateTime beginDate;
            DateTime endDate;

            dataTableResult<ActivityData> year;

            monthDate = voDate(param);

            beginDate = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month), 23, 59, 59);

            year = ActivityView(beginDate, endDate, "monthly");

            var result = from d in year.query
                         select new
                         {
                             date = String.Format("{0:MMMM d, yyyy}", d.dateStart),
                             safety = d.safety.ToString(),
                             skills = d.skills.ToString(),
                             esl = d.esl.ToString(),
                             basGarden = d.basGarden.ToString(),
                             advGarden = d.advGarden.ToString(),
                             finEd = d.finEd.ToString(),
                             osha = d.osha.ToString(),
                             drilldown = from g in d.drilldown
                                         group g by new { g.info, g.activityType } into x
                                         select new
                                         {
                                             name = x.Key.info,
                                             type = x.Key.activityType,
                                             count = x.Sum(y => y.count)
                                         }
                         };

            return Json(new
            {
                iTotalRecords = year.totalCount, //total records, before filtering
                iTotalDisplayRecords = year.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxYearAct(jQueryDataTableParam param)
        {
            DateTime yearDate;
            DateTime beginDate;
            DateTime endDate;

            dataTableResult<ActivityData> year;

            yearDate = voDate(param);

            //From www.investopedia.com, Fiscal Year-End: The completion of a one-year, or 12-month, accounting period.
            //A firm's fiscal year-end does not necessarily need to fall on December 31, and can actually fall on any day throughout the year.
            beginDate = yearDate.AddMonths(-12).Date;
            endDate = new DateTime(yearDate.Year, yearDate.Month, yearDate.Day, 23, 59, 59);

            year = YearActView(beginDate, endDate);

            var result = from d in year.query
                         select new
                         {
                             date = "Quarter beginning " + String.Format("{0:MMMM d, yyyy}", d.dateStart) + " and ending " + String.Format("{0:MMMM d, yyyy}", d.dateEnd),
                             safety = d.safety.ToString(),
                             skills = d.skills.ToString(),
                             esl = d.esl.ToString(),
                             basGarden = d.basGarden.ToString(),
                             advGarden = d.advGarden.ToString(),
                             finEd = d.finEd.ToString(),
                             osha = d.osha.ToString(),
                             drilldown = from g in d.drilldown
                                         group g by new { g.info, g.activityType } into x
                                         select new
                                         {
                                             name = x.Key.info,
                                             type = x.Key.activityType,
                                             count = x.Sum(y => y.count)
                                         }
                         };

            return Json(new
            {
                iTotalRecords = year.totalCount, //total records, before filtering
                iTotalDisplayRecords = year.filteredCount, //total records, after filtering
                param.sEcho, //unaltered copy of sEcho sent from the client side
                aaData = result
            });
        }
        #endregion

        #region Worker Reports
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxWeekWkr(jQueryDataTableParam param)
        {
            DateTime wDate;
            DateTime beginDate;
            DateTime endDate;

            wDate = voDate(param);
            beginDate = wDate.AddDays(-6).Date;
            endDate = new DateTime(wDate.Year, wDate.Month, wDate.Day, 23, 59, 59);

            dataTableResult<NewWorkerData> newWkr = NewWorkerView(beginDate, endDate, "weekly");

            var result = from d in newWkr.query
                         orderby d.dateStart
                         select new
                         {
                             date = String.Format("{0:dddd}", d.dateStart),
                             singleAdults = d.singleAdults.ToString(),
                             familyHouseholds = d.familyHouseholds.ToString(),
                             newSingleAdults = d.newSingleAdults.ToString(),
                             newFamilyHouseholds = d.newFamilyHouseholds.ToString(),
                             d.zipCompleteness
                         };

            return Json(new
            {
                iTotalRecords = newWkr.totalCount,
                iTotalDisplayRecords = newWkr.filteredCount,
                param.sEcho,
                aaData = result
            });
        }
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxMonthWkr(jQueryDataTableParam param)
        {
            DateTime mDate;
            DateTime beginDate;
            DateTime endDate;

            mDate = voDate(param);
            beginDate = new DateTime(mDate.Year, mDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(mDate.Year, mDate.Month, DateTime.DaysInMonth(mDate.Year, mDate.Month));

            dataTableResult<NewWorkerData> newWkr = NewWorkerView(beginDate, endDate, "monthly");

            var result = from d in newWkr.query
                         select new
                         {
                             date = String.Format("{0:MMMM d}", d.dateStart),
                             singleAdults = d.singleAdults.ToString(),
                             familyHouseholds = d.familyHouseholds.ToString(),
                             newSingleAdults = d.newSingleAdults.ToString(),
                             newFamilyHouseholds = d.newFamilyHouseholds.ToString(),
                             d.zipCompleteness
                         };

            return Json(new
            {
                iTotalRecords = newWkr.totalCount,
                iTotalDisplayRecords = newWkr.filteredCount,
                param.sEcho,
                aaData = result
            });
        }
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxYearWkr(jQueryDataTableParam param)
        {
            DateTime yDate;
            DateTime beginDate;
            DateTime endDate;

            yDate = voDate(param);
            beginDate = yDate.AddMonths(-12).Date;
            endDate = new DateTime(yDate.Year, yDate.Month, yDate.Day, 23, 59, 59);

            dataTableResult<NewWorkerData> newWkr = NewWorkerView(beginDate, endDate, "yearly");

            var result = from d in newWkr.query
                         select new
                         {
                             date = String.Format("{0:MM/d/yyyy}", d.dateStart) + " to " + String.Format("{0:MM/d/yyyy}", d.dateEnd),
                             singleAdults = d.singleAdults.ToString(),
                             familyHouseholds = d.familyHouseholds.ToString(),
                             newSingleAdults = d.newSingleAdults.ToString(),
                             newFamilyHouseholds = d.newFamilyHouseholds.ToString(),
                             d.zipCompleteness
                         };

            return Json(new
            {
                iTotalRecords = newWkr.totalCount,
                iTotalDisplayRecords = newWkr.filteredCount,
                param.sEcho,
                aaData = result
            });
        }
        #endregion

        #region Employer Reports
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxWeekEmp(jQueryDataTableParam param)
        {
            DateTime wDate;
            DateTime beginDate;
            DateTime endDate;

            wDate = voDate(param);
            beginDate = wDate.AddDays(-6).Date;
            endDate = new DateTime(wDate.Year, wDate.Month, wDate.Day, 23, 59, 59);

            dataTableResult<ZipModel> newEmp = EmployerReportView(beginDate, endDate);

            var result = from d in newEmp.query
                         select new
                         {
                             d.zips,
                             jobs = d.jobs.ToString(),
                             emps = d.emps.ToString(),
                             drilldown = from j in d.skills
                                         select new
                                         {
                                             skill = j.info, j.count
                                         }
                         };

            return Json(new
            {
                iTotalRecords = newEmp.totalCount,
                iTotalDisplayRecords = newEmp.filteredCount,
                param.sEcho,
                aaData = result
            });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult AjaxMonthEmp(jQueryDataTableParam param)
        {
            DateTime mDate;
            DateTime beginDate;
            DateTime endDate;

            mDate = voDate(param);
            beginDate = new DateTime(mDate.Year, mDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(mDate.Year, mDate.Month, DateTime.DaysInMonth(mDate.Year, mDate.Month));

            dataTableResult<ZipModel> newEmp = EmployerReportView(beginDate, endDate);

            var result = from d in newEmp.query
                         select new
                         {
                             d.zips,
                             jobs = d.jobs.ToString(),
                             emps = d.emps.ToString(),
                             drilldown = from j in d.skills
                                         select new
                                         {
                                             skill = j.info, j.count
                                         }
                         };

            return Json(new
            {
                iTotalRecords = newEmp.totalCount,
                iTotalDisplayRecords = newEmp.filteredCount,
                param.sEcho,
                aaData = result
            });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult AjaxYearEmp(jQueryDataTableParam param)
        {
            DateTime yDate;
            DateTime beginDate;
            DateTime endDate;

            yDate = voDate(param);
            beginDate = yDate.AddMonths(-12).Date;
            endDate = new DateTime(yDate.Year, yDate.Month, yDate.Day, 23, 59, 59);

            dataTableResult<ZipModel> newEmp = EmployerReportView(beginDate, endDate);

            var result = from d in newEmp.query
                         select new
                         {
                             d.zips,
                             jobs = d.jobs.ToString(),
                             emps = d.emps.ToString(),
                             drilldown = from j in d.skills
                                         select new
                                         {
                                             skill = j.info, j.count
                                         }
                         };

            return Json(new
            {
                iTotalRecords = newEmp.totalCount,
                iTotalDisplayRecords = newEmp.filteredCount,
                param.sEcho,
                aaData = result
            });
        }
        #endregion

        private DateTime voDate(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            if (vo.date != null) return DateTime.Parse(vo.date.ToString());
            return DateTime.Now;
        }
        #endregion

        #region DataTablesStuff
        // The following methods organize the above service-layer views for return to Ajax/DataTables and the GUI.
        #region For Summary Reports
        private dataTableResult<DailySumData> DaySumView(DateTime date)
        {
            IEnumerable<DailySumData> query;
            query = repServ.DailySumController(date);
            var result = GetDataTableResult(query);
            return result; // ...for DataTables.
        }

        private dataTableResult<WeeklySumData> WeekSumView(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<WeeklySumData> query;
            query = repServ.WeeklySumController(beginDate, endDate);
            var result = GetDataTableResult(query);
            return result;
        }

        private dataTableResult<MonthlySumData> monthSumView(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<MonthlySumData> query;
            query = repServ.MonthlySumController(beginDate, endDate);
            var result = GetDataTableResult(query);
            return result;
        }

        private dataTableResult<YearSumData> yearSumView(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<YearSumData> query;
            query = repServ.YearlySumController(beginDate, endDate);
            var result = GetDataTableResult(query);
            return result;
        }
        #endregion
        #region For Activity Reports
        private dataTableResult<ActivityData> ActivityView(DateTime beginDate, DateTime endDate, string reportType)
        {
            IEnumerable<ActivityData> query;
            query = repServ.ActivityReportController(beginDate, endDate, reportType);
            var result = GetDataTableResult(query);
            return result;
        }

        private dataTableResult<ActivityData> YearActView(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<ActivityData> query;
            query = repServ.YearlyActController(beginDate, endDate);
            var result = GetDataTableResult(query);
            return result;
        }
        #endregion
        private dataTableResult<NewWorkerData> NewWorkerView(DateTime beginDate, DateTime endDate, string reportType)
        {
            IEnumerable<NewWorkerData> query;
            query = repServ.NewWorkerController(beginDate, endDate, reportType);
            var result = GetDataTableResult(query);
            return result;
        }

        private dataTableResult<ZipModel> EmployerReportView(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<ZipModel> query;
            query = repServ.EmployerReportController(beginDate, endDate);
            var result = GetDataTableResult(query);
            return result;
        }
        private dataTableResult<T> GetDataTableResult<T>(IEnumerable<T> query)
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