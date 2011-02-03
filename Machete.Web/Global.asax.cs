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

        }

protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();
    RegisterGlobalFilters(GlobalFilters.Filters);
    RegisterRoutes(RouteTable.Routes);
    IUnityContainer container = GetUnityContainer();
    DependencyResolver.SetResolver(new UnityDependencyResolver(container));
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
        .RegisterType<ICategoryRepository, CategoryRepository>(new HttpContextLifetimeManager<ICategoryRepository>())
        .RegisterType<IExpenseRepository, ExpenseRepository>(new HttpContextLifetimeManager<IExpenseRepository>())
        .RegisterType<ICategoryService, CategoryService>(new HttpContextLifetimeManager<ICategoryService>())
        .RegisterType<IExpenseService, ExpenseService>(new HttpContextLifetimeManager<IExpenseService>());
        return container;         
    }
 }
}