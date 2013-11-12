using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Service
{
    public class InstallBootstrapper
    {
        public IUnityContainer Build()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<Installer, MacheteServiceInstaller>("Installer", new ContainerControlledLifetimeManager())
                     .RegisterType<Installer, MacheteServiceProcessInstaller>("ProcessInstaller", new ContainerControlledLifetimeManager());
            return container;
        }
    }
}
