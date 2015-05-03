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
    public interface IWorkerRequestService : IService<WorkerRequest>
    {
        WorkerRequest GetWorkerRequestsByNum(int woid, int wrid);
    }
    public class WorkerRequestService : ServiceBase<WorkerRequest>, IWorkerRequestService
    {
        //
        public WorkerRequestService(IWorkerRequestRepository wrRepo, IUnitOfWork uow) : base(wrRepo, uow)
        {
            this.logPrefix = "WorkerRequest";
        }

        public WorkerRequest GetWorkerRequestsByNum(int woid, int wkrid)
        {
            return repo.Get(wr => wr.WorkOrderID == woid && wr.WorkerID == wkrid);
        }

    }
}