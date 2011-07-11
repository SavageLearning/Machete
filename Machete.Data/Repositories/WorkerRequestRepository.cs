using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
    public class WorkerRequestRepository : RepositoryBase<WorkerRequest>, IWorkerRequestRepository
    {
        public WorkerRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IWorkerRequestRepository : IRepository<WorkerRequest>
    {
    }
}