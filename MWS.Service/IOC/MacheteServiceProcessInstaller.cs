using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Service
{
    /// <summary>
    /// Extension to contain Machete defaults
    /// </summary>
    internal class MacheteServiceProcessInstaller : ServiceProcessInstaller
    {
        public MacheteServiceProcessInstaller() : base()
        {
            Account = ServiceAccount.LocalSystem;
        }
    }
}
