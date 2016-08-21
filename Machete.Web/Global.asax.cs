#region COPYRIGHT
// File:     Global.asax.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
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
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.IoC;
using Machete.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Machete.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute("Error", "{*url}", new { controller = "Error", action = "Http404" });

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session != null)
            {
                CultureInfo ci = (CultureInfo)this.Session["Culture"];
                //Checking first if there is no value in session 
                //and set default language 
                //this can happen for first user's request
                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en";

                    //Try to get values from Accept lang HTTP header
                    if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length != 0)
                    {
                        //Gets accepted list 
                        langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);
                    }
                    ci = new CultureInfo(langName);
                    this.Session["Culture"] = ci;
                }
                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }


        protected void Application_Start()
        {
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            AreaRegistration.RegisterAllAreas();
            // from MVC 4 template
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            ModelBinders.Binders.Add(typeof(List<WorkerRequest>), new workerRequestBinder());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            var initializer = new MacheteInitializer();
            Database.SetInitializer(initializer);
            IUnityContainer container = GetUnityContainer();
            var db = container.Resolve<IDatabaseFactory>();
            initializer.InitializeDatabase(db.Get());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            //Lookups.Initialize(container.Resolve<ILookupCache>(), container.Resolve<IDatabaseFactory>()); // Static object; used in cshtml files; used instead of proper view models
            Lookups.Initialize(container.Resolve<ILookupCache>()); // Static object; used in cshtml files; used instead of proper view models
            MacheteMapper.Initialize(); // AutoMapper
        }

        private IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()
            .RegisterType<IControllerActivator, CustomControllerActivator>()
            //.RegisterType<IFormsAuthenticationService, FormsAuthenticationService>()
            //.RegisterType<IMembershipService, AccountMembershipService>()
            //.RegisterInstance<MembershipProvider>(Membership.Provider)
            .RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new PerRequestLifetimeManager())//HttpContextLifetimeManager<IUserStore<ApplicationUser>>())
            .RegisterType<IMyUserManager<ApplicationUser>, MyUserManager>(new PerRequestLifetimeManager())//HttpContextLifetimeManager<IMyUserManager<ApplicationUser>>())
            //.RegisterInstance<IDatabaseFactory>(new DatabaseFactory())
            //.RegisterType<IDatabaseFactory, DatabaseFactory>(new ContainerControlledLifetimeManager(), new InjectionConstructor("macheteConnection"))
            .RegisterType<IDatabaseFactory, DatabaseFactory>(new PerRequestLifetimeManager(), new InjectionConstructor("macheteConnection"))
            .RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager())
            .RegisterInstance<IEmailConfig>(new EmailConfig())
            // 
            .RegisterType<IPersonRepository, PersonRepository>(new PerRequestLifetimeManager())
            .RegisterType<IWorkerSigninRepository, WorkerSigninRepository>(new PerRequestLifetimeManager())
            .RegisterType<IWorkerRepository, WorkerRepository>(new PerRequestLifetimeManager())
            .RegisterType<IWorkerRequestRepository, WorkerRequestRepository>(new PerRequestLifetimeManager())
            .RegisterType<IImageRepository, ImageRepository>(new PerRequestLifetimeManager())
            .RegisterType<IEmployerRepository, EmployerRepository>(new PerRequestLifetimeManager())
            .RegisterType<IEmailRepository, EmailRepository>(new PerRequestLifetimeManager())
            .RegisterType<IWorkOrderRepository, WorkOrderRepository>(new PerRequestLifetimeManager())
            .RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>(new PerRequestLifetimeManager())
            .RegisterType<ILookupRepository, LookupRepository>(new PerRequestLifetimeManager())
            .RegisterType<IEventRepository, EventRepository>(new PerRequestLifetimeManager())
            .RegisterType<IActivityRepository, ActivityRepository>(new PerRequestLifetimeManager())
            .RegisterType<IActivitySigninRepository, ActivitySigninRepository>(new PerRequestLifetimeManager())
            // 
            .RegisterType<ILookupService, LookupService>(new PerRequestLifetimeManager())
            .RegisterType<IActivitySigninService, ActivitySigninService>(new PerRequestLifetimeManager())
            .RegisterType<IActivityService, ActivityService>(new PerRequestLifetimeManager())
            .RegisterType<IEventService, EventService>(new PerRequestLifetimeManager())
            .RegisterType<IPersonService, PersonService>(new PerRequestLifetimeManager())
            .RegisterType<IWorkerSigninService, WorkerSigninService>(new PerRequestLifetimeManager())
            .RegisterType<IWorkerService, WorkerService>(new PerRequestLifetimeManager())
            .RegisterType<IWorkerRequestService, WorkerRequestService>(new PerRequestLifetimeManager())
            .RegisterType<IEmployerService, EmployerService>(new PerRequestLifetimeManager())
            .RegisterType<IEmailService, EmailService>(new PerRequestLifetimeManager())
            .RegisterType<IWorkOrderService, WorkOrderService>(new PerRequestLifetimeManager())
            .RegisterType<IWorkAssignmentService, WorkAssignmentService>(new PerRequestLifetimeManager())
            .RegisterType<IImageService, ImageService>(new PerRequestLifetimeManager())
            .RegisterType<IReportService, ReportService>(new PerRequestLifetimeManager())
            // 
            .RegisterType<IWorkerCache, WorkerCache>(new ContainerControlledLifetimeManager())
            .RegisterType<ILookupCache, LookupCache>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}