using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using Machete.Web.Controllers.Api;
using Machete.Web.Maps;
using Machete.Web.Maps.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer;
using Machete.Data;
using Machete.Data.Tenancy;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable MemberCanBePrivate.Global

namespace Machete.Web
{
    public class Startup
    {

        private readonly RsaSecurityKey _signingKey;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            LocalEnv = env;
            
            using (RSA rsa = RSA.Create()) {
                rsa.KeySize = 4096;                
                _signingKey = new RsaSecurityKey(rsa);
            }
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment LocalEnv { get; }

    /// <summary>
    /// Defines the ASP.NET Core middleware pipeline. This method gets called by the runtime.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
        {
            var tenants = Configuration.GetSection("Tenants").Get<TenantMapping>();
            var connString = Configuration.GetConnectionString(tenants.Tenants["default"]);

            services.ConfigureJwt(_signingKey, Configuration);

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
            services.AddLocalization(options => options.ResourcesPath = StartupConfiguration.ResourcesFolder);

            services.AddDbContext<MacheteContext>(builder =>
            {
                builder.UseLazyLoadingProxies()
                       .UseSqlServer(connString, with => with.MigrationsAssembly("Machete.Data"));
            });

            services.ConfigureAuthentication(Configuration);

            var mapperConfig = new MapperConfiguration(maps =>
            {
                maps.AllowNullCollections = true;
                //maps.CreateMissingTypeMaps = false;
                maps.ConfigureMvc();
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

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#configure-localization
            // https://github.com/aspnet/AspNetCore/issues/6332
            // https://stackoverflow.com/questions/34753498/self-referencing-loop-detected-in-asp-net-core
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddMvc(options =>
            {
                options.MaxValidationDepth = 4; // if there is a recursive error, don't go crazy
                options.SuppressChildValidationForOneToManyRelationships();
                // if (LocalEnv.IsDevelopment()) options.Filters.Add(new AllowAnonymousFilter());
                // options.Filters.Add(new AuthorizeFilter()); }) // <~ for JWT auth                    
            })

            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                            
            services.AddSpaStaticFiles(angularApp =>
            {
                angularApp.RootPath = "dist";
            });            

            services.ConfigureDependencyInjection();

            // Headers configuration:
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // The pipeline is sequential; since all other elements rely on the headers, this must remain at the top
            app.UseForwardedHeaders();
            // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.2#scenarios-and-use-cases
            app.Use((context, next) =>
            {
                context.Request.Scheme = Environment.GetEnvironmentVariable("MACHETE_USE_HTTPS_SCHEME") ?? "http";
                return next();
            });

            var envIsDevelopment = env.IsDevelopment();
            if (envIsDevelopment)
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2 (Ibid.)
            var esCulture = CultureInfo.CreateSpecificCulture("es-US");
            var dateformat = new DateTimeFormatInfo
            { 
                ShortDatePattern = "MM/dd/yyyy", 
                LongDatePattern = "MM/dd/yyyy hh:mm:ss tt" 
            };
            esCulture.DateTimeFormat = dateformat;
            var supportedCultures = new[]
            {
                // Ibid. #globalization-and-localization-terms
                // https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
                // https://en.wikipedia.org/wiki/ISO_3166-1
                new CultureInfo("en-US"),
                esCulture
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
            

            //app.UseHttpsRedirection();

            // For the original MVC app. Serves CSS, JS, etc. from Content. Because this includes the Angular app,
            // this should be kept when de-fusing the two projects. This doesn't represent an issue or technical debt
            // because pretty much this entire method should be ported over for a new project anyway.
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/Content"
            });

            // TODO favicon.ico is missing?

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

            // https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/angular?view=aspnetcore-2.2
            app.UseSpaStaticFiles();            
            //Add the EndpointRoutingMiddleware
            app.UseRouting();
            // Migration from 2.2 to 3.1 MS recommends this order
            // https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#routing-startup-code
            // This refers to the policies set in the services object. An invalid name will force the default policy.
            app.UseCors(StartupConfiguration.AllowCredentials);
            app.UseCookiePolicy(new CookiePolicyOptions {
                MinimumSameSitePolicy = env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.None
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Machete API v1");
                //c.RoutePrefix = string.Empty;            
            });
            // Endpoint order based on MS recommendation 
            // https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#authorization
            app.UseEndpoints(endpoints =>
            {
                // endpoints.map
                endpoints.MapLegacyMvcRoutes();
                endpoints.MapApiRoutes();
            });
            // https://github.com/aspnet/Mvc/issues/4842
            // app.UseMvc(routes => {
                // keep separate for future api-only port:
                // routes.MapLegacyMvcRoutes();
                // routes.MapApiRoutes();
            // });
            // https://stackoverflow.com/questions/48216929/how-to-configure-asp-net-core-server-routing-for-multiple-spas-hosted-with-spase
            app.Map("/rx", rx => {
                rx.UseSpa(rxApp => {
                     rxApp.Options.SourcePath = "../RX";
                    if (envIsDevelopment) rxApp.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                });
            });
            app.Map("/V2", ng => {
                // https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/angular?view=aspnetcore-2.2
                app.UseSpa(angularApp =>
                {
                    angularApp.Options.SourcePath = "../UI";

                    if (envIsDevelopment) angularApp.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                });
            });


            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/Content"
            });

            // The following code is trying to get lookup static IDs as if they were coming from a singleton database.
            // It ignores the fact that the database provider is multi-tenant, and does not expose a singleton tenant.
            //
            // Alas, the serviceScope object does nothing anyway as the line which populates static IDs is commented out.
            // The apparent purpose is better achieved by improving the Lookups initialization (above).
            //
            // using (var serviceScope = app.ApplicationServices.CreateScope())
            // {
            //     var services = serviceScope.ServiceProvider;
            //     var svc = services.GetService<ILookupService>();
            //     //svc.populateStaticIds();
            // }
        }
    }
}
