using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MWS.Service
{
    public partial class MacheteWindowsService : ServiceBase
    {
        private static System.Timers.Timer aTimer;
        public MacheteWindowsService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists(EVcfg.source))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    EVcfg.source, EVcfg.log);
            }
            MWSEventLog.Source = EVcfg.source;
            MWSEventLog.Log = EVcfg.log;

            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        protected override void OnStart(string[] args)
        {
            MWSEventLog.WriteEntry("MWS starting");
            aTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            MWSEventLog.WriteEntry("MWS stopping");
        }

        protected override void OnContinue()
        {
            MWSEventLog.WriteEntry("MWS continue event");
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            MWSEventLog.WriteEntry(string.Format("The Elapsed event was raised at {0}", e.SignalTime));
        }
    }
    /// <summary>
    /// configuration strings for MWS's event log
    /// </summary>
    public struct EVcfg
    {
        public const string source = "MacheteWindowsService";
        public const string log = "MWSLog";
    }
}
