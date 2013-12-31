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
    //No writing necessary. Only fetching data.
    public interface IReportService : IReportBase
    {
        dataTableResult<dailyData> DailyView(DateTime dclDate);
        dataTableResult<weeklyData> WeeklyView(DateTime wecDate);
        dataTableResult<monthlyData> monthlyView(DateTime mwdDate);
        dataTableResult<yearSumData> yearlyView(DateTime yDate);
        dataTableResult<jzcData> jzcView(DateTime jzcDate);
    }

    public class ReportService : ReportBase, IReportService
    {
        public ReportService(IWorkOrderRepository woRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo,
                             IWorkerSigninRepository wsiRepo,
                             IWorkerRequestRepository wrRepo,
                             ILookupRepository lookRepo,
                             ILookupCache lookCache,
                             IActivitySigninRepository asRepo) : base(woRepo, waRepo, wRepo, wsiRepo, wrRepo, lookRepo, lookCache, asRepo)
        {}
        
        /// <summary>
        /// A simple count of worker signins for a single day.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <returns></returns>

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
        /// A simple count of worker signins for the given period.
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

            //;WITH foo AS
            //( FROM dbo.WorkerSignins
            query = wsiQ
            // WHERE dateforsignin <= endDate
                .Where(whr => whr.dateforsignin <= endDate)
            // GROUP BY dwccardnum
                .GroupBy(gb => gb.dwccardnum)
            // SELECT dwccardnum, MIN(dateforsignin) AS firstSignin
                .Select(group => new {
                    dwccardnum = group.Key,
                    firstSignin = group.Min(m => m.dateforsignin)
                })
            //)
            // FROM foo
            // WHERE firstSignin >= beginDate
                .Where(whr => whr.firstSignin >= beginDate)
            // GROUP BY fistSignin
                .GroupBy(gb => EntityFunctions.TruncateTime(gb.firstSignin))
            // SELECT firstSignin as date, COUNT(dwccardnum) AS count;
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
        /// <summary>
        /// Lists most popular zip codes for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
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

        #region United Way (Yearly Summary) Report

        //This way of organizing the data is becoming nightmarish. We need some new objects and drilldowns. See
        //weekly report for an example.
        public IQueryable<reportUnit> WorkersInTempJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var waQ = waRepo.GetAllQ();

            query = waQ
                .Where(whr => !whr.workOrder.permanentPlacement && 
                    EntityFunctions.TruncateTime(whr.workOrder.dateTimeofWork) >= beginDate &&
                    EntityFunctions.TruncateTime(whr.workOrder.dateTimeofWork) <= endDate)
                .GroupBy(gb => new
                {
                    dtow = EntityFunctions.TruncateTime(gb.workOrder.dateTimeofWork),
                    worker = gb.workerAssignedID
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dtow,
                    count = group.Count() > 0 ? group.Count() : 0
                });

            return query;
        }

        public IQueryable<activityUnit> GetAllActivitySignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<activityUnit> query;

            //ensure we are getting all relevant times (no assumptions)
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
                        date = EntityFunctions.TruncateTime(aj.dateforsignin)
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
                    EntityFunctions.TruncateTime(signin.date) <= endDate &&
                    EntityFunctions.TruncateTime(signin.date) >= beginDate)
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
        public IQueryable<ESLAssessed> AdultsEnrolledAndAssessedInESL(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ESLAssessed> query;

            //ensure we are getting all relevant times (no assumptions)
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
                    pid = asqJoinOn.personID,
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
                        wqasq,
                        tEN = look.text_EN
                    })
                .Join(lQ,
                    wqasq => wqasq.wqasq.aid,
                    look => look.ID,
                    (wqasq, look) => new
                    {
                        inner = wqasq.wqasq,
                        typeEN = look.text_EN,
                        nameEN = wqasq.tEN
                    })
                .Where(whr => whr.nameEN == "English Class 1" || whr.nameEN == "English Class 2")
                .GroupBy(gb => new
                {
                    person = gb.inner.pid,
                })
                .Select(sel => new ESLAssessed
                {
                    date = sel.First().inner.dfsi,
                    personID = (int)sel.Key.person,
                    minutesInClass = sel.Sum(s => s.inner.time.Minutes + (s.inner.time.Hours * 60))
                });
            return query;
        }


        #endregion

        #region Financial Education
        
        public IQueryable<reportUnit> WorkersReceivingIntroToLoans(DateTime beginDate, DateTime endDate)
        {
            return null;
        }

        public IQueryable<reportUnit> CLTraining(DateTime beginDate, DateTime endDate)
        {
            return null;
        }

        #endregion

        #endregion

        #region Monthly Status Report (New)
        public IQueryable<reportUnit> NewlyEnrolled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => EntityFunctions.TruncateTime(whr.dateOfMembership) >= beginDate
                           && EntityFunctions.TruncateTime(whr.dateOfMembership) <= endDate)
                .GroupBy(gb => new
                {
                    dom = EntityFunctions.TruncateTime(gb.dateOfMembership)
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dom,
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<reportUnit> NewlyExpired(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => EntityFunctions.TruncateTime(whr.memberexpirationdate) >= beginDate
                           && EntityFunctions.TruncateTime(whr.memberexpirationdate) <= endDate)
                .GroupBy(gb => new
                {
                    dom = EntityFunctions.TruncateTime(gb.memberexpirationdate)
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dom,
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<reportUnit> StillEnrolled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => EntityFunctions.TruncateTime(whr.dateOfMembership) < beginDate)
                .GroupBy(gb => new
                {
                    dom = EntityFunctions.TruncateTime(gb.dateOfMembership)
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dom,
                    count = group.Count()
                });

            return query;
        }

        // Financial Literacy, Job Skills Training and ESL graduates already covered in yearly summary report f(x)s

        public IQueryable<reportUnit> UnduplicatedWorkersWhoRecievedTempJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var waQ = waRepo.GetAllQ();

            query = waQ
                .Where(whr => !whr.workOrder.permanentPlacement && 
                    EntityFunctions.TruncateTime(whr.workOrder.dateTimeofWork) <= endDate &&
                    EntityFunctions.TruncateTime(whr.workOrder.dateTimeofWork) >= beginDate)
                .GroupBy(gb => gb.workerAssignedID)
                .Select(group => new
                {
                    worker = group.Key,
                    firstTempAssignment = EntityFunctions.TruncateTime(group.Min(m => m.workOrder.dateTimeofWork))
                })
                .GroupBy(gb => gb.firstTempAssignment)
                .Select(group => new reportUnit
                {
                    count = group.Count(),
                    date = group.Key
                })
                .OrderBy(ob => ob.date);

            return query;
        }

        public IQueryable<reportUnit> WorkersPlacedInPermanentJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            //ensure we are getting all relevant times (no assumptions)
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var waQ = waRepo.GetAllQ();

            query = waQ
                .Where(whr => whr.workOrder.permanentPlacement &&
                    EntityFunctions.TruncateTime(whr.workOrder.dateTimeofWork) >= beginDate &&
                    EntityFunctions.TruncateTime(whr.workOrder.dateTimeofWork) <= endDate)
                .GroupBy(gb => new
                {
                    dtow = EntityFunctions.TruncateTime(gb.workOrder.dateTimeofWork),
                    worker = gb.workerAssignedID
                })
                .Select(group => new reportUnit
                {
                    date = group.Key.dtow,
                    count = group.Count() > 0 ? group.Count() : 0
                });

            return query;
        }


        #endregion

        #region HMIS (Worker Demographics Report--Quarterly)
        public IQueryable<reportUnit> PersonZipCodePercentages()
        {
            IQueryable<reportUnit> query;

            var wQ = wRepo.GetAllQ();
            int numWorkers = wQ.Where(whr => whr.dateOfMembership != null).Count();

            query = wQ
                .GroupBy(gb => new
                {
                    zip = gb.Person.zipcode
                })
                .OrderByDescending(ob => ob.Count())
                .Select(group => new reportUnit
                {
                    count = ((group.Count() >= 0 ? group.Count() : 0) * 100) / numWorkers,
                    info = group.Key.zip ?? ""
                });

            return query;
        }
        public IQueryable<reportUnit> SingleAdults()
        {
            IQueryable<reportUnit> query;
            IQueryable<int> marriedIDQuery;

            var lQ = lookRepo.GetAllQ();
            marriedIDQuery = lQ
                .Where(whr => whr.text_EN == "Married")
                .Select(thing => thing.ID);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.maritalstatus != marriedIDQuery.First())
                .GroupBy(gb => new
                {
                    maritalStatus = gb.maritalstatus
                })
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }
        public IQueryable<reportUnit> NewlyEnrolledSingleAdults(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            IQueryable<int> marriedIDQuery;

            var lQ = lookRepo.GetAllQ();
            marriedIDQuery = lQ
                .Where(whr => whr.text_EN == "Married")
                .Select(thing => thing.ID);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.maritalstatus != marriedIDQuery.First())
                .Where(whr => EntityFunctions.TruncateTime(whr.dateOfMembership) <= endDate &&
                              EntityFunctions.TruncateTime(whr.dateOfMembership) >= beginDate)
                .GroupBy(gb => new
                {
                    maritalStatus = gb.maritalstatus
                })
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }
        public IQueryable<reportUnit> FamilyHouseholds()
        {
            IQueryable<reportUnit> query;
            IQueryable<int> marriedIDQuery;

            var lQ = lookRepo.GetAllQ();
            marriedIDQuery = lQ
                .Where(whr => whr.text_EN == "Married")
                .Select(thing => thing.ID);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.maritalstatus == marriedIDQuery.First())
                .GroupBy(gb => new
                {
                    maritalStatus = gb.maritalstatus
                })
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }
        public IQueryable<reportUnit> NewlyEnrolledFamilyHouseholds(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;
            IQueryable<int> marriedIDQuery;

            var lQ = lookRepo.GetAllQ();
            marriedIDQuery = lQ
                .Where(whr => whr.text_EN == "Married")
                .Select(thing => thing.ID);

            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.maritalstatus == marriedIDQuery.First())
                .Where(whr => EntityFunctions.TruncateTime(whr.dateOfMembership) <= endDate &&
                              EntityFunctions.TruncateTime(whr.dateOfMembership) >= beginDate)
                .GroupBy(gb => new
                {
                    maritalStatus = gb.maritalstatus
                })
                .Select(group => new reportUnit
                {
                    count = group.Count() >= 0 ? group.Count() : 0
                });

            return query;
        }

        //Demographic Standards go here -- need objects besides reportUnit so that we can group and do drilldowns
        #endregion

        #region Client Profile Report

        #region Section I
        public IQueryable<reportUnit> ZipCode(DateTime beginDate, DateTime endDate)
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

        public IQueryable<reportUnit> Homeless(DateTime beginDate, DateTime endDate)
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
        #endregion

        #region Section II
        public IQueryable<reportUnit> HouseholdComposition(DateTime beginDate, DateTime endDate)
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
        #endregion

        #region Section III
        public IQueryable<reportUnit> Income(DateTime beginDate, DateTime endDate)
        {
            IQueryable<reportUnit> query;

            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
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
        #endregion

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
                    uniqueSignins = dailyUnique.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0,
                    totalSignins = dailySignins.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault(),
                    totalAssignments = dailyAssignments.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault(),
                    cancelledJobs = dailyCancelled.Count() > 0 ? dailyCancelled.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() : 0
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
                    totalSignins = weeklySignins.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    noWeekJobs = weeklyAssignments.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    weekEstDailyHours = g.hours,
                    weekEstPayment = g.wages,
                    weekHourlyWage = g.avg,
                    topJobs = weeklyJobs.Where(whr => whr.date == g.date)
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        //it's just a space that changed at the bottom
        public IEnumerable<monthlyData> MonthlyController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<reportUnit> signins;
            IEnumerable<reportUnit> unique;
            IEnumerable<TypeOfDispatchReport> dispatch;
            IEnumerable<AverageWages> average;

            IEnumerable<reportUnit> newPpl;
            IEnumerable<reportUnit> pplLeft;
            IEnumerable<reportUnit> pplStay;
            //IEnumerable<reportUnit> finLit;
            //IEnumerable<reportUnit> jobSkills;
            IEnumerable<reportUnit> eslGrads;
            IEnumerable<reportUnit> undupDispatch;
            IEnumerable<reportUnit> permPlaced;
            
            IEnumerable<monthlyData> q; 

            signins = CountSignins(beginDate, endDate).ToList();
            unique = CountUniqueSignins(beginDate, endDate).ToList();
            dispatch = CountTypeofDispatch(beginDate, endDate).ToList();
            average = HourlyWageAverage(beginDate, endDate).ToList();
            newPpl = NewlyEnrolled(beginDate, endDate).ToList();
            pplLeft = NewlyExpired(beginDate, endDate).ToList();
            pplStay = StillEnrolled(beginDate, endDate).ToList();
            //finLit = null;
            //jobSkills = null;
            eslGrads = AdultsEnrolledAndAssessed(beginDate, endDate).ToList();
            undupDispatch = WorkersInTempJobs(beginDate, endDate).ToList();
            permPlaced = WorkersInPermanentJobs(beginDate, endDate).ToList();

            q = average
                .Select(g => new monthlyData
                {
                    date = g.date,
                    totalSignins = signins.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    uniqueSignins = unique.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    dispatchedDWCList = dispatch.Where(whr => whr.date == g.date).Select(h => h.dwcList).FirstOrDefault(),
                    dispatchedHHHList = dispatch.Where(whr => whr.date == g.date).Select(h => h.hhhList).FirstOrDefault(),
                    dispatchedDWCPropio = dispatch.Where(whr => whr.date == g.date).Select(h => h.dwcPropio).FirstOrDefault(),
                    dispatchedHHHPropio = dispatch.Where(whr => whr.date == g.date).Select(h => h.hhhPropio).FirstOrDefault(),
                    totalHours = g.hours,
                    totalIncome = g.wages,
                    avgIncomePerHour = g.avg,
                    newlyEnrolled = newPpl.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    peopleWhoLeft = pplLeft.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    peopleWhoStayed = pplStay.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    //financialLiterates = ,
                    //jobSkillsTrainees = ,
                    gradFromESL = eslGrads.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    unduplicatedDispatched = undupDispatch.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    permanentPlacements = permPlaced.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        public IEnumerable<yearSumData> YearlyController(DateTime beginDate, DateTime endDate)
        {
            #region variables
            IEnumerable<reportUnit> temporaryPlacements;
            IEnumerable<activityUnit> safetyTrainees;
            IEnumerable<activityUnit> skillsTrainees;
            IEnumerable<reportUnit> eslAssessed;
            IEnumerable<reportUnit> basicGardenTrainees;
            IEnumerable<reportUnit> advGardenTrainees;
            IEnumerable<reportUnit> finTrainees;

            //For safety:
            //IEnumerable<activityUnit> oshaTrainees;
            //IEnumerable<activityUnit> chemTrainees;
            //IEnumerable<activityUnit> movingTrainees; 
            
            //For skills (includes leadership)
            //IEnumerable<activityUnit> careTrainees;
            //IEnumerable<activityUnit> heatTrainees;
            //IEnumerable<activityUnit> asbestosTrainees;
            //IEnumerable<activityUnit> computerTrainees;
            //IEnumerable<activityUnit> greenTrainees;
            //IEnumerable<activityUnit> msfCount;
            //IEnumerable<activityUnit> assemblyCount; 

            //Can replace the above:
            IEnumerable<activityUnit> getAllClassAttendance;
            IEnumerable<DateTime> getAllDates;
            IEnumerable<yearSumData> q;

            temporaryPlacements = WorkersInTempJobs(beginDate, endDate).ToList();
            getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            safetyTrainees = getAllClassAttendance.Where(safety => safety.activityType == "Health & Safety");
            skillsTrainees = getAllClassAttendance.Where(skills => skills.activityType == "Skills Training" || skills.activityType == "Leadership Development");
            eslAssessed = AdultsEnrolledAndAssessedInESL(beginDate, endDate).ToList();
            basicGardenTrainees = getAllClassAttendance.Where(basic => basic.info == "Basic Gardening");
            advGardenTrainees = getAllClassAttendance.Where(adv => adv.info == "Advanced Gardening");
            finTrainees = getAllClassAttendance.Where(fin => fin.info == "Financial Education");

            getAllDates
            #endregion

            q = getAllClassAttendance
                .Select(g => new yearSumData
                {
                    date = g.date,
                    temporaryPlacements = temporaryPlacements.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    safetyTrainees = safetyTrainees.Where(whr => whr.date == g.date),
                    skillsTrainees = skillsTrainees.Where(whr => whr.date == g.date),
                    eslAssessed = eslAssessed.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    basicGardenTrainees = basicGardenTrainees.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    advGardenTrainees = advGardenTrainees.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault(),
                    finTrainees = finTrainees.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault()
                });

            return q;
        }

        #region Work Order Reports
        /// <summary>
        /// Jobs and Zip Codes controller. The jobs and zip codes report was
        /// initially requested by Mountain View and is just a good idea so
        /// that centers can see what their orders are and where they're coming
        /// from.
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
            topZips = ListZipCodes(beginDate, endDate).ToList();
            topJobs = ListJobs(beginDate, endDate).ToList();

            q = assignments
                .Select(g => new jzcData
                {
                    date = g.date,
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
        #endregion

        #endregion

        #region DataTablesStuff
        // The following methods organize the above service-layer views for return to Ajax/DataTables and the GUI.

        public dataTableResult<dailyData> DailyView(DateTime date)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<dailyData> query;
            
            beginDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);//.AddDays(DateTime.DaysInMonth(date.Year, date.Month + 1));

            query = DailyController(beginDate, endDate);

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

            query = WeeklyController(beginDate, endDate);

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

            query = MonthlyController(beginDate, endDate);

            var result = GetDataTableResult<monthlyData>(query);

            return result;
        }

        public dataTableResult<yearSumData> yearlyView(DateTime yDate)
        {
            DateTime beginDate;
            DateTime endDate;
            IEnumerable<yearSumData> query;

            beginDate = new DateTime(yDate.Year, yDate.Month, 1, 0, 0, 0);
            endDate = new DateTime(yDate.Year, yDate.Month, System.DateTime.DaysInMonth(yDate.Year, yDate.Month), 23, 59, 59).AddMonths(12);

            query = YearlyController(beginDate, endDate);

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

            query = jzcController(beginDate, endDate);

            var result = GetDataTableResult<jzcData>(query);

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

    #region Classes for Summary Reports
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
        public int? uniqueSignins { get; set; }
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
        public int? weekEstDailyHours { get; set; }
        public double? weekEstPayment { get; set; }
        public double? weekHourlyWage { get; set; }
        public IEnumerable<reportUnit> topJobs { get; set; }
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
        public int? uniqueSignins { get; set; }
        public int? dispatchedDWCList { get; set; }
        public int? dispatchedHHHList { get; set; }
        public int? dispatchedDWCPropio { get; set; }
        public int? dispatchedHHHPropio { get; set; }
        public int? totalHours { get; set; }
        public double? totalIncome { get; set; }
        public double? avgIncomePerHour { get; set; }
        // the following were added just prior to Jan 2014 for Casa Latina
        public int? newlyEnrolled { get; set; }
        public int? peopleWhoLeft { get; set; }
        public int? peopleWhoStayed { get; set; }
        public int? financialLiterates { get; set; }
        public int? jobSkillsTrainees { get; set; }
        public int? gradFromESL { get; set; }
        public int? unduplicatedDispatched { get; set; }
        public int? permanentPlacements { get; set; }
    }


    public class jzcData
    {
        public DateTime? date { get; set; }
        public string zips { get; set; }
        public string zipsCount { get; set; }
        public string jobs { get; set; }
        public string jobsCount { get; set; }
    }

    public class yearSumData
    {
        public DateTime? date { get; set; }
        public int? temporaryPlacements { get; set; }
        public IEnumerable<reportUnit> safetyTrainees { get; set; }
        public IEnumerable<reportUnit> skillsTrainees { get; set; }
        public int? eslAssessed { get; set; }
        public int? basicGardenTrainees { get; set; }
        public int? advGardenTrainees { get; set; }
        public int? finTrainees { get; set; }
    }
    #endregion
}