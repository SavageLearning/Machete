using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Microsoft.Practices.Unity;
using MWS.Core;
using MWS.Core.Providers;
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
        public System.Diagnostics.EventLog MWSEventLog;
        private System.ComponentModel.IContainer components = null;
        internal IUnityContainer container;
        public List<MacheteInstance> instances = new List<MacheteInstance>();

        /// <summary>
        /// Service process called by the Program.Main
        /// </summary>
        /// <param name="unity"></param>
        public MacheteWindowsService()
        {
            this.ServiceName = EventViewerConfig.source;
            var bootstrapper = new ServiceBootstrapper();

            try
            {
                container = bootstrapper.Build();
            }
            catch (Exception e)
            {
                var eventhandler = new EventHandler();
                eventhandler.MWSEventLog.WriteEntry(e.ToString());
                return;
            }

            this.MWSEventLog = container.Resolve<IEventHandler>().MWSEventLog;
            //
            //
            var cfg = new MacheteWindowsServiceConfiguration();
            foreach (Instance instanceCfg in cfg.Instances)
            {
                instances.Add(new MacheteInstance(container.Resolve<IEmailServiceProvider>(), instanceCfg));
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
            foreach (var instance in instances)
            {
                instance.StartAll();
            }
        }

        protected override void OnStop()
        {
            MWSEventLog.WriteEntry("MWS stopping");
            foreach (var instance in instances)
            {
                instance.StopAll();
            }
        }

        protected override void OnContinue()
        {
            MWSEventLog.WriteEntry("MWS continue event");
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
