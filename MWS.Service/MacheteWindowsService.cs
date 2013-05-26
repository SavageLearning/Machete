using Microsoft.Practices.Unity;
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
    public class MacheteWindowsService : ServiceBase
    {
        private static System.Timers.Timer aTimer;
        private System.Diagnostics.EventLog MWSEventLog;
        private System.ComponentModel.IContainer components = null;
        private IUnityContainer container;

        public MacheteWindowsService(IUnityContainer unity)
        {
            
            this.MWSEventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.MWSEventLog)).BeginInit();
            // 
            // MWSEventLog
            // 
            this.MWSEventLog.Log = EVcfg.log;
            this.MWSEventLog.Source = EVcfg.source;
            // 
            // ServiceHost
            // 
            this.ServiceName = "MacheteWindowsService";
            ((System.ComponentModel.ISupportInitialize)(this.MWSEventLog)).EndInit();

            if (!System.Diagnostics.EventLog.SourceExists(EVcfg.source))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    EVcfg.source, EVcfg.log);
            }
            container = unity;
            if (unity == null) MWSEventLog.WriteEntry("Unity container is null");

            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
