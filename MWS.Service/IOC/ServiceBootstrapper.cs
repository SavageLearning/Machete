using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Microsoft.Practices.Unity;
using MWS.Core;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MWS.Service
{
    public class ServiceBootstrapper
    {
        public IUnityContainer Build()
        {
            IUnityContainer c = new UnityContainer();
            c.RegisterType<IEmailManager, EmailManager>();
            c.RegisterInstance<MacheteWindowsService>(new MacheteWindowsService(c));
            c.RegisterType<IEmailRepository, EmailRepository>();
            c.RegisterType<IEmailService, EmailService>();
            c.RegisterType<IWorkOrderRepository, WorkOrderRepository>();
            c.RegisterType<IWorkOrderService, WorkOrderService>();
            c.RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>();
            c.RegisterType<IWorkAssignmentService, WorkAssignmentService>();
            c.RegisterType<IWorkerRepository, WorkerRepository>();
            c.RegisterType<IWorkerService, WorkerService>();
            c.RegisterType<IWorkerSigninRepository, WorkerSigninRepository>();
            c.RegisterType<IWorkerSigninService, WorkerSigninService>();
            c.RegisterType<ILookupRepository, LookupRepository>();
            c.RegisterType<ILookupService, LookupService>();
            c.RegisterType<IUnitOfWork, UnitOfWork>();
            c.RegisterInstance<IDatabaseFactory>(new DatabaseFactory());
            // LookupCache will populate static values in the domain for service lookups
            c.RegisterInstance<ILookupCache>(new LookupCache(c.Resolve<Func<IDatabaseFactory>>()));
            return c;
        }
    }
}
