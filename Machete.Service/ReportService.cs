using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Data.Objects;
using System.Data.Objects.SqlClient;

using Machete.Domain;
using Machete.Domain.Entities;

using Machete.Data;
using Machete.Data.Infrastructure;

using Machete.Service.shared;

using NLog;

namespace Machete.Service
{
    // Other interfaces implement IService, which is a tool for writing a type of record.
    // No writing necessary here. Only reporting.

    public interface IReportService : IReportBase
    {
        dataTableResult<dailyData> DailyView(DateTime dclDate);
        dataTableResult<weeklyData> WeeklyView(DateTime wecDate);
        dataTableResult<monthlyData> monthlyView(DateTime mwdDate);
        dataTableResult<jzcData> jzcView(DateTime jzcDate);
    }

    public class ReportService : ReportBase, IReportService
    {

        // Repository declarations
        private readonly IWorkOrderRepository woRepo;
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IWorkerRequestRepository wrRepo;
        private readonly ILookupRepository lookRepo;

        // IoC
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
        /// A simple count of worker signins for a single day.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <returns></returns>
        public int CountSignins(DateTime beginDate)
        {
            int query = 0;
            var wsiQ = wsiRepo.GetAllQ();

            query = wsiQ.Where(whr => EntityFunctions.TruncateTime(whr.dateforsignin) == EntityFunctions.TruncateTime(beginDate)).Count();

            return query;
        }
        /// <summary>
        /// A simple count of worker signins for the given period.
        /// </summary>
        /// <param name="beginDate">DateTime, not null</param>
        /// <param name="endDate">DateTime, null</param>
        /// <returns>int</returns>
        public IQueryable<reportUnit> CountSignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            var wsiQ = wsiRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            // this seems to work fine, though in other methods
            // it's preferable to do truncation before
            // the GroupBy clause.
            query = wsiQ
                .Where(whr => whr.dateforsignin >= beginDate
                           && whr.dateforsignin <= endDate)
                .GroupBy(gb => EntityFunctions.TruncateTime(gb.dateforsignin))
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }

        /// <summary>
        /// A simple count of unique worker signins for a single day.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <returns></returns>
        public int CountUniqueSignins(DateTime beginDate)
        {
            //didn't actually do this part, set it up so compiler would hush
            int query = 0;
            var wsiQ = wsiRepo.GetAllQ();

            query = wsiQ.Where(whr => EntityFunctions.TruncateTime(whr.dateforsignin) == EntityFunctions.TruncateTime(beginDate)).Count();

            return query;
        }
        /// <summary>
        /// A simple count of worker signins for the given period.
        /// </summary>
        /// <param name="beginDate">DateTime, not null</param>
        /// <param name="endDate">DateTime, null</param>
        /// <returns>int</returns>
        public IQueryable<reportUnit> CountUniqueSignins(DateTime beginDate, DateTime endDate)
        {
            //didn't actually do this part, set it up so compiler would hush
            IQueryable<reportUnit> query;
            var wsiQ = wsiRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            // this seems to work fine, though in other methods
            // it's preferable to do truncation before
            // the GroupBy clause.
            query = wsiQ
                .Where(whr => whr.dateforsignin >= beginDate
                           && whr.dateforsignin <= endDate)
                .GroupBy(gb => EntityFunctions.TruncateTime(gb.dateforsignin))
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }


        public int CountAssignments(DateTime beginDate)
        {
            int query = 0;
            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);

            query = waQ
                .Join(woQ, wa => wa.workOrderID, wo => wo.ID, (wa, wo) => new
                {
                    wa,
                    date = EntityFunctions.TruncateTime(wo.dateTimeofWork)
                })
                .Where(whr => whr.date == beginDate)
                .Count();

            return query;
        }


        public IQueryable<reportUnit> CountAssignments(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            query = waQ
                .Join(woQ, wa => wa.workOrderID, wo => wo.ID, (wa, wo) => new
                {
                    wa,
                    date = EntityFunctions.TruncateTime(wo.dateTimeofWork)
                })
                .Where(whr => whr.date >= beginDate
                           && whr.date <= endDate)
                .GroupBy(gb => gb.date)
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }


        public int CountCancelled(DateTime beginDate)
        {
            int query = 0;
            var woQ = woRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);

            query = woQ.Where(whr => EntityFunctions.TruncateTime(whr.dateTimeofWork) == beginDate
                                  && whr.status == WorkOrder.iCancelled)
                                  .Count();
            return query;
        }

        public IQueryable<reportUnit> CountCancelled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            var woQ = woRepo.GetAllQ();

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            query = woQ.Where(whr => EntityFunctions.TruncateTime(whr.dateTimeofWork) == beginDate
                                  && whr.status == WorkOrder.iCancelled)
                .GroupBy(gb => EntityFunctions.TruncateTime(gb.dateTimeofWork))
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }
        /// <summary>
        /// Counts by type of dispatch (DWC, HHH, Propio/ea.)
        /// </summary>
        /// <param name="dateRequested">A single DateTime parameter</param>
        /// <returns>IQueryable</returns>
        public IQueryable<TypeOfDispatchReport> CountTypeofDispatch(DateTime beginDate, DateTime endDate)
        {
            IQueryable<TypeOfDispatchReport> query;

            var waQ = waRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var wrQ = wrRepo.GetAllQ();

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            //we need a left join of the second table because not everyone is requested
            //requests are "propio patron" orders whether or not Casa knows the employers' names.
            query = waQ
                .GroupJoin(wrQ,
                    wa => new { waid = (int)wa.workOrderID, waw = (int?)wa.workerAssignedID },
                    wr => new { waid = (int)wr.WorkOrderID, waw = (int?)wr.WorkerID },
                    (wa, wr) => new
                    {
                        wa,
                        reqOrderID = wr.FirstOrDefault().WorkOrderID,
                        reqWorkerID = wr.FirstOrDefault().WorkerID
                    })
                .Join(woQ, wr => wr.wa.workOrderID, wo => wo.ID,
                    (wr, wo) => new
                    {
                        wr,
                        timeOfWork = EntityFunctions.TruncateTime(wo.dateTimeofWork)
                    })
                .Join(wQ, wo => wo.wr.wa.workerAssignedID, w => w.ID,
                    (wo, w) => new
                    {
                        wo,
                        dwcList = w.typeOfWorkID == Worker.iDWC ? (wo.wr.reqWorkerID == w.ID ? 0 : 1) : 0,
                        hhhList = w.typeOfWorkID == Worker.iHHH ? (wo.wr.reqWorkerID == w.ID ? 0 : 1) : 0,
                        dwcPatron = w.typeOfWorkID == Worker.iDWC ? (wo.wr.reqWorkerID == w.ID ? 1 : 0) : 0,
                        hhhPatron = w.typeOfWorkID == Worker.iHHH ? (wo.wr.reqWorkerID == w.ID ? 1 : 0) : 0
                    })
                .Where(whr => whr.wo.timeOfWork >= beginDate
                           && whr.wo.timeOfWork <= endDate)
                .GroupBy(gb => gb.wo.timeOfWork)
                .Select(group => new TypeOfDispatchReport
                    {
                        date = group.Key ?? DateTime.Now, //this is annoying
                        dwcList = group.Sum(a => a.dwcList == null ? 0 : a.dwcList),
                        dwcPropio = group.Sum(a => a.dwcPatron == null ? 0 : a.dwcPatron),
                        hhhList = group.Sum(a => a.hhhList == null ? 0 : a.hhhList),
                        hhhPropio = group.Sum(a => a.hhhPatron == null ? 0 : a.hhhPatron),
                    })
                .OrderBy(fini => fini.date);

            return query;
        }

        /// <summary>
        /// Grabs a sum of hours and wages and averages them.
        /// </summary>
        /// <param name="beginDate">Start date for the query.</param>
        /// <param name="endDate">End date for the query.</param>
        /// <returns>IQueryable</returns>
        public IQueryable<AverageWages> HourlyWageAverage(DateTime beginDate, DateTime endDate)
        {
            IQueryable<AverageWages> query;

            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var wsiQ = wsiRepo.GetAllQ();

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            query = waQ
                .Join(woQ,
                    wa => wa.workOrderID,
                    wo => wo.ID,
                    (wa, wo) => new
                    {
                        wa,
                        woDate = EntityFunctions.TruncateTime(wo.dateTimeofWork)
                    })
                .Where(whr => whr.woDate >= beginDate
                           && whr.woDate <= endDate)
                .GroupBy(gb => gb.woDate)
                .Select(wec => new AverageWages
                          {
                              date = wec.Key ?? DateTime.Now,
                              hours = wec.Sum(wo => wo.wa.hours),
                              wages = wec.Sum(wo => wo.wa.hourlyWage * wo.wa.hours),
                              avg = wec.Sum(wo => wo.wa.hours) == 0 ? 0 : wec.Sum(wo => wo.wa.hourlyWage * wo.wa.hours) / wec.Sum(wo => wo.wa.hours)
                          }
                     );

            return query;
        }

        public IQueryable<reportUnit> ListJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = waQ
                .Join(woQ,
                    wa => wa.workOrderID,
                    wo => wo.ID,
                    (wa, wo) => new
                    {
                        wa,
                        workDate = EntityFunctions.TruncateTime(wo.dateTimeofWork)
                    })
                .Join(lQ,
                    wawo => wawo.wa.skillID,
                    l => l.ID,
                    (wawo, l) => new
                    {
                        wawo,
                        enText = l.text_EN
                    })
                .Where(whr => whr.wawo.workDate >= beginDate
                           && whr.wawo.workDate <= endDate)
                .GroupBy(gb => new { gb.enText, gb.wawo.workDate })
                .OrderByDescending(ob => ob.Key.workDate)
                .ThenByDescending(ob => ob.Count())
                .Select(group => new reportUnit
                {
                    date = group.Key.workDate,
                    count = group.Count() > 0 ? group.Count() : 0,
                    info = group.Key.enText ?? ""
                });

            return query;
        }

        public IQueryable<reportUnit> ListZipCodes(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = woQ
                .Where(whr => EntityFunctions.TruncateTime(whr.dateTimeofWork) >= beginDate
                           && EntityFunctions.TruncateTime(whr.dateTimeofWork) <= endDate)
                .GroupBy(gb => new
                {
                    dtow = EntityFunctions.TruncateTime(gb.dateTimeofWork),
                    zip = gb.zipcode
                })
                .OrderByDescending(ob => ob.Key.dtow)
                .ThenByDescending(ob => ob.Count())
                .Select(group => new reportUnit
                {
                    date = group.Key.dtow,
                    count = group.Count() > 0 ? group.Count() : 0,
                    info = group.Key.zip ?? ""
                });

            return query;
        }

        public dataTableResult<dailyData> DailyView(DateTime date)
        {
            IEnumerable<TypeOfDispatchReport> dclCurrent;
            IEnumerable<reportUnit> dailySignins;
            IEnumerable<reportUnit> dailyAssignments;
            IEnumerable<reportUnit> dailyCancelled;
            IEnumerable<dailyData> q;
            var result = new dataTableResult<dailyData>();

            DateTime beginDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59).AddDays(DateTime.DaysInMonth(date.Year, date.Month + 1));

            //call methods that return data for given date(s)
            dclCurrent = CountTypeofDispatch(beginDate, endDate).ToList();
            dailySignins = CountSignins(beginDate, endDate).ToList();
            dailyAssignments = CountAssignments(beginDate, endDate).ToList();
            dailyCancelled = CountCancelled(beginDate, endDate).ToList();

            q = dclCurrent
                .Select(group => new dailyData
                {
                    date = group.date,
                    dwcList = group.dwcList,
                    dwcPropio = group.dwcPropio,
                    hhhList = group.hhhList,
                    hhhPropio = group.hhhPropio,
                    totalSignins = dailySignins.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault(),
                    totalAssignments = dailyAssignments.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault(),
                    cancelledJobs = dailyCancelled.Count() > 0 ? dailyCancelled.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() : 0
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count(); //should theoretically be "1",
            result.query = q; //these need to stay set properly to be
            result.totalCount = waRepo.GetAllQ().Count(); // returned
            return result; // ...for DataTables.
        }

        public dataTableResult<weeklyData> WeeklyView(DateTime weekDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<AverageWages> weeklyWages;
            IEnumerable<reportUnit> weeklySignins;
            IEnumerable<reportUnit> weeklyAssignments;
            IEnumerable<reportUnit> weeklyJobsBySector;
            IEnumerable<weeklyData> q;
            var result = new dataTableResult<weeklyData>(); // note the type. define it well.

            beginDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 0, 0, 0).AddDays(-6);
            endDate = new DateTime(weekDate.Year, weekDate.Month, weekDate.Day, 23, 59, 59);

            weeklyWages = HourlyWageAverage(beginDate, endDate).ToList();
            weeklySignins = CountSignins(beginDate, endDate).ToList();
            weeklyAssignments = CountAssignments(beginDate, endDate).ToList();
            weeklyJobsBySector = ListJobs(beginDate, endDate).ToList();

            q = weeklyWages
                .Select(g => new weeklyData
                {
                    dayofweek = g.date.DayOfWeek,
                    date = g.date,
                    totalSignins = weeklySignins.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    noWeekJobs = weeklyAssignments.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    weekJobsSector = weeklyJobsBySector
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => a + b.info + " (" + b.count.ToString() + "), "),
                    weekEstDailyHours = g.hours,
                    weekEstPayment = g.wages,
                    weekHourlyWage = g.avg
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count();
            result.query = q;
            result.totalCount = waRepo.GetAllQ().Count();
            return result;
        }

        public dataTableResult<monthlyData> monthlyView(DateTime monthDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<reportUnit> signins;
            IEnumerable<TypeOfDispatchReport> dispatch;
            IEnumerable<AverageWages> average;
            IEnumerable<monthlyData> q; // query for monthlyWithDetail result
            var result = new dataTableResult<monthlyData>(); // note the type. define it well.

            beginDate = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(monthDate.Year, monthDate.Month, System.DateTime.DaysInMonth(monthDate.Year, monthDate.Month));

            signins = CountSignins(beginDate, endDate).ToList();
            dispatch = CountTypeofDispatch(beginDate, endDate).ToList();
            average = HourlyWageAverage(beginDate, endDate).ToList();

            q = average
                .Select(g => new monthlyData
                {
                    date = g.date,
                    totalSignins = signins.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    totalDWCSignins = dispatch.Where(whr => whr.date == g.date).Select(h => h.dwcList).FirstOrDefault(),
                    totalHHHSignins = dispatch.Where(whr => whr.date == g.date).Select(h => h.hhhList).FirstOrDefault(),
                    dispatchedDWCSignins = dispatch.Where(whr => whr.date == g.date).Select(h => h.dwcPropio).FirstOrDefault(),
                    dispatchedHHHSignins = dispatch.Where(whr => whr.date == g.date).Select(h => h.hhhPropio).FirstOrDefault(),
                    totalHours = g.hours,
                    totalIncome = g.wages,
                    avgIncomePerHour = g.avg
                });

            q = q.OrderBy(p => p.date);

            result.filteredCount = q.Count();
            // data should include one month and already be organized as such
            // dataTables rows can be defined at view level
            result.query = q;
            result.totalCount = waRepo.GetAllQ().Count();
            return result;
        }

        public dataTableResult<jzcData> jzcView(DateTime jzcDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<reportUnit> assignments;
            IEnumerable<reportUnit> topZips;
            IEnumerable<reportUnit> topJobs;
            IEnumerable<jzcData> q;
            var result = new dataTableResult<jzcData>(); // note the type. define it well.

            beginDate = new DateTime(jzcDate.Year, jzcDate.Month, jzcDate.Day, 0, 0, 0);
            endDate = new DateTime(jzcDate.Year, jzcDate.Month, jzcDate.Day, 23, 59, 59);

            assignments = CountAssignments(beginDate, endDate).ToList();
            topZips = ListZipCodes(beginDate, endDate).ToList();
            topJobs = ListJobs(beginDate, endDate).ToList();

            q = assignments
                .Select(g => new jzcData
                {
                    date = g.date,
                    jobs = topJobs
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => a + b.info + " (" + b.count.ToString() + "), "),
                    zips = topZips
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => a + b.info + " (" + b.count.ToString() + "), "),
                });

            q = q.OrderBy(ob => ob.date);

            result.filteredCount = q.Count();
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
    public class dailyData
    {
        public DateTime? date { get; set; }
        public int? dwcList { get; set; }
        public int? dwcPropio { get; set; }
        public int? hhhList { get; set; }
        public int? hhhPropio { get; set; }
        public int? totalSignins { get; set; }
        public int? cancelledJobs { get; set; }
        public int? totalAssignments { get; set; }
    }
    /// <summary>
    /// A class to contain the data for the Weekly Report for El Centro
    /// int totalSignins, int noWeekJobs, int weekJobsSector, int
    /// weekEstDailyHours, double weekEstPayment, double weekHourlyWage
    /// </summary>
    public class weeklyData
    {
        public DayOfWeek? dayofweek { get; set; }
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
    public class monthlyData
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

    public class jzcData
    {
        public DateTime? date { get; set; }
        public string jobs { get; set; }
        public string zips { get; set; }
    }

}