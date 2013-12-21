using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Microsoft.Practices.Unity;
using MWS.Core;
using MWS.Core.Providers;
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

        public IUnityContainer Build(InstanceCollection instances)
        {
            container.RegisterInstance<IEventHandler>(new EventHandler());

            foreach (var i in instances)
            {
                //
                // Core DB objects
                container.RegisterType<IDatabaseFactory, DatabaseFactory>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(i.ConnStringName));

                container.RegisterType<IUnitOfWork, UnitOfWork>(
                    i.Name, 
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));
                //
                // Repositories
                container.RegisterType<IEmailRepository, EmailRepository>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<IWorkOrderRepository, WorkOrderRepository>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<IWorkerRepository, WorkerRepository>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(
                        new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<IWorkerSigninRepository, WorkerSigninRepository>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<ILookupRepository, LookupRepository>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<ILookupCache, LookupCache>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<IDatabaseFactory>(i.Name)));

                container.RegisterType<IWorkerCache, WorkerCache>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<Func<IDatabaseFactory>>(i.Name))
                    );

                //
                // Services
                container.RegisterType<IEmailService, EmailService>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(
                        new ResolvedParameter<IEmailRepository>(i.Name),
                        new ResolvedParameter<IWorkOrderService>(i.Name),
                        new ResolvedParameter<IUnitOfWork>(i.Name)
                        )
                    );

                container.RegisterType<IWorkOrderService, WorkOrderService>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(
                        new ResolvedParameter<IWorkOrderRepository>(i.Name),
                        new ResolvedParameter<IWorkAssignmentService>(i.Name),
                        new ResolvedParameter<IUnitOfWork>(i.Name)
                        )
                    );

                container.RegisterType<IWorkAssignmentService, WorkAssignmentService>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(
                        new ResolvedParameter<IWorkAssignmentRepository>(i.Name),
                        new ResolvedParameter<IWorkerRepository>(i.Name),
                        new ResolvedParameter<ILookupRepository>(i.Name),
                        new ResolvedParameter<IWorkerSigninRepository>(i.Name),
                        new ResolvedParameter<IWorkerCache>(i.Name),
                        new ResolvedParameter<ILookupCache>(i.Name),
                        new ResolvedParameter<IUnitOfWork>(i.Name)
                        )
                    );

                container.RegisterType<IEmailServiceProvider, EmailServiceProvider>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(
                        new ResolvedParameter<Func<IEmailQueueManager>>(i.Name),
                        new ResolvedParameter<Func<IDatabaseFactory>>(i.Name)
                        )
                    );

                container.RegisterType<IEmailQueueManager, EmailQueueManager>(
                    i.Name,
                    new PerResolveLifetimeManager(),
                    new InjectionConstructor(
                            new ResolvedParameter<IEmailService>(i.Name),
                            new ResolvedParameter<IUnitOfWork>(i.Name)
                        )
                    );
                //container.RegisterType<IWorkerService, WorkerService>();
                //container.RegisterType<IWorkerSigninService, WorkerSigninService>();
                //container.RegisterType<ILookupService, LookupService>();
                //// LookupCache will populate static values in the domain for service lookups
            }
            return container;
        }
    }
}
