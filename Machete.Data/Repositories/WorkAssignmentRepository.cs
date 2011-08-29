using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;
using System.Data.Entity;

namespace Machete.Data
{
    public class WorkAssignmentRepository : RepositoryBase<WorkAssignment>, IWorkAssignmentRepository
    {
        private readonly IDbSet<WorkAssignment> dbset;
        public WorkAssignmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<WorkAssignment>();
        }
        //protected MacheteContext DataContext
        //{
        //    get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        //}
        override public IQueryable<WorkAssignment> GetAllQ()
        {
            return dbset.Include(a => a.workOrder).AsNoTracking().AsQueryable();
        }
    }
    public interface IWorkAssignmentRepository : IRepository<WorkAssignment>
    {
    }
}
