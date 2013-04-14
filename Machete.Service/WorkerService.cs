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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;

namespace Machete.Service
{
    public interface IWorkerService : IService<Worker>
    {
        Worker GetWorkerByNum(int dwccardnum);
        void RefreshCache();
        dataTableResult<Worker> GetIndexView(viewOptions o);
    }
    public class WorkerService : ServiceBase<Worker>, IWorkerService
    {
        public WorkerService(IWorkerRepository wRepo, IUnitOfWork uow) : base(wRepo, uow)
        {
            this.logPrefix = "Worker";
        }
        public Worker GetWorkerByNum(int dwccardnum)
        {
            Worker worker = repo.Get(w => w.dwccardnum == dwccardnum);
            return worker;
        }

        public override Worker Create(Worker record, string user)
        {
            var result = base.Create(record, user);
            RefreshCache();
            return result;
        }

        public void RefreshCache()
        {
            ((IWorkerRepository)repo).RefreshCache();
        }

        public dataTableResult<Worker> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Worker>();
            //Get all the records
            IQueryable<Worker> q = repo.GetAllQ();
            result.totalCount = q.Count();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            //ORDER BY based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //Limit results to the display length and offset
            result.filteredCount = q.Count();
            result.query = q.Skip(o.displayStart).Take(o.displayLength);
            return result;
        }
    }
}