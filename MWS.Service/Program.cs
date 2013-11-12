using Microsoft.Practices.Unity;
using MWS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MWS.Service
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var bootstrapper = new ServiceBootstrapper();
            var eventhandler = new EventHandler();
            eventhandler.Initialize();
            bootstrapper.container.RegisterInstance<IEventHandler>(eventhandler);

            var runme = false;
            try
            {
                bootstrapper.Build();
                runme = true;
            }
            catch (Exception e)
            {
                eventhandler.MWSEventLog.WriteEntry(e.ToString());
            }
            finally
            {
                
            }
            if (!runme) return;
            ServiceBase[] services = new ServiceBase[]
            {
                bootstrapper.container.Resolve<MacheteWindowsService>()
            };
            //ServiceBase.Run(services);
            if (Environment.UserInteractive)
            {
                RunInteractive(services);
            }
            else
            {
                ServiceBase.Run(services);
            }
        }
        //
        // http://coding.abel.nu/2012/05/debugging-a-windows-service-project/
        static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("Started");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(
                "Press any key to stop the services and end the process...");
            Console.ReadKey();
            Console.WriteLine();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Stopped");
            }

            Console.WriteLine("All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }
    }
}
