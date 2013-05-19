using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Service.IOC
{
    internal class InstallBootstrapper
    {
        public IUnityContainer Build()
        {
            IUnityContainer container = new UnityContainer();
            return container;
        }
    }
}
