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
using Machete.Web.ViewModel;
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
using System.Web.Http;
using Unity.Mvc4;
using AutoMapper;

namespace Machete.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
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
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            ModelBinders.Binders.Add(typeof(List<WorkerRequest>), new workerRequestBinder());
            var initializer = new MacheteInitializer();
            Database.SetInitializer(initializer);
            IUnityContainer container = GetUnityContainer();
            var db = container.Resolve<IDatabaseFactory>();
            initializer.InitializeDatabase(db.Get());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()
            //.RegisterType<IControllerActivator, CustomControllerActivator>()
            .RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new PerResolveLifetimeManager())//HttpContextLifetimeManager<IUserStore<ApplicationUser>>())
            .RegisterType<IMyUserManager<ApplicationUser>, MyUserManager>(new PerResolveLifetimeManager())//HttpContextLifetimeManager<IMyUserManager<ApplicationUser>>())
            .RegisterType<IDatabaseFactory, DatabaseFactory>(new PerResolveLifetimeManager(), new InjectionConstructor("macheteConnection"))
            .RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager())
            .RegisterInstance<IEmailConfig>(new EmailConfig())
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
            .RegisterType<IEventRepository, EventRepository>(new PerResolveLifetimeManager())
            .RegisterType<IActivityRepository, ActivityRepository>(new PerResolveLifetimeManager())
            .RegisterType<IActivitySigninRepository, ActivitySigninRepository>(new PerResolveLifetimeManager())
            // 
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
            .RegisterType<IWorkAssignmentService, WorkAssignmentService>(new PerResolveLifetimeManager())
            .RegisterType<IImageService, ImageService>(new PerResolveLifetimeManager())
            .RegisterType<IReportService, ReportService>(new PerResolveLifetimeManager())
            // 
            .RegisterType<IWorkerCache, WorkerCache>(new ContainerControlledLifetimeManager())
            .RegisterType<ILookupCache, LookupCache>(new ContainerControlledLifetimeManager())
            .RegisterType<IDefaults, Defaults>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}