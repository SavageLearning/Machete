using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;
using System.Data.Entity;

namespace Machete.Data
{
    public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
    {
        private readonly IDbSet<Worker> dbset;
        private MacheteContext dataContext;
        public WorkerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<Worker>();
        }
        override public IQueryable<Worker> GetAllQ()
        {
            return dbset.Include(a => a.Person).AsQueryable();
        }
    }
    public interface IWorkerRepository : IRepository<Worker>
    {
    }
}

