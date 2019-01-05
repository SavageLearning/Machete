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
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface IWorkerService : IService<Worker>
    {
        Worker GetByMemberID(int dwccardnum);
        int GetNextWorkerNum();
        dataTableResult<DTO.WorkerList> GetIndexView(viewOptions o);
        // IQueryable<Worker> GetPriorEmployees(int employerId);
        bool ExpireMembers();
        bool ReactivateMembers();
    }
    public class WorkerService : ServiceBase<Worker>, IWorkerService
    {
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkOrderRepository woRepo;
        private readonly IPersonRepository pRepo;
        private readonly IMapper map;
        private readonly ILookupRepository lRepo;
        private readonly IWorkerRepository wRepo;

        public WorkerService(IWorkerRepository wRepo, 
            ILookupRepository lRepo,
            IUnitOfWork uow, 
            IWorkAssignmentRepository waRepo, 
            IWorkOrderRepository woRepo, 
            IPersonRepository pRepo,
            IMapper map)
            : base(wRepo, uow)
        {
            this.logPrefix = "Worker";
            this.waRepo = waRepo;
            this.woRepo = woRepo;
            this.pRepo = pRepo;
            this.map = map;
            this.lRepo = lRepo;
            this.wRepo = wRepo;
        }

        public Worker GetByMemberID(int dwccardnum)
        {
            return wRepo.GetByMemberID(dwccardnum);
        }

        public int GetNextWorkerNum()
        {
            var all = repo.GetAllQ().Select(x => x.dwccardnum);
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
            record.Person = pRepo.GetById(record.ID);
            updateComputedFields(ref record);
            var result = base.Create(record, user);
            uow.Commit();
            return result;
        }

        public override void Save(Worker record, string user)
        {
            updateComputedFields(ref record);
            base.Save(record, user);
        }

        private void updateComputedFields(ref Worker record)
        {
            record.memberStatusEN = lRepo.GetById(record.memberStatusID).text_EN;
            record.memberStatusES = lRepo.GetById(record.memberStatusID).text_ES;
            record.typeOfWork = lRepo.GetById(record.typeOfWorkID).ltrCode;
            record.skillCodes = getSkillCodes(record);
            record.fullNameAndID = record.dwccardnum + " " + record.Person.fullName;
        }

        public string getSkillCodes(Worker w)
        {
            string rtnstr = "E" + w.englishlevelID + " ";
            if (w.skill1 != null)
            {
                var lookup = lRepo.GetById((int)w.skill1);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
            }
            if (w.skill2 != null)
            {
                var lookup = lRepo.GetById((int)w.skill2);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
            }
            if (w.skill3 != null)
            {
                var lookup = lRepo.GetById((int)w.skill3);
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
            IQueryable<Worker> q = repo.GetAllQ();
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