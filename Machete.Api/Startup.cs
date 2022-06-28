using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.HttpOverrides;
using Machete.Service.Tenancy;
using System.Security.Cryptography;
using Machete.Service;
using AutoMapper;
using Machete.Api.Maps;
using Machete.Api.Controllers;
using Microsoft.OpenApi.Models;
using System.Linq;
using Machete.Api.Helpers;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Machete.Service.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Machete.Service.Identity;
using Machete.Service.BackgroundServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Machete.Api
{
    public class Startup
    {
        private readonly RsaSecurityKey _signingKey;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            LocalEnv = env;

            // wrapping in using () {} statememt throws as it interfears with
            // DI life mamangement: https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes
            RSA rsa = RSA.Create();
            rsa.KeySize = 4096;
            _signingKey = new RsaSecurityKey(rsa);
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment LocalEnv { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tenants = Configuration.GetSection("Tenants").Get<TenantMapping>();
            var connString = Configuration.GetConnectionString(tenants.Tenants["default"]);
            services.AddControllersWithViews();

            services.AddDbContext<MacheteContext>(builder =>
            {
                builder.UseLazyLoadingProxies()
                       .UseSqlServer(connString, with => with.MigrationsAssembly("Machete.Service"));
            });

            services.AddHostedService<RecurringBackgroundService>();
            services.AddTransient<UserManager<MacheteUser>, UserManager<MacheteUser>>();
            services.AddTransient<IPasswordHasher<MacheteUser>, PasswordHasher<MacheteUser>>();
            services.AddTransient<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.AddTransient<IdentityErrorDescriber, IdentityErrorDescriber>();
            services.AddTransient<IUserStore<MacheteUser>, MacheteUserStore>();
            services.AddTransient<IRoleStore<MacheteRole>, MacheteRoleStore>();
            services.AddScoped<ITenantIdentificationService, TenantIdentificationService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<IEmailConfig, EmailConfig>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IActivitySigninService, ActivitySigninService>();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<IEmployerService, EmployerService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IOnlineOrdersService, OnlineOrdersService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IReportsV2Service, ReportsV2Service>();
            services.AddScoped<IScheduleRuleService, ScheduleRuleService>();
            services.AddScoped<ITransportRuleService, TransportRuleService>();
            services.AddScoped<ITransportCostRuleService, TransportCostRuleService>();
            services.AddScoped<ITransportProvidersService, TransportProvidersService>();
            services.AddScoped<ITransportProvidersAvailabilityService, TransportProvidersAvailabilityService>();
            services.AddScoped<IWorkAssignmentService, WorkAssignmentService>();
            services.AddScoped<IWorkerRequestService, WorkerRequestService>();
            services.AddScoped<IWorkerSigninService, WorkerSigninService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
            services.AddTransient<IWorkerActions, WorkerActions>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddAuthentication(options =>
            {
                //Sets cookie authentication scheme
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })

            .AddCookie(cookie =>
            {
                //Sets the cookie name and maxage, so the cookie is invalidated.
                cookie.Cookie.Name = "keycloak.cookie";
                cookie.Cookie.MaxAge = TimeSpan.FromMinutes(60);
                cookie.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                cookie.SlidingExpiration = true;
            })
            .AddOpenIdConnect(options =>
            {
                /*
                 * ASP.NET core uses the http://*:5000 and https://*:5001 ports for default communication with the OIDC middleware
                 * The app requires load balancing services to work with :80 or :443
                 * These needs to be added to the keycloak client, in order for the redirect to work.
                 * If you however intend to use the app by itself then,
                 * Change the ports in launchsettings.json, but beware to also change the options.CallbackPath and options.SignedOutCallbackPath!
                 * Use LB services whenever possible, to reduce the config hazzle :)
                */

                //Use default signin scheme
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //Keycloak server
                options.Authority = Configuration.GetSection("Keycloak")["ServerRealm"];
                //Keycloak client ID
                options.ClientId = Configuration.GetSection("Keycloak")["ClientId"];
                //Keycloak client secret
                options.ClientSecret = Configuration.GetSection("Keycloak")["ClientSecret"];
                //Keycloak .wellknown config origin to fetch config
                options.MetadataAddress = Configuration.GetSection("Keycloak")["Metadata"];
                //Require keycloak to use SSL
                options.RequireHttpsMetadata = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                //Save the token
                options.SaveTokens = true;
                //Token response type, will sometimes need to be changed to IdToken, depending on config.
                options.ResponseType = OpenIdConnectResponseType.Code;
                //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
                options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true
                };


            });
            /*
             * Policy based authentication
             */
            services.AddAuthorization(options =>
            {
                //Create policy with more than one claim
                options.AddPolicy("users", policy =>
                policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                        (c.Value == "user") || (c.Value == "admin"))));
                //Create policy with only one claim
                options.AddPolicy("admins", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "admin"));
                //Create a policy with a claim that doesn't exist or you are unauthorized to
                options.AddPolicy("noaccess", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "noaccess"));
            });

            var mapperConfig = new MapperConfiguration(maps =>
            {
                maps.AllowNullCollections = true;
                //maps.CreateMissingTypeMaps = false;
                maps.ConfigureApi();
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<PaginationMetaDataSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Machete.Api", Version = "v1" });
                c.ResolveConflictingActions(a => a.First()); // necessary for controller action inheritance
            });

            services.AddSpaStaticFiles(angularApp =>
            {
                angularApp.RootPath = "dist";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

                //Enable https only for http service behind a load balancer
                //Disable for local testing on http only
                app.Use((context, next) =>
                {
                    context.Request.Scheme = "https";
                    return next();
                });

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // begin React login page
            var identityFileProvider =
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Identity"));
            var identityRequestPath = new PathString("/id/login");
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = identityFileProvider,
                RequestPath = identityRequestPath,
                DefaultFileNames = new[] { "index.html" }
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = identityFileProvider,
                RequestPath = identityRequestPath
            });
            // end React login page

            app.UseRouting();

            //Uses the defined policies and customizations from configure services
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Machete API v1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                var host = string.Empty;
                endpoints.MapControllerRoute(
                    name: "DefaultApi",
                    pattern: "api/{controller}/{id?}",
                    defaults: new { controller = "Home" },
                    constraints: new { controller = GetControllerNames() }
                    );
                endpoints.MapControllerRoute(
                    name: "LoginApi",
                    pattern: $"{host.IdentityRoute()}{{action}}",
                    defaults: new { controller = "Identity" },
                    constraints: new { action = "accounts|login|authorize|logoff" }
                );
                endpoints.MapControllerRoute(
                    name: "WellKnownToken",
                    pattern: $"{host.WellKnownRoute()}{{action}}",
                    defaults: new { controller = "Identity" },
                    constraints: new { action = "openid-configuration|jwks" }
                );
                endpoints.MapControllerRoute(
                    name: "NotFound",
                    pattern: "{*path}",
                    defaults: new { controller = "Error", action = "NotFound" }
                );
            });
            // https://stackoverflow.com/questions/48216929/how-to-configure-asp-net-core-server-routing-for-multiple-spas-hosted-with-spase
            // app.Map("/rx", rx =>
            // {
            //     rx.UseSpa(rxApp =>
            //     {
            //         rxApp.Options.SourcePath = "../RX";
            //         if (envIsDevelopment) rxApp.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            //     });
            // });
            app.Map("/V2", ng =>
            {
                // https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/angular?view=aspnetcore-2.2
                app.UseSpa(angularApp =>
                {
                    angularApp.Options.SourcePath = "../UI";

                    if (env.IsDevelopment()) angularApp.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                });
            });
        }
        /// <summary>
        /// Return the names of the Machete API controllers within the calling assembly (e.g., Machete.Web).
        /// </summary>
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
    }

}
