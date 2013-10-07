using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Domain.Entities;
using Machete.Data;
using System.Data.Objects;
using Machete.Data.Infrastructure;
using System.Text.RegularExpressions;
using System.Data.Objects.SqlClient;
using System.Globalization;
using NLog;

namespace Machete.Service
{
    // Other interfaces implement IService, which is a tool for writing a type of record.
    // No writing necessary here. Only reporting.

    public interface IReportService
    {
        IQueryable<DailyCasaLatinaReport> DailyCasaLatina(DateTime beginDate, DateTime endDate);
        IQueryable<WeeklyElCentroReport> WeeklyElCentro(DateTime beginDate, DateTime endDate);
        IQueryable<MonthlyWithDetailReport> MonthlyWithDetail(DateTime beginDate, DateTime endDate);
        dataTableResult<dclData> dclView(DateTime dclDate);
        dataTableResult<wecData> wecView(DateTime wecDate);
        dataTableResult<mwdData> mwdView(DateTime mwdDate);
    }

    public class ReportService : IReportService
    {
        // Initialize the following:
        private readonly IWorkOrderRepository woRepo;
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IWorkerRequestRepository wrRepo;
        private readonly ILookupRepository lookRepo;

        //Inject constructor (see Global.asax.cs):
        public ReportService(IWorkOrderRepository woRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo,
                             IWorkerSigninRepository wsiRepo,
                             IWorkerRequestRepository wrRepo,
                             ILookupRepository lookRepo)
        {
            this.woRepo = woRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
            this.wsiRepo = wsiRepo;
            this.wrRepo = wrRepo;
            this.lookRepo = lookRepo;
        }

        /// <summary>
        /// L2E query for daily Casa Latina Machete report
        /// Returns information about daily lists, future jobs
        /// </summary>
        /// <param name="dateRequested">A single DateTime parameter</param>
        /// <returns>IQueryable</returns>
        public IQueryable<DailyCasaLatinaReport> DailyCasaLatina(DateTime beginDate, DateTime endDate)
        {
            //dateRequested = Convert.ToDateTime(dateRequested.Date);
            //not sure yet if above hack is needed
            IQueryable<DailyCasaLatinaReport> query;

            var waQ = waRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var wrQ = wrRepo.GetAllQ();
            //var lQ = lookRepo.GetAllQ();
            //int dwc = 20;
            //int hhh = 21;
            //int cancelled = 45;

            query = waQ
                .GroupJoin(wrQ,
                    wa => new { waid = (int)wa.workOrderID, waw = (int?)wa.workerAssignedID },
                    wr => new { waid = (int)wr.WorkOrderID, waw = (int?)wr.WorkerID },
                    (wa, wr) => new { wa, 
                        reqOrderID = wr.FirstOrDefault().WorkOrderID,
                        reqWorkerID = wr.FirstOrDefault().WorkerID
                    })
                .GroupJoin(woQ, wr => wr.wa.workOrderID, wo => wo.ID,
                    (wr, wo) => new
                    {
                        wr,
                        cancelledJobs = wo.FirstOrDefault().status == WorkOrder.iCancelled ? 1 : 0,
                        timeOfWork = wo.FirstOrDefault().dateTimeofWork == null ? DateTime.Now : EntityFunctions.TruncateTime(wo.FirstOrDefault().dateTimeofWork)
                    })
                .GroupJoin(wQ, wo => wo.wr.wa.workerAssignedID, w => w.ID,
                    (wo, w) => new {
                        wo,
                        dwcList = w.FirstOrDefault().typeOfWorkID == Worker.iDWC ? (wo.wr.reqWorkerID == w.FirstOrDefault().ID ? 0 : 1) : 0,
                        hhhList = w.FirstOrDefault().typeOfWorkID == Worker.iHHH ? (wo.wr.reqWorkerID == w.FirstOrDefault().ID ? 0 : 1) : 0,
                        dwcPatron = w.FirstOrDefault().typeOfWorkID == Worker.iDWC ? (wo.wr.reqWorkerID == w.FirstOrDefault().ID ? 1 : 0) : 0,
                        hhhPatron = w.FirstOrDefault().typeOfWorkID == Worker.iHHH ? (wo.wr.reqWorkerID == w.FirstOrDefault().ID ? 1 : 0) : 0
                    })
                .Where(whr => whr.wo.timeOfWork >= beginDate
                           && whr.wo.timeOfWork <= endDate)
                .GroupBy(gb => gb.wo.timeOfWork)
                .Select(group => new DailyCasaLatinaReport
                    {
                        date = group.Key ?? DateTime.Now, //second condition can't be reached AFAIK
                        dwcList = group.Sum(a => a.dwcList == null ? 0 : a.dwcList),
                        dwcPropio = group.Sum(a => a.dwcPatron == null ? 0 : a.dwcPatron),
                        hhhList = group.Sum(a => a.hhhList == null ? 0 : a.hhhList),
                        hhhPropio = group.Sum(a => a.hhhPatron == null ? 0 : a.hhhPatron),
                        totalSignins = group.Sum(a => a.dwcList == null ? 0 : a.dwcList) + group.Sum(a => a.dwcPatron == null ? 0 : a.dwcPatron) + group.Sum(a => a.hhhList == null ? 0 : a.hhhList) + group.Sum(a => a.hhhPatron == null ? 0 : a.hhhPatron),
                        totalAssignments = group.Count(),
                        cancelledJobs = group.Sum(a => a.wo.cancelledJobs == null ? 0 : a.wo.cancelledJobs)
                    })
                .OrderBy(fini => fini.date);

            return query;
        }

        /// <summary>
        /// L2E query for Weekly El Centro (wec) report for Machete
        /// returns information about work orders and earnings at a
        /// weekly interval.
        /// </summary>
        /// <param name="beginDate">Start date for the query.</param>
        /// <param name="endDate">End date for the query.</param>
        /// <returns>IQueryable</returns>
        public IQueryable<WeeklyElCentroReport> WeeklyElCentro(DateTime beginDate, DateTime endDate)
        {
            IQueryable<WeeklyElCentroReport> query;

            var wsiQ = wsiRepo.GetAllQ();
            var waQ = waRepo.GetAllQ();

            query = wsiQ
                .GroupJoin(waQ, wsi => wsi.ID, wa => wa.workerAssignedID,
                    (wsi, wa) => new //LEFT JOIN
                        {
                            wsi, //seems wsi-wawsi, but works (left in left join, then:)
                            waid = wa.FirstOrDefault().ID == null ? 0 : 1, //to sum, below
                            waworkorderid = wa.FirstOrDefault().workOrderID == null ? 0 : 1, //same
                            wahours = wa.FirstOrDefault().hours == null ? 0 : wa.FirstOrDefault().hours, //already ok
                            wahourlywage = wa.FirstOrDefault().hourlyWage == null ? 0 : wa.FirstOrDefault().hourlyWage
                        }
                     )
                .Where(whr => whr.wsi.dateforsignin >= beginDate
                           && whr.wsi.dateforsignin <= endDate)
                .GroupBy(gb => gb.wsi.dateforsignin)
                .Select(wec => new WeeklyElCentroReport
                          {
                              date = wec.Key == null ? new DateTime(2013, 1, 1, 0, 0, 0) : wec.Key,
                              totalSignins = wec.Count() > 0 ? wec.Count() : 0, //this was orig. wsiQ, so a count is a count of wsi
                              noWeekJobs = wec.Sum(nwj => nwj.waid), // should never be null, see above
                              weekEstDailyHours = wec.Sum(wedh => wedh.wahours),
                              weekEstPayment = wec.Sum(wep => wep.wahourlywage * wep.wahours),
                              weekHourlyWage = wec.Sum(whwtwo => whwtwo.wahours) == 0 ? 0 : wec.Sum(whw => whw.wahourlywage * whw.wahours) / wec.Sum(whwtwo => whwtwo.wahours)
                          }
                     );

            return query;
        }

        public IQueryable<WeeklyJobsBySector> WeeklyJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<WeeklyJobsBySector> query;

            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = waQ
                .GroupJoin(woQ,
                    wa => wa.workOrderID,
                    wo => wo.ID,
                    (wa, wo) => new
                    {
                        wa,
                        workDate = wo.FirstOrDefault().dateTimeofWork
                    })
                .GroupJoin(lQ,
                    wawo => wawo.wa.skillID,
                    l => l.ID,
                    (wawo, l) => new
                    {
                        wawo,
                        enText = l.FirstOrDefault().text_EN
                    })
                .Where(whr => whr.wawo.workDate >= beginDate
                           && whr.wawo.workDate <= endDate)
                .GroupBy(gb => new { gb.enText, gb.wawo.workDate })
                .OrderByDescending(ob => ob.Key.workDate)
                .ThenByDescending(ob => ob.Count())
                .Select(group => new WeeklyJobsBySector
                {
                    jobsDate = group.Key.workDate,
                    jobsEngText = group.Key.enText ?? "",
                    jobsCount = group.Count() > 0 ? group.Count() : 0
                });

            return query;
        }

        /// <summary>
        /// LINQ to Entities query for the Monthly With Detail report,
        /// a Machete report that returns basic information about work
        /// orders at a monthly interval.
        /// </summary>
        /// <param name="beginDate">The start date for the query.</param>
        /// <param name="endDate">The end date for the query.</param>
        /// <returns>IQueryable</returns>
        public IQueryable<MonthlyWithDetailReport> MonthlyWithDetail(DateTime beginDate, DateTime endDate)
        {
            IQueryable<MonthlyWithDetailReport> query;

            var wsiQ = wsiRepo.GetAllQ();
            var waQ = waRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            //uses workersignins, workassignments, workers
            query = wsiQ
                .Where(wsi => wsi.dateforsignin >= beginDate &&
                              wsi.dateforsignin <= endDate)
                .GroupJoin(waQ, // necessary for left outer join
                      wsi => wsi.WorkAssignmentID,
                      wa => wa.ID,
                      (wsi, wa) => new
                      {
                          wsi,
                          // In SQL, the transformations below would be done in the select;
                          // checking for null b/c it will be null if there's no match in the left outer join
                          wahours = wa.FirstOrDefault().hours == null ? 0 : wa.FirstOrDefault().hours,
                          wadays = wa.FirstOrDefault().days == null ? 0 : wa.FirstOrDefault().days,
                          wawage = wa.FirstOrDefault().hourlyWage == null ? 0 : wa.FirstOrDefault().hourlyWage
                      })
                .Join(wQ, // inner join
                    x => x.wsi.WorkerID,
                    w => w.ID,
                    (xx, ww) => new
                    {
                        xx,
                        // these are analogous to 'case' statements in SQL, transforming booleans to 1s or 0s 
                        // to sum them below. This is the first half of 'make SQL Server do the math'
                        DWC = (ww.typeOfWorkID == 20 ? 1 : 0),
                        HHH = (ww.typeOfWorkID == 21 ? 1 : 0),
                        DWCdispatched = (ww.typeOfWorkID == 20 && xx.wsi.WorkAssignmentID != null ? 1 : 0),
                        HHHdispatched = (ww.typeOfWorkID == 21 && xx.wsi.WorkAssignmentID != null ? 1 : 0),
                        totalHours = (xx.wahours * xx.wadays),
                        totalIncome = (xx.wahours * xx.wadays * xx.wawage),
                    })
                .GroupBy(a => a.xx.wsi.dateforsignin)
                .Select(group => new MonthlyWithDetailReport
                {
                    date = group.Key,
                    // and the second half of 'make SQL do the math'. all the above stuff was to get to
                    // this group by. SQL server will execute all of the resulting SQL in <1 second. 
                    totalSignins = group.Count(),
                    totalDWCSignins = group.Sum(b => b.DWC),
                    totalHHHSignins = group.Sum(b => b.HHH),
                    dispatchedDWCSignins = group.Sum(b => b.DWCdispatched),
                    dispatchedHHHSignins = group.Sum(b => b.HHHdispatched),
                    totalHours = group.Sum(b => b.totalHours),
                    totalIncome = group.Sum(b => b.totalIncome)//,
                    // this last field isnt working. it can be calculated at the MVC Controller layer, before the data goes out
                    // it's just: totalHours * totalIncome
                    //avgIncomePerHour = group.Sum(b => b.totalIncome / b.totalHours)
                })
                .OrderBy(a => a.date);

            return query;
        }


        public dataTableResult<dclData> dclView(DateTime dclDate)
        {
            IEnumerable<DailyCasaLatinaReport> dclCurrent;
            //IEnumerable<DailyCasaLatinaReport> dclFuture;
            IEnumerable<dclData> q;
            var result = new dataTableResult<dclData>();

            DateTime beginDate = new DateTime(dclDate.Year, dclDate.Month, dclDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(dclDate.Year, dclDate.Month, dclDate.Day, 23, 59, 59).AddDays(DateTime.DaysInMonth(dclDate.Year, dclDate.Month + 1));

            //new DateTime(dclDate.Year, dclDate.Month, dclDate.Day, 23, 59, 59);
            //DateTime futureDate =

            dclCurrent = DailyCasaLatina(beginDate, endDate).ToList(); 
            //dclFuture = DailyCasaLatina(endDate, futureDate).ToList();

            q = dclCurrent
                //.Join(dclFuture,
                //    cur => cur.date,
                //    fut => fut.date,
                //    (cur, fut) => new
                //    { cur,
                //        fut.dwcList,
                //        fut.dwcPropio,
                //        fut.hhhList,
                //        fut.hhhPropio,
                //        fut.totalSignins
                //    })
                //.GroupBy(gb => gb.cur.date)
                .Select(group => new dclData
                {
                    date = group.date,
                    dwcList = group.dwcList,
                    dwcPropio = group.dwcPropio,
                    hhhList = group.hhhList,
                    hhhPropio = group.hhhPropio,
                    totalSignins = group.totalSignins,
                    totalAssignments = group.totalAssignments,
                    cancelledJobs = group.cancelledJobs//,
                    //futureDWC = group.FirstOrDefault().dwcList,
                    //futureDWCpropio = group.FirstOrDefault().dwcPropio,
                    //futureHHH = group.FirstOrDefault().hhhList,
                    //futureHHHpropio = group.FirstOrDefault().hhhPropio,
                    //futureTotal = group.FirstOrDefault().totalSignins
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count(); //should theoretically be "1",
            result.query = q; //these need to stay set properly to be
            result.totalCount = waRepo.GetAllQ().Count(); // returned
            return result; // ...for DataTables.
        }

        public dataTableResult<wecData> wecView(DateTime wecDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<WeeklyElCentroReport> wecResult;
            IEnumerable<WeeklyJobsBySector> wecJobs;
            IEnumerable<wecData> q; // query for monthlyWithDetail result
            var result = new dataTableResult<wecData>(); // note the type. define it well.

            beginDate = new DateTime(wecDate.Year, wecDate.Month, wecDate.Day, 0, 0, 0).AddDays(-6);
            endDate = new DateTime(wecDate.Year, wecDate.Month, wecDate.Day, 23, 59, 59);

            wecResult = WeeklyElCentro(beginDate, endDate).ToList();            
            wecJobs = WeeklyJobs(beginDate, endDate).ToList();

            //.Select(x => x.enText).Aggregate("", (str, obj) => str + obj + " (" + group.Count().ToString() + ")")

            //there is a random bug here where 'job' comes up empty.
            q = wecResult
                .GroupJoin(wecJobs,
                    res => res.date,
                    job => job.jobsDate,
                    (res, job) => new {
                        date = res.date,
                        total = res.totalSignins,
                        week = res.noWeekJobs,
                        hours = res.weekEstDailyHours,
                        pay = res.weekEstPayment,
                        jobDate = job.First().jobsDate,
                        jobText = wecJobs
                            .Where(whr => whr.jobsDate == job.FirstOrDefault().jobsDate)
                            .Aggregate("", (a, b) => a + b.jobsEngText + " (" + b.jobsCount.ToString() + "), "),
                        jobCount = job.First().jobsCount})
                .GroupBy(gb => new { gb.jobText, gb.date })
                .OrderBy(ob => ob.Key.date)
                .Select(g => new wecData
                {
                    date = g.Key.date,
                    totalSignins = g.FirstOrDefault().total,
                    noWeekJobs = g.FirstOrDefault().week,
                    weekJobsSector = g.FirstOrDefault().jobText,
                    weekEstDailyHours = g.FirstOrDefault().hours,
                    weekEstPayment = g.FirstOrDefault().pay,
                    weekHourlyWage = g.FirstOrDefault().hours == 0 ? 0 : (g.FirstOrDefault().pay / g.FirstOrDefault().hours)
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count();
            result.query = q;
            result.totalCount = waRepo.GetAllQ().Count();
            return result;
        }

        public dataTableResult<mwdData> mwdView(DateTime mwdDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<MonthlyWithDetailReport> mwdResult;
            IEnumerable<mwdData> q; // query for monthlyWithDetail result
            var result = new dataTableResult<mwdData>(); // note the type. define it well.

            beginDate = new DateTime(mwdDate.Year, mwdDate.Month, 1);
            endDate = new DateTime(mwdDate.Year, mwdDate.Month, System.DateTime.DaysInMonth(mwdDate.Year, mwdDate.Month));

            mwdResult = MonthlyWithDetail(beginDate, endDate).ToList();

            q = mwdResult
                .Select(g => new mwdData
                {
                    date = g.date,
                    totalSignins = g.totalSignins,
                    totalDWCSignins = g.totalDWCSignins,
                    totalHHHSignins = g.totalHHHSignins,
                    dispatchedDWCSignins = g.dispatchedDWCSignins,
                    dispatchedHHHSignins = g.dispatchedHHHSignins,
                    totalHours = g.totalHours,
                    totalIncome = g.totalIncome,
                    avgIncomePerHour = g.totalHours != 0 ? (g.totalIncome / g.totalHours) : 0
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count();
            // data should include one month and already be organized as such
            // dataTables rows can be defined at view level
            result.query = q;
            result.totalCount = waRepo.GetAllQ().Count();
            return result;
        }
    }

    /// <summary>
    /// A class to contain the data for the Daily Report for Casa Latina
    /// int dwcList, int dwcPropio, int hhhList, int hhhPropio, int
    /// totalSignins, int cancelledJobs, int dwcFuture, int
    /// dwcPropioFuture, int hhhFuture, int hhhPropioFuture,
    /// int futureTotal
    /// </summary>
    public class dclData
    {
        public DateTime? date { get; set; }
        public int? dwcList { get; set; }
        public int? dwcPropio { get; set; }
        public int? hhhList { get; set; }
        public int? hhhPropio { get; set; }
        public int? totalSignins { get; set; }
        public int? cancelledJobs { get; set; }
        public int? totalAssignments { get; set; }
        //public DateTime? futureDate { get; set; }
        //public int? futureDWC { get; set; }
        //public int? futureDWCpropio { get; set; }
        //public int? futureHHH { get; set; }
        //public int? futureHHHpropio { get; set; }
        //public int? futureTotal { get; set; }
        //public int? futureCancelled { get; set; }
    }
    /// <summary>
    /// A class to contain the data for the Weekly Report for El Centro
    /// int totalSignins, int noWeekJobs, int weekJobsSector, int
    /// weekEstDailyHours, double weekEstPayment, double weekHourlyWage
    /// </summary>
    public class wecData 
    {
        public DateTime? date { get; set; }
        public int? totalSignins { get; set; }
        public int? noWeekJobs { get; set; }
        public string weekJobsSector { get; set; }
        public int? weekEstDailyHours { get; set; }
        public double? weekEstPayment { get; set; }
        public double? weekHourlyWage { get; set; }
    }

    /// <summary>
    /// A class containing all of the data for the Monthly Report with Details
    /// DateTime date, int totalDWCSignins, int totalHHHSignins
    /// dispatchedDWCSignins, int dispatchedHHHSignins
    /// </summary>
    public class mwdData
    {

        public DateTime? date { get; set; }
        public int? totalSignins { get; set; }
        public int? totalDWCSignins { get; set; }
        public int? totalHHHSignins { get; set; }
        public int? dispatchedDWCSignins { get; set; }
        public int? dispatchedHHHSignins { get; set; }
        public int? totalHours { get; set; }
        public double? totalIncome { get; set; }
        public double? avgIncomePerHour { get; set; }
    }

}