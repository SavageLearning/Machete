using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

using Machete.Domain;
using Machete.Domain.Entities;

using Machete.Data;
using Machete.Data.Infrastructure;

using NLog;

namespace Machete.Service
{
    #region public class ReportService (Interface and Constructor)
    public interface IReportService
    {
        IEnumerable<dailyData> DailyController(DateTime beginDate, DateTime endDate);
        IEnumerable<weeklyData> WeeklyController(DateTime beginDate, DateTime endDate);
        IEnumerable<monthlyData> MonthlySummaryController(DateTime beginDate, DateTime endDate);
        IEnumerable<TypeOfDispatchReport> MonthlyOrderController(DateTime beginDate, DateTime endDate);
        IEnumerable<yearSumData> YearlyController(DateTime beginDate, DateTime endDate);
        IEnumerable<jzcData> jzcController(DateTime beginDate, DateTime endDate);
        IEnumerable<newWorkerData> NewWorkerController(DateTime beginDate, DateTime endDate);
    }

    public class ReportService : IReportService
    {
        protected readonly IWorkOrderRepository woRepo;
        protected readonly IWorkAssignmentRepository waRepo;
        protected readonly IWorkerRepository wRepo;
        protected readonly IWorkerSigninRepository wsiRepo;
        protected readonly IWorkerRequestRepository wrRepo;
        protected readonly ILookupRepository lookRepo;
        protected readonly ILookupCache lookCache;
        protected readonly IActivitySigninRepository asRepo;

        public ReportService(IWorkOrderRepository woRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo,
                             IWorkerSigninRepository wsiRepo,
                             IWorkerRequestRepository wrRepo,
                             ILookupRepository lookRepo,
                             ILookupCache lookCache,
                             IActivitySigninRepository asRepo)
        {
            this.woRepo = woRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
            this.wsiRepo = wsiRepo;
            this.wrRepo = wrRepo;
            this.lookRepo = lookRepo;
            this.lookCache = lookCache;
            this.asRepo = asRepo;
        }

    #endregion

        #region BasicFunctions
        /// <summary>
        /// A simple count of worker signins for the given period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>IQueryable of type ReportUnit </returns>
        public IQueryable<reportUnit> CountSignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            var wsiQ = wsiRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            query = wsiQ
                .Where(whr => whr.dateforsignin >= beginDate
                           && whr.dateforsignin <= endDate)
                .GroupBy(gb => DbFunctions.TruncateTime(gb.dateforsignin))
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }

        /// <summary>
        /// A simple count of unduplicated worker signins for the given period.
        /// </summary>
        /// <param name="beginDate">DateTime, not null</param>
        /// <param name="endDate">DateTime, null</param>
        /// <returns>int</returns>
        public IQueryable<reportUnit> CountUniqueSignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            var wsiQ = wsiRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            query = wsiQ
                .Where(whr => whr.dateforsignin <= endDate)
                .GroupBy(gb => gb.dwccardnum)
                .Select(group => new {
                    dwccardnum = group.Key,
                    firstSignin = group.Min(m => m.dateforsignin)
                })
                .Where(whr => whr.firstSignin >= beginDate)
                .GroupBy(gb => DbFunctions.TruncateTime(gb.firstSignin))
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });
            //GO
            return query;
        }

        /// <summary>
        /// Counts work assignments for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
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
                    date = DbFunctions.TruncateTime(wo.dateTimeofWork)
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

        /// <summary>
        /// Counts cancelled orders for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<reportUnit> CountCancelled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            var woQ = woRepo.GetAllQ();

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            query = woQ.Where(whr => DbFunctions.TruncateTime(whr.dateTimeofWork) == beginDate
                                  && whr.status == WorkOrder.iCancelled)
                .GroupBy(gb => DbFunctions.TruncateTime(gb.dateTimeofWork))
                .Select(g => new reportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }

        /// <summary>
        /// Counts by type of dispatch (DWC, HHH, Propio/ea.). Very Casa Latina specific, but these
        /// numbers can also be used by other centers, especially where they have women's programs.
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
            var loD = lookCache.getByKeys("worktype", "DWC");
            var loH = lookCache.getByKeys("worktype", "HHH");

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

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
                        timeOfWork = DbFunctions.TruncateTime(wo.dateTimeofWork)
                    })
                .Join(wQ, wo => wo.wr.wa.workerAssignedID, w => w.ID,
                    (wo, w) => new
                    {
                        wo,
                        dwcList = w.typeOfWorkID == loD ? (wo.wr.reqWorkerID == w.ID ? 0 : 1) : 0,
                        hhhList = w.typeOfWorkID == loH ? (wo.wr.reqWorkerID == w.ID ? 0 : 1) : 0,
                        dwcPatron = w.typeOfWorkID == loD ? (wo.wr.reqWorkerID == w.ID ? 1 : 0) : 0,
                        hhhPatron = w.typeOfWorkID == loH ? (wo.wr.reqWorkerID == w.ID ? 1 : 0) : 0
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
        /// Grabs a sum of hours and wages and averages them for a given time period.
        /// </summary>
        /// <param name="beginDate">Start date for the query.</param>
        /// <param name="endDate">End date for the query.</param>
        /// <returns></returns>
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
                        woDate = DbFunctions.TruncateTime(wo.dateTimeofWork)
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

        /// <summary>
        /// Lists jobs in order of occurrence for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
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
                        workDate = DbFunctions.TruncateTime(wo.dateTimeofWork)
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

        /// <summary>
        /// Lists most popular zip codes for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<reportUnit> ListOrderZipCodes(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = woQ
                .Where(whr => DbFunctions.TruncateTime(whr.dateTimeofWork) >= beginDate
                           && DbFunctions.TruncateTime(whr.dateTimeofWork) <= endDate)
                .GroupBy(gb => new
                {
                    dtow = DbFunctions.TruncateTime(gb.dateTimeofWork),
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

        /// <summary>
        /// Returns the number of workers who received temporary work, excluding permanent placements, for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<reportUnit> WorkersInTempJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var waQ = waRepo.GetAllQ();

            query = waQ
                .Where(whr => !whr.workOrder.permanentPlacement && 
                    DbFunctions.TruncateTime(whr.workOrder.dateTimeofWork) >= beginDate &&
                    DbFunctions.TruncateTime(whr.workOrder.dateTimeofWork) <= endDate)
                //This is no good. We don't want one count for each date, we want one count for each worker.
                .GroupBy(gb => new
                {
                    dtow = DbFunctions.TruncateTime(gb.workOrder.dateTimeofWork),
                    worker = gb.workerAssignedID
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dtow,
                    count = group.Count() > 0 ? group.Count() : 0
                });

            return query;
        }

        /// <summary>
        /// Returns all activity signins for a given time period grouped by date, activityType and activityName.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, count, info (activityName), activityType</returns>
        public IQueryable<activityUnit> GetAllActivitySignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<activityUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var asQ = asRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = asQ.Join(lQ,
                    aj => aj.Activity.name,
                    lj => lj.ID,
                    (aj, lj) => new
                    {
                        name = lj.text_EN,
                        type = aj.Activity.type,
                        person = aj.person,
                        date = DbFunctions.TruncateTime(aj.dateforsignin)
                    })
                .Join(lQ,
                    aj => aj.type,
                    lj => lj.ID,
                    (aj, lj) => new
                    {
                        name = aj.name,
                        type = lj.text_EN,
                        person = aj.person,
                        date = aj.date
                    })
                .Where(signin => signin.person != null &&
                    DbFunctions.TruncateTime(signin.date) <= endDate &&
                    DbFunctions.TruncateTime(signin.date) >= beginDate)
                .GroupBy(gb => new { gb.date, gb.name, gb.type })
                .Select(grouping => new activityUnit
                {
                    info = grouping.Key.name,
                    activityType = grouping.Key.type,
                    date = grouping.Key.date,
                    count = grouping.Count()
                });
            return query;
        }
        
        /// <summary>
        /// Returns dates of completion, minutes completed, and the dwccardnum for all adults attending >12hrs. ESL.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, dwccardnum, minutesInClass</returns>
        public IQueryable<ESLAssessed> AdultsEnrolledAndAssessedInESL(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ESLAssessed> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var asQ = asRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Join(asQ,
                wqJoinOn => wqJoinOn.Person.ID,
                asqJoinOn => asqJoinOn.personID,
                (wqJoinOn, asqJoinOn) => new
                {
                    wqJoinOn,
                    dwc = wqJoinOn.dwccardnum,
                    aid = asqJoinOn.Activity.type,
                    name = asqJoinOn.Activity.name,
                    dfsi = asqJoinOn.dateforsignin,
                    time = asqJoinOn.Activity.dateEnd - asqJoinOn.Activity.dateStart
                })
                .Join(lQ,
                    wqasq => wqasq.name,
                    look => look.ID,
                    (wqasq, look) => new
                    {
                        inner = wqasq,
                        nameEN = look.text_EN
                    })
                .Where(whr => whr.nameEN.Contains("English Class"))
                .GroupBy(gb => new
                {
                    cardnum = gb.inner.dwc,
                })
                .Select(sel => new ESLAssessed
                {
                    date = sel.OrderByDescending(ai => ai.inner.dfsi).First().inner.dfsi,
                    dwccardnum = (int)sel.Key.cardnum,
                    minutesInClass = sel.Sum(s => s.inner.time.Minutes + (s.inner.time.Hours * 60))
                })
                .Where(sec => sec.minutesInClass >= 720);

            return query;
        }

        /// <summary>
        /// Returns a count of new members by date of membership.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, count</returns>
        public IQueryable<reportUnit> NewlyEnrolled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var dates = new List<DateTime>();
            for(var dt = beginDate; dt <= endDate; dt.AddDays(1))
                dates.Add(dt);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => DbFunctions.TruncateTime(whr.dateOfMembership) >= beginDate
                           && DbFunctions.TruncateTime(whr.dateOfMembership) <= endDate)
                .GroupBy(gb => new
                {
                    dom = DbFunctions.TruncateTime(gb.dateOfMembership)
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dom,
                    count = group.Count()
                });

            return query;
        }

        /// <summary>
        /// Returns a count of expired members by expiration date.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, count</returns>
        public IQueryable<reportUnit> NewlyExpired(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => DbFunctions.TruncateTime(whr.memberexpirationdate) >= beginDate
                           && DbFunctions.TruncateTime(whr.memberexpirationdate) <= endDate)
                .GroupBy(gb => new
                {
                    dom = DbFunctions.TruncateTime(gb.memberexpirationdate)
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dom,
                    count = group.Count()
                });

            return query;
        }

        /// <summary>
        /// Returns a count of continuing members, not including new members.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>info, count</returns>
        public IQueryable<reportUnit> StillEnrolled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => DbFunctions.TruncateTime(whr.dateOfMembership) < beginDate
                    && DbFunctions.TruncateTime(whr.memberexpirationdate) >= endDate
                    && !whr.isExpelled
                    && !whr.isSanctioned)
                .GroupBy(gb => gb.dwccardnum)
                .Select(group => new reportUnit
                {
                    info = group.Key.ToString(),
                    count = 1
                });

            return query;
        }

        /// <summary>
        /// Returns a count of unduplicated (first) signins for days in the given date range.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, count</returns>
        public IQueryable<reportUnit> UnduplicatedWorkersWhoRecievedTempJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var waQ = waRepo.GetAllQ();

            query = waQ
                .Where(whr => !whr.workOrder.permanentPlacement && 
                    DbFunctions.TruncateTime(whr.workOrder.dateTimeofWork) <= endDate)
                .GroupBy(gb => gb.workerAssigned.dwccardnum)
                .Select(group => new
                {
                    dwccardnum = group.Key,
                    firstTempAssignment = DbFunctions.TruncateTime(group.Min(m => m.workOrder.dateTimeofWork))
                })
                .Where(whr => whr.firstTempAssignment >= beginDate)
                .GroupBy(gb => gb.firstTempAssignment)
                .Select(group => new reportUnit
                {
                    date = group.Key,
                    count = group.Count()
                })
                .OrderBy(ob => ob.date);

            return query;
        }

        /// <summary>
        /// Returns workers placed in permanent jobs during the given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, count</returns>
        public IQueryable<reportUnit> WorkersPlacedInPermanentJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var waQ = waRepo.GetAllQ();

            query = waQ
                .Where(whr => whr.workOrder.permanentPlacement &&
                    DbFunctions.TruncateTime(whr.workOrder.dateTimeofWork) >= beginDate &&
                    DbFunctions.TruncateTime(whr.workOrder.dateTimeofWork) <= endDate)
                .GroupBy(gb => new
                {
                    dtow = DbFunctions.TruncateTime(gb.workOrder.dateTimeofWork),
                    worker = gb.workerAssignedID
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dtow,
                    count = group.Count() > 0 ? group.Count() : 0
                });

            return query;
        }

        /// <summary>
        /// This currently returns a percentage of how many people have zip codes in their profiles.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public double PersonZipCodeCompleteness(DateTime beginDate, DateTime endDate)
        {
            double query;

            var wQ = wRepo.GetAllQ().Where(whr => whr.memberexpirationdate >= beginDate && whr.dateOfMembership <= endDate);

            query = Math.Round((((double)wQ.Count() / (double)wQ.Where(zip => zip.Person.zipcode != null).Count()) * 100), 2, MidpointRounding.AwayFromZero);

            return query;
        }

        public IQueryable<reportUnit> SingleAdults(DateTime beginDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);

            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(worker => worker.memberexpirationdate >= beginDate
                    && !worker.livewithchildren)
                .Join(lQ, worker => worker.maritalstatus, lookup => lookup.ID, (worker, lookup) => lookup.text_EN)
                .Where(mStatus => mStatus != "Married")
                .GroupBy(gb => gb)
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }

        public IQueryable<reportUnit> NewlyEnrolledSingleAdults(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(worker => DbFunctions.TruncateTime(worker.dateOfMembership) >= beginDate 
                                && DbFunctions.TruncateTime(worker.dateOfMembership) <= endDate 
                                && !worker.livewithchildren)
                .Join(lQ, worker => worker.maritalstatus, lookup => lookup.ID, (worker, lookup) => lookup.text_EN)
                .Where(mStatus => mStatus != "Married")
                .GroupBy(gb => gb)
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }

        public IQueryable<reportUnit> FamilyHouseholds(DateTime beginDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);

            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(worker => worker.memberexpirationdate >= beginDate)
                .Join(lQ, worker => worker.maritalstatus, lookup => lookup.ID, (worker, lookup) => new { status = lookup.text_EN, children = worker.livewithchildren })
                .Where(worker => worker.status == "Married" || worker.children)
                .GroupBy(gb => gb)
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }

        public IQueryable<reportUnit> NewlyEnrolledFamilyHouseholds(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(worker => worker.memberexpirationdate > beginDate)
                .Where(whr => DbFunctions.TruncateTime(whr.dateOfMembership) <= endDate &&
                              DbFunctions.TruncateTime(whr.dateOfMembership) >= beginDate)
                .Join(lQ, worker => worker.maritalstatus, lookup => lookup.ID, (worker, lookup) => new { status = lookup.text_EN, children = worker.livewithchildren })
                .Where(worker => worker.status == "Married" || worker.children)
                .GroupBy(gb => gb)
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }

        /// <summary>
        /// newWorkerData is 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, singleAdults, familyHouseholds, newSingleAdults, newFamilyHouseholds, zipCodeCompleteness</returns>
        public newWorkerData NewWorkers(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<reportUnit> singleAdultsTotal;
            IEnumerable<reportUnit> familyHouseholdsTotal;
            IEnumerable<reportUnit> singleAdultsNewlyEnrolled;
            IEnumerable<reportUnit> familyHouseholdsNewlyEnrolled;
            double zipCodes;

            newWorkerData q;

            singleAdultsTotal = SingleAdults(beginDate).ToList();
            familyHouseholdsTotal = FamilyHouseholds(beginDate).ToList();
            singleAdultsNewlyEnrolled = NewlyEnrolledSingleAdults(beginDate, endDate).ToList();
            familyHouseholdsNewlyEnrolled = NewlyEnrolledFamilyHouseholds(beginDate, endDate).ToList();
            zipCodes = PersonZipCodeCompleteness(beginDate, endDate);

            q = new newWorkerData();
            q.date = endDate;
            q.singleAdults = familyHouseholdsNewlyEnrolled.First().count ?? 0;
            q.familyHouseholds = singleAdultsNewlyEnrolled.First().count ?? 0;
            q.newSingleAdults = familyHouseholdsTotal.First().count ?? 0;
            q.newFamilyHouseholds = singleAdultsTotal.First().count ?? 0;
            q.zipCodeCompleteness = zipCodes;

            return q;
        }

        public IQueryable<reportUnit> ClientProfileZipCode(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.Person != null && whr.Person.zipcode != null)
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(grp => grp.Person.zipcode)
                .Select(group => new reportUnit
                {
                    count = group.Count(),
                    info = group.Key
                });

            return query;

        }

        public IQueryable<reportUnit> ClientProfileHomeless(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(grp => grp.homeless)
                .Select(group => new reportUnit
                {
                    info = group.Key == null ? "Unknown" : group.Key.ToString(),
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<reportUnit> ClientProfileHouseholdComposition(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(grp => new
                {
                    maritalStatus = grp.maritalstatus,
                    withChildren = grp.livewithchildren
                })
                .Join(lQ,
                    gJoin => gJoin.Key.maritalStatus,
                    lJoin => lJoin.ID,
                    (gJoin, lJoin) => new
                    {
                        maritalStatus = lJoin.text_EN,
                        withChildren = gJoin.Key.withChildren ? "With Children" : "Without Children",
                        count = gJoin.Count()
                    })
                .Select(glJoin => new reportUnit
                {
                    info = glJoin.maritalStatus + ", " + glJoin.withChildren,
                    count = glJoin.count
                });

            return query;
        }

        public IQueryable<reportUnit> ClientProfileIncome(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .Join(lQ,
                    wJoin => wJoin.incomeID,
                    lJoin => lJoin.ID,
                    (gJoin, lJoin) => new
                    {
                        incomeLevel = lJoin.text_EN == "Less than $15,000" ? "Very low (< 30% median)" :
                                        lJoin.text_EN == "Between $15,000 and $25,000" ? "Moderate (> 50% median)" :
                                        lJoin.text_EN != "unknown" ? "Above moderate (> 80% median)" :
                                        "Unknown"
                    })
                .GroupBy(grp => grp.incomeLevel)
                .Select(group => new reportUnit
                {
                    info = group.Key,
                    count = group.Count()
                })
                .OrderBy(ob => ob.count);

            return query;
        }

        public IQueryable<reportUnit> ClientProfileWorkerAge(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .Select(worker => new
                {
                    age = (new DateTime(1753, 1, 1) + (DateTime.Now - worker.dateOfBirth)).Year - 1753,
                    dob = worker.dateOfBirth
                })
                .Select(ageO => ageO.dob == new DateTime(1900, 1, 1) ? "Unknown" :
                                ageO.age <= 5 ? "0 to 5 years" :
                                ageO.age <= 12 ? "6 to 12 years" :
                                ageO.age <= 18 ? "13 to 18 years" :
                                ageO.age <= 29 ? "19 to 29 years" :
                                ageO.age <= 45 ? "30 to 45 years" :
                                ageO.age <= 64 ? "46 to 64 years" :
                                ageO.age <= 84 ? "65 to 84 years" :
                                "85+ years")
                .GroupBy(ageCategory => ageCategory)
                .Select(group => new reportUnit
                {
                    info = group.Key,
                    count = group.Count()
                });

            return query;
			
        }

        public IQueryable<reportUnit> ClientProfileGender(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.Person.gender)
                .Join(lQ,
                    group => group.Key,
                    look => look.ID,
                    (group, look) => new reportUnit
                    {
                        count = group.Count(),
                        info = look.text_EN
                    });

            return query;
        }

        public IQueryable<reportUnit> ClientProfileHasDisability(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.disabled)
                .Select(group => new reportUnit
                {
                    info = group.Key ? "Yes" : "No",
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<reportUnit> ClientProfileRaceEthnicity(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.RaceID)
                .Join(lQ,
                    gJoin => gJoin.Key,
                    lJoin => lJoin.ID,
                    (gJoin, lJoin) => new
                    {
                        race = lJoin.text_EN,
                        count = gJoin.Count()
                    })
                .Select(glJoin => new reportUnit
                {
                    info = glJoin.race,
                    count = glJoin.count
                });

            return query;
        }

        public IQueryable<reportUnit> ClientProfileRefugeeImmigrant(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.immigrantrefugee)
                .Select(group => new reportUnit
                {
                    info = group.Key ? "Yes" : "No",
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<reportUnit> ClientProfileEnglishLevel(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.englishlevelID)
                .Select(group => new reportUnit
                {
                    info = "English " + group.Key,
                    count = group.Count()
                })
                .OrderBy(ob => ob.info);

            return query;
        }

        #endregion

        #region ReportData

        /// <summary>
        /// Controller for daily summary report.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dailyData> DailyController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<TypeOfDispatchReport> dclCurrent;
            IEnumerable<reportUnit> dailySignins;
            IEnumerable<reportUnit> dailyUnique;
            IEnumerable<reportUnit> dailyAssignments;
            IEnumerable<reportUnit> dailyCancelled;
            IEnumerable<dailyData> q;

            dclCurrent = CountTypeofDispatch(beginDate, endDate).ToList();
            dailySignins = CountSignins(beginDate, endDate).ToList();
            dailyUnique = CountUniqueSignins(beginDate, endDate).ToList();
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
                    uniqueSignins = dailyUnique.Where(whr => whr.date == group.date).Select(g => g.count).First() ?? 0,
                    totalSignins = dailySignins.Where(whr => whr.date == group.date).Select(g => g.count).First() ?? 0,
                    totalAssignments = dailyAssignments.Where(whr => whr.date == group.date).Select(g => g.count).First() ?? 0,
                    cancelledJobs = dailyCancelled.Where(whr => whr.date == group.date).Select(g => g.count).First() ?? 0
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        /// <summary>
        /// Controller for weekly summary report.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<weeklyData> WeeklyController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<AverageWages> weeklyWages;
            IEnumerable<reportUnit> weeklySignins;
            IEnumerable<reportUnit> weeklyAssignments;
            IEnumerable<reportUnit> weeklyJobs;
            IEnumerable<weeklyData> q;

            weeklyWages = HourlyWageAverage(beginDate, endDate).ToList();
            weeklySignins = CountSignins(beginDate, endDate).ToList();
            weeklyAssignments = CountAssignments(beginDate, endDate).ToList();
            weeklyJobs = ListJobs(beginDate, endDate).ToList();

            q = weeklyWages
                .Select(g => new weeklyData
                {
                    dayofweek = g.date.DayOfWeek,
                    date = g.date,
                    totalSignins = weeklySignins.Where(whr => whr.date == g.date).Select(h => h.count).First() ?? 0,
                    noWeekJobs = weeklyAssignments.Where(whr => whr.date == g.date).Select(h => h.count).First() ?? 0,
                    weekEstDailyHours = g.hours,
                    weekEstPayment = g.wages,
                    weekHourlyWage = g.avg,
                    topJobs = weeklyJobs.Where(whr => whr.date == g.date)
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        //it's just a space that changed at the bottom
        public IEnumerable<monthlyData> MonthlySummaryController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<reportUnit> signins;
            IEnumerable<reportUnit> unique;
            IEnumerable<reportUnit> dispatch;
            IEnumerable<AverageWages> average;

            IEnumerable<reportUnit> newPpl;
            IEnumerable<reportUnit> pplLeft;
            IEnumerable<reportUnit> pplStay;
            IEnumerable<reportUnit> undupDispatch;
            IEnumerable<reportUnit> permPlaced;

            IEnumerable<activityUnit> getAllClassAttendance;

            IEnumerable<monthlyData> q; 

            IEnumerable<DateTime> getAllDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                    .Select(offset => beginDate.AddDays(offset))
                    .ToArray(); 

            getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            signins = CountSignins(beginDate, endDate).ToList();
            unique = CountUniqueSignins(beginDate, endDate).ToList();
            dispatch = CountAssignments(beginDate, endDate).ToList();
            average = HourlyWageAverage(beginDate, endDate).ToList();
            newPpl = NewlyEnrolled(beginDate, endDate).ToList();
            pplLeft = NewlyExpired(beginDate, endDate).ToList();

            undupDispatch = WorkersInTempJobs(beginDate, endDate).ToList();
            pplStay = StillEnrolled(beginDate, endDate).ToList();
            permPlaced = WorkersPlacedInPermanentJobs(beginDate, endDate).ToList();

            q = getAllDates
                .Select(g => new monthlyData
                {
                    date = DbFunctions.TruncateTime(g) ?? endDate,
                    totalSignins = signins.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    uniqueSignins = unique.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    dispatched = dispatch.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    unduplicatedDispatched = undupDispatch.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    totalHours = average.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.hours).First(),
                    totalIncome = average.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.wages).First(),
                    avgIncomePerHour = average.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.avg).First(),
                    newlyEnrolled = newPpl.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    peopleWhoLeft = pplLeft.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    peopleWhoStayed = pplStay,
                    peopleWhoWentToClass = getAllClassAttendance.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    permanentPlacements = permPlaced.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        public IEnumerable<TypeOfDispatchReport> MonthlyOrderController(DateTime beginDate, DateTime endDate)
        {            
            IEnumerable<TypeOfDispatchReport> dispatch;

            dispatch = CountTypeofDispatch(beginDate, endDate).ToList();

            var result = dispatch.Select(g => new TypeOfDispatchReport
                {
                    date = g.date,
                    dwcList = dispatch.Where(whr => whr.date == DbFunctions.TruncateTime(g.date)).Select(h => h.dwcList).First(),
                    hhhList = dispatch.Where(whr => whr.date == DbFunctions.TruncateTime(g.date)).Select(h => h.hhhList).First(),
                    dwcPropio = dispatch.Where(whr => whr.date == DbFunctions.TruncateTime(g.date)).Select(h => h.dwcPropio).First(),
                    hhhPropio = dispatch.Where(whr => whr.date == DbFunctions.TruncateTime(g.date)).Select(h => h.hhhPropio).First(),
                });

            return result;
        }

        public IEnumerable<monthlyActivityData> MonthlyActivityController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<activityUnit> getAllClassAttendance;
            IEnumerable<reportUnit> finLit;
            IEnumerable<reportUnit> jobSkills;
            IEnumerable<ESLAssessed> eslGrads;

            IEnumerable<DateTime> getAllDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                .Select(offset => beginDate.AddDays(offset))
                .ToArray(); 

            getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            jobSkills = getAllClassAttendance.Where(skills => skills.activityType == "Skills Training");
            finLit = getAllClassAttendance.Where(fin => fin.info == "Financial Education");
            eslGrads = AdultsEnrolledAndAssessedInESL(beginDate, endDate).ToList();

            var result = getAllDates
                .Select(g => new monthlyActivityData
                    {
                        financialLiterates = finLit.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                        jobSkillsTrainees = jobSkills.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                        gradFromESL = eslGrads.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Count()
                    }
                );

            return result;
        }

        public IEnumerable<yearSumData> YearlyController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<reportUnit> temporaryPlacements;
            IEnumerable<activityUnit> safetyTrainees;
            IEnumerable<activityUnit> skillsTrainees;
            IEnumerable<ESLAssessed> eslAssessed;
            IEnumerable<reportUnit> basicGardenTrainees;
            IEnumerable<reportUnit> advGardenTrainees;
            IEnumerable<reportUnit> finTrainees;

            IEnumerable<activityUnit> getAllClassAttendance;
            
            IEnumerable<yearSumData> q;

            IEnumerable<DateTime> getAllDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                    .Select(offset => beginDate.AddDays(offset))
                    .ToArray(); 

            temporaryPlacements = WorkersInTempJobs(beginDate, endDate).ToList();
            getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            safetyTrainees = getAllClassAttendance.Where(safety => safety.activityType == "Health & Safety");
            skillsTrainees = getAllClassAttendance.Where(skills => skills.activityType == "Skills Training" || skills.activityType == "Leadership Development");
            eslAssessed = AdultsEnrolledAndAssessedInESL(beginDate, endDate).ToList();
            basicGardenTrainees = getAllClassAttendance.Where(basic => basic.info == "Basic Gardening");
            advGardenTrainees = getAllClassAttendance.Where(adv => adv.info == "Advanced Gardening");
            finTrainees = getAllClassAttendance.Where(fin => fin.info == "Financial Education");

            q = getAllDates
                .Select(g => new yearSumData
                {
                    date = DbFunctions.TruncateTime(g),
                    temporaryPlacements = temporaryPlacements.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    safetyTrainees = safetyTrainees.Where(whr => whr.date == DbFunctions.TruncateTime(g)),
                    skillsTrainees = skillsTrainees.Where(whr => whr.date == DbFunctions.TruncateTime(g)),
                    eslAssessed = eslAssessed.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Count(),
                    basicGardenTrainees = basicGardenTrainees.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    advGardenTrainees = advGardenTrainees.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0,
                    finTrainees = finTrainees.Where(whr => whr.date == DbFunctions.TruncateTime(g)).Select(h => h.count).First() ?? 0
                });

            return q;
        }

        public IEnumerable<newWorkerData> NewWorkerController(DateTime beginDate, DateTime endDate)
        {
            List<newWorkerData> query;

            query = new List<newWorkerData>();

            for (var i = 0; i <= 4; ++i)
            {
                beginDate = beginDate.AddMonths(-3);
                var cont = NewWorkers(beginDate, endDate);
                query.Add(cont);
                endDate = endDate.AddMonths(-3);
            }

            return query.AsEnumerable();
        }


        public IEnumerable<reportUnit> ClientProfileReportController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<reportUnit> zipcodes;
            IEnumerable<reportUnit> homeless;
            IEnumerable<reportUnit> householdComposition;
            IEnumerable<reportUnit> income;
            IEnumerable<reportUnit> age;
            IEnumerable<reportUnit> gender;
            IEnumerable<reportUnit> disabilities;
            IEnumerable<reportUnit> race;
            IEnumerable<reportUnit> refugeeImmigrant;
            IEnumerable<reportUnit> englishLevel;
            IEnumerable<reportUnit> q;

            zipcodes = ClientProfileZipCode(beginDate, endDate).ToList();
            homeless = ClientProfileHomeless(beginDate, endDate).ToList();
            householdComposition = ClientProfileHouseholdComposition(beginDate, endDate).ToList();
            income = ClientProfileIncome(beginDate, endDate).ToList();
            age = ClientProfileWorkerAge(beginDate, endDate).ToList();
            gender = ClientProfileGender(beginDate, endDate).ToList();
            disabilities = ClientProfileHasDisability(beginDate, endDate).ToList();
            race = ClientProfileRaceEthnicity(beginDate, endDate).ToList();
            refugeeImmigrant = ClientProfileRefugeeImmigrant(beginDate, endDate).ToList();
            englishLevel = ClientProfileEnglishLevel(beginDate, endDate).ToList();

            q = zipcodes.Concat(homeless.Concat(householdComposition.Concat(income.Concat(age.Concat(gender.Concat(disabilities.Concat(race.Concat(refugeeImmigrant.Concat(englishLevel)))))))));
            return q;
        }

        /// <summary>
        /// Jobs and Zip Codes controller. The jobs and zip codes report was
        /// initially requested by Mountain View and centers can see what their 
        /// orders are and where they're coming from.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<jzcData> jzcController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<reportUnit> assignments;
            IEnumerable<reportUnit> topZips;
            IEnumerable<reportUnit> topJobs;
            IEnumerable<jzcData> q;

            assignments = CountAssignments(beginDate, endDate).ToList();
            topZips = ListOrderZipCodes(beginDate, endDate).ToList();
            topJobs = ListJobs(beginDate, endDate).ToList();

            q = assignments
                .Select(g => new jzcData
                {
                    date = g.date ?? endDate,
                    zips = topZips
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => b.info + ", " + a),
                    zipsCount = topZips
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => b.count.ToString() + ", " + a),
                    jobs = topJobs
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => b.info + ", " + a),
                    jobsCount = topJobs
                        .Where(whr => whr.date == g.date)
                        .Aggregate("", (a, b) => b.count.ToString() + ", " + a)
                });

            q = q.OrderBy(ob => ob.date);

            return q;
        }

    }
    #endregion

    #region Report Models

    public class TypeOfDispatchReport
    {
        public DateTime date { get; set; }
        public int dwcList { get; set; }
        public int dwcPropio { get; set; }
        public int hhhList { get; set; }
        public int hhhPropio { get; set; }
    }

    public class AverageWages
    {
        public DateTime date { get; set; }
        public int hours { get; set; }
        public double wages { get; set; }
        public double avg { get; set; }
    }

    public class ESLAssessed
    {
        public DateTime date { get; set; }
        public int dwccardnum { get; set; }
        public int minutesInClass { get; set; }
    }

    public class activityUnit : reportUnit
    {
        public string activityType { get; set; }
    }

    public class reportUnit
    {
        public DateTime? date { get; set; }
        public int? count { get; set; }
        public string info { get; set; }
    }

    public class dailyData
    {
        public DateTime date { get; set; }
        public int dwcList { get; set; }
        public int dwcPropio { get; set; }
        public int hhhList { get; set; }
        public int hhhPropio { get; set; }
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int cancelledJobs { get; set; }
        public int totalAssignments { get; set; }
    }
    /// <summary>
    /// A class to contain the data for the Weekly Report for El Centro
    /// int totalSignins, int noWeekJobs, int weekJobsSector, int
    /// weekEstDailyHours, double weekEstPayment, double weekHourlyWage
    /// </summary>
    public class weeklyData
    {
        public DayOfWeek dayofweek { get; set; }
        public DateTime date { get; set; }
        public int totalSignins { get; set; }
        public int noWeekJobs { get; set; }
        public int weekEstDailyHours { get; set; }
        public double weekEstPayment { get; set; }
        public double weekHourlyWage { get; set; }
        public IEnumerable<reportUnit> topJobs { get; set; }
    }

    /// <summary>
    /// A class containing all of the data for the Monthly Report with Details
    /// DateTime date, int totalDWCSignins, int totalHHHSignins
    /// dispatchedDWCSignins, int dispatchedHHHSignins
    /// </summary>
    public class monthlyData
    {
        public DateTime date { get; set; }
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int dispatched { get; set; }
        public int totalHours { get; set; }
        public double totalIncome { get; set; }
        public double avgIncomePerHour { get; set; }
        public int newlyEnrolled { get; set; }
        public int peopleWhoLeft { get; set; }
        public IEnumerable<reportUnit> peopleWhoStayed { get; set; }
        public int peopleWhoWentToClass { get; set; }
        public int unduplicatedDispatched { get; set; }
        public int permanentPlacements { get; set; }
    }

    public class monthlyActivityData
    {
        public int financialLiterates { get; set; }
        public int jobSkillsTrainees { get; set; }
        public int gradFromESL { get; set; }
    }

    public class jzcData
    {
        public DateTime date { get; set; }
        public string zips { get; set; }
        public string zipsCount { get; set; }
        public string jobs { get; set; }
        public string jobsCount { get; set; }
    }

    public class newWorkerData
    {
        public DateTime? date { get; set; }
        public int singleAdults { get; set; }
        public int familyHouseholds { get; set; }
        public int newSingleAdults { get; set; }
        public int newFamilyHouseholds { get; set; }
        public double zipCodeCompleteness { get; set; }
    }

    public class yearSumData
    {
        public DateTime? date { get; set; }
        public int temporaryPlacements { get; set; }
        public IEnumerable<activityUnit> safetyTrainees { get; set; }
        public IEnumerable<activityUnit> skillsTrainees { get; set; }
        public int eslAssessed { get; set; }
        public int basicGardenTrainees { get; set; }
        public int advGardenTrainees { get; set; }
        public int finTrainees { get; set; }
    }

    #endregion
}