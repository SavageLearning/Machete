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
        public IUnityContainer container { get; set; }

        public ServiceBootstrapper()
        {
            container = new UnityContainer();
        }

        public IUnityContainer Build()
        {
            container.RegisterType<IEmailManager, EmailManager>();
            container.RegisterInstance<MacheteWindowsService>(new MacheteWindowsService(container));
            container.RegisterType<IEmailRepository, EmailRepository>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IWorkOrderRepository, WorkOrderRepository>();
            container.RegisterType<IWorkOrderService, WorkOrderService>();
            container.RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>();
            container.RegisterType<IWorkAssignmentService, WorkAssignmentService>();
            container.RegisterType<IWorkerRepository, WorkerRepository>();
            container.RegisterType<IWorkerService, WorkerService>();
            container.RegisterType<IWorkerSigninRepository, WorkerSigninRepository>();
            container.RegisterType<IWorkerSigninService, WorkerSigninService>();
            container.RegisterType<ILookupRepository, LookupRepository>();
            container.RegisterType<ILookupService, LookupService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterInstance<IDatabaseFactory>(new DatabaseFactory());
            // LookupCache will populate static values in the domain for service lookups
            container.RegisterInstance<ILookupCache>(new LookupCache(container.Resolve<Func<IDatabaseFactory>>()));
            return container;
        }
    }
}
