using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
    public class WorkOrderRepository : RepositoryBase<WorkOrder>, IWorkOrderRepository
    {
        private readonly IDbSet<WorkOrder> dbset;
        private MacheteContext dataContext;
        public WorkOrderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<WorkOrder>();
        }
        protected MacheteContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
        public virtual IQueryable<WorkOrder> GetAllQ()
        {
            return dbset.Include(a => a.workAssignments).AsQueryable();
        }
    }
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
    }
}
