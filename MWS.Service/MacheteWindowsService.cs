using Microsoft.Practices.Unity;
using MWS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MWS.Service
{
    public class MacheteWindowsService : ServiceBase
    {
        private static System.Timers.Timer aTimer;
        private System.Diagnostics.EventLog MWSEventLog;
        private System.ComponentModel.IContainer components = null;
        internal IUnityContainer container;
        private bool running { get; set; }

        public MacheteWindowsService(IUnityContainer unity)
        {
            running = false;
            setupEventLog();
            if (unity == null) throw new Exception("Unity container is null");
            container = unity;

            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            
        }

        private void setupEventLog()
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
            this.ServiceName = EVcfg.source;
            ((System.ComponentModel.ISupportInitialize)(this.MWSEventLog)).EndInit();

            if (!System.Diagnostics.EventLog.SourceExists(EVcfg.source))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    EVcfg.source, EVcfg.log);
            }
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
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("EmailManager.ProcessQueue executed at {0}", e.SignalTime));

            if (running == true) return;  // return if prior event is still running
            running = true;
            sb.Append(ProcessEmailQueue());
            running = false;
            MWSEventLog.WriteEntry(sb.ToString());
        }

        public string ProcessEmailQueue()
        {
            var sb = new StringBuilder();
            try
            {
                if (container == null) throw new Exception("Unity container is null");
                var em = container.Resolve<EmailManager>();
                em.ProcessQueue();
                if (em.sentStack.Count() > 0)
                {
                    sb.AppendLine(string.Format("{0}", em.getSent()));
                }
                if (em.exceptionStack.Count() > 0)
                {
                    sb.AppendLine(string.Format("SendEmail exceptions: {0}", em.getExceptions()));
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(string.Format("Exception caught: {0}", ex.Message));
            }
            return sb.ToString();
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
