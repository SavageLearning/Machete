using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // Anything going to the old hirer system, send to Angular
            routes.MapRoute(
                name: "HirerAccount",
                url: "HirerAccount/{*url}",
                defaults: new { controller = "V2", action = "Index" }
                );
            routes.MapRoute(
                name: "HirerWorkOrder",
                url: "HirerWorkOrder/{*url}",
                defaults: new { controller = "V2", action = "Index" }
                );
            routes.MapRoute(
                name: "V2",
                url: "V2/{*url}",
                defaults: new { controller = "V2", action = "Index" }
                );
            // MVC controller/action routes
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new
                {
                    serverRoute = new ServerRouteConstraint(url =>
                    {
                        return (
                            !url.PathAndQuery.StartsWith("/V2", StringComparison.InvariantCultureIgnoreCase) //||
                           // !url.PathAndQuery.StartsWith("/HirerAccount", StringComparison.InvariantCultureIgnoreCase) 
                         );
                    })
                }
            );
            routes.MapRoute("404", "{*url}", new { controller = "Home", action = "NotFound" });
        }
    }
    public class ServerRouteConstraint : IRouteConstraint
    {
        private readonly Func<Uri, bool> _predicate;

        public ServerRouteConstraint(Func<Uri, bool> predicate)
        {
            this._predicate = predicate;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            return this._predicate(httpContext.Request.Url);
        }
    }
}