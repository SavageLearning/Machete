using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Microsoft.Practices.Unity;
using MWS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;

namespace MWS.Service
{
    public class MacheteWindowsService : ServiceBase
    {
        private static System.Timers.Timer aTimer;
        private System.Diagnostics.EventLog MWSEventLog;
        private System.ComponentModel.IContainer components = null;
        internal IUnityContainer container;
        private bool running { get; set; }
        private int interval = 5000; // 5 seconds

        public MacheteWindowsService(IUnityContainer unity)
        {
            running = false;
            setupEventLog();
            if (unity == null) throw new Exception("Unity container is null");
            container = unity;

            if (ConfigurationManager.AppSettings["TimerInterval"] != null)
            {
                interval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]) * 1000;
            }
            Email.iTransmitAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["TransmitAttempts"]);

            aTimer = new System.Timers.Timer(interval);
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
            if (running == true) return;  // return if prior event is still running

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("EmailManager.ProcessQueue executed at {0}", e.SignalTime));
            running = true;
            try
            {
                sb.Append(ProcessEmailQueue());
            }
            catch (Exception ex)
            {
                sb.AppendLine(string.Format("Exception caught: {0}", ex.Message));
            }
            running = false;
            MWSEventLog.WriteEntry(sb.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ProcessEmailQueue()
        {
            var sb = new StringBuilder();
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = new TimeSpan(0, 0, 0, 10)
            };
            if (container == null) throw new Exception("Unity container is null");

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                using (var context = new MacheteContext())
                {
                    IDatabaseFactory factory = container.Resolve<IDatabaseFactory>();
                    factory.Set(context);

                    var em = container.Resolve<EmailManager>();
                    sb.AppendLine(em.getDiagnostics());
                    sb.AppendLine("AppSettings.config location:");
                    sb.AppendLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
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
                scope.Complete();
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
