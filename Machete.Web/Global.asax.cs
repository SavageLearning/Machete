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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Machete.Web.Models;
using System.Web.Security;
using Machete.Web.IoC;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Web.ViewModel;
using Machete.Service;
using Machete.Web.Controllers;
using System.Globalization;
using System.Threading;
using System.Data.Entity;

using System.Data.Entity.ModelConfiguration;
using Machete.Web.Helpers;
using AutoMapper;

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
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.Add(typeof(List<WorkerRequest>), new workerRequestBinder());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer<MacheteContext>(new MacheteInitializer());
            IUnityContainer container = GetUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            WorkerCache.Initialize(new MacheteContext()); //TODO: migrate to Unity DI container
            Lookups.Initialize(container.Resolve<LookupCache>()); // Static object; used in cshtml files; used instead of proper view models
            MacheteMapper.Initialize(); // AutoMapper
        }

        private IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()
            .RegisterType<IControllerActivator, CustomControllerActivator>()
            .RegisterType<IFormsAuthenticationService, FormsAuthenticationService>()
            .RegisterType<IMembershipService, AccountMembershipService>()
            .RegisterInstance<MembershipProvider>(Membership.Provider)
            .RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>())
            .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>())
            // 
            .RegisterType<IPersonRepository, PersonRepository>(new HttpContextLifetimeManager<IPersonRepository>())
            .RegisterType<IWorkerSigninRepository, WorkerSigninRepository>(new HttpContextLifetimeManager<IWorkerSigninRepository>())
            .RegisterType<IWorkerRepository, WorkerRepository>(new HttpContextLifetimeManager<IWorkerRepository>())
            .RegisterType<IWorkerRequestRepository, WorkerRequestRepository>(new HttpContextLifetimeManager<IWorkerRequestRepository>())
            .RegisterType<IImageRepository, ImageRepository>(new HttpContextLifetimeManager<IImageRepository>())
            .RegisterType<IEmployerRepository, EmployerRepository>(new HttpContextLifetimeManager<IEmployerRepository>())
            .RegisterType<IEmailRepository, EmailRepository>(new HttpContextLifetimeManager<IEmailRepository>())
            .RegisterType<IWorkOrderRepository, WorkOrderRepository>(new HttpContextLifetimeManager<IWorkOrderRepository>())
            .RegisterType<IWorkAssignmentRepository, WorkAssignmentRepository>(new HttpContextLifetimeManager<IWorkAssignmentRepository>())
            .RegisterType<ILookupRepository, LookupRepository>(new HttpContextLifetimeManager<ILookupRepository>())
            .RegisterType<IEventRepository, EventRepository>(new HttpContextLifetimeManager<IEventRepository>())
            .RegisterType<IActivityRepository, ActivityRepository>(new HttpContextLifetimeManager<IActivityRepository>())
            .RegisterType<IActivitySigninRepository, ActivitySigninRepository>(new HttpContextLifetimeManager<IActivitySigninRepository>())
            // 
            .RegisterType<ILookupService, LookupService>(new HttpContextLifetimeManager<ILookupService>())
            .RegisterType<IActivitySigninService, ActivitySigninService>(new HttpContextLifetimeManager<IActivitySigninService>())
            .RegisterType<IActivityService, ActivityService>(new HttpContextLifetimeManager<IActivityService>())
            .RegisterType<IEventService, EventService>(new HttpContextLifetimeManager<IEventService>())
            .RegisterType<IPersonService, PersonService>(new HttpContextLifetimeManager<IPersonService>())
            .RegisterType<IWorkerSigninService, WorkerSigninService>(new HttpContextLifetimeManager<IWorkerSigninService>())
            .RegisterType<IWorkerService, WorkerService>(new HttpContextLifetimeManager<IWorkerService>())
            .RegisterType<IWorkerRequestService, WorkerRequestService>(new HttpContextLifetimeManager<IWorkerRequestService>())
            .RegisterType<IEmployerService, EmployerService>(new HttpContextLifetimeManager<IEmployerService>())
            .RegisterType<IEmailService, EmailService>(new HttpContextLifetimeManager<IEmailService>())
            .RegisterType<IWorkOrderService, WorkOrderService>(new HttpContextLifetimeManager<IWorkOrderService>())
            .RegisterType<IWorkAssignmentService, WorkAssignmentService>(new HttpContextLifetimeManager<IWorkAssignmentService>())
            .RegisterType<IImageService, ImageService>(new HttpContextLifetimeManager<IImageService>());
            // 
            container.RegisterInstance<ILookupCache>(new LookupCache(container.Resolve<Func<IDatabaseFactory>>()));
            return container;
        }
    }
}