using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Machete.Data;
using Machete.Web.Helpers.Api;
using Machete.Web.Helpers.Api.Identity;
using Machete.Web.ViewModel.Api.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Machete.Web
{
    /// <summary>
    /// A class containing WebHost and RouteBuilder extension methods.
    /// </summary>
    public static class StartupConfiguration
    {
        public static IWebHost CreateOrMigrateDatabase(this IWebHost webhost)
        {
            using (var scope = webhost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MacheteContext>();
                context.Database.Migrate();
                MacheteConfiguration.Seed(context, webhost.Services);
            }

            return webhost;
        }

        private static string GetControllerNames()
        {
            var controllerNames = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(x =>
                    x.IsSubclassOf(typeof(ControllerBase)) &&
                    x.FullName.StartsWith(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".Controllers"))
                .ToList()
                .Select(x => x.Name.Replace("Controller", ""));

            return string.Join("|", controllerNames);
        }
       
        public static void MapLegacyMvcRoutes(this IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Account}/{action=Login}/{id?}");
            routes.MapRoute(
                name: "V2",
                template: "V2/{*url}",
                defaults: new { controller = "V2", action = "Index" }
            );
        }
        
        public static void MapApiRoutes(this IRouteBuilder routes)
        {
            var host = string.Empty;

            routes.MapRoute(
                name: "DefaultApi",
                template: "api/{controller}/{id?}", // id? == was RouteParameter.Optional
                defaults: new { controller = "Home" },
                constraints: new { controller = GetControllerNames() }
            );
            routes.MapRoute(
                name: "LoginApi",
                template: $"{host.IdentityRoute()}{{action}}",
                defaults: new { controller = "Identity" },
                constraints: new { action = "accounts|login|authorize|logoff" }
            );
            routes.MapRoute(
                name: "WellKnownToken",
                template: $"{host.WellKnownRoute()}{{action}}",
                defaults: new { controller = "Identity" },
                constraints: new { action = "openid-configuration|jwks" }
            );
            routes.MapRoute(
                name: "NotFound",
                template: "{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
        }
        
        public static void ConfigureJwt(this IServiceCollection services, RsaSecurityKey signingKey, IConfigurationSection jwtAppSettingOptions)
        {
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = credentials;
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
            
            // Do the following if you want to switch to JWT auth. We currently only generate a token for future use.
            //
                                                             // // PLEASE DO NOT REMOVE // //            
//            services.AddAuthentication(options => {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            }).AddJwtBearer(configureOptions => {
//                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
//                configureOptions.TokenValidationParameters = tokenValidationParameters;
//                configureOptions.SaveToken = true;
//            });
//            services.AddAuthorization(options =>
//            {
//                // TODO put roles here, if using JWT for auth; this will require refactoring attribute methods:
//                options.AddPolicy("ApiUser", policy =>
//                    policy.RequireClaim("role", "api_access"));
//            });
            
            services.AddScoped<JwtIssuerOptions>(); // <~ this may need to go later in the pipeline.
        }

    }
}
