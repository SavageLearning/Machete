using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;
using System.Data.Entity;

namespace Machete.Data
{
    public class WorkerSigninRepository: RepositoryBase<WorkerSignin>, IWorkerSigninRepository
    {
        private readonly IDbSet<WorkerSignin> dbset;
        public WorkerSigninRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<WorkerSignin>();
        }
        override public IQueryable<WorkerSignin> GetAllQ()
        {
            return dbset.Include(a => a.worker).AsQueryable();
        }
    }
    public interface IWorkerSigninRepository : IRepository<WorkerSignin>
    {
    }
}
