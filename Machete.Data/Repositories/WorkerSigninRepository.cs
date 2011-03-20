using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
    public class WorkerSigninRepository: RepositoryBase<WorkerSignin>, IWorkerSigninRepository
    {
        public WorkerSigninRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IWorkerSigninRepository : IRepository<WorkerSignin>
    {
    }
}
