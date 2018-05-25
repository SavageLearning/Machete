using AutoMapper;
using Machete.Api;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System;

namespace Machete.Api
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static IUnityContainer Get()
        {
            return new UnityContainer()
            .RegisterType<IUserStore<MacheteUser>, UserStore<MacheteUser>>(new HierarchicalLifetimeManager())//HttpContextLifetimeManager<IUserStore<ApplicationUser>>())
            //.RegisterType<IUserManager<MacheteUser>, UserManager>(new HierarchicalLifetimeManager())//HttpContextLifetimeManager<IMyUserManager<ApplicationUser>>())
            .RegisterType<IDatabaseFactory, DatabaseFactory>(new HierarchicalLifetimeManager(), new InjectionConstructor("macheteConnection"))
            .RegisterType<IReadOnlyContext, ReadOnlyContext>(new PerResolveLifetimeManager(), new InjectionConstructor("readonlyConnection"))
            .RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager())
            .RegisterType<IEmailConfig, EmailConfig>(new HierarchicalLifetimeManager())
            .RegisterInstance<IMapper>(new MapperConfig().getMapper())
            // 
            .RegisterType<IPersonRepository, PersonRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkerSigninRepository, WorkerSigninRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkerRepository, WorkerRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkerRequestRepository, WorkerRequestRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IImageRepository, ImageRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IEmployerRepository, EmployerRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IEmailRepository, EmailRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkOrderRepository, WorkOrderRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>(new HierarchicalLifetimeManager())
            .RegisterType<ILookupRepository, LookupRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IReportsRepository, ReportsRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IEventRepository, EventRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IActivityRepository, ActivityRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IConfigRepository, ConfigRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IActivitySigninRepository, ActivitySigninRepository>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportProvidersRepository, TransportProvidersRepository>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportProvidersAvailabilityRepository, TransportProvidersAvailabilityRepository>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportRuleRepository, TransportRuleRepository>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportCostRuleRepository, TransportCostRuleRepository>(new HierarchicalLifetimeManager())
            .RegisterType<IScheduleRuleRepository, ScheduleRuleRepository>(new HierarchicalLifetimeManager())
            // 
            .RegisterType<IConfigService, ConfigService>(new HierarchicalLifetimeManager())
            .RegisterType<ILookupService, LookupService>(new HierarchicalLifetimeManager())
            .RegisterType<IActivitySigninService, ActivitySigninService>(new HierarchicalLifetimeManager())
            .RegisterType<IActivityService, ActivityService>(new HierarchicalLifetimeManager())
            .RegisterType<IEventService, EventService>(new HierarchicalLifetimeManager())
            .RegisterType<IPersonService, PersonService>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkerSigninService, WorkerSigninService>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkerService, WorkerService>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkerRequestService, WorkerRequestService>(new HierarchicalLifetimeManager())
            .RegisterType<IEmployerService, EmployerService>(new HierarchicalLifetimeManager())
            .RegisterType<IEmailService, EmailService>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkOrderService, WorkOrderService>(new HierarchicalLifetimeManager())
            .RegisterType<IWorkAssignmentService, WorkAssignmentService>(new HierarchicalLifetimeManager())
            .RegisterType<IImageService, ImageService>(new HierarchicalLifetimeManager())
            .RegisterType<IReportService, ReportService>(new HierarchicalLifetimeManager())
            .RegisterType<IReportsV2Service, ReportsV2Service>(new HierarchicalLifetimeManager())
            .RegisterType<IOnlineOrdersService, OnlineOrdersService>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportProvidersService, TransportProvidersService>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportProvidersAvailabilityService, TransportProvidersAvailabilityService>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportRuleService, TransportRuleService>(new HierarchicalLifetimeManager())
            .RegisterType<ITransportCostRuleService, TransportCostRuleService>(new HierarchicalLifetimeManager())
            .RegisterType<IScheduleRuleService, ScheduleRuleService>(new HierarchicalLifetimeManager())
            ;
        }
    }
}
