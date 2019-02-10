using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Data.Repositories;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Helpers.Api.Identity;
using Machete.Web.Maps;
using Machete.Web.Maps.Api;
using Machete.Web.ViewModel.Api.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace Machete.Web
{
    public class Startup
    {
        private RsaSecurityKey _signingKey;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            using (RSA rsa = RSA.Create()) {
                rsa.KeySize = 4096;                
                _signingKey = new RsaSecurityKey(rsa);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The services (Dependency Injection) method for the ASP.NET Core middleware pipeline.
        ///
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
        ///
        /// JWT: https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/Startup.cs
        /// 
        /// This method gets called by the runtime.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {   
            var connString = Configuration.GetConnectionString("DefaultConnection");

            services.ConfigureJwt(_signingKey, Configuration.GetSection(nameof(JwtIssuerOptions)));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //;
                .AddCookie(options =>
                    options.LoginPath = "/Account/Login"
                );

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddDbContext<MacheteContext>(builder => { builder
                .UseLazyLoadingProxies()
                .UseSqlServer(connString, with => with.MigrationsAssembly("Machete.Data"));
            });

            services.AddIdentity<MacheteUser, IdentityRole>()
                .AddEntityFrameworkStores<MacheteContext>()
                .AddDefaultTokenProviders(); // <~ keep for JWT auth

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings; we are relying on validation
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            }); // <~ keep for JWT auth

            // Cookie settings
            services.ConfigureApplicationCookie(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(10); // for testing only
                options.Cookie.HttpOnly = true; // prevent JavaScript access
                options.Cookie.Expiration = TimeSpan.FromDays(150); // half a year?
                // these paths are the defaults, declared explicitly
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            }); // <~ keep for JWT auth

            // for JWT auth, this will have to be reconfigured for "AllowAllOrigins" (included here, but commented out)
            services.AddCors(options => options.AddPolicy("AllowCredentials", builder => {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                })
//                                             .AddPolicy("AllowAllOrigins", builder => {
//                    builder.AllowAnyOrigin()
//                           .AllowAnyHeader()
//                           .AllowAnyMethod();
//                })
            );

            var mapperConfig = new MapperConfiguration(maps => {
                maps.ConfigureMvc();
                maps.ConfigureApi();
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
            services.AddMvc( /*config => { config.Filters.Add(new AuthorizeFilter()); }*/)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivitySigninRepository, ActivitySigninRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IEmailConfig, EmailConfig>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IEmployerRepository, EmployerRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IWorkAssignmentRepository, WorkAssignmentRepository>();
            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IWorkerRequestRepository, WorkerRequestRepository>();
            services.AddScoped<IWorkerSigninRepository, WorkerSigninRepository>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            services.AddScoped<ITransportRuleRepository, TransportRuleRepository>();
            services.AddScoped<ITransportProvidersRepository, TransportProvidersRepository>();
            services.AddScoped<ITransportProvidersAvailabilityRepository, TransportProvidersAvailabilityRepository>();

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
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportsV2Service, ReportsV2Service>();
            services.AddScoped<ITransportRuleService, TransportRuleService>();
            services.AddScoped<ITransportProvidersService, TransportProvidersService>();
            services.AddScoped<ITransportProvidersAvailabilityService, TransportProvidersAvailabilityService>();
            services.AddScoped<IWorkAssignmentService, WorkAssignmentService>();
            services.AddScoped<IWorkerRequestService, WorkerRequestService>();
            services.AddScoped<IWorkerSigninService, WorkerSigninService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();

            services.AddScoped<IDefaults, Defaults>();
            services.AddScoped<IModelBindingAdaptor, ModelBindingAdaptor>();

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#use-a-custom-provider
            // They imply that this is only for "custom" providers but the RequestLocalizationOptions in Configure aren't populated
            // unless you use this.
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("es-US")
                    // we use es-US because we are not fully equipped to support international dates.
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        /// <summary>
        /// The Configure method for the ASP.NET Core middleware pipeline.
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.2#the-configure-method
        ///
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2 (Ibid.)
            var supportedCultures = new[]
            {
                // Ibid. #globalization-and-localization-terms
                // https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
                // https://en.wikipedia.org/wiki/ISO_3166-1
                new CultureInfo("en-US"),
                new CultureInfo("es-US"),
                // we use es-US because we are not fully equipped to support international dates.
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            // the preceding will attempt to guess the user's culture. For several reasons that's not what we want.
            // Ibid. #set-the-culture-programmatically

            app.UseCors("AllowAllOrigins"); // TODO review

            app.UseHttpsRedirection();

            // For the original MVC app. Serves CSS, JS, etc. from Content. Because this includes the Angular app,
            // this should be kept when de-fusing the two projects. This doesn't represent an issue or technical debt
            // because pretty much this entire method should be ported over for a new project anyway.
            app.UseStaticFiles("/Content"); // TODO check + remove if redundant with the one below; I think it is
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/Content"
            });
            
            // TODO favicon.ico is missing?
            
            // begin React login page
            var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Content", @"React"));
            var requestPath = new PathString("/id/login");
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath,
                DefaultFileNames = new[] { "index.html" }
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath
            });
            // end React login page
            
            // uncomment if we should happen to add a wwwroot directory; e.g., if we want to refactor `Content`
            // app.UseDefaultFiles();
            // app.UseStaticFiles(); 

            app.UseCookiePolicy();

            app.UseAuthentication();

            // note the separation here; keep these separate for future port to api-only project
            app.UseMvc(routes => {
                routes.MapLegacyMvcRoutes();
                routes.MapApiRoutes();
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/Content"
            });
        }
    }
}
