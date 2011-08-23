using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;
using System.Globalization;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace Machete.Service
{
    public interface IWorkAssignmentService
    {
        IEnumerable<WorkAssignment> GetMany();
        IEnumerable<WorkAssignment> GetMany(Func<WorkAssignment, bool> where);
        IQueryable<WorkAssignment> GetManyQ(Func<WorkAssignment, bool> where);
        IQueryable<WorkAssignment> GetManyQ();
        //IEnumerable<WorkAssignment> GetManyByWO(int woID);
        IQueryable<WorkAssignmentSummary> GetSummary(string search);
        WorkAssignment Get(int id);
        WorkAssignment Create(WorkAssignment workAssignment, string user);
        void Delete(int id, string user);
        void Save(WorkAssignment workAssignment, string user);
        ServiceIndexView<WorkAssignment> GetIndexView(CultureInfo CI,
                                                        string search,
                                                        DateTime? date,
                                                        int? dwccardnum,
                                                        int? woid,
                                                        bool orderDescending,
                                                        int? displayStart,
                                                        int? displayLength,
                                                        string sortColName);
    }

    // Business logic for WorkAssignment record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkAssignmentService : IWorkAssignmentService
    {
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILookupRepository lRepo;
        private readonly MacheteContext DB;
        private static Regex isTimeSpecific = new Regex(@"^\s*\d{1,2}[\/-_]\d{1,2}[\/-_]\d{2,4}\s+\d{1,2}:\d{1,2}");
        private static Regex isDaySpecific = new Regex(@"^\s*\d{1,2}\/\d{1,2}\/\d{2,4}");
        private static Regex isMonthSpecific = new Regex(@"^\s*\d{1,2}\/\d{4,4}");
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentService", "");
        private WorkAssignment _workAssignment;
        //
        public WorkAssignmentService(IWorkAssignmentRepository waRepo, IWorkerRepository wRepo, ILookupRepository lRepo, IUnitOfWork unitOfWork)
        {
            this.waRepo = waRepo;
            this.unitOfWork = unitOfWork;
            this.wRepo = wRepo;
            this.lRepo = lRepo;
            DB = new MacheteContext();
        }

        public IEnumerable<WorkAssignment> GetMany()
        {
            return waRepo.GetAll();
        }
        public IEnumerable<WorkAssignment> GetMany(Func<WorkAssignment, bool> where)
        {
            return waRepo.GetMany(where);
        }
        public IQueryable<WorkAssignment> GetManyQ()
        {
            return waRepo.GetAllQ().AsQueryable();
        }

        public IQueryable<WorkAssignment> GetManyQ(Func<WorkAssignment, bool> where)
        {

            return waRepo.GetAllQ().Where(where).AsQueryable();
        }

        public WorkAssignment Get(int id)
        {
            var workAssignment = waRepo.GetById(id);
            return workAssignment;
        }

        public ServiceIndexView<WorkAssignment> GetIndexView(
                                                    CultureInfo CI,
                                                    string search,
                                                    DateTime? date,
                                                    int? dwccardnum,
                                                    int? woid,
                                                    bool orderDescending,
                                                    int? displayStart,
                                                    int? displayLength,
                                                    string sortColName)
        {
            IQueryable<WorkAssignment> queryableWA = waRepo.GetAllQ();
            IEnumerable<WorkAssignment> enumedWA;
            IEnumerable<WorkAssignment> filteredWA;
            bool isDateTime = false;

            IEnumerable<Lookup> lCache = LookupCache.getCache();
            // 
            // DATE
            //
            if (date != null)
            {
                queryableWA = queryableWA.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, date) == 0 ? true : false);
            }
            // 
            // WOID
            //
            if (woid != null && woid != 0) queryableWA = queryableWA.Where(p => p.workOrderID==woid);
            // 
            // SEARCH STRING
            //
            if (!string.IsNullOrEmpty(search))
            {
                DateTime parsedTime;
                if (isDateTime = DateTime.TryParse(search, out parsedTime))
                {
                    if (isMonthSpecific.IsMatch(search))  //Regex for month/year
                        queryableWA = queryableWA.Where(p => EntityFunctions.DiffMonths(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                        queryableWA = queryableWA.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                        queryableWA = queryableWA.Where(p => EntityFunctions.DiffHours(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                }
                else
                {
                    queryableWA = queryableWA
                        .Join(lRepo.GetAllQ(), wa => wa.skillID, sk => sk.ID, (wa, sk) => new { wa, sk })
                        .Where(p => SqlFunctions.StringConvert((decimal)p.wa.workOrder.paperOrderNum).Contains(search) ||
                            p.wa.description.Contains(search) ||
                            p.sk.text_EN.Contains(search) ||
                            p.sk.text_ES.Contains(search) ||
                            //p.dateupdated.ToString().ContainsOIC(param.sSearch) ||
                            p.wa.Updatedby.Contains(search)).Select(p => p.wa);
                }
            }
            int? skill1 =null; 
            int? skill2 =null; 
            int? skill3= null;
            int? skill4 = null;
            int? skill5 = null;
            int? skill6 = null;
            Stack<int> primeskills = new Stack<int>();
            Stack<int> skills = new Stack<int>();

            if (dwccardnum != null && dwccardnum != 0)
            {
                Worker worker = WorkerCache.getCache(w => w.dwccardnum == dwccardnum).FirstOrDefault();
                if (worker != null)
                {
                    if (worker.skill1 != null) primeskills.Push((int)worker.skill1);
                    if (worker.skill2 != null) primeskills.Push((int)worker.skill2);
                    if (worker.skill3 != null) primeskills.Push((int)worker.skill3);

                    foreach (var skillid in primeskills)
                    {
                        skills.Push(skillid);
                        Lookup skill = LookupCache.getByID(skillid);
                        foreach (var subskill in lCache.Where(a => a.category == skill.category &&
                                                                   a.subcategory == skill.subcategory &&
                                                                   a.level < skill.level))
                        {
                            skills.Push(subskill.ID);
                        }
                    }
                    if (skills.Count() != 0) skill1 = skills.Pop();
                    if (skills.Count() != 0) skill2 = skills.Pop();
                    if (skills.Count() != 0) skill3 = skills.Pop();
                    if (skills.Count() != 0) skill4 = skills.Pop();
                    if (skills.Count() != 0) skill5 = skills.Pop();
                    if (skills.Count() != 0) skill6 = skills.Pop();
                    //enumedWA = queryableWA.AsEnumerable();
                    filteredWA = queryableWA.Join(lCache,
                                                       wa => wa.skillID,
                                                       sk => sk.ID,
                                                       (wa, sk) => new { wa, sk })
                                                 .Where(jj => jj.wa.englishLevelID <= worker.englishlevelID &&
                                                              jj.sk.typeOfWorkID.Equals(worker.typeOfWorkID) && (
                                                              jj.wa.skillID.Equals(skill1) ||
                                                              jj.wa.skillID.Equals(skill2) ||
                                                              jj.wa.skillID.Equals(skill3) ||
                                                              jj.wa.skillID.Equals(skill4) ||
                                                              jj.wa.skillID.Equals(skill5) ||
                                                              jj.wa.skillID.Equals(skill6) ||
                                                              jj.sk.speciality == false)
                                                              )
                                                //.Select(jj => jj.wa).AsQueryable();
                                                 .Select(jj => jj.wa);
                }
                else
                {
                    filteredWA = queryableWA.AsEnumerable();
                }
            }
            {
                filteredWA = queryableWA.AsEnumerable();
            }
            ////Sort the Persons based on column selection
            switch (sortColName)
            {
                case "pWAID": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.pseudoID) : filteredWA.OrderBy(p => p.pseudoID); break;
                case "skill": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.skillID) : filteredWA.OrderBy(p => p.skillID); break;
                case "earnings": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.hourlyWage * p.hours * p.days) : filteredWA.OrderBy(p => p.hourlyWage * p.hours * p.days); break;
                case "hourlywage": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.hourlyWage) : filteredWA.OrderBy(p => p.hourlyWage); break;
                case "hours": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.hours) : filteredWA.OrderBy(p => p.hours); break;
                case "days": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.days) : filteredWA.OrderBy(p => p.days); break;
                case "WOID": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.workOrderID) : filteredWA.OrderBy(p => p.workOrderID); break;
                case "WAID": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.ID) : filteredWA.OrderBy(p => p.ID); break;
                case "description": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.description) : filteredWA.OrderBy(p => p.description); break;                
                case "updatedby": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.Updatedby) : filteredWA.OrderBy(p => p.Updatedby); break;
                case "dateupdated": filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.dateupdated) : filteredWA.OrderBy(p => p.dateupdated); break;
                default: filteredWA = orderDescending ? filteredWA.OrderByDescending(p => p.workOrder.dateTimeofWork) : filteredWA.OrderBy(p => p.workOrder.dateTimeofWork); break;
            }
            filteredWA = filteredWA.ToList();
            var filtered = filteredWA.Count();
            //Limit results to the display length and offset
            filteredWA = filteredWA.Skip((int)displayStart).Take((int)displayLength);
           
           var total = waRepo.GetAllQ().Count();
           return new ServiceIndexView<WorkAssignment>
           {
               query = filteredWA.AsEnumerable(),
               filteredCount = filtered,
               totalCount = total
           };
      }

        public IQueryable<WorkAssignmentSummary> GetSummary(string search)
        {
            IQueryable<WorkAssignment> query;
            if (!string.IsNullOrEmpty(search))
                query = QueryDate(waRepo.GetAllQ(), search);
            else
                query = waRepo.GetAllQ();
            var sum_query = from wa in query
                            group wa by new
                            {
                                dateSoW = EntityFunctions.TruncateTime(wa.workOrder.dateTimeofWork),
                                //dateSoW = wa.workOrder.dateTimeofWork,
                                wa.workOrder.status
                            } into dayGroup
                            select new WorkAssignmentSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.status,
                                count = dayGroup.Count()
                            };

            return sum_query;
        }


        public WorkAssignment Create(WorkAssignment workAssignment, string user)
        {
            workAssignment.createdby(user);
            _workAssignment = waRepo.Add(workAssignment);
            unitOfWork.Commit();
            _log(workAssignment.ID, user, "WorkAssignment created");
            return _workAssignment;
        }

        public void Delete(int id, string user)
        {
            var workAssignment = waRepo.GetById(id);
            waRepo.Delete(workAssignment);
            _log(id, user, "WorkAssignment deleted");
            unitOfWork.Commit();
        }

        public void Save(WorkAssignment workAssignment, string user)
        {
            workAssignment.updatedby(user);
            _log(workAssignment.ID, user, "WorkAssignment edited");
            unitOfWork.Commit();
        }

        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
        private IQueryable<WorkAssignment> QueryDate(IQueryable<WorkAssignment> query, string search)
        {

            //Using DateTime.TryParse as determiner of date/string
            DateTime parsedTime;
            if (DateTime.TryParse(search, out parsedTime))
            {
                if (isMonthSpecific.IsMatch(search))  //Regex for month/year
                    return query.Where(p => EntityFunctions.DiffMonths(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                    return query.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                    return query.Where(p => EntityFunctions.DiffHours(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
            }
            return query;

        }
    }
}