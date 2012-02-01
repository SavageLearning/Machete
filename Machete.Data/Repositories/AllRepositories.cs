using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;
using System.Data.Entity;

namespace Machete.Data
{
    // : RepositoryBase<Person> -- [ class-base ]
    //              the direct base class of the class being defined
    //                  if there is base class, then object is the base class
    // , IPersonRepository -- [ interface-type ] 
    //              defined a (weak) contract 
    //                  - which methods are available
    //                  - what their names are
    //                  - what types they take
    //                  - what types they return
    public interface IWorkerSigninRepository : IRepository<WorkerSignin> { }
    public interface IEmployerRepository : IRepository<Employer> { }
    public interface IWorkOrderRepository : IRepository<WorkOrder> { }
    public interface IWorkerRequestRepository : IRepository<WorkerRequest> { }
    public interface IWorkerRepository : IRepository<Worker> { }
    public interface IWorkAssignmentRepository : IRepository<WorkAssignment> { }
    public interface IPersonRepository : IRepository<Person> { }
    public interface IEventRepository : IRepository<Event> { }
    public interface IImageRepository : IRepository<Image> { }
    public interface ILookupRepository : IRepository<Lookup> { }
    /// <summary>
    /// 
    /// </summary>
    public class WorkerSigninRepository : RepositoryBase<WorkerSignin>, IWorkerSigninRepository
    {
        private readonly IDbSet<WorkerSignin> dbset;
        public WorkerSigninRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<WorkerSignin>();
        }
        override public IQueryable<WorkerSignin> GetAllQ()
        {
            //return dbset.Include(a => a.worker).AsQueryable();
            return dbset.AsNoTracking().AsQueryable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EmployerRepository : RepositoryBase<Employer>, IEmployerRepository
    {
        public EmployerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderRepository : RepositoryBase<WorkOrder>, IWorkOrderRepository
    {
        private readonly IDbSet<WorkOrder> dbset;
        public WorkOrderRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            dbset = base.DataContext.Set<WorkOrder>();
        }
        override public IQueryable<WorkOrder> GetAllQ()
        {
            return dbset.Include(a => a.workAssignments).Include(a => a.workerRequests).AsNoTracking().AsQueryable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkerRequestRepository : RepositoryBase<WorkerRequest>, IWorkerRequestRepository
    {
        public WorkerRequestRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
    {
        private readonly IDbSet<Worker> dbset;
        //private MacheteContext dataContext;
        public WorkerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<Worker>();
        }
        override public IQueryable<Worker> GetAllQ()
        {
            return dbset.Include(a => a.Person).AsNoTracking().AsQueryable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkAssignmentRepository : RepositoryBase<WorkAssignment>, IWorkAssignmentRepository
    {
        private readonly IDbSet<WorkAssignment> dbset;
        public WorkAssignmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = base.DataContext.Set<WorkAssignment>();
        }

        override public IQueryable<WorkAssignment> GetAllQ()
        {
            return dbset.Include(a => a.workOrder).AsNoTracking().AsQueryable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LookupRepository : RepositoryBase<Lookup>, ILookupRepository
    {
        public LookupRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
}

