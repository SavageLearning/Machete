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

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddDbContext<MacheteContext>(builder => { builder
                .UseLazyLoadingProxies()
                .UseSqlServer(connString, with => with.MigrationsAssembly("Machete.Data"));
            });

            services.AddIdentity<MacheteUser, IdentityRole>()
                .AddEntityFrameworkStores<MacheteContext>()
                .AddDefaultTokenProviders(); // <~ keep for JWT auth

            // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=macos
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-2.2
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-2.2
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
//                    .AddFacebook(facebookOptions =>
//                    {
//                        facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
//                        facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
//                        facebookOptions.CallbackPath = "/id/signin-facebook";
//                    })
//                    .AddGoogle(googleOptions =>
//                    {
//                        googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
//                        googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
//                        googleOptions.CallbackPath = "/id/signin-google";
//                    });
            // see notes at:
            // https://github.com/aspnet/Security/issues/1756#issuecomment-388855389
            // https://stackoverflow.com/a/50767346/2496266
            // "The /signin-{provider} route is handled by the middleware, not by your MVC controller.
            //  Your external login should route to something like `/ExternalLoginCallback`."
            //            
            // Great, so why is it commented out?
            // https://github.com/aspnet/AspNetCore/issues/1871
            // https://stackoverflow.com/questions/51883634/asp-net-core-2-1-web-api-identity-and-external-login-provider
            // Basically, this would work great if it was an ASP.NET Core MVC app, but APIs are less suited to this
            // sort of thing. So I am rolling my own in IdentityController.cs --but thinking of IdentityServer4 perhaps.

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
                options.Cookie.Expiration = TimeSpan.FromDays(150); // TODO half a year?
                options.Cookie.SameSite = SameSiteMode.None;
                
                // these paths are the defaults, declared explicitly:
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            }); // <~ keep for JWT auth

            // for JWT auth, this will have to be reconfigured for "AllowAllOrigins" (included here, but commented out)
            services.AddCors(options => {
                options.AddPolicy(StartupConfiguration.AllowCredentials, builder => {
                    builder.WithOrigins("https://localhost:4213", "https://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });    

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

            services.ConfigureDi();

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

            // This refers to the policies set in the services object. An invalid name will force the default policy.
            app.UseCors(StartupConfiguration.AllowCredentials);

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
            var fileProvider =
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Identity"));
            var requestPath = new PathString("/id/login");
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath,
                DefaultFileNames = new[] {"index.html"}
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

            app.UseCookiePolicy(new CookiePolicyOptions {
                MinimumSameSitePolicy = SameSiteMode.None
            });

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
