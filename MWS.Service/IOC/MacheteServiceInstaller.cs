using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Service
{
    internal class MacheteServiceInstaller : ServiceInstaller
    {
        public MacheteServiceInstaller() : base()
        {
            ServiceName = EventViewerConfig.source; //TODO: separate from eventcfg constants
            DisplayName = "Machete Windows Service";
            Description = "Processes Machete email queue";
            StartType = System.ServiceProcess.ServiceStartMode.Automatic;
        }
    }
}
