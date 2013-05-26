using Microsoft.Practices.Unity;
using MWS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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
            IUnityContainer container = bootstrapper.Build();
            ServiceBase[] services = new ServiceBase[]
            {
                container.Resolve<MacheteWindowsService>()
            };
            ServiceBase.Run(services);
        }
    }
}
