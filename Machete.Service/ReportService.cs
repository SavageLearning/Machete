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
    // Other interfaces seem to implement IService, which is a tool for writing a type of record.
    // See [workingDirectory]\Machete.Service\shared\ServiceBase.cs -- didn't do that here.

    public interface IReportService
    {
        IQueryable<DailyCasaLatinaReport> DailyCasaLatina(DateTime dateRequested);
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
        public IQueryable<DailyCasaLatinaReport> DailyCasaLatina(DateTime dateRequested)
        {
            IQueryable<DailyCasaLatinaReport> query;

            var waQ = waRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var wrQ = wrRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            int dwc = 20;
            int hhh = 21;
            int complete = 44;

            query = waQ
                       .GroupJoin(lQ, dalet => dalet.skillID, look => look.ID,
                                     (dalet, look) => new
                                     {
                                         dalet,
                                         enSkillText = look.FirstOrDefault().text_EN
                                     }) //currently envisioning a left outer join of
                                        //all .skillID, with English text available
                                        //for column and condition matches from the
                                        //next three joins.
                       .GroupJoin(wrQ, gimel => gimel.dalet.workOrderID, wr => wr.WorkOrderID,
                                      (gimel, wr) => new
                                      {
                                          gimel,
                                          reqWorkerID = wr.FirstOrDefault().WorkerID,
                                          reqOrderID = wr.FirstOrDefault().WorkOrderID
                                      }) //now envisioning a join on the original table
                                         //where any match in workerID is joined. THIS
                                         //IS A PROBLEM, I actually need to join on two
                                         //conditions to avoid duplicates.
                       .GroupJoin(woQ, bet => bet.gimel.dalet.workOrderID, wo => wo.ID,
                                      (bet, wo) => new
                                      {
                                          bet,
                                          timeOfWork = wo.FirstOrDefault().dateTimeofWork
                                      }) //now envisioning yet another join where the
                                         //dateTimeofWork property from woQ is stamped
                                         //onto all matches of woQ's ID column. since
                                         //woQ.ID is the common point of reference for
                                         //like, everything, there should be no nulls.
                       .GroupJoin(wQ, alef => alef.bet.gimel.dalet.workerAssignedID, w => w.ID,
                                     (alef, w) => new
                                    {
                                        alef,
                                        listDWC = alef.bet.reqOrderID == 0 ? (w.FirstOrDefault().typeOfWorkID == dwc ? 1 : 0) : 0,
                                        propioDWC = alef.bet.reqWorkerID == alef.bet.gimel.dalet.workerAssignedID ? 
                                                        (w.FirstOrDefault().typeOfWorkID == dwc ? 1 : 0) : 0,
                                        listHHH = alef.bet.reqOrderID == 0 ? (w.FirstOrDefault().typeOfWorkID == hhh ? 1 : 0) : 0,
                                        propioHHH = alef.bet.reqWorkerID == alef.bet.gimel.dalet.workerAssignedID ?
                                                        (w.FirstOrDefault().typeOfWorkID == hhh ? 1 : 0) : 0,
                                        // here I'm stuck because there's no way to do the future conditions.
                                    })  
                       .Where(x => x.alef.timeOfWork == dateRequested)
                       .GroupBy(y => y.alef.bet.gimel.dalet.ID)
                       .Select(group => new DailyCasaLatinaReport 
                                    {
                                        dwcList = group.Sum(z => z.listDWC),
                                        dwcPropio = group.Sum(z => z.propioDWC),
                                        hhhList = group.Sum(z => z.listHHH),
                                        hhhPropio = group.Sum(z => z.propioHHH),
                                        totalSignins = 0,
                                        dwcFuture = 0,
                                        dwcPropioFuture = 0,
                                        hhhFuture = 0,
                                        hhhPropioFuture = 0,
                                        futureTotal = 0
                                    });

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
            var woQ = woRepo.GetAllQ();

            query = wsiQ //begin "with" clause; first line is "FROM"
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
                        .GroupJoin(woQ, wsiwa => wsiwa.waworkorderid, wo => wo.ID, // xx is the h
                                       (wsiwa, wsiwawo) => new //prepare ye to receive thy second left join
                                            {
                                                wsiwa,
                                                wodatetimeofwork = wsiwawo.FirstOrDefault().dateTimeofWork == null ? default(DateTime) : wsiwawo.FirstOrDefault().dateTimeofWork
                                            }
                                  )
                        .Where(www => www.wodatetimeofwork >= beginDate //WHERE
                                   && www.wodatetimeofwork <= endDate)
                //end "WITH" clause; assume "FROM" already handled
                        .GroupBy(gb => gb.wodatetimeofwork)
                        .Select(wec => new WeeklyElCentroReport
                                    {
                                        date = wec.Key,
                                        totalSignins = wec.Count(), //this was orig. wsiQ, so a count is a count of wsi
                                        noWeekJobs = wec.Sum(nwj => nwj.wsiwa.waid),
                                        //weekJobsSector = and this would be the reason they're all commented out
                                        weekEstDailyHours = wec.Sum(wedh => wedh.wsiwa.wahours),
                                        weekEstPayment = wec.Sum(wep => wep.wsiwa.wahourlywage * wep.wsiwa.wahours),
                                        weekHourlyWage = wec.Sum(whwtwo => whwtwo.wsiwa.wahours) == 0 ? 0 : wec.Sum(whw => whw.wsiwa.wahourlywage * whw.wsiwa.wahours) / wec.Sum(whwtwo => whwtwo.wsiwa.wahours)
                                    }
                               )
                        .OrderBy(fini => fini.date);

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
            DateTime dateRequested;
            IEnumerable<DailyCasaLatinaReport> dclResult;
            IEnumerable<dclData> q;
            var result = new dataTableResult<dclData>();

            dateRequested = dclDate;

            dclResult = DailyCasaLatina(dateRequested); // not sending to list...

            q = dclResult
                .Select( g => new dclData
                {
                    dwcList = g.dwcList,
                    dwcPropio = g.dwcPropio,
                    hhhList = g.hhhList,
                    hhhPropio = g.hhhPropio,
                    totalSignins = g.totalSignins,
                    cancelledJobs = g.cancelledJobs,
                    dwcFuture = g.dwcFuture,
                    dwcPropioFuture = g.dwcPropioFuture,
                    hhhFuture = g.hhhFuture,
                    hhhPropioFuture = g.hhhPropioFuture,
                    futureTotal = g.futureTotal
                });

            // no need for an order by. this should be a single line of data.

            result.filteredCount = q.Count(); //even though should be '1',
            result.query = q; //these need to stay set properly to be
            result.totalCount = waRepo.GetAllQ().Count(); // returned
            return result; // ...for DataTables.
        }

        public dataTableResult<wecData> wecView(DateTime wecDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<WeeklyElCentroReport> wecResult;
            IEnumerable<wecData> q; // query for monthlyWithDetail result
            var result = new dataTableResult<wecData>(); // note the type. define it well.

            beginDate = new DateTime(wecDate.Year, wecDate.Month, (wecDate.Day - 6));
            endDate = wecDate;

            wecResult = WeeklyElCentro(beginDate, endDate).ToList();

            q = wecResult
                .Select(g => new wecData
                {
                    totalSignins = g.totalSignins,
                    noWeekJobs = g.noWeekJobs,
//                    weekJobsSector = g.weekJobsSector,
                    weekEstDailyHours = g.weekEstDailyHours,
                    weekEstPayment = g.weekEstPayment,
                    weekHourlyWage = g.weekEstDailyHours != 0 ? (g.weekEstPayment / g.weekEstDailyHours) : 0
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count();
            // data should include one month and already be organized as such
            // dataTables rows can be defined at view level
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
        public int? dwcList { get; set; }
        public int? dwcPropio { get; set; }
        public int? hhhList { get; set; }
        public int? hhhPropio { get; set; }
        public int? totalSignins { get; set; }
        public int? cancelledJobs { get; set; }
        public int? dwcFuture { get; set; }
        public int? dwcPropioFuture { get; set; }
        public int? hhhFuture { get; set; }
        public int? hhhPropioFuture { get; set; }
        public int? futureTotal { get; set; }
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
//        public string? weekJobsSector { get; set; }
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