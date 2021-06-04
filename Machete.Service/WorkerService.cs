#region COPYRIGHT
// File:     WorkerService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Service.Infrastructure;
using Machete.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface IWorkerService : IService<Worker>
    {
        Worker GetByMemberID(int dwccardnum);
        bool MemberExists(int dwccardnum);
        bool MemberExists(int dwccardnum, int excludeId);
        int GetNextWorkerNum();
        dataTableResult<DTO.WorkerList> GetIndexView(viewOptions o);
        // IQueryable<Worker> GetPriorEmployees(int employerId);
        bool ExpireMembers();
        bool ReactivateMembers();
    }
    public class WorkerService : ServiceBase2<Worker>, IWorkerService
    {
        private readonly DbSet<Lookup> lSet;
        private readonly DbSet<Person> pSet;

        public WorkerService(IDatabaseFactory dbf, IMapper map) : base(dbf, map)
        {
            this.logPrefix = "Worker";
            lSet = this.db.Set<Lookup>();
            pSet = this.db.Set<Person>();
        }

        public new IQueryable<Worker> GetAll()
        {
            return dbset.Include(a => a.Person).AsNoTracking().AsQueryable();
        }
        public Worker GetByMemberID(int dwccardnum)
        {
                        var q = from o in dbset.AsQueryable()
                    where o.dwccardnum.Equals(dwccardnum)
                    select o;
            return q.FirstOrDefault();
        }
        
        /// <summary>
        /// Finds if membernumber exists in db.
        /// </summary>
        /// <param name="dwccardnum">the number to check for a duplicate</param>
        /// <returns>bool</returns>
        public bool MemberExists(int dwccardnum)
        {
            return !(this.GetByMemberID(dwccardnum) is null);
        }
        
        /// <summary>
        /// Finds if membernumber exists in db, excludes excludedId from search
        /// </summary>
        /// <param name="dwccardnum">the number to check for a duplicate</param>
        /// <param name="excludeId">the worker record to exclude from duplicate search.</param>
        /// <returns>bool</returns>
        public bool MemberExists(int dwccardnum, int excludeId)
        {
            var workerToExclude= Get(excludeId);
            var searchCriteriaMet = dwccardnum != workerToExclude.dwccardnum;
            return searchCriteriaMet && MemberExists(dwccardnum);
        }

        public int GetNextWorkerNum()
        {
            var all = GetAll().Select(x => x.dwccardnum);
            var asc = all.OrderBy(x => x).FirstOrDefault();
            if (asc == 0)
            {
                return 10000;
            }
            var desc = all.OrderByDescending(x => x).FirstOrDefault();
            if (desc < 99999)
            {
                return desc + 1;
            }
            else if (desc == 99999 && asc > 10000)
            {
                return asc - 1;
            }
            else
            {
                throw new ArgumentOutOfRangeException("The minimum and maximum card numbers are already taken. Reorganize your members' card numbers to automatically generate new numbers.");
            }
        }

        public override Worker Create(Worker record, string user)
        {
            record.Person = pSet.Find(record.ID);
            updateComputedFields(ref record);
            var result = base.Create(record, user);
            return result;
        }

        public override void Save(Worker record, string user)
        {
            updateComputedFields(ref record);
            base.Save(record, user);
        }

        private void updateComputedFields(ref Worker record)
        {
            record.memberStatusEN = lSet.Find(record.memberStatusID).text_EN;
            record.memberStatusES = lSet.Find(record.memberStatusID).text_ES;
            record.typeOfWork = lSet.Find(record.typeOfWorkID).ltrCode;
            record.skillCodes = getSkillCodes(record);
            record.fullNameAndID = record.dwccardnum + " " + record.Person.fullName;
        }

        public string getSkillCodes(Worker w)
        {
            string rtnstr = "E" + w.englishlevelID + " ";
            if (w.skill1 != null)
            {
                var lookup = lSet.Find((int)w.skill1);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
            }
            if (w.skill2 != null)
            {
                var lookup = lSet.Find((int)w.skill2);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
            }
            if (w.skill3 != null)
            {
                var lookup = lSet.Find((int)w.skill3);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level;
            }
            return rtnstr;
        }

        public override void Delete(int id, string user)
        {
            base.Delete(id, user);
        }
        public dataTableResult<DTO.WorkerList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.WorkerList>();
            //Get all the records
            IQueryable<Worker> q = GetAll();
            result.totalCount = q.Count();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            //ORDER BY based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //Limit results to the display length and offset
            result.filteredCount = q.Count();
            result.query = q.ProjectTo<DTO.WorkerList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
            return result;
        }

        /// <summary>
        /// Expires active workers based on expiration date
        /// </summary>
        /// <param name="db"></param>
        /// <returns>true if at least one record expired</returns>
        public bool ExpireMembers()
        {
            bool rtn = false;
            IList<Worker> list = GetMany(w =>
                    w.memberexpirationdate < DateTime.Now &&
                    w.memberStatusID == Worker.iActive)
                    .ToList();
            //
            if (list.Count() > 0) rtn = true;
            //
            foreach (Worker wkr in list)
            {
                wkr.memberStatusID = Worker.iExpired;
                Save(wkr, "ExpirationBot");
            }

            return rtn;
        }
        /// <summary>
        /// Reactivates sanctioned workers based on reactivation date
        /// </summary>
        /// <param name="db"></param>
        /// <returns>true if at least one record reactivated</returns>
        public bool ReactivateMembers()
        {
            bool rtn = false;
            IList<Worker> list = GetMany(w =>
                    w.memberReactivateDate != null &&
                    w.memberReactivateDate < DateTime.Now &&
                    w.memberStatusID == Worker.iSanctioned)
                    .ToList();
            //
            if (list.Count() > 0) rtn = true;
            //
            foreach (Worker wkr in list)
            {
                wkr.memberStatusID = Worker.iActive;
                Save(wkr, "ReactivationBot");
            }

            return rtn;
        }
    }
}