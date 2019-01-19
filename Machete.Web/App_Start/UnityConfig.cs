using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace Machete.Web.App_Start
{
    public static class UnityConfig
    { 
        public static IUnityContainer GetUnityContainer()
        {
        //Create UnityContainer          
        IUnityContainer container = new UnityContainer()
        //.RegisterType<IControllerActivator, CustomControllerActivator>()
        .RegisterType<IUserStore<MacheteUser>, UserStore<MacheteUser>>(new PerResolveLifetimeManager())//HttpContextLifetimeManager<IUserStore<ApplicationUser>>())
        .RegisterType<IMacheteUserManager<MacheteUser>, MacheteUserManager>(new PerResolveLifetimeManager())//HttpContextLifetimeManager<IMyUserManager<ApplicationUser>>())
        .RegisterType<Data.Infrastructure.IDatabaseFactory, DatabaseFactory>(new PerResolveLifetimeManager(), new InjectionConstructor("macheteConnection"))
        .RegisterType<IReadOnlyContext, ReadOnlyContext>(new PerResolveLifetimeManager(), new InjectionConstructor("readonlyConnection"))
        .RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager())
        .RegisterType<IEmailConfig, EmailConfig>(new PerResolveLifetimeManager())
        .RegisterInstance<IMapper>(new MapperConfig().getMapper())
        // 
        .RegisterType<IPersonRepository, PersonRepository>(new PerResolveLifetimeManager())
        .RegisterType<IWorkerSigninRepository, WorkerSigninRepository>(new PerResolveLifetimeManager())
        .RegisterType<IWorkerRepository, WorkerRepository>(new PerResolveLifetimeManager())
        .RegisterType<IWorkerRequestRepository, WorkerRequestRepository>(new PerResolveLifetimeManager())
        .RegisterType<IImageRepository, ImageRepository>(new PerResolveLifetimeManager())
        .RegisterType<IEmployerRepository, EmployerRepository>(new PerResolveLifetimeManager())
        .RegisterType<IEmailRepository, EmailRepository>(new PerResolveLifetimeManager())
        .RegisterType<IWorkOrderRepository, WorkOrderRepository>(new PerResolveLifetimeManager())
        .RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>(new PerResolveLifetimeManager())
        .RegisterType<ILookupRepository, LookupRepository>(new PerResolveLifetimeManager())
        .RegisterType<IReportsRepository, ReportsRepository>(new PerResolveLifetimeManager())
        .RegisterType<IEventRepository, EventRepository>(new PerResolveLifetimeManager())
        .RegisterType<IActivityRepository, ActivityRepository>(new PerResolveLifetimeManager())
        .RegisterType<IConfigRepository, ConfigRepository>(new PerResolveLifetimeManager())
        .RegisterType<IActivitySigninRepository, ActivitySigninRepository>(new PerResolveLifetimeManager())
        .RegisterType<ITransportProvidersRepository, TransportProvidersRepository>(new HierarchicalLifetimeManager())
        .RegisterType<ITransportProvidersAvailabilityRepository, TransportProvidersAvailabilityRepository>(new HierarchicalLifetimeManager())
        .RegisterType<ITransportRuleRepository, TransportRuleRepository>(new HierarchicalLifetimeManager())
        .RegisterType<ITransportCostRuleRepository, TransportCostRuleRepository>(new HierarchicalLifetimeManager())
        .RegisterType<IScheduleRuleRepository, ScheduleRuleRepository>(new HierarchicalLifetimeManager())

        // 
        .RegisterType<IConfigService, ConfigService>(new PerResolveLifetimeManager())
        .RegisterType<ILookupService, LookupService>(new PerResolveLifetimeManager())
        .RegisterType<IActivitySigninService, ActivitySigninService>(new PerResolveLifetimeManager())
        .RegisterType<IActivityService, ActivityService>(new PerResolveLifetimeManager())
        .RegisterType<IEventService, EventService>(new PerResolveLifetimeManager())
        .RegisterType<IPersonService, PersonService>(new PerResolveLifetimeManager())
        .RegisterType<IWorkerSigninService, WorkerSigninService>(new PerResolveLifetimeManager())
        .RegisterType<IWorkerService, WorkerService>(new PerResolveLifetimeManager())
        .RegisterType<IWorkerRequestService, WorkerRequestService>(new PerResolveLifetimeManager())
        .RegisterType<IEmployerService, EmployerService>(new PerResolveLifetimeManager())
        .RegisterType<IEmailService, EmailService>(new PerResolveLifetimeManager())
        .RegisterType<IWorkOrderService, WorkOrderService>(new PerResolveLifetimeManager())
        .RegisterType<IOnlineOrdersService, OnlineOrdersService>(new PerResolveLifetimeManager())
        .RegisterType<IWorkAssignmentService, WorkAssignmentService>(new PerResolveLifetimeManager())
        .RegisterType<IImageService, ImageService>(new PerResolveLifetimeManager())
        .RegisterType<IReportService, ReportService>(new PerResolveLifetimeManager())
        .RegisterType<IReportsV2Service, ReportsV2Service>(new PerResolveLifetimeManager())
        .RegisterType<ITransportProvidersService, TransportProvidersService>(new HierarchicalLifetimeManager())
        .RegisterType<ITransportProvidersAvailabilityService, TransportProvidersAvailabilityService>(new HierarchicalLifetimeManager())
        .RegisterType<ITransportRuleService, TransportRuleService>(new HierarchicalLifetimeManager())
        .RegisterType<ITransportCostRuleService, TransportCostRuleService>(new HierarchicalLifetimeManager())
        .RegisterType<IScheduleRuleService, ScheduleRuleService>(new HierarchicalLifetimeManager())
        // 
        .RegisterType<IDefaults, Defaults>(new ContainerControlledLifetimeManager());

        return container;
    }

}
}
