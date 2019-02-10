using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Machete.Web
{
    public class Program
    {
        /// <summary>
        /// The program's Main method; entry point for the application.
        /// </summary>
        public static void Main(string[] args) => CustomWebHostBuilder(args).Build()
            .CreateOrMigrateDatabase()
            .Run();        

        /// <summary>
        /// A stripped down version of the default WebHost object configuration, this method gives just the absolute
        /// basics necessary to run an MVC app on POSIX. It does not contain configuration for IIS, instead using only
        /// the Kestrel development server; ideal for hosting with containers.
        /// </summary>
        public static IWebHostBuilder CustomWebHostBuilder(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
              //.UseContentRoute() //uncomment for static content route
                .ConfigureAppConfiguration((host, config) => {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureLogging((app, logging) => {
                    logging.AddConfiguration(app.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>();
    }
}
