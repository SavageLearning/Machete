#region COPYRIGHT
// File:     WorkerRequestService.cs
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

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Service;
using Machete.Service.Infrastructure;
using Machete.Domain;
using Microsoft.EntityFrameworkCore;

namespace Machete.Service
{
    public interface IWorkerRequestService : IService<WorkerRequest>
    {
        WorkerRequest GetByID(int workOrderID, int workerID);
        List<WorkerRequest> GetAllByWorkOrderID(int workOrderID);
    }
    public class WorkerRequestService : ServiceBase2<WorkerRequest>, IWorkerRequestService
    {
        public WorkerRequestService(IDatabaseFactory db, IMapper map) : base(db, map)
        {
        }

        public WorkerRequest GetByID(int workOrderID, int workerID)
        {
            return dbset.Find(workOrderID, workerID);
        }

        public List<WorkerRequest> GetAllByWorkOrderID(int workOrderID)
        {
            return dbset.AsNoTracking().Where(workerRequests => workerRequests.WorkOrderID == workOrderID).ToList();
        }

        public new IQueryable<WorkerRequest> GetAll()
        {
            return dbset.Include(a => a.workerRequested).AsNoTracking().AsQueryable();
        }
    }
}
