using Elmah.Contrib.WebApi;
using IdentityServer3.AccessTokenValidation;
using Machete.Service;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;

[assembly: OwinStartupAttribute(typeof(Machete.Api.Startup))]
namespace Machete.Api
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
                Authority = ConfigurationManager.AppSettings["IdentityProvider"], 

                // For access to the introspection endpoint
                ClientId = ConfigurationManager.AppSettings["IdentityClientId"],
                ClientSecret = ConfigurationManager.AppSettings["IdentityClientSecret"],
                
                RequiredScopes = new[] { "api", "profile", "email" }
            });

            // Wire Web API
            var config = new HttpConfiguration();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.DependencyResolver = new UnityResolver(UnityConfig.Get());
            //config.Filters.Add(new AuthorizeAttribute());
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
            //            = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            var lserv = (LookupService)config.DependencyResolver.GetService(typeof(ILookupService));
            lserv.populateStaticIds();
        }
    }
}