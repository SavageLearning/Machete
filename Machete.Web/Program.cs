using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Machete.Web
{
    public class Program
    {
        /// <summary>
        /// The program's Main method; entry point for the application.
        /// The program is not designed to accept arguments. Secrets from
        /// the dotnet cli user store can be passed in as environment variables.
        /// </summary>
        public static async Task Main(string[] args)
        {
            IWebHost webhost = CreateWebHostBuilder(args).Build();

            await webhost.CreateOrMigrateDatabase();

            await webhost.RunAsync();
        }

        /// <summary>
        /// A stripped down version of the default WebHost object configuration, this method gives just the absolute
        /// basics necessary to run an MVC app on POSIX. It does not contain configuration for IIS, instead using only
        /// the Kestrel development server; ideal for hosting with containers.
        /// </summary>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            new WebHostBuilder()
             // .UseContentRoute() //uncomment for static content route
                .ConfigureAppConfiguration((host, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");

                    if (host.HostingEnvironment.IsDevelopment())
                        config.AddUserSecrets<Startup>();
                    else
                        config.AddEnvironmentVariables(prefix: "MACHETE_");
                })
                .UseKestrel(
//                (context, kestrelOptions) =>
//                {
//                    var certSettings = context.Configuration.GetSection("CertificateSettings");
//                    var certificate = new X509Certificate2(certSettings.GetValue<string>("filename"), certSettings.GetValue<string>("password"));
//                    
//                    kestrelOptions.AddServerHeader = false;
//                    kestrelOptions.Listen(IPAddress.Loopback, 4213, listenerOptions =>
//                    {
//                        listenerOptions.UseHttps(certificate);
//                    });
//                }
                )
                .ConfigureLogging((app, logging) =>
                {
                    logging.AddConfiguration(app.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);

                })
                .UseStartup<Startup>()
                .UseUrls("http://*:4213");
    }
}
