using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using System.Data.Objects;
using System.Text.RegularExpressions;
using System.Data.Objects.SqlClient;


namespace Machete.Service
{
    /// <summary>
    /// Contains all service-layer queries for manipulating Entity lists from DB
    /// </summary>
    public static class IndexViewBase
    {
        private static Regex isTimeSpecific = new Regex(@"^\s*\d{1,2}[\/-_]\d{1,2}[\/-_]\d{2,4}\s+\d{1,2}:\d{1,2}");
        private static Regex isDaySpecific = new Regex(@"^\s*\d{1,2}\/\d{1,2}\/\d{2,4}");
        private static Regex isMonthSpecific = new Regex(@"^\s*\d{1,2}\/\d{4,4}");
        #region SIGNINS
        public static void diffDays<T>(viewOptions o, ref IQueryable<T> q) where T : Signin
        {
            q = q.Where(p => EntityFunctions.DiffDays(p.dateforsignin, o.date) == 0 ? true : false);
        }
        public static void typeOfWork<T>(viewOptions o, ref IQueryable<T> q) where T : Signin
        {
            q = q.Where(wsi => wsi.worker.typeOfWorkID == o.typeofwork_grouping)
                 .Select(wsi => wsi);
        }
        #endregion
        #region WORKASSIGNMENTS
        public static void typeOfWork(viewOptions o, ref IQueryable<WorkAssignment> q, ILookupRepository lRepo)
        {
            q = q.Join(lRepo.GetAllQ(),
                        wa => wa.skillID,
                        sk => sk.ID,
                        (wa, sk) => new { wa, sk })
                    .Where(jj => jj.sk.typeOfWorkID == o.typeofwork_grouping)
                    .Select(jj => jj.wa);
        }
        public static void diffDays(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            DateTime sunday;
            if (o.date.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                sunday = o.date.Value.AddDays(1);
                q = q.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, o.date) == 0 ? true : false ||
                    EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, sunday) == 0 ? true : false
                    );
            }
            else
            {
                q = q.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, o.date) == 0 ? true : false);
            }
        }
        public static void WOID(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workOrderID == o.woid);
        }
        public static void status(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workOrder.status == o.status);
        }
        public static void filterPending(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workOrder.status != WorkOrder.iPending);
        }
        public static void waGrouping(viewOptions o, ref IQueryable<WorkAssignment> q, ILookupRepository lRepo)
        {
            switch (o.wa_grouping)
            {
                case "open": q = q.Where(p => p.workerAssignedID == null); break;
                case "assigned": q = q.Where(p => p.workerAssignedID != null); break;
                case "requested":
                    q = q.Where(p => p.workerAssignedID == null && p.workOrder.workerRequests.Any() == true);

                    break;
                case "skilled": q = q.Join(lRepo.GetAllQ(),
                                    wa => wa.skillID,
                                    sk => sk.ID,
                                    (wa, sk) => new { wa, sk })
                             .Where(jj => jj.sk.speciality == true && jj.wa.workerAssigned == null)
                             .Select(jj => jj.wa);
                    break;
            }
        }
        /// <summary>
        /// Filter WA queryable on a partial date string
        /// </summary>
        /// <param name="search">string that is part of a date</param>
        /// <param name="parsedTime">datetime.parse of the same string</param>
        /// <param name="query">WorkAssignment queryable</param>
        public static void filterOnDatePart(string search, DateTime parsedTime, ref IQueryable<WorkAssignment> query)
        {
            if (isMonthSpecific.IsMatch(search))  //Regex for month/year
                query = query.Where(p => EntityFunctions.DiffMonths(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
            if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                query = query.Where(p => EntityFunctions.DiffDays(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
            if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                query = query.Where(p => EntityFunctions.DiffHours(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
            //throw new ArgumentException("Date string not valid for Month,Day, or Hour pattern");
 
        }
        /// <summary>
        /// Filter WA queryable on a partial date string
        /// </summary>
        /// <param name="search">string that is part of a date</param>
        /// <param name="query">WorkAssignment queryable</param>
        public static void filterOnDatePart(string search, ref IQueryable<WorkAssignment> query)
        {
            DateTime parsedTime;
            if (DateTime.TryParse(search,out parsedTime))
                filterOnDatePart(search, parsedTime, ref query);           
        }
        public static void search(viewOptions o, ref IQueryable<WorkAssignment> q, ILookupRepository lRepo)
        {
            bool isDateTime = false;
            DateTime parsedTime;
            if (isDateTime = DateTime.TryParse(o.search, out parsedTime))            
                filterOnDatePart(o.search, parsedTime, ref q);            
            else
            {
                q = q
                    .Join(lRepo.GetAllQ(), wa => wa.skillID, sk => sk.ID, (wa, sk) => new { wa, sk })
                    .Where(p => SqlFunctions.StringConvert((decimal)p.wa.workOrder.paperOrderNum).Contains(o.search) ||
                        p.wa.description.Contains(o.search) ||
                        p.sk.text_EN.Contains(o.search) ||
                        p.sk.text_ES.Contains(o.search) ||
                        //p.dateupdated.ToString().ContainsOIC(param.sSearch) ||
                        p.wa.Updatedby.Contains(o.search)).Select(p => p.wa);
            }
        }      
        public static IEnumerable<WorkAssignment> filterOnSkill(viewOptions o, IQueryable<WorkAssignment> q)
        {
            //  "Machete --A series of good intentions, marinated in panic."
            //
            // Kludge panic. horrible flattening job of relational data.
            // Skills have hierarchy (painter, skilled painter, master painter)
            // kludge assumes hierarchy tree will never lead to more than 6 skill IDs to
            // match against. Hacked this late one night. 
            int? skill1 = null;
            int? skill2 = null;
            int? skill3 = null;
            int? skill4 = null;
            int? skill5 = null;
            int? skill6 = null;

            IEnumerable<WorkAssignment> filteredWA = q.AsEnumerable();
            Stack<int> primeskills = new Stack<int>();
            Stack<int> skills = new Stack<int>();
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
                    foreach (var subskill in LookupCache.getCache()
                        .Where(a => a.category == skill.category &&
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
                filteredWA = filteredWA.Join(LookupCache.getCache(), //LINQ
                                                   wa => wa.skillID,
                                                   sk => sk.ID,
                                                   (wa, sk) => new { wa, sk }
                                            )
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

            return filteredWA;            
        }
        public static void sortOnColName(string name, bool descending, ref IEnumerable<WorkAssignment> q)
        {
            switch (name)
            {
                case "pWAID": q = descending ? q.OrderByDescending(p => string.Format("{0,5:D5}", p.workOrder.paperOrderNum) + "-" + p.pseudoID) : q.OrderBy(p => string.Format("{0,5:D5}", p.workOrder.paperOrderNum) + "-" + p.pseudoID); break;
                case "skill": q = descending ? q.OrderByDescending(p => p.skillID) : q.OrderBy(p => p.skillID); break;
                case "earnings": q = descending ? q.OrderByDescending(p => p.hourlyWage * p.hours * p.days) : q.OrderBy(p => p.hourlyWage * p.hours * p.days); break;
                case "hourlywage": q = descending ? q.OrderByDescending(p => p.hourlyWage) : q.OrderBy(p => p.hourlyWage); break;
                case "hours": q = descending ? q.OrderByDescending(p => p.hours) : q.OrderBy(p => p.hours); break;
                case "days": q = descending ? q.OrderByDescending(p => p.days) : q.OrderBy(p => p.days); break;
                case "WOID": q = descending ? q.OrderByDescending(p => p.workOrderID) : q.OrderBy(p => p.workOrderID); break;
                case "WAID": q = descending ? q.OrderByDescending(p => p.ID) : q.OrderBy(p => p.ID); break;
                case "description": q = descending ? q.OrderByDescending(p => p.description) : q.OrderBy(p => p.description); break;
                case "updatedby": q = descending ? q.OrderByDescending(p => p.Updatedby) : q.OrderBy(p => p.Updatedby); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                case "assignedWorker": q = descending ? q.OrderByDescending(p => p.workerAssigned == null ? 0 : p.workerAssigned.dwccardnum) : q.OrderBy(p => p.workerAssigned == null ? 0 : p.workerAssigned.dwccardnum); break;
                default: q = descending ? q.OrderByDescending(p => p.workOrder.dateTimeofWork) : q.OrderBy(p => p.workOrder.dateTimeofWork); break;
            }
        }
        #endregion
        #region WORKORDERS
        public static void search(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            bool isDateTime = false;
            DateTime parsedTime;
            if (isDateTime = DateTime.TryParse(o.search, out parsedTime))
            {
                if (isMonthSpecific.IsMatch(o.search))  //Regex for month/year
                    q = q.Where(p => EntityFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isDaySpecific.IsMatch(o.search))  //Regex for day/month/year
                    q = q.Where(p => EntityFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isTimeSpecific.IsMatch(o.search)) //Regex for day/month/year time
                    q = q.Where(p => EntityFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);
            }
            else
            {
                q = q
                    .Where(p => SqlFunctions.StringConvert((decimal)p.ID).Contains(o.search) ||
                                SqlFunctions.StringConvert((decimal)p.paperOrderNum).Contains(o.search) ||
                                p.contactName.Contains(o.search) ||
                                p.workSiteAddress1.Contains(o.search) ||
                                p.Updatedby.Contains(o.search));
            }
        }
        public static void filterEmployer(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            q = q.Where(p => p.EmployerID.Equals((int)o.EmployerID)); //EmployerID for WorkOrderIndex view
        }
        public static void filterStatus(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            q = q.Where(p => p.status.Equals((int)o.status));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static IQueryable<WorkOrder> filterDateTimeOfWork(IQueryable<WorkOrder> query, string search)
        {

            //Using DateTime.TryParse as determiner of date/string
            DateTime parsedTime;
            if (DateTime.TryParse(search, out parsedTime))
            {
                if (isMonthSpecific.IsMatch(search))  //Regex for month/year
                    return query.Where(p => EntityFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                    return query.Where(p => EntityFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                    return query.Where(p => EntityFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);
            }
            return query;
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<WorkOrder> q)
        {
            switch (name)
            {
                //case "WOID": orderedWO = orderDescending ? q.OrderByDescending(p => p.dateTimeofWork) : q.OrderBy(p => p.dateTimeofWork); break;
                case "status": q = descending ? q.OrderByDescending(p => p.status) : q.OrderBy(p => p.status); break;
                case "transportMethod": q = descending ? q.OrderByDescending(p => p.transportMethodID) : q.OrderBy(p => p.transportMethodID); break;
                case "WAcount": q = descending ? q.OrderByDescending(p => p.workAssignments.Count) : q.OrderBy(p => p.workAssignments.Count); break;
                case "contactName": q = descending ? q.OrderByDescending(p => p.contactName) : q.OrderBy(p => p.contactName); break;
                case "workSiteAddress1": q = descending ? q.OrderByDescending(p => p.workSiteAddress1) : q.OrderBy(p => p.workSiteAddress1); break;
                case "updatedby": q = descending ? q.OrderByDescending(p => p.Updatedby) : q.OrderBy(p => p.Updatedby); break;
                case "WOID": q = descending ? q.OrderByDescending(p => p.paperOrderNum) : q.OrderBy(p => p.paperOrderNum); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                default: q = descending ? q.OrderByDescending(p => p.dateTimeofWork) : q.OrderBy(p => p.dateTimeofWork); break;
            }
        }
        #endregion
        #region EMPLOYERS
        public static void sortOnColName(string name, bool descending, ref IQueryable<Employer> q)
        {
            switch (name)
            {
                case "active": q = descending ? q.OrderByDescending(p => p.active) : q.OrderBy(p => p.active); break;
                case "name": q = descending ? q.OrderByDescending(p => p.name) : q.OrderBy(p => p.name); break;
                case "address1": q = descending ? q.OrderByDescending(p => p.address1) : q.OrderBy(p => p.address1); break;
                case "city": q = descending ? q.OrderByDescending(p => p.city) : q.OrderBy(p => p.city); break;
                case "phone": q = descending ? q.OrderByDescending(p => p.phone) : q.OrderBy(p => p.phone); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                default: q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
            }
        }
        public static void search(viewOptions o, ref IQueryable<Employer> q)
        {
            q = q.Where(p => //p.active.ToString().Contains(o.search) ||
                            p.name.Contains(o.search) ||
                            p.address1.Contains(o.search) ||
                            p.phone.Contains(o.search) ||
                            p.city.Contains(o.search));
        }
        #endregion
        #region PERSONS
        public static void search(viewOptions o, ref IQueryable<Person> q)
        {
            q = q
                .Where(p => p.firstname1.Contains(o.search) ||
                            p.firstname2.Contains(o.search) ||
                            p.lastname1.Contains(o.search) ||
                            p.lastname2.Contains(o.search) ||
                            p.phone.Contains(o.search));
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<Person> q)
        {
            switch (name)
            {
                case "active": q = descending ? q.OrderByDescending(p => p.active) : q.OrderBy(p => p.active); break;
                case "firstname1": q = descending ? q.OrderByDescending(p => p.firstname1) : q.OrderBy(p => p.firstname1); break;
                case "firstname2": q = descending ? q.OrderByDescending(p => p.firstname2) : q.OrderBy(p => p.firstname2); break;
                case "lastname1": q = descending ? q.OrderByDescending(p => p.lastname1) : q.OrderBy(p => p.lastname1); break;
                case "lastname2": q = descending ? q.OrderByDescending(p => p.lastname2) : q.OrderBy(p => p.lastname2); break;
                case "phone": q = descending ? q.OrderByDescending(p => p.phone) : q.OrderBy(p => p.phone); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                default: q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
            }
        }
        #endregion
        #region WORKERS
        public static void search(viewOptions o, ref IQueryable<Worker> q)
        {
            q = q.Where(p => p.dwccardnum.ToString().ContainsOIC(o.search) ||
                            p.active.ToString().ContainsOIC(o.search) ||
                            p.Person.firstname1.ContainsOIC(o.search) ||
                            p.Person.firstname2.ContainsOIC(o.search) ||
                            p.Person.lastname1.ContainsOIC(o.search) ||
                            p.Person.lastname2.ContainsOIC(o.search) ||
                            p.memberexpirationdate.ToString().ContainsOIC(o.search));
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<Worker> q)
        {
            switch (name)
            {
                case "dwccardnum": q = descending ? q.OrderByDescending(p => p.dwccardnum) : q.OrderBy(p => p.dwccardnum); break;
                case "wkrStatus": q = descending ? q.OrderByDescending(p => p.memberStatus) : q.OrderBy(p => p.memberStatus); break;
                case "firstname1": q = descending ? q.OrderByDescending(p => p.Person.firstname1) : q.OrderBy(p => p.Person.firstname1); break;
                case "firstname2": q = descending ? q.OrderByDescending(p => p.Person.firstname2) : q.OrderBy(p => p.Person.firstname2); break;
                case "lastname1": q = descending ? q.OrderByDescending(p => p.Person.lastname1) : q.OrderBy(p => p.Person.lastname1); break;
                case "lastname2": q = descending ? q.OrderByDescending(p => p.Person.lastname2) : q.OrderBy(p => p.Person.lastname2); break;
                case "memberexpirationdate": q = descending ? q.OrderByDescending(p => p.memberexpirationdate) : q.OrderBy(p => p.memberexpirationdate); break;
                default: q = descending ? q.OrderByDescending(p => p.ID) : q.OrderBy(p => p.ID); break;
            }
        }
        #endregion
        #region ACTIVITIES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="q"></param>
        /// <param name="lRepo"></param>
        public static void search(viewOptions o, ref IEnumerable<Activity> q)
        {
            q = q //LookupCache will be slow and needs to be converted to IQueryable
                .Where(p => LookupCache.byID(p.name, o.CI.TwoLetterISOLanguageName).ContainsOIC(o.search) ||
                            p.notes.ContainsOIC(o.search) ||
                            p.teacher.ContainsOIC(o.search) ||
                            LookupCache.byID(p.type, o.CI.TwoLetterISOLanguageName).ContainsOIC(o.search) ||
                            p.dateStart.ToString().ContainsOIC(o.search) ||
                            p.dateEnd.ToString().ContainsOIC(o.search));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="descending"></param>
        /// <param name="isoLandCode"></param>
        /// <param name="q"></param>
        public static void sortOnColName(string name, bool descending, string isoLandCode, ref IEnumerable<Activity> q)
        {
            switch (name)
            {
                case "name":
                    q = descending ?
                        q.OrderByDescending(p => LookupCache.byID(p.name, isoLandCode)) :
                        q.OrderBy(p => LookupCache.byID(p.name, isoLandCode));
                    break;
                case "type":
                    q = descending ?
                        q.OrderByDescending(p => LookupCache.byID(p.type, isoLandCode)) :
                        q.OrderBy(p => LookupCache.byID(p.type, isoLandCode));
                    break;
                case "count":
                    q = descending ?
                        q.OrderByDescending(p => p.Signins.Count()) :
                        q.OrderBy(p => p.Signins.Count());
                    break;
                case "teacher":
                    q = descending ?
                        q.OrderByDescending(p => p.teacher) :
                        q.OrderBy(p => p.teacher);
                    break;
                case "dateStart":
                    q = descending ?
                        q.OrderByDescending(p => p.dateStart) :
                        q.OrderBy(p => p.dateStart);
                    break;
                case "dateEnd":
                    q = descending ?
                        q.OrderByDescending(p => p.dateEnd) :
                        q.OrderBy(p => p.dateEnd);
                    break;
                case "dateupdated":
                    q = descending ?
                        q.OrderByDescending(p => p.dateupdated) :
                        q.OrderBy(p => p.dateupdated);
                    break;
                default:
                    q = descending ? q.OrderByDescending(p => p.dateStart) :
                        q.OrderBy(p => p.dateStart);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        public static void getUnassociated(int personID, ref IQueryable<Activity> q, IActivitySigninRepository asRepo)
        {
            q = q.Join(asRepo.GetAllQ(), a => a.ID, az => az.ActivityID, (a, az) => new { a, az })
                 .Where(p => p.az.WorkerID == personID)
                 .Select(p => p.a)
                 .Distinct();

        }

        #endregion
    }
}
