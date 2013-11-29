using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Service
{
    public interface IEventHandler
    {
        void Initialize();
        System.Diagnostics.EventLog MWSEventLog { get; set; }

    }
    public class EventHandler : IEventHandler
    {
        public System.Diagnostics.EventLog MWSEventLog {get; set;} 

        public EventHandler() { }

        public void Initialize()
        {
            this.MWSEventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.MWSEventLog)).BeginInit();
            // 
            // MWSEventLog
            // 
            this.MWSEventLog.Log = EventViewerConfig.log;
            this.MWSEventLog.Source = EventViewerConfig.source;
            // 
            // ServiceHost
            // 
            ((System.ComponentModel.ISupportInitialize)(this.MWSEventLog)).EndInit();

            if (!System.Diagnostics.EventLog.SourceExists(EventViewerConfig.source))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    EventViewerConfig.source, EventViewerConfig.log);
            }
        }

    }
}
