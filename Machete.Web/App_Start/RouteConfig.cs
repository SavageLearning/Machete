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
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new
                {
                    serverRoute = new ServerRouteConstraint(url =>
                    {
                        return !url.PathAndQuery.StartsWith("/V2",
                            StringComparison.InvariantCultureIgnoreCase);
                    })
                }
            );
            routes.MapRoute("angular", "{*url}", new { controller = "V2", action = "index" });
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