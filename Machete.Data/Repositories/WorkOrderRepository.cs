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
        //override protected MacheteContext DataContext
        //{
        //    override get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        //}
        override public IQueryable<WorkOrder> GetAllQ()
        {
            return dbset.Include(a => a.workAssignments).Include(a => a.workerRequests).AsNoTracking().AsQueryable();
        }
    }
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
    }
}
