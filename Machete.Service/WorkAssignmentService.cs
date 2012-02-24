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
        IQueryable<WorkAssignmentSummary> GetSummary(string search);
        WorkAssignment Get(int id);
        WorkAssignment Create(WorkAssignment workAssignment, string user);
        bool Assign(WorkAssignment assignment, WorkerSignin signin, string user);
        bool Unassign(int? wsiid, int? waid, string user);
        void Delete(int id, string user);
        void Save(WorkAssignment workAssignment, string user);
        ServiceIndexView<WorkAssignment> GetIndexView(DispatchOptions o);
    }

    // Business logic for WorkAssignment record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkAssignmentService : IWorkAssignmentService
    {
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILookupRepository lRepo;
        private readonly IWorkerRequestRepository wrRepo;
        private readonly MacheteContext DB;
        private static int lkup_dwc;
        private static int lkup_hhh;
        private static Regex isTimeSpecific = new Regex(@"^\s*\d{1,2}[\/-_]\d{1,2}[\/-_]\d{2,4}\s+\d{1,2}:\d{1,2}");
        private static Regex isDaySpecific = new Regex(@"^\s*\d{1,2}\/\d{1,2}\/\d{2,4}");
        private static Regex isMonthSpecific = new Regex(@"^\s*\d{1,2}\/\d{4,4}");
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentService", "");
        private WorkAssignment _workAssignment;
        //
        public WorkAssignmentService(IWorkAssignmentRepository waRepo, 
                                     IWorkerRepository wRepo, 
                                     ILookupRepository lRepo, 
                                     IWorkerSigninRepository wsiRepo,
                                     IWorkerRequestRepository wrRepo,
                                     IUnitOfWork unitOfWork)
        {
            this.waRepo = waRepo;
            this.unitOfWork = unitOfWork;
            this.wRepo = wRepo;
            this.lRepo = lRepo;
            this.wsiRepo = wsiRepo;
            this.wrRepo = wrRepo;
            DB = new MacheteContext();

        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        #region GET__()
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
        #endregion
        public ServiceIndexView<WorkAssignment> GetIndexView(DispatchOptions o)
        {
            IQueryable<WorkAssignment> queryableWA = waRepo.GetAllQ();
            lkup_dwc = LookupCache.getSingleEN("worktype", "(DWC) Day Worker Center");
            lkup_hhh = LookupCache.getSingleEN("worktype", "(HHH) Household Helpers");
            IEnumerable<WorkAssignment> filteredWA;
            bool isDateTime = false;
            DateTime sunday;
            IEnumerable<Lookup> lCache = LookupCache.getCache();
            // 
            // DATE
            //
            if (o.date != null)
            {
                if (o.date.Value.DayOfWeek == DayOfWeek.Saturday)
                {
                    sunday = o.date.Value.AddDays(1);
                    queryableWA = queryableWA.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, o.date) == 0 ? true : false ||
                        EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, sunday) == 0 ? true : false
                        );
                }
                else
                {
                    queryableWA = queryableWA.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, o.date) == 0 ? true : false);
                }
            }
            // 
            // typeofwork ( DWC / HHH )
            //          
            if (o.typeofwork_grouping == lkup_dwc)
            {
                queryableWA = queryableWA.Join(lRepo.GetAllQ(),
                                                wa => wa.skillID,
                                                sk => sk.ID,
                                                (wa, sk) => new { wa, sk })
                                         .Where(jj => jj.sk.typeOfWorkID == lkup_dwc)
                                         .Select(jj => jj.wa);                                             
            }
            if (o.typeofwork_grouping == lkup_hhh)
            {
                queryableWA = queryableWA.Join(lRepo.GetAllQ(),
                                                wa => wa.skillID,
                                                sk => sk.ID,
                                                (wa, sk) => new { wa, sk })
                                         .Where(jj => jj.sk.typeOfWorkID == lkup_hhh)
                                         .Select(jj => jj.wa);
            }          
            // 
            // WOID
            //
            if (o.woid != null && o.woid != 0) queryableWA = queryableWA.Where(p => p.workOrderID == o.woid);
            // 
            // Status filtering
            //
            if (o.status != null && o.status != 0)
            {
                queryableWA = queryableWA.Where(p => p.workOrder.status == o.status);
            }
            // 
            // pending filtering
            //
            if (o.showPending == false)
            {
                //int pending = LookupCache.getSingleEN("orderstatus", "Pending");
                queryableWA = queryableWA.Where(p => p.workOrder.status != woStatus.pending);
            }
            // 
            // wa_grouping
            //
            switch (o.wa_grouping) 
            {
                case "open": queryableWA = queryableWA.Where(p => p.workerAssignedID == null); break;
                case "assigned": queryableWA = queryableWA.Where(p => p.workerAssignedID != null); break;
                case "requested": 
                    queryableWA = queryableWA.Where(p => p.workerAssignedID == null && p.workOrder.workerRequests.Any() == true); 

                    break;
                case "skilled": queryableWA = queryableWA.Join(lRepo.GetAllQ(),
                                    wa => wa.skillID,
                                    sk => sk.ID,
                                    (wa, sk) => new { wa, sk })
                             .Where(jj => jj.sk.speciality == true && jj.wa.workerAssigned == null)
                             .Select(jj => jj.wa);                    
                    break;                                  
            }
            // 
            // SEARCH STRING
            //
            if (!string.IsNullOrEmpty(o.search))
            {
                DateTime parsedTime;
                if (isDateTime = DateTime.TryParse(o.search, out parsedTime))
                {
                    if (isMonthSpecific.IsMatch(o.search))  //Regex for month/year
                        queryableWA = queryableWA.Where(p => EntityFunctions.DiffMonths(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isDaySpecific.IsMatch(o.search))  //Regex for day/month/year
                        queryableWA = queryableWA.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isTimeSpecific.IsMatch(o.search)) //Regex for day/month/year time
                        queryableWA = queryableWA.Where(p => EntityFunctions.DiffHours(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
                }
                else
                {
                    queryableWA = queryableWA
                        .Join(lRepo.GetAllQ(), wa => wa.skillID, sk => sk.ID, (wa, sk) => new { wa, sk })
                        .Where(p => SqlFunctions.StringConvert((decimal)p.wa.workOrder.paperOrderNum).Contains(o.search) ||
                            p.wa.description.Contains(o.search) ||
                            p.sk.text_EN.Contains(o.search) ||
                            p.sk.text_ES.Contains(o.search) ||
                            //p.dateupdated.ToString().ContainsOIC(param.sSearch) ||
                            p.wa.Updatedby.Contains(o.search)).Select(p => p.wa);
                }
            }
            //
            // Skill kludge. Assuming there won't be more than 6 cascading skill matches
            // horrible flattening job of relational data
            //
            int? skill1 =null; 
            int? skill2 =null; 
            int? skill3= null;
            int? skill4 = null;
            int? skill5 = null;
            int? skill6 = null;
            Stack<int> primeskills = new Stack<int>();
            Stack<int> skills = new Stack<int>();
            //
            // filter on member ID, showing only assignments available to the member based on their
            // skills
            if (o.dwccardnum != null && o.dwccardnum != 0)
            {
                Worker worker = WorkerCache.getCache().FirstOrDefault(w => w.dwccardnum == o.dwccardnum);
                if (worker != null)
                {
                    if (worker.skill1 != null) primeskills.Push((int)worker.skill1);
                    if (worker.skill2 != null) primeskills.Push((int)worker.skill2);
                    if (worker.skill3 != null) primeskills.Push((int)worker.skill3);

                    foreach (var skillid in primeskills)
                    {
                        skills.Push(skillid);
                        Lookup skill = LookupCache.getBySkillID(skillid);
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
                    filteredWA = queryableWA.AsEnumerable();
                    filteredWA = filteredWA.Join(lCache,
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
                                                 .Select(jj => jj.wa).AsEnumerable();
                }
                else
                {
                    filteredWA = queryableWA.AsEnumerable();
                }
            } else
            {
                filteredWA = queryableWA.AsEnumerable();
            }
            //
            //Sort the Persons based on column selection
            filteredWA = filteredWA.GroupJoin(WorkerCache.getCache(), wa => wa.workerAssignedID, wc => wc.ID, (wa, wc) => new { wa, wc })
                            .SelectMany(jj => jj.wc.DefaultIfEmpty(new Worker { ID = 0, dwccardnum = 0}),
                            (jj, row) => jj.wa);

            switch (o.sortColName)
            {
                case "pWAID": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => string.Format("{0,5:D5}", p.workOrder.paperOrderNum) + "-" + p.pseudoID) : filteredWA.OrderBy(p => string.Format("{0,5:D5}", p.workOrder.paperOrderNum) + "-" + p.pseudoID); break;
                case "skill": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.skillID) : filteredWA.OrderBy(p => p.skillID); break;
                case "earnings": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.hourlyWage * p.hours * p.days) : filteredWA.OrderBy(p => p.hourlyWage * p.hours * p.days); break;
                case "hourlywage": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.hourlyWage) : filteredWA.OrderBy(p => p.hourlyWage); break;
                case "hours": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.hours) : filteredWA.OrderBy(p => p.hours); break;
                case "days": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.days) : filteredWA.OrderBy(p => p.days); break;
                case "WOID": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.workOrderID) : filteredWA.OrderBy(p => p.workOrderID); break;
                case "WAID": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.ID) : filteredWA.OrderBy(p => p.ID); break;
                case "description": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.description) : filteredWA.OrderBy(p => p.description); break;
                case "updatedby": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.Updatedby) : filteredWA.OrderBy(p => p.Updatedby); break;
                case "dateupdated": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.dateupdated) : filteredWA.OrderBy(p => p.dateupdated); break;
                case "assignedWorker": filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.workerAssigned == null ? 0 : p.workerAssigned.dwccardnum) : filteredWA.OrderBy(p => p.workerAssigned == null ? 0 : p.workerAssigned.dwccardnum); break;
                default: filteredWA = o.orderDescending ? filteredWA.OrderByDescending(p => p.workOrder.dateTimeofWork) : filteredWA.OrderBy(p => p.workOrder.dateTimeofWork); break;
            }
            filteredWA = filteredWA.ToList();
            var filtered = filteredWA.Count();
            //Limit results to the display length and offset
            if ((int)o.displayLength >= 0)
                filteredWA = filteredWA.Skip((int)o.displayStart).Take((int)o.displayLength);
           
           var total = waRepo.GetAllQ().Count();
           return new ServiceIndexView<WorkAssignment>
           {
               query = filteredWA.AsEnumerable(),
               filteredCount = filtered,
               totalCount = total
           };
      }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waid"></param>
        /// <param name="wsiid"></param>
        /// <returns></returns>
        #region ASSIGNS
        public bool Assign(WorkAssignment asmt, WorkerSignin signin, string user)
        {
            //Assignments must be explicitly unassigned first; throws exception if either record is in assigned state
            if (signin == null) throw new NullReferenceException("WorkerSignin is null");
            if (signin.WorkAssignmentID != null) throw new MacheteDispatchException("WorkerSignin already associated with WorkAssignment ID " + signin.WorkAssignmentID);
            if (signin.WorkerID == null) throw new MacheteIntegrityException("WorkerSignin key not associated with a Worker record. Check worker record, does it have a public ID?");
            if (asmt == null) throw new NullReferenceException("WorkAssignment is null");
            if (asmt.workerSigninID != null) throw new MacheteDispatchException("WorkAssignment already associated with WorkerSignin ID " + asmt.workerSigninID);
            if (asmt.workerSigninID == null && asmt.workerAssignedID != null && asmt.workerAssignedID != signin.WorkerID) 
                throw new MacheteDispatchException("Orphaned WorkAssignment, associated with Worker ID " + asmt.workerAssignedID +"; Unassign first, the assign to new Worker");
            Worker worker = wRepo.Get(w => w.dwccardnum == signin.dwccardnum);
            if (worker == null) throw new NullReferenceException("Worker for key " + signin.dwccardnum.ToString() + " is null");
            if (worker.ID != signin.WorkerID) throw new MacheteIntegrityException("WorkerSignin's internal WorkerID and public worker ID don't match");
            //
            // Link signin with 
            signin.WorkAssignmentID = asmt.ID;
            asmt.workerSigninID = signin.ID;
            asmt.workerAssignedID = worker.ID;
            asmt.updatedby(user);
            signin.updatedby(user);
            unitOfWork.Commit();
            _log(asmt.ID, user, "WSIID:" + signin.ID + " Assign successful");
            return true;
        }

        public bool Unassign(int? waid, int? wsiid, string user)
        {
            WorkAssignment asmt = null;
            WorkerSignin signin = null;
            if (wsiid != null)
            {
                signin = wsiRepo.GetById((int)wsiid);
            }
            if (waid != null)
                asmt = waRepo.GetById((int)waid);


            if (signin == null && asmt == null) throw new NullReferenceException("Signin and WorkAssignment are both null");
            //
            //Try unassign with WorkAssignment record only
            if (signin == null) // Assignment processing
            {
                // legacy assignment; only thing to do is clear it.
                // wssid(null), 
                if (asmt.workerSigninID == null) //throw new MacheteIntegrityException("Unassign called on non-assigned WorkAssignment");
                {
                    asmt.workerAssignedID = null;
                    unitOfWork.Commit();
                    return true;
                }
                // Proper dispatch, called with only WorkAssignment
                else
                {
                    signin = wsiRepo.GetById((long)asmt.workerSigninID);
                }
            }
            else if (asmt == null) //asmt must be null -- Signin processing
            {
                if (signin.WorkAssignmentID == null) throw new MacheteIntegrityException("Unassign called on non-assigned WorkerSignin");
                asmt = waRepo.GetById((long)signin.WorkAssignmentID);
            }
            //Have both assignment and signin. 
            if (signin == null || asmt == null) throw new NullReferenceException("Signin and WorkAssignment are both null");
            //Try unassign with WorkerSignin only 
            if (signin.WorkAssignmentID != asmt.ID) throw new MacheteIntegrityException("WorkerSignin doesn't link back to referencing Assignment");          
            if (asmt.workerSigninID != signin.ID) throw new MacheteIntegrityException("WorkAssignment doesn't link back to referencing WorkerSignin");            
            signin.WorkAssignmentID = null;
            asmt.workerSigninID = null;
            asmt.workerAssignedID = null;
            asmt.updatedby(user);
            signin.updatedby(user);
            unitOfWork.Commit();
            _log(asmt.ID, user, "WSIID:" + signin.ID + " Unassign successful");
            return true;
        }
        #endregion
        #region CRUD
        public WorkAssignment Create(WorkAssignment workAssignment, string user)
        {
            workAssignment.createdby(user);
            _workAssignment = waRepo.Add(workAssignment);
            unitOfWork.Commit();
            _log(workAssignment.ID, user, "WorkAssignment created");
            return _workAssignment;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>

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
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns></returns>
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
    public class DispatchOptions
    {
            public CultureInfo CI;
            public string search;
            public DateTime? date;
            public int? dwccardnum;
            public int? woid;
            public int? status;
            public bool showPending;
            public bool orderDescending;
            public int? displayStart;
            public int? displayLength;
            public string sortColName;
            public string wa_grouping;
            public int? typeofwork_grouping;
    }
}