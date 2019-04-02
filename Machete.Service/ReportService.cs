using System;
using System.Collections.Generic;
using System.Linq;
using Machete.Data;
using Machete.Domain;

namespace Machete.Service
{
    public interface IReportService
    {
        IEnumerable<DailySumData> DailySumController(DateTime date);
        IEnumerable<WeeklySummaryData> WeeklySumController(DateTime beginDate, DateTime endDate);
        IEnumerable<MonthlySummaryData> MonthlySumController(DateTime beginDate, DateTime endDate);
        IEnumerable<YearSumData> YearlySumController(DateTime beginDate, DateTime endDate);
        IEnumerable<ActivityData> ActivityReportController(DateTime beginDate, DateTime endDate, string reportType);
        IEnumerable<ActivityData> YearlyActController(DateTime beginDate, DateTime endDate);
        IEnumerable<ZipModel> EmployerReportController(DateTime beginDate, DateTime endDate);
        IEnumerable<NewWorkerData> NewWorkerController(DateTime beginDate, DateTime endDate, string reportType);
        IEnumerable<ReportUnit> CountSignins(DateTime beginDate, DateTime endDate);
        IEnumerable<ReportUnit> CountUniqueSignins(DateTime beginDate, DateTime endDate);
        IEnumerable<ReportUnit> CountAssignments(DateTime beginDate, DateTime endDate);
        IEnumerable<ReportUnit> CountCancelled(DateTime beginDate);
        IEnumerable<TypeOfDispatchModel> CountTypeofDispatch(DateTime beginDate, DateTime endDate);
        IEnumerable<AverageWageModel> HourlyWageAverage(DateTime beginDate, DateTime endDate);
        IEnumerable<ReportUnit> ListJobs(DateTime beginDate, DateTime endDate);
        IEnumerable<ZipModel> ListOrdersByZipCode(DateTime beginDate, DateTime endDate);
    }

    // update 2019-1-11; heavily refactored to remove as much code as possible, reason being it was slowing down
    // the IDE with improvement suggestions (and probably slowing the build and tests). Hopefully better now.
    public class ReportService : IReportService
    {
        private readonly IWorkOrderRepository woRepo;
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IWorkerRequestRepository wrRepo;
        private readonly ILookupRepository lookRepo;
        private readonly IEmployerRepository eRepo;
        private readonly IActivitySigninRepository asRepo;

        public ReportService(IWorkOrderRepository woRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo,
                             IWorkerSigninRepository wsiRepo,
                             IWorkerRequestRepository wrRepo,
                             ILookupRepository lookRepo,
                             IEmployerRepository eRepo,
                             IActivitySigninRepository asRepo)
        {
            this.woRepo = woRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
            this.wsiRepo = wsiRepo;
            this.wrRepo = wrRepo;
            this.lookRepo = lookRepo;
            this.eRepo = eRepo;
            this.asRepo = asRepo;
        }

        /// <summary>
        /// A simple count of worker signins for the given period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<ReportUnit> CountSignins(DateTime beginDate, DateTime endDate)
        {
            var wsiQ = wsiRepo.GetAllQ();

            var query = wsiQ
                .Where(whr => whr.dateforsignin >= beginDate
                              && whr.dateforsignin <= endDate)
                .GroupBy(gb => gb.dateforsignin.Date)
                .Select(g => new ReportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });

            return query;
        }

        /// <summary>
        /// A simple count of unduplicated worker signins for the given period.
        /// Note: Casa's policy is that these should reset on beginDate, but that
        /// isn't truly "unduplicated" within the program.
        /// </summary>
        /// <param name="beginDate">DateTime, not null</param>
        /// <param name="endDate">DateTime, null</param>
        public IEnumerable<ReportUnit> CountUniqueSignins(DateTime beginDate, DateTime endDate)
        {
            var wsiQ = wsiRepo.GetAllQ();
            var query = wsiQ
                .Where(whr => whr.dateforsignin <= endDate)
                .GroupBy(gb => gb.dwccardnum)
                .Select(group => new {
                    dwccardnum = group.Key,
                    firstSignin = group.Min(m => m.dateforsignin)
                })
                .Where(whr => whr.firstSignin >= beginDate)
                .GroupBy(gb => gb.firstSignin.Date)
                .Select(g => new ReportUnit
                {
                    date = g.Key,
                    count = g.Count(),
                    info = ""
                });
            
            return query;
        }

        /// <summary>
        /// Counts work assignments for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<ReportUnit> CountAssignments(DateTime beginDate, DateTime endDate)
        {
            var waQ = waRepo.GetAllQ();
            var query = waQ
                .Where(whr => whr.workOrder.dateTimeofWork.Date >= beginDate
                              && whr.workOrder.dateTimeofWork.Date <= endDate)
                .GroupBy(gb => gb.workOrder.dateTimeofWork.Date)
                .Select(g => new ReportUnit
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
        public IEnumerable<ReportUnit> CountCancelled(DateTime beginDate)
        {
            var woQ = woRepo.GetAllQ();


            var query = woQ.Where(whr => whr.dateTimeofWork.Date == beginDate
                                                && whr.statusID == WorkOrder.iCancelled)
                .GroupBy(gb => gb.dateTimeofWork.Date)
                .Select(g => new ReportUnit
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
        public IEnumerable<TypeOfDispatchModel> CountTypeofDispatch(DateTime beginDate, DateTime endDate)
        {
            var waQ = waRepo.GetAllQ();
            var wrQ = wrRepo.GetAllQ();
            var loD = lookRepo.GetByKey(LCategory.worktype, "DWC").ID;
            var loH = lookRepo.GetByKey(LCategory.worktype, "HHH").ID;

            IQueryable<TypeOfDispatchModel> query = waQ
                .GroupJoin(wrQ,
                    wa => new { waid = wa.workOrderID, waw = wa.workerAssignedID },
                    wr => new { waid = wr.WorkOrderID, waw = (int?)wr.WorkerID },
                    (wa, wr) => new
                    {
                        dtow = wa.workOrder.dateTimeofWork.Date,
                        wa.workerAssignedID,
                        dwcList = wa.workerAssigned.typeOfWorkID == loD ? (wr.FirstOrDefault().WorkerID == wa.workerAssigned.ID ? 0 : 1) : 0,
                        hhhList = wa.workerAssigned.typeOfWorkID == loH ? (wr.FirstOrDefault().WorkerID == wa.workerAssigned.ID ? 0 : 1) : 0,
                        dwcPatron = wa.workerAssigned.typeOfWorkID == loD ? (wr.FirstOrDefault().WorkerID == wa.workerAssigned.ID ? 1 : 0) : 0,
                        hhhPatron = wa.workerAssigned.typeOfWorkID == loH ? (wr.FirstOrDefault().WorkerID == wa.workerAssigned.ID ? 1 : 0) : 0
                    })
                .Where(whr => whr.dtow >= beginDate
                              && whr.dtow <= endDate)
                .GroupBy(gb => gb.dtow)
                .Select(group => new TypeOfDispatchModel
                {
                    date = group.Key,
                    count = group.Count(),
                    dwcList = group.Sum(a => a.dwcList),
                    dwcPropio = group.Sum(a => a.dwcPatron),
                    hhhList = group.Sum(a => a.hhhList),
                    hhhPropio = group.Sum(a => a.hhhPatron)
                })
                .OrderBy(fini => fini.date);

            return query;
        }

        /// <summary>
        /// Grabs a sum of hours and wages and averages them for a given time period.
        /// </summary>
        /// <param name="beginDate">Start date for the query.</param>
        /// <param name="endDate">End date for the query.</param>
        public IEnumerable<AverageWageModel> HourlyWageAverage(DateTime beginDate, DateTime endDate)
        {
            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            
            var query = waQ
                .Join(woQ,
                    wa => wa.workOrderID,
                    wo => wo.ID,
                    (wa, wo) => new
                    {
                        wa,
                        woDate = wo.dateTimeofWork.Date
                    })
                .Where(whr => whr.woDate >= beginDate
                              && whr.woDate <= endDate)
                .GroupBy(gb => gb.woDate)
                .Select(wec => new AverageWageModel
                    {
                        date = wec.Key,
                        hours = wec.Sum(wo => wo.wa.hours),
                        wages = wec.Sum(wo => wo.wa.hourlyWage * wo.wa.hours),
                        avg = wec.Sum(wo => wo.wa.hours) == 0 
                            ? 0 
                            : wec.Sum(wo => wo.wa.hourlyWage * wo.wa.hours) 
                              / wec.Sum(wo => wo.wa.hours)
                    }
                );

            return query;
        }

        /// <summary>
        /// Lists jobs in order of occurrence for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<ReportUnit> ListJobs(DateTime beginDate, DateTime endDate)
        {
            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            var query = waQ
                .Join(woQ,
                    wa => wa.workOrderID,
                    wo => wo.ID,
                    (wa, wo) => new
                    {
                        wa,
                        workDate = wo.dateTimeofWork.Date
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
                .Select(group => new ReportUnit
                {
                    date = group.Key.workDate,
                    count = group.Any() ? group.Count() : 0,
                    info = group.Key.enText ?? ""
                });

            return query;
        }

        /// <summary>
        /// Lists most popular zip codes for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<ZipModel> ListOrdersByZipCode(DateTime beginDate, DateTime endDate)
        {
            var waQ = waRepo.GetAllQ();
            var eQ = eRepo.GetAllQ();
            var waQ1 = waRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            var jobsByZip = waQ1
                .Join(lQ,
                    wa => wa.skillID,
                    l => l.ID,
                    (wa, l) => new
                    {
                        dtow = wa.workOrder.dateTimeofWork.Date,
                        zip = wa.workOrder.zipcode,
                        enText = l.text_EN
                    })
                .Where(whr => whr.dtow >= beginDate
                              && whr.dtow <= endDate)
                .GroupBy(gb => new { zips = gb.zip, jobs = gb.enText })
                .OrderByDescending(ob => ob.Count())
                .Select(x => new ReportUnit
                {
                    zip = x.Key.zips,
                    count = x.Any() ? x.Count() : 0,
                    info = x.Key.jobs
                });

            var query = waQ
                .Where(w => w.workOrder.dateTimeofWork >= beginDate 
                            && w.workOrder.dateTimeofWork <= endDate)
                .GroupBy(a => new { zip = a.workOrder.zipcode })
                .Select(x => new ZipModel
                {
                    zips = x.Key.zip,
                    jobs = x.Count(),
                    emps = eQ.Count(w => w.zipcode == x.Key.zip && w.active),
                    skills = jobsByZip.Where(w => w.zip == x.Key.zip)
                });

            return query;
        }

        private IQueryable<PlacementUnit> WorkersInJobs(DateTime beginDate, DateTime endDate)
        {
            var waQ = waRepo.GetAllQ();

            var undup = waQ
                .Where(whr => whr.workOrder.dateTimeofWork >= beginDate
                    && whr.workOrder.dateTimeofWork <= endDate)
	            .GroupBy(g => g.workerAssigned.dwccardnum)
	            .Select(a => new {
		            date = a.Min(x => x.workOrder.dateTimeofWork.Date),
		            undup = true
	            })
	            .GroupBy(gb => gb.date)
	            .Select(b => new {
		            dtow = b.Key,
		            undupCount = b.Count(up => up.undup)
	            });

            var query = waQ
                .Where(whr => whr.workOrder.dateTimeofWork >= beginDate
                              && whr.workOrder.dateTimeofWork <= endDate)
                .GroupBy(g => new
                {
                    dtow = g.workOrder.dateTimeofWork.Date
                })
                .Select(c => new
                {
                    c.Key.dtow,
                    permCount = c.Count(pp => pp.workOrder.permanentPlacement),
                    tempCount = c.Count(tp => !tp.workOrder.permanentPlacement)
                })
                .Join(undup, x => x.dtow, y => y.dtow, (x, y) => new
                {
                    x.dtow,
                    undup = y.undupCount,
                    perm = x.permCount,
                    temp = x.tempCount
                })
                .Select(d => new PlacementUnit
                {
                    date = d.dtow,
                    count = d.perm + d.temp,
                    undupCount = d.undup,
                    permCount = d.perm,
                    tempCount = d.temp
                });

            return query;
        }

        /// <summary>
        /// Returns all activity signins for a given time period grouped by date, activityType and activityName.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>date, count, info (activityName), activityType</returns>
        private IQueryable<ReportUnit> GetAllActivitySignins(DateTime beginDate, DateTime endDate)
        {
            var asQ = asRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            var query = asQ.Join(lQ,
                    aj => aj.Activity.nameID,
                    lj => lj.ID,
                    (aj, lj) => new
                    {
                        name = lj.text_EN,
                        type = aj.Activity.typeID,
                        aj.person,
                        date = aj.Activity.dateStart.Date
                    })
                .Join(lQ,
                    aj => aj.type,
                    lj => lj.ID,
                    (aj, lj) => new
                    {
                        aj.name,
                        type = lj.text_EN,
                        aj.person,
                        aj.date
                    })
                .Where(signin => signin.person != null &&
                                 signin.date <= endDate &&
                                 signin.date >= beginDate)
                .GroupBy(gb => new { gb.date, gb.name, gb.type })
                .Select(grouping => new ReportUnit
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
        private IQueryable<ReportUnit> AdultsEnrolledAndAssessedInESL()
        {
            var asQ = asRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            var query = asQ
                .Join(lQ,
                    asi => asi.Activity.nameID,
                    look => look.ID,
                    (asi, look) => new {
                        dwc = asi.dwccardnum,
                        name = look.text_EN,
                        date = asi.Activity.dateStart.Date,
                        mins = 60 * asi.Activity.dateEnd.Subtract(asi.Activity.dateStart).Hours
                               + asi.Activity.dateEnd.Subtract(asi.Activity.dateStart).Minutes
                    })
                .Where(whr => whr.name.Contains("English"))
                .GroupBy(gb => gb.dwc)
                .Select(sel => new {
                    date = sel.Select(asi => asi.date).Max(),
                    dwccardnum = sel.Key,
                    minutesInClass = sel.Sum(s => s.mins)
                })
                .Where(sec => sec.minutesInClass >= 720)
                .GroupBy(last => last.date)
                .Select(x => new ReportUnit {
                    date = x.Key,
                    count = x.Count(),
                    info = ""
                });

            return query;
        }

        /// <summary>
        /// Returns a count of new, expired, and still active members by enumerated dates within the given period.
        /// This is a resource intensive query, because it targets specific data with daily granularity; this usage 
        /// can be offset by reducing the interval and increasing the unit of measure.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="unitOfMeasure">The unit of time to measure; "days" or "months".</param>
        /// <param name="interval">The interval of time (7 days, 3 months) as int</param>
        private IQueryable<StatusUnit> MemberStatusByDate(DateTime beginDate, DateTime endDate, string unitOfMeasure, int interval)
        {
            IQueryable<StatusUnit> query;

            var wQ = wRepo.GetAllQ();

            if (unitOfMeasure == "months" && interval > 0)
            {
                var months = ((endDate.Year - beginDate.Year) * 12) + endDate.Month - beginDate.Month;
                query = Enumerable
                  .Range(0, months / interval)
                  .Select(w => endDate.AddMonths(w * -interval))
                  .Select(x => new StatusUnit
                      {
                          date = x,
                          count = wQ.Count(y => y.dateOfMembership < x && y.memberexpirationdate > x),
                          enrolledOnDate = wQ.Count(y => y.dateOfMembership >= DbFunctions.AddMonths(x, -interval) && y.dateOfMembership <= x),
                          expiredOnDate = wQ.Count(y => y.memberexpirationdate >= DbFunctions.AddMonths(x, -interval) && y.memberexpirationdate <= x)
                      })
                  .AsQueryable();
                return query;
            }

            if (unitOfMeasure == "days" && interval > 0)
            {
                query = Enumerable
                    .Range(0, 1 + (endDate.Subtract(beginDate).Days / interval))
                    .Select(w => endDate.AddDays(w * -interval))
                    .Select(x => new StatusUnit
                    {
                        date = x,
                        count = wQ.Count(y => y.dateOfMembership < x && y.memberexpirationdate > x),
                        enrolledOnDate = wQ.Count(y => y.dateOfMembership == x),
                        expiredOnDate = wQ.Count(y => y.memberexpirationdate == x)
                    })
                    .AsQueryable();

                return query;
            }

            throw new Exception("unitOfMeasure must be \"months\" or \"days\" and interval must be a positive integer > 0.");
        }

        private IQueryable<MemberDateModel> SingleAdults()
        {
            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            var query = wQ
                .Join(lQ, worker => worker.maritalstatus, lookup => lookup.ID, (worker, lookup) => new
                {
                    card = worker.dwccardnum,
                    zip = worker.Person.zipcode,
                    memDate = worker.dateOfMembership,
                    expDate = worker.memberexpirationdate,
                    kids = worker.livewithchildren,
                    mStatus = lookup.text_EN
                })
                .Where(worker => !worker.kids.Value && worker.mStatus != "Married")
                .Select(x => new MemberDateModel
                {
                    dwcnum = x.card,
                    zip = x.zip,
                    memDate = x.memDate,
                    expDate = x.expDate
                });

            return query;
        }

        private IQueryable<MemberDateModel> FamilyHouseholds()
        {
            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            var query = wQ
                .Join(lQ, worker => worker.maritalstatus, lookup => lookup.ID, (worker, lookup) => new
                {
                    card = worker.dwccardnum,
                    zip = worker.Person.zipcode,
                    memDate = worker.dateOfMembership,
                    expDate = worker.memberexpirationdate,
                    kids = worker.livewithchildren,
                    mStatus = lookup.text_EN
                })
                .Where(worker => worker.kids.Value && worker.mStatus == "Married")
                .Select(x => new MemberDateModel
                {
                    dwcnum = x.card,
                    zip = x.zip,
                    memDate = x.memDate,
                    expDate = x.expDate
                });

            return query;
        }

        /// <summary>
        /// Controller for daily summary report.
        /// </summary>
        /// <param name="date"></param>
        public IEnumerable<DailySumData> DailySumController(DateTime date)
        {
            var beginDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            IEnumerable<TypeOfDispatchModel> dclCurrent = CountTypeofDispatch(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> dailySignins = CountSignins(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> dailyUnique = CountUniqueSignins(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> dailyAssignments = CountAssignments(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> dailyCancelled = CountCancelled(beginDate).ToList();

            var q = dclCurrent
                .Select(group => new DailySumData
                {
                    date = group.date,
                    dwcList = group.dwcList,
                    dwcPropio = group.dwcPropio,
                    hhhList = group.hhhList,
                    hhhPropio = group.hhhPropio,
                    uniqueSignins = dailyUnique.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0,
                    totalSignins = dailySignins.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0,
                    totalAssignments = dailyAssignments.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0,
                    cancelledJobs = dailyCancelled.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        /// <summary>
        /// Controller for weekly summary report.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<WeeklySummaryData> WeeklySumController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<AverageWageModel> weeklyWages = HourlyWageAverage(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> weeklySignins = CountSignins(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> weeklyAssignments = CountAssignments(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> weeklyJobs = ListJobs(beginDate, endDate).ToList();

            var q = weeklyWages
                .Select(g => new WeeklySummaryData
                {
                    dayofweek = g.date.DayOfWeek,
                    date = g.date,
                    totalSignins = weeklySignins.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault() ?? 0,
                    noWeekJobs = weeklyAssignments.Where(whr => whr.date == g.date).Select(h => h.count).FirstOrDefault() ?? 0,
                    weekEstDailyHours = g.hours,
                    weekEstPayment = g.wages,
                    weekHourlyWage = g.avg,
                    topJobs = weeklyJobs.Where(whr => whr.date == g.date)
                });

            q = q.OrderBy(p => p.date);

            return q;
        }

        public IEnumerable<MonthlySummaryData> MonthlySumController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<DateTime> getAllDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                    .Select(offset => beginDate.AddDays(offset))
                    .ToArray();

            IEnumerable<ReportUnit> signins = CountSignins(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> unique = CountUniqueSignins(beginDate, endDate).ToList();
            var classes = GetAllActivitySignins(beginDate, endDate).ToList();
            IEnumerable<PlacementUnit> workers = WorkersInJobs(beginDate, endDate).ToList();
            IEnumerable<AverageWageModel> average = HourlyWageAverage(beginDate, endDate).ToList();
            IEnumerable<StatusUnit> status = MemberStatusByDate(beginDate, endDate, "days", 1).ToList();

            var query = getAllDates
                .Select(g => new MonthlySummaryData
                {
                    dateStart = g,
                    dateEnd = g.AddDays(1),
                    totalSignins = signins.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.count).FirstOrDefault() ?? 0,
                    uniqueSignins = unique.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.count).FirstOrDefault() ?? 0, //dd
                    peopleWhoWentToClass = classes.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.count).FirstOrDefault() ?? 0,
                    dispatched = workers.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.count).FirstOrDefault() ?? 0,
                    tempDispatched = workers.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.tempCount).FirstOrDefault() ?? 0, //dd
                    permanentPlacements = workers.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.permCount).FirstOrDefault() ?? 0, //dd
                    undupDispatched = workers.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.undupCount).FirstOrDefault() ?? 0, //dd
                    totalHours = average.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.hours).FirstOrDefault(),
                    totalIncome = average.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.wages).FirstOrDefault(),
                    avgIncomePerHour = average.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.avg).FirstOrDefault(),
                    stillHere = status.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.count).FirstOrDefault() ?? 0,
                    newlyEnrolled = status.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.enrolledOnDate).FirstOrDefault() ?? 0, //dd
                    peopleWhoLeft = status.Where(w => w.date >= g && w.date < g.AddDays(1)).Select(h => h.expiredOnDate).FirstOrDefault() ?? 0 //dd
                });

            return query;
        }

        public IEnumerable<ActivityData> ActivityReportController(DateTime beginDate, DateTime endDate, string reportType)
        {
            IEnumerable<DateTime> getDates;

            if (reportType == "weekly" || reportType == "monthly")
            {
                 getDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                    .Select(offset => beginDate.AddDays(offset))
                    .ToArray();
            }
            else throw new Exception("Please select \"weekly\" or \"monthly\" as the report type.");

            var getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            var eslAssessed = AdultsEnrolledAndAssessedInESL().ToList();
            var safetyTrainees = getAllClassAttendance
                        .Where(whr => whr.activityType == "Health & Safety");
            var skillsTrainees = getAllClassAttendance
                        .Where(whr => whr.activityType == "Skills Training" || whr.activityType == "Leadership Development");
            var basGardenTrainees = getAllClassAttendance.Where(basic => basic.info == "Basic Gardening");
            var advGardenTrainees = getAllClassAttendance.Where(adv => adv.info == "Advanced Gardening");
            var finTrainees = getAllClassAttendance.Where(fin => fin.info == "Financial Education");
            var oshaTrainees = getAllClassAttendance.Where(osha => osha.info.Contains("OSHA"));

            var q = getDates
                .Select(g => new ActivityData
                {
                    dateStart = g,
                    safety = safetyTrainees.Count(whr => whr.date == g),
                    skills = skillsTrainees.Count(whr => whr.date == g),
                    esl = eslAssessed.Count(whr => whr.date == g),
                    basGarden = basGardenTrainees.Count(whr => whr.date == g),
                    advGarden = advGardenTrainees.Count(whr => whr.date == g),
                    finEd = finTrainees.Count(whr => whr.date == g),
                    osha = oshaTrainees.Count(whr => whr.date == g),
                    drilldown = getAllClassAttendance.Where(whr => whr.date == g)
                });

            return q;
        }

        public IEnumerable<YearSumData> YearlySumController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<DateTime> getDates = Enumerable.Range(1, 4)
                    .Select(offset => endDate.AddMonths(-offset * 3))
                    .ToArray();

            var signins = CountSignins(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> unique = CountUniqueSignins(beginDate, endDate).ToList();
            IEnumerable<ReportUnit> classes = GetAllActivitySignins(beginDate, endDate).ToList();
            IEnumerable<PlacementUnit> workers = WorkersInJobs(beginDate, endDate).ToList();
            IEnumerable<AverageWageModel> average = HourlyWageAverage(beginDate, endDate).ToList();
            IEnumerable<StatusUnit> status = MemberStatusByDate(beginDate, endDate, "months", 3).ToList();

            var q = getDates
                .Select(x => new YearSumData
                {
                    dateStart = x,
                    dateEnd = x.AddMonths(3),
                    totalSignins = signins.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.count).Sum() ?? 0,
                    uniqueSignins = unique.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.count).Sum() ?? 0, //dd
                    peopleWhoWentToClass = classes.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.count).Sum() ?? 0,
                    dispatched = workers.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.count).Sum() ?? 0,
                    tempDispatched = workers.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.tempCount).Sum() ?? 0, //dd
                    permanentPlacements = workers.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.permCount).Sum() ?? 0, //dd
                    undupDispatched = workers.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.undupCount).Sum() ?? 0, //dd
                    totalHours = average.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.hours).Sum(),
                    totalIncome = average.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.wages).Sum(),
                    avgIncomePerHour = average.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.hours).Sum() == 0 ? 0
                        : average.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.avg * h.hours).Sum()
                          / average.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.hours).Sum(),
                    stillHere = status.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.count).Sum() ?? 0,
                    newlyEnrolled = status.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.enrolledOnDate).Sum() ?? 0, //dd
                    peopleWhoLeft = status.Where(y => y.date >= x && y.date < x.AddMonths(3)).Select(h => h.expiredOnDate).Sum() ?? 0 //dd
                });

            return q;
        }

        public IEnumerable<ActivityData> YearlyActController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<DateTime> getDates = Enumerable.Range(1, 4)
                .Select(offset => endDate.AddMonths(-offset * 3))
                .ToArray();

            var getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            var eslAssessed = AdultsEnrolledAndAssessedInESL().ToList();
            var safetyTrainees = getAllClassAttendance
                        .Where(whr => whr.info == "Chemical Safety" );
            var skillsTrainees = getAllClassAttendance
                        .Where(whr => whr.activityType == "Class" || whr.activityType == "Workshops" || whr.info == "Mujeres sin Fronteras");
            var basGardenTrainees = getAllClassAttendance.Where(basic => basic.info == "Basic Gardening");
            var advGardenTrainees = getAllClassAttendance.Where(adv => adv.info == "Advanced Gardening");
            var finTrainees = getAllClassAttendance.Where(fin => fin.info == "Financial Education");
            var oshaTrainees = getAllClassAttendance.Where(osha => osha.info.Contains("Health & Safety"));

            var q = getDates
                .Select(x => new ActivityData
                {
                    dateStart = x,
                    dateEnd = x.AddMonths(3).AddDays(1),
                    safety = safetyTrainees.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    skills = skillsTrainees.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    esl = eslAssessed.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    basGarden = basGardenTrainees.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    advGarden = advGardenTrainees.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    finEd = finTrainees.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    osha = oshaTrainees.Count(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)),
                    drilldown = getAllClassAttendance.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1))
                });

            return q;
        }

        /// <summary>
        /// NewWorkerController returns an IEnumerable containing the counts of single members, new single members, family
        /// members, and new family members. It does not include zip code completeness, which must be called separately.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<NewWorkerData> NewWorkerController(DateTime beginDate, DateTime endDate, string reportType)
        {
            IEnumerable<NewWorkerData> q;
            IEnumerable<DateTime> getDates;

            IEnumerable<MemberDateModel> singleAdults = SingleAdults().ToList();
            IEnumerable<MemberDateModel> familyHouseholds = FamilyHouseholds().ToList();

            switch (reportType) {
                case "weekly":
                case "monthly":
                    getDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                        .Select(offset => endDate.AddDays(-offset))
                        .ToArray();

                    q = getDates
                        .Select(x => new NewWorkerData
                        {
                            dateStart = x,
                            dateEnd = x.AddDays(1),
                            singleAdults = singleAdults.Count(y => y.expDate >= x && y.memDate < x.AddDays(1)),
                            familyHouseholds = familyHouseholds.Count(y => y.expDate >= x && y.memDate < x.AddDays(1)),
                            newSingleAdults = singleAdults.Count(y => y.memDate >= x && y.memDate < x.AddDays(1)),
                            newFamilyHouseholds = familyHouseholds.Count(y => y.memDate >= x && y.memDate < x.AddDays(1)),
                            zipCompleteness = singleAdults.Count(y => y.zip != null && y.expDate >= x && y.memDate < x.AddDays(1))
                                              + familyHouseholds.Count(y => y.zip != null && y.expDate >= x && y.memDate < x.AddDays(1))
                        });
                    break;
                case "yearly":
                    getDates = Enumerable.Range(1, 4)
                        .Select(offset => endDate.AddMonths(-offset * 3))
                        .ToArray();

                    q = getDates
                        .Select(x => new NewWorkerData
                        {
                            dateStart = x.AddDays(1),
                            dateEnd = x.AddMonths(3).AddDays(1),
                            singleAdults = singleAdults.Count(y => y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1)),
                            familyHouseholds = familyHouseholds.Count(y => y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1)),
                            newSingleAdults = singleAdults.Count(y => y.memDate >= x && y.memDate < x.AddMonths(3).AddDays(1)),
                            newFamilyHouseholds = familyHouseholds.Count(y => y.memDate >= x && y.memDate < x.AddMonths(3).AddDays(1)),
                            zipCompleteness = singleAdults.Count(y => y.zip != null && y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1))
                                              + familyHouseholds.Count(y => y.zip != null && y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1))
                        });
                    break;
                default:
                    throw new Exception("Report type must be \"weekly\", \"monthly\" or \"yearly\".");
            }

            return q;
        }

        /// <summary>
        /// Jobs and Zip Codes controller. The jobs and zip codes report was
        /// initially requested by Mountain View and centers can see what their 
        /// orders are and where they're coming from.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public IEnumerable<ZipModel> EmployerReportController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<ZipModel> topZips = ListOrdersByZipCode(beginDate, endDate).ToList();
            return topZips;
        }

    }

    public class AverageWageModel
    {
        public DateTime date { get; set; }
        public double hours { get; set; }
        public double wages { get; set; }
        public double avg { get; set; }
    }

    public class MemberDateModel
    {
        public int dwcnum { get; set; }
        public string zip { get; set; }
        public DateTime memDate { get; set; }
        public DateTime expDate { get; set; }
    }

    public class TypeOfDispatchModel : ReportUnit
    {
        public int dwcList { get; set; }
        public int dwcPropio { get; set; }
        public int hhhList { get; set; }
        public int hhhPropio { get; set; }
    }

    public class StatusUnit : ReportUnit
    {
        public int? expiredOnDate { get; set; }
        public int? enrolledOnDate { get; set; }
    }

    public class PlacementUnit : ReportUnit
    {
        public int? undupCount { get; set; }
        public int? permCount { get; set; }
        public int? tempCount { get; set; }
    }

    /// <summary>
    /// This is the basic unit of the Report service layer.
    /// All of the values are nullable to make the unit as
    /// extensible as possible.
    /// </summary>
    public class ReportUnit
    {
        public DateTime? date { get; set; }
        public int? count { get; set; }
        public string info { get; set; }
        public string zip { get; set; }
        public string activityType { get; set; }
    }

    public class DailySumData : TypeOfDispatchModel
    {
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int cancelledJobs { get; set; }
        public int totalAssignments { get; set; }
    }

    public class WeeklySummaryData
    {
        public DayOfWeek dayofweek { get; set; }
        public DateTime date { get; set; }
        public int totalSignins { get; set; }
        public int noWeekJobs { get; set; }
        public double weekEstDailyHours { get; set; }
        public double weekEstPayment { get; set; }
        public double weekHourlyWage { get; set; }
        public IEnumerable<ReportUnit> topJobs { get; set; }
    }

    public class MonthlySummaryData
    {
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int dispatched { get; set; }
        public int tempDispatched { get; set; }
        public int permanentPlacements { get; set; }
        public int undupDispatched { get; set; }
        public double totalHours { get; set; }
        public double totalIncome { get; set; }
        public double avgIncomePerHour { get; set; }
        public int stillHere { get; set; }
        public int newlyEnrolled { get; set; }
        public int peopleWhoLeft { get; set; }
        public int peopleWhoWentToClass { get; set; }
    }

    public class YearSumData
    {
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int dispatched { get; set; }
        public int tempDispatched { get; set; }
        public int permanentPlacements { get; set; }
        public int undupDispatched { get; set; }
        public double totalHours { get; set; }
        public double totalIncome { get; set; }
        public double avgIncomePerHour { get; set; }
        public int stillHere { get; set; }
        public int newlyEnrolled { get; set; }
        public int peopleWhoLeft { get; set; }
        public int peopleWhoWentToClass { get; set; }
    }

    public class ZipModel
    {
        public string zips { get; set; }
        public int jobs { get; set; }
        public int emps { get; set; }
        public IEnumerable<ReportUnit> skills { get; set; }
    }

    public class NewWorkerData
    {
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public int singleAdults { get; set; }
        public int familyHouseholds { get; set; }
        public int newSingleAdults { get; set; }
        public int newFamilyHouseholds { get; set; }
        public int zipCompleteness { get; set; }
    }
    
    public class ActivityData
    {
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public int safety { get; set; }
        public int skills { get; set; }
        public int esl { get; set; }
        public int basGarden { get; set; }
        public int advGarden { get; set; }
        public int finEd { get; set; }
        public int osha { get; set; }
        public IEnumerable<ReportUnit> drilldown { get; set; }
    }
}
