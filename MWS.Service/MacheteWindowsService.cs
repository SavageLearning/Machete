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
        public System.Diagnostics.EventLog MWSEventLog;
        private System.ComponentModel.IContainer components = null;
        internal IUnityContainer container;
        private bool running { get; set; }
        private int interval = 5000; // 5 seconds
        /// <summary>
        /// Service process called by the Program.Main
        /// </summary>
        /// <param name="unity"></param>
        public MacheteWindowsService(IUnityContainer unity)
        {
            running = false;
            if (unity == null) throw new Exception("Unity container is null");
            container = unity;
            this.ServiceName = EventViewerConfig.source;
            this.MWSEventLog = container.Resolve<IEventHandler>().MWSEventLog;
            //
            //
            if (ConfigurationManager.AppSettings["TimerInterval"] != null)
            {
                interval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]) * 1000;
            }
            Email.iTransmitAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["TransmitAttempts"]);

            aTimer = new System.Timers.Timer(interval);
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
             if (running == true) return;  // return if prior event is still running

            var sb = new StringBuilder();
            var cfg = new EmailServerConfig();
            sb.AppendLine(string.Format("EmailManager.ProcessQueue executed at {0}", e.SignalTime));
            running = true;
            try
            {
                sb.Append(ProcessEmailQueue(cfg));
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
        public string ProcessEmailQueue(EmailServerConfig cfg)
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

                    var em = container.Resolve<EmailQueueManager>();
                    sb.AppendLine(em.getDiagnostics());
                    sb.AppendLine("AppSettings.config location:");
                    sb.AppendLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    em.ProcessQueue(cfg);
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
    public struct EventViewerConfig
    {
        public const string source = "MacheteWindowsService";
        public const string log = "MWSLog";
    }
}
