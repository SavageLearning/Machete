using Microsoft.Practices.Unity;
using NLog;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Machete.Data;
using Machete.Data.Infrastructure;
using MWS.Core.Providers;

namespace MWS.Core
{
    public interface IMacheteInstance
    {
        public void StartAll();
        public void StopAll();
        public IInstance InstanceConfig { get; set; }
    }

    public class MacheteInstance : IMacheteInstance
    {
        bool running { get; set; }
        int interval = 5000; // 5 seconds
        Timer emailTimer;
        public IInstance InstanceConfig {get; set;}
        public IEmailServiceProvider Provider { get; set; }
        Logger nlog;
        LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "MWS.Service", "");

        public MacheteInstance(IEmailServiceProvider p, IInstance cfg)
        {
            InstanceConfig = cfg;
            Provider = p;
            running = false;
            //
            //
            if (InstanceConfig.EmailQueue.TimerIntervalSeconds != null)
            {
                interval = InstanceConfig.EmailQueue.TimerIntervalSeconds * 1000;
            }
            //
            //
            nlog = LogManager.GetCurrentClassLogger();
            //
            //
            emailTimer = new Timer(interval);
            emailTimer.Elapsed += new ElapsedEventHandler(ProcessEmailQueue);
        }

        public void StartAll()
        {
            emailTimer.Enabled = true;

        }

        public void StopAll()
        {
            emailTimer.Enabled = false;

        }

        private void log(string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            nlog.Log(levent);
        }

        //public void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    if (running == true) return;  // return if prior event is still running

        //    var sb = new StringBuilder();
        //    sb.AppendLine(string.Format("EmailManager.ProcessQueue executed at {0}", e.SignalTime));
        //    running = true;
        //    try
        //    {
        //        sb.Append(ProcessEmailQueue(InstanceConfig.EmailQueue.EmailServer));
        //    }
        //    catch (Exception ex)
        //    {
        //        sb.AppendLine(string.Format("Exception caught: {0}", ex.Message));
        //    }
        //    running = false;
        //    log(sb.ToString());
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ProcessEmailQueue(object source, ElapsedEventArgs e)
        {
            //var sb = new StringBuilder();
            //var options = new TransactionOptions
            //{
            //    IsolationLevel = IsolationLevel.Serializable,
            //    Timeout = new TimeSpan(0, 0, 0, 10)
            //};

            //using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            //{
            //    using (var context = new MacheteContext())
            //    {
                    IDatabaseFactory factory = container.Resolve<IDatabaseFactory>();
                    factory.Set(context);

                    var em = container.Resolve<EmailQueueManager>();
                    //sb.AppendLine(em.getDiagnostics());
                    //sb.AppendLine("AppSettings.config location:");
                    //sb.AppendLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    em.ProcessQueue(InstanceConfig.EmailQueue.EmailServer);
                    //if (em.sentStack.Count() > 0)
                    //{
                    //    sb.AppendLine(string.Format("{0}", em.getSent()));
                    //}
                    //if (em.exceptionStack.Count() > 0)
                    //{
                    //    sb.AppendLine(string.Format("SendEmail exceptions: {0}", em.getExceptions()));
                    //}
            //    }
            //    scope.Complete();
            //}

            //return sb.ToString();
        }
    }
}
