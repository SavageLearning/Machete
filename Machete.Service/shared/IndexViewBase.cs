#region COPYRIGHT
// File:     IndexViewBase.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
// License:  GPL v3
// Project:  Machete.Service
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Linq;
using System.Text.RegularExpressions;

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
            q = q.Where(p => DbFunctions.DiffDays(p.dateforsignin, (DateTime)o.date) == 0);
        }
        public static void search(viewOptions o, ref IQueryable<WorkerSignin> q)
        {
            q = q.Where(wsi => SqlFunctions.StringConvert((decimal)wsi.dwccardnum).Contains(o.sSearch) ||
                            wsi.worker.Person.firstname1.Contains(o.sSearch) ||
                            wsi.worker.Person.firstname2.Contains(o.sSearch) ||
                            wsi.worker.Person.lastname1.Contains(o.sSearch) ||
                            wsi.worker.Person.lastname2.Contains(o.sSearch)
                            );
        }
        public static void search<T>(viewOptions o, ref IEnumerable<T> e, IEnumerable<Worker> wcache) where T : Signin
        {
            e = e.Join(wcache, s => s.dwccardnum, w => w.dwccardnum, (s, w) => new { s, w })
                .Where(p => p.w.dwccardnum.ToString().ContainsOIC(o.sSearch) ||
                            p.w.Person.firstname1.ContainsOIC(o.sSearch) ||
                            p.w.Person.firstname2.ContainsOIC(o.sSearch) ||
                            p.w.Person.lastname1.ContainsOIC(o.sSearch) ||
                            p.w.Person.lastname2.ContainsOIC(o.sSearch))
                .Select(a => a.s);
        }
        public static void dwccardnum<T>(viewOptions o, ref IQueryable<T> q) where T : Signin
        {
            q = q.Where(wsi => wsi.dwccardnum == o.dwccardnum)
                 .Select(wsi => wsi);
        }
        public static void typeOfWork<T>(viewOptions o, ref IQueryable<T> q) where T : WorkerSignin
        {
            q = q.Where(wsi => wsi.worker.typeOfWorkID == o.typeofwork_grouping)
                 .Select(wsi => wsi);
        }
        public static void waGrouping(viewOptions o, ref IQueryable<WorkerSignin> q, IWorkerRequestService wrServ)
        {
            switch (o.wa_grouping)
            {
                case "open": q = q.Where(p => p.WorkAssignmentID == null); break;
                case "assigned": q = q.Where(p => p.WorkAssignmentID != null); break;
                case "skilled": q = q
                                     .Where(wsi => wsi.WorkAssignmentID == null &&
                                         wsi.worker.skill1 != null ||
                                         wsi.worker.skill2 != null ||
                                         wsi.worker.skill3 != null
                    );

                    break;
                case "requested":
                    if (o.date == null) throw new MacheteIntegrityException("Date cannot be null for Requested filter");
                    q = q.Where(p => p.WorkAssignmentID == null);
                    q = q.Join(wrServ.GetAll(), //LINQ
                                wsi => new
                                {
                                    K1 = (int)wsi.WorkerID,
                                    K2 = wsi.dateforsignin.Date
                                },
                                wr => new
                                {
                                    K1 = wr.WorkerID,
                                    K2 = wr.workOrder.dateTimeofWork.Date
                                },
                                (wsi, wr) => wsi);
                    break;
            }
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<WorkerSignin> q)
        {
            switch (name)
            {
                case "dwccardnum": q = descending ? q.OrderByDescending(p => p.dwccardnum) : q.OrderBy(p => p.dwccardnum); break;
                case "firstname1": q = descending ? q.OrderByDescending(p => p.worker.Person.firstname1) : q.OrderBy(p => p.worker.Person.firstname1); break;
                case "firstname2": q = descending ? q.OrderByDescending(p => p.worker.Person.firstname2) : q.OrderBy(p => p.worker.Person.firstname2); break;
                case "lastname1": q = descending ? q.OrderByDescending(p => p.worker.Person.lastname1) : q.OrderBy(p => p.worker.Person.lastname1); break;
                case "lastname2": q = descending ? q.OrderByDescending(p => p.worker.Person.lastname2) : q.OrderBy(p => p.worker.Person.lastname2); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                case "dateforsigninstring": q = descending ? q.OrderByDescending(p => p.dateforsignin) : q.OrderBy(p => p.dateforsignin); break;
                case "expirationDate": q = descending ? q.OrderByDescending(p => p.worker.memberexpirationdate) : q.OrderBy(p => p.worker.memberexpirationdate); break;
                case "lotterySequence":
                    q = descending ? 
                        q.OrderByDescending(p => p.lottery_sequence != null).ThenByDescending(p => p.lottery_sequence) :
                        q.OrderBy(p => p.lottery_sequence == null).ThenBy(p => p.lottery_sequence);
                    break;
                default: q = descending ? q.OrderByDescending(p => p.dateforsignin) : q.OrderBy(p => p.dateforsignin); break;
            }
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
                q = q.Where(p => 
                    DbFunctions.DiffDays(p.workOrder.dateTimeofWork, (DateTime)o.date) == 0
                 || DbFunctions.DiffDays(p.workOrder.dateTimeofWork, sunday) == 0
                );
            }
            else
            {
                q = q.Where(p => DbFunctions.DiffDays(p.workOrder.dateTimeofWork, (DateTime)o.date) == 0);
            }
        }
        public static void WOID(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workOrderID == o.woid);
        }
        public static void WID(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workerAssignedID == o.personID);
        }
        public static void status(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workOrder.statusID == o.status);
        }
        public static void filterPending(viewOptions o, ref IQueryable<WorkAssignment> q)
        {
            q = q.Where(p => p.workOrder.statusID != WorkOrder.iPending);
        }
        public static void waGrouping(viewOptions o, ref IQueryable<WorkAssignment> q, ILookupRepository lRepo)
        {
            //var completedID = LookupCache.getByKeys(LCategory.orderstatus, LOrderStatus.Completed);
            switch (o.wa_grouping)
            {
                case "open": q = q.Where(p => p.workerAssignedID == null 
                                           && p.workOrder.statusID == WorkOrder.iActive);
                    break;
                case "assigned": q = q.Where(p => p.workerAssignedID != null 
                                               && p.workOrder.statusID == WorkOrder.iActive); break;
                case "requested":
                    q = q.Where(p => p.workerAssignedID == null 
                                  && p.workOrder.workerRequests.Any() == true 
                                  && p.workOrder.statusID == WorkOrder.iActive);

                    break;
                case "skilled": q = q.Join(lRepo.GetAllQ(),
                                    wa => wa.skillID,
                                    sk => sk.ID,
                                    (wa, sk) => new { wa, sk })
                             .Where(jj => jj.sk.speciality == true 
                                       && jj.wa.workerAssigned == null 
                                       && jj.wa.workOrder.statusID == WorkOrder.iActive)
                             .Select(jj => jj.wa);
                    break;
                case "completed":
                    q = q.Where(wa => wa.workOrder.statusID == WorkOrder.iCompleted);
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
                query = query.Where(p => DbFunctions.DiffMonths(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
            if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                query = query.Where(p => DbFunctions.DiffDays(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
            if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                query = query.Where(p => DbFunctions.DiffHours(p.workOrder.dateTimeofWork, parsedTime) == 0 ? true : false);
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
            if (DateTime.TryParse(search, out parsedTime))
                filterOnDatePart(search, parsedTime, ref query);
        }
        public static void search(viewOptions o, ref IQueryable<WorkAssignment> q, ILookupRepository lRepo)
        {
            bool isDateTime = false;
            DateTime parsedTime;
            if (isDateTime = DateTime.TryParse(o.sSearch, out parsedTime))
                filterOnDatePart(o.sSearch, parsedTime, ref q);
            else
            {
                q = q
                    .Join(lRepo.GetAllQ(), wa => wa.skillID, sk => sk.ID, (wa, sk) => new { wa, sk })
                    .Where(p => SqlFunctions.StringConvert((decimal)p.wa.workOrder.paperOrderNum).Contains(o.sSearch) ||
                        p.wa.description.Contains(o.sSearch) ||
                        p.sk.text_EN.Contains(o.sSearch) ||
                        p.sk.text_ES.Contains(o.sSearch) ||
                        p.wa.workOrder.contactName.Contains(o.sSearch) ||
                        p.wa.workOrder.Employer.name.Contains(o.sSearch) ||
                        //p.dateupdated.ToString().ContainsOIC(param.sSearch) ||
                        p.wa.updatedby.Contains(o.sSearch)).Select(p => p.wa);
            }
        }
        public static IEnumerable<WorkAssignment> filterOnSkill(
            viewOptions o, 
            IQueryable<WorkAssignment> q,
            ILookupRepository l,
            Worker worker)
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
            //Worker worker = wcache.FirstOrDefault(w => w.dwccardnum == o.dwccardnum);
            if (worker != null)
            {
                if (worker.skill1 != null) primeskills.Push((int)worker.skill1);
                if (worker.skill2 != null) primeskills.Push((int)worker.skill2);
                if (worker.skill3 != null) primeskills.Push((int)worker.skill3);

                foreach (var skillid in primeskills)
                {
                    skills.Push(skillid);
                    Lookup skill = l.GetById(skillid);
                    foreach (var subskill in l.GetManyQ(a => a.category == skill.category &&
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
                filteredWA = filteredWA.Join(l.GetAllQ(), //LINQ
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
        public static void sortOnColName(string name, bool descending, ref IQueryable<WorkAssignment> q)
        {
            switch (name)
            {
                case "pWAID": q = descending ? q.OrderByDescending(p => p.fullWAID) : q.OrderBy(p => p.fullWAID); break;
                case "skill": q = descending ? q.OrderByDescending(p => p.skillID) : q.OrderBy(p => p.skillID); break;
                case "earnings": q = descending ? q.OrderByDescending(p => p.hourlyWage * p.hours * p.days) : q.OrderBy(p => p.hourlyWage * p.hours * p.days); break;
                case "hourlywage": q = descending ? q.OrderByDescending(p => p.hourlyWage) : q.OrderBy(p => p.hourlyWage); break;
                case "hours": q = descending ? q.OrderByDescending(p => p.hours) : q.OrderBy(p => p.hours); break;
                case "days": q = descending ? q.OrderByDescending(p => p.days) : q.OrderBy(p => p.days); break;
                case "WOID": q = descending ? q.OrderByDescending(p => p.workOrderID) : q.OrderBy(p => p.workOrderID); break;
                case "WAID": q = descending ? q.OrderByDescending(p => p.ID) : q.OrderBy(p => p.ID); break;
                case "description": q = descending ? q.OrderByDescending(p => p.description) : q.OrderBy(p => p.description); break;
                case "updatedby": q = descending ? q.OrderByDescending(p => p.updatedby) : q.OrderBy(p => p.updatedby); break;
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
            if (isDateTime = DateTime.TryParse(o.sSearch, out parsedTime))
            {
                if (isMonthSpecific.IsMatch(o.sSearch))  //Regex for month/year
                    q = q.Where(p => DbFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isDaySpecific.IsMatch(o.sSearch))  //Regex for day/month/year
                    q = q.Where(p => DbFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isTimeSpecific.IsMatch(o.sSearch)) //Regex for day/month/year time
                    q = q.Where(p => DbFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);
            }
            else
            {
                q = q
                    .Where(p => SqlFunctions.StringConvert((decimal)p.ID).Contains(o.sSearch) ||
                                SqlFunctions.StringConvert((decimal)p.paperOrderNum).Contains(o.sSearch) ||
                                p.contactName.Contains(o.sSearch) ||
                                p.workSiteAddress1.Contains(o.sSearch) ||
                                p.updatedby.Contains(o.sSearch));
            }
        }
        public static void filterEmployerByGuid(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            q = q.Where(p => p.Employer.onlineSigninID.Equals((string)o.employerGuid)); //EmployerID for WorkOrderIndex view
        }

        public static void filterEmployer(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            q = q.Where(p => p.EmployerID.Equals((int)o.EmployerID)); //EmployerID for WorkOrderIndex view
        }
        public static void filterStatus(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            q = q.Where(p => p.statusID.Equals((int)o.status));
        }
        public static void filterOnlineSource(viewOptions o, ref IQueryable<WorkOrder> q)
        {
            q = q.Where(p => p.onlineSource == true);
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
                    return query.Where(p => DbFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                    return query.Where(p => DbFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                    return query.Where(p => DbFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);
            }
            return query;
        }
        public static void sortOnColName(string name, bool descending, string language, ref IQueryable<WorkOrder> q)
        {
            switch (name)
            {
                //case "WOID": orderedWO = orderDescending ? q.OrderByDescending(p => p.dateTimeofWork) : q.OrderBy(p => p.dateTimeofWork); break;
                case "status": q = descending ? q.OrderByDescending(p => p.statusID) : q.OrderBy(p => p.statusID); break;
                case "transportMethod":
                    if (language == "en")
                    {
                        q = descending ? q.OrderByDescending(p => p.transportMethodEN) : q.OrderBy(p => p.transportMethodEN);
                        break;
                    }
                    else
                    {
                        q = descending ? q.OrderByDescending(p => p.transportMethodES) : q.OrderBy(p => p.transportMethodES);
                        break;
                    }
                case "WAcount": q = descending ? q.OrderByDescending(p => p.workAssignments.Count) : q.OrderBy(p => p.workAssignments.Count); break;         
                case "contactName": q = descending ? q.OrderByDescending(p => p.contactName) : q.OrderBy(p => p.contactName); break;
                case "workSiteAddress1": q = descending ? q.OrderByDescending(p => p.workSiteAddress1) : q.OrderBy(p => p.workSiteAddress1); break;
                case "updatedby": q = descending ? q.OrderByDescending(p => p.updatedby) : q.OrderBy(p => p.updatedby); break;
                case "onlineSource": q = descending ? q.OrderByDescending(p => p.onlineSource) : q.OrderBy(p => p.onlineSource); break;
                case "emailSentCount": q = descending ? q.OrderByDescending(p => p.Emails.Count()) : q.OrderBy(p => p.Emails.Count()); break;
                case "emailErrorCount": q = descending ? q.OrderByDescending(p => p.Emails.Count()) : q.OrderBy(p => p.Emails.Count()); break;
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
                case "zipcode": q = descending ? q.OrderByDescending(p => p.zipcode) : q.OrderBy(p => p.zipcode); break;
                case "cellphone": q = descending ? q.OrderByDescending(p => p.cellphone) : q.OrderBy(p => p.cellphone); break;
                case "onlineSource": q = descending ? q.OrderByDescending(p => p.onlineSource) : q.OrderBy(p => p.onlineSource); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                default: q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
            }
        }
        public static void search(viewOptions o, ref IQueryable<Employer> q)
        {
            q = q.Where(p => //p.active.ToString().Contains(o.sSearch) ||
                            p.name.Contains(o.sSearch) ||
                            p.address1.Contains(o.sSearch) ||
                            p.phone.Contains(o.sSearch) ||
                            p.cellphone.Contains(o.sSearch) || 
                            p.zipcode.Contains(o.sSearch) ||
                            p.email.Contains(o.sSearch) ||
                            p.city.Contains(o.sSearch));
        }
        public static void filterOnlineSource(viewOptions o, ref IQueryable<Employer> q)
        {
            q = q.Where(p => p.onlineSource == true);
        }
        #endregion
        #region PERSONS
        public static void search(viewOptions o, ref IQueryable<Person> q)
        {
            q = q
                .Where(p => SqlFunctions.StringConvert((decimal)p.Worker.dwccardnum).Contains(o.sSearch) ||
                            p.firstname1.Contains(o.sSearch) ||
                            p.firstname2.Contains(o.sSearch) ||
                            p.lastname1.Contains(o.sSearch) ||
                            p.lastname2.Contains(o.sSearch) ||
                            p.phone.Contains(o.sSearch));
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<Person> q)
        {
            switch (name)
            {
                case "active": q = descending ? q.OrderByDescending(p => p.active) : q.OrderBy(p => p.active); break;
                case "dwccardnum": q = descending ? q.OrderByDescending(p => p.Worker.dwccardnum) : q.OrderBy(p => p.Worker.dwccardnum); break;
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
        public static void getWorkers(viewOptions o, ref IQueryable<Person> q)
        {
            q = q.Where(x => x.Worker.dwccardnum > 0);
        }

        public static void getNotWorkers(viewOptions o, ref IQueryable<Person> q)
        {
            q = q.Where(x => x.Worker == null);
        }

        public static void getExpiredWorkers(viewOptions o, int exp, ref IQueryable<Person> q)
        {
            q = q.Where(x => x.Worker.memberStatusID == exp);
        }

        /// <summary>
        /// Returns a list of sanctioned or expelled workers.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="q"></param>
        public static void getSExWorkers(viewOptions o, int s, int Ex, ref IQueryable<Person> q)
        {
            q = q.Where(x => x.Worker.memberStatusID == s || x.Worker.memberStatusID == Ex);
        }

        public static void search(viewOptions o, ref IQueryable<Worker> q)
        {
            q = q.Where(p => SqlFunctions.StringConvert((decimal)p.dwccardnum).Contains(o.sSearch) ||
                            p.Person.firstname1.Contains(o.sSearch) ||
                            p.Person.firstname2.Contains(o.sSearch) ||
                            p.Person.lastname1.Contains(o.sSearch) ||
                            p.Person.lastname2.Contains(o.sSearch) //||
                //DbFunctions.p.memberexpirationdate.ToString().Contains(o.sSearch)
                            );
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<Worker> q)
        {
            switch (name)
            {
                case "dwccardnum": q = descending ? q.OrderByDescending(p => p.dwccardnum) : q.OrderBy(p => p.dwccardnum); break;
                case "memberStatus": q = descending ? q.OrderByDescending(p => p.memberStatusID) : q.OrderBy(p => p.memberStatusID); break;
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
        public static void unauthenticatedView(DateTime date, ref IQueryable<Activity> q)
        {
            // Shows classes within 30min of start and up to 30min after end
            q = q.Where(p => DbFunctions.DiffMinutes(date, p.dateStart) <= 30 &&
                             DbFunctions.DiffMinutes(date, p.dateEnd) >= -30);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="q"></param>
        /// <param name="lRepo"></param>
        public static void search(viewOptions o, ref IQueryable<Activity> q)
        {
            switch(o.CI.TwoLetterISOLanguageName.ToUpperInvariant())
            {
                case "ES":
                    q = q.Where(p => p.notes.Contains(o.sSearch) ||
                        p.teacher.Contains(o.sSearch) ||
                        p.dateStart.ToString().Contains(o.sSearch) ||
                        p.nameES.ToString().Contains(o.sSearch) ||
                        p.typeES.ToString().Contains(o.sSearch) ||
                        p.dateEnd.ToString().Contains(o.sSearch));
                    break;
                case "EN":
                default:
                    q = q.Where(p => p.notes.Contains(o.sSearch) ||
                        p.teacher.Contains(o.sSearch) ||
                        p.dateStart.ToString().Contains(o.sSearch) ||
                        p.nameEN.ToString().Contains(o.sSearch) ||
                        p.typeEN.ToString().Contains(o.sSearch) ||
                        p.dateEnd.ToString().Contains(o.sSearch));
                        break;
            }

        }
        public static void search(viewOptions o, ref IQueryable<ActivitySignin> q)
        {
            q = q.Where(asi => SqlFunctions.StringConvert((decimal)asi.dwccardnum).Contains(o.sSearch) ||
                            asi.person.firstname1.Contains(o.sSearch) ||
                            asi.person.firstname2.Contains(o.sSearch) ||
                            asi.person.lastname1.Contains(o.sSearch) ||
                            asi.person.lastname2.Contains(o.sSearch) 
                            );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="descending"></param>
        /// <param name="isoLandCode"></param>
        /// <param name="q"></param>
        public static void sortOnColName(string name, bool descending, ref IQueryable<Activity> q)
        {
            switch (name)
            {
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
        public static void sortOnColName(string name, bool descending, ref IQueryable<ActivitySignin> q)
        {
            switch (name)
            {
                case "dwccardnum": q = descending ? q.OrderByDescending(p => p.dwccardnum) : q.OrderBy(p => p.dwccardnum); break;
                case "firstname1": q = descending ? q.OrderByDescending(p => p.person.firstname1) : q.OrderBy(p => p.person.firstname1); break;
                case "firstname2": q = descending ? q.OrderByDescending(p => p.person.firstname2) : q.OrderBy(p => p.person.firstname2); break;
                case "lastname1": q = descending ? q.OrderByDescending(p => p.person.lastname1) : q.OrderBy(p => p.person.lastname1); break;
                case "lastname2": q = descending ? q.OrderByDescending(p => p.person.lastname2) : q.OrderBy(p => p.person.lastname2); break;
                case "dateupdated": q = descending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                case "dateforsigninstring": q = descending ? q.OrderByDescending(p => p.dateforsignin) : q.OrderBy(p => p.dateforsignin); break;
                case "expirationDate": q = descending ? q.OrderByDescending(p => p.person.Worker.memberexpirationdate) : q.OrderBy(p => p.person.Worker.memberexpirationdate); break;
                default: q = descending ? q.OrderByDescending(p => p.dateforsignin) : q.OrderBy(p => p.dateforsignin); break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="q"></param>
        /// <param name="asRepo"></param>
        public static void getUnassociated(int personID, ref IQueryable<Activity> q, IRepository<Activity> arepo, IActivitySigninRepository asRepo)
        {
            //
            //SELECT extent1.* FROM  [dbo].[Activities] AS [Extent1]
            //LEFT OUTER JOIN [dbo].[ActivitySignins] AS [Extent2] ON 
            //        ([Extent1].[ID] = [Extent2].[activityID]) AND 
            //        ([Extent2].[WorkerID] = <personID> )
            //WHERE [Extent2].[activityID] IS NULL
            q = from b in arepo.GetAllQ()
                join aa in
                    // joins activities (a) to activity signins (az) 
                    // where az.personID
                    (from a in q
                     join az in asRepo.GetAllQ() on a.ID equals az.activityID into grouped
                     from az2 in grouped.DefaultIfEmpty()
                     where az2.personID == personID
                     select a)
                 on b.ID equals aa.ID into h
                from i in h.DefaultIfEmpty()
                where i == null
                select b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="q"></param>
        /// <param name="asRepo"></param>
        public static void getAssociated(int personID, ref IQueryable<Activity> q, IActivitySigninRepository asRepo)
        {
            q = from a in q
                join az in asRepo.GetAllQ() on a.ID equals az.activityID into g
                from f in g.DefaultIfEmpty()
                where f.personID == personID
                select a;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="q"></param>
        public static void GetAssociated(int personID, ref IQueryable<ActivitySignin> q)
        {
            q = q.Where(az => az.personID == personID);
        }
        #endregion
        #region LOOKUPS
        public static void byCategory(viewOptions o, ref IQueryable<Lookup> q)
        {
            q = q.Where(p => p.category == o.category);
        }
        public static void search(viewOptions o, ref IQueryable<Lookup> q)
        {
            q = q
                .Where(p => p.text_EN.Contains(o.sSearch) ||
                            p.text_ES.Contains(o.sSearch) ||
                            p.category.Contains(o.sSearch) ||
                            p.subcategory.Contains(o.sSearch));
        }
        public static void sortOnColName(string name, bool descending, ref IQueryable<Lookup> q)
        {
            switch (name)
            {
                case "category": q = descending ? q.OrderByDescending(p => p.category) : q.OrderBy(p => p.category); break;
                case "text_EN": q = descending ? q.OrderByDescending(p => p.text_EN) : q.OrderBy(p => p.text_EN); break;
                case "text_ES": q = descending ? q.OrderByDescending(p => p.text_ES) : q.OrderBy(p => p.text_ES); break;
                case "selected": q = descending ? q.OrderByDescending(p => p.selected) : q.OrderBy(p => p.selected); break;
                case "subcategory": q = descending ? q.OrderByDescending(p => p.subcategory) : q.OrderBy(p => p.subcategory); break;
                default: q = descending ? q.OrderByDescending(p => p.text_EN) : q.OrderBy(p => p.text_EN); break;
            }
        }
        #endregion

        #region EMAILS
        public static void filterOnWorkorder(viewOptions o, ref IQueryable<Email> q)
        {
            if (o.woid == 0) return;
            q = q.SelectMany(email => email.WorkOrders.Where(w => w.ID == o.woid), (email, wo) => email);
        }

        public static void filterOnEmployer(viewOptions o, ref IQueryable<Email> q)
        {
            if (o.EmployerID == null) return;
            q = q.Where(ee => ee.ID == (int)o.EmployerID)
                 .SelectMany(e => e.WorkOrders, (e, wo) => wo)
                 .SelectMany(wwo => wwo.Emails, (wwo, email) => email);
        }

        public static void search(viewOptions o, ref IQueryable<Email> q)
        {
            q = q.Where(p => //p.active.ToString().Contains(o.sSearch) ||
                            p.subject.Contains(o.sSearch) ||
                            p.emailTo.Contains(o.sSearch) ||
                            p.updatedby.Contains(o.sSearch));
        }

        public static void filterOnID(viewOptions o, ref IQueryable<Email> q)
        {
            q = q.Where(e => e.ID == o.emailID);
        }

        public static void sortOnColName(string name, bool descending, ref IEnumerable<Email> e)
        {
            switch (name)
            {
                case "recordid": e = descending ? e.OrderByDescending(p => p.ID) : e.OrderBy(p => p.ID);break;
                case "emailTo": e = descending ? e.OrderByDescending(p => p.emailTo) : e.OrderBy(p => p.emailTo); break;
                case "subject": e = descending ? e.OrderByDescending(p => p.subject) : e.OrderBy(p => p.subject); break;
                case "status": e = descending ? e.OrderByDescending(p => p.statusID) : e.OrderBy(p => p.statusID); break;
                case "transmitAttempts": e = descending ? e.OrderByDescending(p => p.transmitAttempts) : e.OrderBy(p => p.transmitAttempts); break;
                case "lastAttempt": e = descending ? e.OrderByDescending(p => p.lastAttempt) : e.OrderBy(p => p.lastAttempt); break;
                case "dateupdated": e = descending ? e.OrderByDescending(p => p.dateupdated) : e.OrderBy(p => p.dateupdated); break;
                //case "": q = descending ? q.OrderByDescending(p => p) : q.OrderBy(p => p);break;
                default: e = descending ? e.OrderByDescending(p => p.dateupdated) : e.OrderBy(p => p.dateupdated); break;
            }
        }
        #endregion

    }
}
