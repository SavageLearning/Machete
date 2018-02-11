#region COPYRIGHT
// File:     AllRepositories.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
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
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Machete.Data
{
    //
    // : RepositoryBase<Person> -- [ class-base ]
    //              the direct base class of the class being defined
    //                  if there is base class, then object is the base class
    // , IRepository<T> -- [ interface-type ] 
    //              defined a (weak) contract 
    //                  - which methods are available
    //                  - what their names are
    //                  - what types they take
    //                  - what types they return
    //
    /////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Interfaces referenced in Service layer 
    //
    public interface IWorkerSigninRepository : IRepository<WorkerSignin> { }
    public interface IEmployerRepository : IRepository<Employer>
    {
        Employer GetBySubject(string subject);
    }
    public interface IEmailRepository : IRepository<Email> 
    {
        IEnumerable<Email> GetEmailsToSend();
    }
    public interface IWorkOrderRepository : IRepository<WorkOrder> { }
    public interface IWorkerRequestRepository : IRepository<WorkerRequest> {
        WorkerRequest GetByWorkerID(int woid, int workerID);
    }
    public interface IWorkerRepository : IRepository<Worker>
    {
        Worker GetByMemberID(int memberID);
    }
    public interface IWorkAssignmentRepository : IRepository<WorkAssignment> { }
    public interface IPersonRepository : IRepository<Person> { }
    public interface IEventRepository : IRepository<Event> { }
    public interface IScheduleRuleRepository : IRepository<ScheduleRule> { }
    public interface ITransportRuleRepository : IRepository<TransportRule> { }
    public interface ITransportCostRuleRepository : IRepository<TransportCostRule> { }
    public interface IImageRepository : IRepository<Image> { }
    public interface ILookupRepository : IRepository<Lookup> 
    {
        void clearSelected(string category);
        Lookup GetByKey(string category, string key);
    }
    public interface IActivityRepository : IRepository<Activity> { }
    public interface IActivitySigninRepository : IRepository<ActivitySignin>
    {
        ActivitySignin GetByPersonID(int actID, int personID);
    }
    public class WorkerSigninRepository : RepositoryBase<WorkerSignin>, IWorkerSigninRepository
    {
        public WorkerSigninRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
        override public IQueryable<WorkerSignin> GetAllQ()
        {
            return dbset.Include(a => a.worker).AsNoTracking().AsQueryable();
        }
    }
    public class ActivitySigninRepository : RepositoryBase<ActivitySignin>, IActivitySigninRepository
    {
        public ActivitySigninRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) { }
        override public IQueryable<ActivitySignin> GetAllQ()
        {
            //return dbset.Include(a => a.worker).AsQueryable();
            return dbset.Include(a => a.Activity).AsNoTracking().AsQueryable();
        }
        public ActivitySignin GetByPersonID(int actID, int personID)
        {
            var q = from o in dbset.AsQueryable()
                    where (o.activityID.Equals(actID) && o.personID.Equals(personID))
                    select o;
            return q.FirstOrDefault();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }

        override public IQueryable<Activity> GetAllQ()
        {
            return dbset.AsNoTracking().AsQueryable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EmployerRepository : RepositoryBase<Employer>, IEmployerRepository
    {
        public EmployerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }

        public Employer GetBySubject(string subject)
        {
            var q = from e in dbset.AsQueryable()
                    where e.onlineSigninID.Equals(subject)
                    select e;
            return q.SingleOrDefault();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EmailRepository : RepositoryBase<Email>, IEmailRepository
    {
        public EmailRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
        public IEnumerable<Email> GetEmailsToSend()
        {
            //var emails = dbset.Where(e => e.statusID == Email.iReadyToSend ||
            //               (e.statusID == Email.iTransmitError && e.transmitAttempts < 10)
            //               );
            var sb = new StringBuilder();
            sb.AppendFormat("select * from Emails e  with (UPDLOCK) where e.statusID = {0} or ", Email.iReadyToSend);
            sb.AppendFormat("(e.statusID = {0} and e.transmitAttempts < {1})", Email.iTransmitError, Email.iTransmitAttempts);
            var set = (DbSet<Email>)dbset;
            return set.SqlQuery(sb.ToString()).AsEnumerable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderRepository : RepositoryBase<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
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

        override public IQueryable<WorkerRequest> GetAllQ()
        {
            return dbset.Include(a => a.workerRequested).AsNoTracking().AsQueryable();
        }

        public WorkerRequest GetByWorkerID(int woid, int workerID)
        {
            var q = from o in dbset.AsQueryable()
                    where (o.WorkOrderID.Equals(woid) && o.WorkerID.Equals(workerID))
                    select o;
            return q.FirstOrDefault();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
    {
        public WorkerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)  { }

        override public IQueryable<Worker> GetAllQ()
        {
            return dbset.Include(a => a.Person).AsNoTracking().AsQueryable();
        }

        public Worker GetByMemberID(int memberID)
        {
            var q = from o in dbset.AsQueryable()
                    where o.dwccardnum.Equals(memberID)
                    select o;
            return q.FirstOrDefault();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkAssignmentRepository : RepositoryBase<WorkAssignment>, IWorkAssignmentRepository
    {
        public WorkAssignmentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }

        override public IQueryable<WorkAssignment> GetAllQ()
        {
            return dbset.Include(a => a.workOrder).Include(b => b.workOrder.Employer).Include(b => b.workerAssigned).AsNoTracking().AsQueryable();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }

        override public IQueryable<Person> GetAllQ()
        {
            return dbset.Include(a => a.Worker).AsNoTracking().AsQueryable();
        }
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
        public void clearSelected(string category) 
        {
            IEnumerable<Lookup> list  = dbset.Where(w => w.category == category).AsEnumerable();
            foreach (var l in list)
            {
                l.selected = false;
            }            
        }

        public Lookup GetByKey(string category, string key)
        {
            var q = from o in dbset.AsQueryable()
                    where (o.category.Equals(category) && o.key.Equals(key))
                    select o;
            return q.FirstOrDefault();
        }
    }

    public class ScheduleRuleRepository : RepositoryBase<ScheduleRule>, IScheduleRuleRepository
    {
        public ScheduleRuleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public class TransportRuleRepository : RepositoryBase<TransportRule>, ITransportRuleRepository
    {
        public TransportRuleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
        public override IQueryable<TransportRule> GetAllQ()
        {
            return dbset.Include(a => a.costRules).AsNoTracking();
        }
    }

    public class TransportCostRuleRepository : RepositoryBase<TransportCostRule>, ITransportCostRuleRepository
    {
        public TransportCostRuleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }

    }
}

