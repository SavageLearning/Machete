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