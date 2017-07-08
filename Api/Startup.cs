using Elmah.Contrib.WebApi;
using IdentityServer3.AccessTokenValidation;
using Machete.Data;
using Machete.Data.Infrastructure;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Allow all origins
            app.UseCors(CorsOptions.AllowAll);
            // Wire token validation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44379/id",

                // For access to the introspection endpoint
                ClientId = "api",
                ClientSecret = "api-secret",

                RequiredScopes = new[] { "api" }
            });

            // Wire Web API
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            // catch all route mapped to ErrorController so 404 errors
            // can be logged in elmah
            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
            config.Filters.Add(new AuthorizeAttribute());
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            app.UseWebApi(config);
        }
    }
}