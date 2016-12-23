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
using System.Linq;

namespace Machete.Service
{
    public interface IWorkerService : IService<Worker>
    {
        Worker GetWorkerByNum(int dwccardnum);
        int GetNextWorkerNum();
        dataTableResult<DTO.WorkerList> GetIndexView(viewOptions o);
        // IQueryable<Worker> GetPriorEmployees(int employerId);
    }
    public class WorkerService : ServiceBase<Worker>, IWorkerService
    {
        private IWorkerCache wcache;
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkOrderRepository woRepo;
        private readonly IPersonRepository pRepo;
        private readonly IMapper map;

        public WorkerService(IWorkerRepository wRepo, 
            IWorkerCache wc, 
            IUnitOfWork uow, 
            IWorkAssignmentRepository waRepo, 
            IWorkOrderRepository woRepo, 
            IPersonRepository pRepo,
            IMapper map)
            : base(wRepo, uow)
        {
            this.wcache = wc;
            this.logPrefix = "Worker";
            this.waRepo = waRepo;
            this.woRepo = woRepo;
            this.pRepo = pRepo;
            this.map = map;
        }

        public Worker GetWorkerByNum(int dwccardnum)
        {
            Worker worker = repo.Get(w => w.dwccardnum == dwccardnum);
            return worker;
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

        /*
        public IQueryable<Worker> GetPriorEmployees(int employerId)
        {
//            IRepository<WorkAssignment> wa = 
//            repo.Get(az => az.activityID == actID && az.personID == perID);
//            IQueryable<Worker> all = repo.GetAllQ();
            //IQueryable<Worker> all = repo.Get(w => w.ID);
            return all;
        }
         * */

        public override Worker Create(Worker record, string user)
        {
            var result = base.Create(record, user);
            wcache.Refresh();
            return result;
        }

        public override void Save(Worker record, string user)
        {
            base.Save(record, user);
            wcache.Refresh();
        }

        public override void Delete(int id, string user)
        {
            base.Delete(id, user);
            wcache.Refresh();
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
    }
}