using Machete.Data;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Machete.Service
{
    #region public class ReportService (Interface and Constructor)
    public interface IReportService
    {
        IEnumerable<DailySumData> DailySumController(DateTime date);
        IEnumerable<WeeklySumData> WeeklySumController(DateTime beginDate, DateTime endDate);
        IEnumerable<MonthlySumData> MonthlySumController(DateTime beginDate, DateTime endDate);
        IEnumerable<YearSumData> YearlySumController(DateTime beginDate, DateTime endDate);
        IEnumerable<ActivityData> ActivityReportController(DateTime beginDate, DateTime endDate, string reportType);
        IEnumerable<ActivityData> YearlyActController(DateTime beginDate, DateTime endDate);
        IEnumerable<ZipModel> EmployerReportController(DateTime beginDate, DateTime endDate);
        IEnumerable<NewWorkerData> NewWorkerController(DateTime beginDate, DateTime endDate, string reportType);
    }

    [SuppressMessage("ReSharper", "ReplaceWithSingleCallToCount")]
    public class ReportService : IReportService
    {
        protected readonly IWorkOrderRepository woRepo;
        protected readonly IWorkAssignmentRepository waRepo;
        protected readonly IWorkerRepository wRepo;
        protected readonly IWorkerSigninRepository wsiRepo;
        protected readonly IWorkerRequestRepository wrRepo;
        protected readonly ILookupRepository lookRepo;
        protected readonly IEmployerRepository eRepo;
        protected readonly IActivitySigninRepository asRepo;

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

    #endregion

        #region BasicFunctions
        /// <summary>
        /// A simple count of worker signins for the given period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns>IQueryable of type ReportUnit </returns>
        public IQueryable<ReportUnit> CountSignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;
            var wsiQ = wsiRepo.GetAllQ();

            query = wsiQ
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
        /// <returns>int</returns>
        public IQueryable<ReportUnit> CountUniqueSignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;
            var wsiQ = wsiRepo.GetAllQ();

            query = wsiQ
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
        /// <returns></returns>
        public IQueryable<ReportUnit> CountAssignments(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;
            var waQ = waRepo.GetAllQ();


            query = waQ
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
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<ReportUnit> CountCancelled(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;
            var woQ = woRepo.GetAllQ();


            query = woQ.Where(whr => whr.dateTimeofWork.Date == beginDate
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
        /// <param name="dateRequested">A single DateTime parameter</param>
        /// <returns>IQueryable</returns>
        public IQueryable<TypeOfDispatchModel> CountTypeofDispatch(DateTime beginDate, DateTime endDate)
        {
            IQueryable<TypeOfDispatchModel> query;

            var waQ = waRepo.GetAllQ();
            var wrQ = wrRepo.GetAllQ();
            var loD = lookRepo.GetByKey(LCategory.worktype, "DWC").ID;
            var loH = lookRepo.GetByKey(LCategory.worktype, "HHH").ID;


            query = waQ
                .GroupJoin(wrQ,
                    wa => new { waid = (int)wa.workOrderID, waw = (int?)wa.workerAssignedID },
                    wr => new { waid = (int)wr.WorkOrderID, waw = (int?)wr.WorkerID },
                    (wa, wr) => new
                    {
                        dtow = wa.workOrder.dateTimeofWork.Date,
                        workerAssignedID = wa.workerAssignedID,
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
                        hhhPropio = group.Sum(a => a.hhhPatron),
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
        public IQueryable<AverageWageModel> HourlyWageAverage(DateTime beginDate, DateTime endDate)
        {
            IQueryable<AverageWageModel> query;

            var waQ = waRepo.GetAllQ();
            var woQ = woRepo.GetAllQ();
            var wsiQ = wsiRepo.GetAllQ();

            //ensure we are getting all relevant times (no assumptions)

            query = waQ
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
        /// <returns></returns>
        public IQueryable<ReportUnit> ListJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;

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
                    count = group.Count() > 0 ? group.Count() : 0,
                    info = group.Key.enText ?? ""
                });

            return query;
        }

        public IQueryable<ZipUnit> ListJobsByZip(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ZipUnit> query;

            var waQ = waRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = waQ
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
                .Select(x => new ZipUnit
                {
                    zip = x.Key.zips,
                    count = x.Count() > 0 ? x.Count() : 0,
                    info = x.Key.jobs
                });

            return query;
        }

        /// <summary>
        /// Lists most popular zip codes for a given time period.
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<ZipModel> ListOrdersByZipCode(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ZipModel> query;

            var waQ = waRepo.GetAllQ();
            var eQ = eRepo.GetAllQ();
            var skillsQ = ListJobsByZip(beginDate, endDate);

            query = waQ
                .Where(w => w.workOrder.dateTimeofWork >= beginDate 
                         && w.workOrder.dateTimeofWork <= endDate)
                .GroupBy(a => new { zip = a.workOrder.zipcode })
                .Select(x => new ZipModel
                {
                    zips = x.Key.zip,
                    jobs = x.Count(),
                    emps = eQ
                            .Where(w => w.zipcode == x.Key.zip
                                     && w.active == true)
                            .Count(),
                    skills = skillsQ.Where(w => w.zip == x.Key.zip),
                });

            return query;
        }

        public IQueryable<PlacementUnit> WorkersInJobs(DateTime beginDate, DateTime endDate)
        {
            IQueryable<PlacementUnit> query;


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

            query = waQ
                .Where(whr => whr.workOrder.dateTimeofWork >= beginDate
                    && whr.workOrder.dateTimeofWork <= endDate)
                .GroupBy(g => new
                    {
                        dtow = g.workOrder.dateTimeofWork.Date,
                    })
                .Select(c => new
                    {
                        dtow = c.Key.dtow,
                        permCount = c.Count(pp => pp.workOrder.permanentPlacement),
                        tempCount = c.Count(tp => !tp.workOrder.permanentPlacement)
                    })
                .Join(undup, x => x.dtow, y => y.dtow, (x, y) => new
                    {
                        dtow = x.dtow,
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
        public IQueryable<ActivityUnit> GetAllActivitySignins(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ActivityUnit> query;

            var asQ = asRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = asQ.Join(lQ,
                    aj => aj.Activity.nameID,
                    lj => lj.ID,
                    (aj, lj) => new
                    {
                        name = lj.text_EN,
                        type = aj.Activity.typeID,
                        person = aj.person,
                        date = aj.Activity.dateStart.Date
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
                    signin.date <= endDate &&
                    signin.date >= beginDate)
                .GroupBy(gb => new { gb.date, gb.name, gb.type })
                .Select(grouping => new ActivityUnit
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
        public IQueryable<ReportUnit> AdultsEnrolledAndAssessedInESL()
        {
            IQueryable<ReportUnit> query;

            var asQ = asRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = asQ
                .Join(lQ,
                asi => asi.Activity.nameID,
                look => look.ID,
                (asi, look) => new
                {
                    dwc = asi.dwccardnum,
                    name = look.text_EN,
                    date = asi.Activity.dateStart.Date,
                    mins = 60 * asi.Activity.dateEnd.Subtract(asi.Activity.dateStart).Hours
                              + asi.Activity.dateEnd.Subtract(asi.Activity.dateStart).Minutes
                })
                .Where(whr => whr.name.Contains("English"))
                .GroupBy(gb => gb.dwc)
                .Select(sel => new
                {
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

                // These are the people who've had more than 12 hours of class and their last class was in the given period.

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
        /// <returns>date, enrolledOnDate, expiredOnDate, count</returns>
        public IQueryable<StatusUnit> MemberStatusByDate(DateTime beginDate, DateTime endDate, string unitOfMeasure, int interval)
        {
            // TODO: Add years as an option for the year-to-year comparison.
            IQueryable<StatusUnit> query;

            var wQ = wRepo.GetAllQ();

            if (unitOfMeasure == "months" && interval > 0)
            {
                int months = ((endDate.Year - beginDate.Year) * 12) + endDate.Month - beginDate.Month;
                query = Enumerable
                  .Range(0, months / interval)
                  .Select(w => endDate.AddMonths(w * -interval))
                  .Select(x => new StatusUnit
                      {
                          date = x,
                          count = wQ.Where(y => y.dateOfMembership < x && y.memberexpirationdate > x).Count(),
                          enrolledOnDate = wQ.Where(y => y.dateOfMembership >= DbFunctions.AddMonths(x, -interval) && y.dateOfMembership <= x).Count(),
                          expiredOnDate = wQ.Where(y => y.memberexpirationdate >= DbFunctions.AddMonths(x, -interval) && y.memberexpirationdate <= x).Count()
                      })
                  .AsQueryable();
                return query;
            }
            else if (unitOfMeasure == "days" && interval > 0)
            {
                query = Enumerable
                    .Range(0, 1 + (endDate.Subtract(beginDate).Days / interval))
                    .Select(w => endDate.AddDays(w * -interval))
                    .Select(x => new StatusUnit
                        {
                            date = x,
                            count = wQ.Where(y => y.dateOfMembership < x && y.memberexpirationdate > x).Count(),
                            enrolledOnDate = wQ.Where(y => y.dateOfMembership == x).Count(),
                            expiredOnDate = wQ.Where(y => y.memberexpirationdate == x).Count()
                        })
                    .AsQueryable();

                return query;
            }
            else throw new Exception("unitOfMeasure must be \"months\" or \"days\" and interval must be a positive integer > 0.");
        }

        public IQueryable<MemberDateModel> SingleAdults()
        {
            IQueryable<MemberDateModel> query;

            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            query = wQ
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

        public IQueryable<MemberDateModel> FamilyHouseholds()
        {
            IQueryable<MemberDateModel> query;

            var lQ = lookRepo.GetAllQ();
            var wQ = wRepo.GetAllQ();

            query = wQ
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

        public IQueryable<ReportUnit> ClientProfileHomeless(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(grp => grp.homeless)
                .Select(group => new ReportUnit
                {
                    info = group.Key == null ? "Unknown" : group.Key.ToString(),
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileHouseholdComposition(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


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
                        withChildren = gJoin.Key.withChildren.Value ? "With Children" : "Without Children",
                        count = gJoin.Count()
                    })
                .Select(glJoin => new ReportUnit
                {
                    info = glJoin.maritalStatus + ", " + glJoin.withChildren,
                    count = glJoin.count
                });

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileIncome(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


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
                .Select(group => new ReportUnit
                {
                    info = group.Key,
                    count = group.Count()
                })
                .OrderBy(ob => ob.count);

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileWorkerAge(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .Select(worker => new
                {
                    age = (new DateTime(1753, 1, 1) + (DateTime.Now - worker.dateOfBirth.Value)).Year - 1753,
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
                .Select(group => new ReportUnit
                {
                    info = group.Key,
                    count = group.Count()
                });

            return query;
			
        }

        public IQueryable<ReportUnit> ClientProfileGender(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.Person.gender)
                .Join(lQ,
                    group => group.Key,
                    look => look.ID,
                    (group, look) => new ReportUnit
                    {
                        count = group.Count(),
                        info = look.text_EN
                    });

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileHasDisability(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.disabled)
                .Select(group => new ReportUnit
                {
                    info = group.Key.Value ? "Yes" : "No",
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileRaceEthnicity(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


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
                .Select(glJoin => new ReportUnit
                {
                    info = glJoin.race,
                    count = glJoin.count
                });

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileRefugeeImmigrant(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


            var wQ = wRepo.GetAllQ();
            var lQ = lookRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.immigrantrefugee)
                .Select(group => new ReportUnit
                {
                    info = group.Key.Value ? "Yes" : "No",
                    count = group.Count()
                });

            return query;
        }

        public IQueryable<ReportUnit> ClientProfileEnglishLevel(DateTime beginDate, DateTime endDate)
        {
            IQueryable<ReportUnit> query;


            var wQ = wRepo.GetAllQ();

            query = wQ
                .Where(whr => whr.memberexpirationdate > beginDate && whr.dateOfMembership < endDate)
                .GroupBy(worker => worker.englishlevelID)
                .Select(group => new ReportUnit
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
        public IEnumerable<DailySumData> DailySumController(DateTime date)
        {
            IEnumerable<TypeOfDispatchModel> dclCurrent;
            IEnumerable<ReportUnit> dailySignins;
            IEnumerable<ReportUnit> dailyUnique;
            IEnumerable<ReportUnit> dailyAssignments;
            IEnumerable<ReportUnit> dailyCancelled;
            IEnumerable<DailySumData> q;

            DateTime beginDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            dclCurrent = CountTypeofDispatch(beginDate, endDate).ToList();
            dailySignins = CountSignins(beginDate, endDate).ToList();
            dailyUnique = CountUniqueSignins(beginDate, endDate).ToList();
            dailyAssignments = CountAssignments(beginDate, endDate).ToList();
            dailyCancelled = CountCancelled(beginDate, endDate).ToList();

            q = dclCurrent
                .Select(group => new DailySumData
                {
                    date = group.date,
                    dwcList = group.dwcList,
                    dwcPropio = group.dwcPropio,
                    hhhList = group.hhhList,
                    hhhPropio = group.hhhPropio,
                    uniqueSignins = dailyUnique.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0,
                    totalSignins = dailySignins.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0,
                    totalAssignments = dailyAssignments.Where(whr => whr.date == group.date).Select(g => g.count).FirstOrDefault() ?? 0, // should be same as group.count...mayhap could avoid this join
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
        /// <returns></returns>
        public IEnumerable<WeeklySumData> WeeklySumController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<AverageWageModel> weeklyWages;
            IEnumerable<ReportUnit> weeklySignins;
            IEnumerable<ReportUnit> weeklyAssignments;
            IEnumerable<ReportUnit> weeklyJobs;
            IEnumerable<WeeklySumData> q;

            weeklyWages = HourlyWageAverage(beginDate, endDate).ToList();
            weeklySignins = CountSignins(beginDate, endDate).ToList();
            weeklyAssignments = CountAssignments(beginDate, endDate).ToList();
            weeklyJobs = ListJobs(beginDate, endDate).ToList();

            q = weeklyWages
                .Select(g => new WeeklySumData
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

        public IEnumerable<MonthlySumData> MonthlySumController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<ReportUnit> signins;
            IEnumerable<ReportUnit> unique;
            IEnumerable<ActivityUnit> classes;
            IEnumerable<PlacementUnit> workers;
            IEnumerable<AverageWageModel> average;
            IEnumerable<StatusUnit> status;

            IEnumerable<MonthlySumData> q; 

            IEnumerable<DateTime> getAllDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                    .Select(offset => beginDate.AddDays(offset))
                    .ToArray();

            signins = CountSignins(beginDate, endDate).ToList();
            unique = CountUniqueSignins(beginDate, endDate).ToList();
            classes = GetAllActivitySignins(beginDate, endDate).ToList();
            workers = WorkersInJobs(beginDate, endDate).ToList();
            average = HourlyWageAverage(beginDate, endDate).ToList();
            status = MemberStatusByDate(beginDate, endDate, "days", 1).ToList();

            q = getAllDates
                .Select(g => new MonthlySumData
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

            return q;
        }

        public IEnumerable<ActivityData> ActivityReportController(DateTime beginDate, DateTime endDate, string reportType)
        {
            IEnumerable<ReportUnit> eslAssessed;
            IEnumerable<ActivityUnit> getAllClassAttendance;
            IEnumerable<ActivityData> q;
            IEnumerable<DateTime> getDates;

            if (reportType == "weekly" || reportType == "monthly")
            {
                 getDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                    .Select(offset => beginDate.AddDays(offset))
                    .ToArray();
            }
            else throw new Exception("Please select \"weekly\" or \"monthly\" as the report type.");

            getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            eslAssessed = AdultsEnrolledAndAssessedInESL().ToList();
            var safetyTrainees = getAllClassAttendance
                        .Where(whr => whr.activityType == "Health & Safety");
            var skillsTrainees = getAllClassAttendance
                        .Where(whr => whr.activityType == "Skills Training" || whr.activityType == "Leadership Development");
            var basGardenTrainees = getAllClassAttendance.Where(basic => basic.info == "Basic Gardening");
            var advGardenTrainees = getAllClassAttendance.Where(adv => adv.info == "Advanced Gardening");
            var finTrainees = getAllClassAttendance.Where(fin => fin.info == "Financial Education");
            var oshaTrainees = getAllClassAttendance.Where(osha => osha.info.Contains("OSHA"));

            q = getDates
                .Select(g => new ActivityData
                {
                    dateStart = g,
                    safety = safetyTrainees.Where(whr => whr.date == g).Count(),
                    skills = skillsTrainees.Where(whr => whr.date == g).Count(),
                    esl = eslAssessed.Where(whr => whr.date == g).Count(),
                    basGarden = basGardenTrainees.Where(whr => whr.date == g).Count(),
                    advGarden = advGardenTrainees.Where(whr => whr.date == g).Count(),
                    finEd = finTrainees.Where(whr => whr.date == g).Count(),
                    osha = oshaTrainees.Where(whr => whr.date == g).Count(),
                    drilldown = getAllClassAttendance.Where(whr => whr.date == g)
                });

            return q;
        }

        public IEnumerable<YearSumData> YearlySumController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<ReportUnit> signins;
            IEnumerable<ReportUnit> unique;
            IEnumerable<ActivityUnit> classes;
            IEnumerable<PlacementUnit> workers;
            IEnumerable<AverageWageModel> average;
            IEnumerable<StatusUnit> status;

            IEnumerable<YearSumData> q;

            IEnumerable<DateTime> getDates = Enumerable.Range(1, 4)
                    .Select(offset => endDate.AddMonths(-offset * 3))
                    .ToArray();

            signins = CountSignins(beginDate, endDate).ToList();
            unique = CountUniqueSignins(beginDate, endDate).ToList();
            classes = GetAllActivitySignins(beginDate, endDate).ToList();
            workers = WorkersInJobs(beginDate, endDate).ToList();
            average = HourlyWageAverage(beginDate, endDate).ToList();
            status = MemberStatusByDate(beginDate, endDate, "months", 3).ToList();

            q = getDates
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
            IEnumerable<ReportUnit> eslAssessed;
            IEnumerable<ActivityUnit> getAllClassAttendance;
            
            IEnumerable<ActivityData> q;

            IEnumerable<DateTime> getDates = Enumerable.Range(1, 4)
                .Select(offset => endDate.AddMonths(-offset * 3))
                .ToArray();

            getAllClassAttendance = GetAllActivitySignins(beginDate, endDate).ToList();
            eslAssessed = AdultsEnrolledAndAssessedInESL().ToList();
            var safetyTrainees = getAllClassAttendance
                        .Where(whr => whr.info == "Chemical Safety" );
            var skillsTrainees = getAllClassAttendance
                        .Where(whr => whr.activityType == "Class" || whr.activityType == "Workshops" || whr.info == "Mujeres sin Fronteras");
            var basGardenTrainees = getAllClassAttendance.Where(basic => basic.info == "Basic Gardening");
            var advGardenTrainees = getAllClassAttendance.Where(adv => adv.info == "Advanced Gardening");
            var finTrainees = getAllClassAttendance.Where(fin => fin.info == "Financial Education");
            var oshaTrainees = getAllClassAttendance.Where(osha => osha.info.Contains("Health & Safety"));

            q = getDates
                .Select(x => new ActivityData
                {
                    dateStart = x,
                    dateEnd = x.AddMonths(3).AddDays(1),
                    safety = safetyTrainees.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
                    skills = skillsTrainees.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
                    esl = eslAssessed.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
                    basGarden = basGardenTrainees.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
                    advGarden = advGardenTrainees.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
                    finEd = finTrainees.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
                    osha = oshaTrainees.Where(y => y.date >= x && y.date < x.AddMonths(3).AddDays(1)).Count(),
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
        /// <returns>date, singleAdults, familyHouseholds, newSingleAdults, newFamilyHouseholds, zipCodeCompleteness</returns>
        public IEnumerable<NewWorkerData> NewWorkerController(DateTime beginDate, DateTime endDate, string reportType)
        {
            IEnumerable<NewWorkerData> q;
            IEnumerable<MemberDateModel> singleAdults;
            IEnumerable<MemberDateModel> familyHouseholds;
            IEnumerable<DateTime> getDates;

            singleAdults = SingleAdults().ToList();
            familyHouseholds = FamilyHouseholds().ToList();

            if (reportType == "weekly" || reportType == "monthly")
            {
                getDates = Enumerable.Range(0, 1 + endDate.Subtract(beginDate).Days)
                   .Select(offset => endDate.AddDays(-offset))
                   .ToArray();

                q = getDates
                    .Select(x => new NewWorkerData
                    {
                        dateStart = x,
                        dateEnd = x.AddDays(1),
                        singleAdults = singleAdults.Where(y => y.expDate >= x && y.memDate < x.AddDays(1)).Count(),
                        familyHouseholds = familyHouseholds.Where(y => y.expDate >= x && y.memDate < x.AddDays(1)).Count(),
                        newSingleAdults = singleAdults.Where(y => y.memDate >= x && y.memDate < x.AddDays(1)).Count(),
                        newFamilyHouseholds = familyHouseholds.Where(y => y.memDate >= x && y.memDate < x.AddDays(1)).Count(),
                        zipCompleteness = singleAdults.Where(y => y.zip != null && y.expDate >= x && y.memDate < x.AddDays(1)).Count()
                                        + familyHouseholds.Where(y => y.zip != null && y.expDate >= x && y.memDate < x.AddDays(1)).Count()
                    });
            }
            else if (reportType == "yearly")
            {
                getDates = Enumerable.Range(1, 4)
                    .Select(offset => endDate.AddMonths(-offset * 3))
                    .ToArray();

                q = getDates
                    .Select(x => new NewWorkerData
                    {
                        dateStart = x.AddDays(1),
                        dateEnd = x.AddMonths(3).AddDays(1),
                        singleAdults = singleAdults.Where(y => y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1)).Count(),
                        familyHouseholds = familyHouseholds.Where(y => y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1)).Count(),
                        newSingleAdults = singleAdults.Where(y => y.memDate >= x && y.memDate < x.AddMonths(3).AddDays(1)).Count(),
                        newFamilyHouseholds = familyHouseholds.Where(y => y.memDate >= x && y.memDate < x.AddMonths(3).AddDays(1)).Count(),
                        zipCompleteness = singleAdults.Where(y => y.zip != null && y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1)).Count()
                                        + familyHouseholds.Where(y => y.zip != null && y.expDate >= x && y.memDate < x.AddMonths(3).AddDays(1)).Count()
                    });
            }
            else throw new Exception("Report type must be \"weekly\", \"monthly\" or \"yearly\".");

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
        public IEnumerable<ZipModel> EmployerReportController(DateTime beginDate, DateTime endDate)
        {
            IEnumerable<ZipModel> topZips;
            topZips = ListOrdersByZipCode(beginDate, endDate).ToList();
            return topZips;
        }

    }
    #endregion

    #region Report Models
    // The standalone models in this section are mostly at the bottom. Models that serve
    // as units of larger reports are at the top. Most of the models extend from a class
    // called "ReportUnit" and the class and its derivatives are in the middle of this
    // section.

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
    /// This gets a little confusing because of all the
    /// different "Zip" models. ZipUnit is a direct
    /// extension of ReportUnit and includes a space
    /// for a zip code string.
    /// </summary>
    public class ZipUnit : ReportUnit
    {
        public string zip { get; set; }
    }

    public class ActivityUnit : ReportUnit
    {
        public string activityType { get; set; }
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
    }

    public class DailySumData : TypeOfDispatchModel
    {
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int cancelledJobs { get; set; }
        public int totalAssignments { get; set; }
    }
    /// <summary>
    /// A class to contain the data for the Weekly Report
    /// int totalSignins, int noWeekJobs, int weekJobsSector, int
    /// weekEstDailyHours, double weekEstPayment, double weekHourlyWage
    /// </summary>
    public class WeeklySumData
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

    /// <summary>
    /// A class containing all of the data for the Monthly Report with Details
    /// DateTime date, int totalDWCSignins, int totalHHHSignins
    /// dispatchedDWCSignins, int dispatchedHHHSignins
    /// </summary>
    public class MonthlySumData
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
        public IEnumerable<ActivityUnit> drilldown { get; set; }
    }

    #endregion
}
