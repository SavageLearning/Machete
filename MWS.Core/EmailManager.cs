using Machete.Data;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Machete.Data.Infrastructure;
using System.Diagnostics;

namespace MWS.Core
{
    public interface IEmailManager
    {
        void ProcessQueue();
        bool SendEmail(Email email, EmailConfig cfg);
        EmailConfig LoadEmailConfig();
    }

    public class EmailManager : IEmailManager
    {
        IEmailService serv;
        IUnitOfWork db;

        public EmailManager(IEmailService eServ, IUnitOfWork uow)
        {
            serv = eServ;
            db =uow;
        }

        public void ProcessQueue()
        {
            var cfg = LoadEmailConfig();
            var emaillist = serv.GetEmailsToSend();
            var exceptionlist = new Stack<Exception>();
            foreach (var e in emaillist)
            {
                try
                {
                    SendEmail(e, cfg);
                    e.status = Email.iSent;
                }
                catch (Exception ex)
                {
                    //  don't log the a repeating message 
                    // keep the queue from flooding eventlog/debug
                    if (exceptionlist.Count() == 0 || 
                        exceptionlist.Peek().Message != ex.Message)
                    {
                        exceptionlist.Push(ex);
                    }
                }
            }
            // compile exception messages and log
            if (exceptionlist.Count() > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine(new System.String('-', 40));
                foreach (Exception exx in exceptionlist)
                {
                    sb.AppendLine(exx.Message);
                }
                sb.AppendLine(new System.String('-', 40));
                Debug.WriteLine(sb.ToString());
            }
            db.Commit();
        }

        public bool SendEmail(Email email, EmailConfig cfg)
        {
            var client = new SmtpClient(cfg.host, cfg.port)
            {
                Credentials = new NetworkCredential(cfg.userName, cfg.password),
                EnableSsl = cfg.enableSSL
            };
            client.Send(email.emailFrom, email.emailTo, email.subject, email.body);
            return true;
        }

        public EmailConfig LoadEmailConfig()
        {
            var cfg = new EmailConfig();
            cfg.host = ConfigurationManager.AppSettings["EmailServerHostName"];
            cfg.port = Convert.ToInt16(ConfigurationManager.AppSettings["EmailServerPort"]);
            cfg.enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailEnableSSL"]);
            cfg.userName = ConfigurationManager.AppSettings["EmailAccount"];
            cfg.password = ConfigurationManager.AppSettings["EmailPassword"];
            return cfg;
        }
    }

    public class EmailConfig
    {
        public string host { get; set; }
        public int port { get; set; }
        public string userName { get; set; }
        public string password {get; set;}
        public bool enableSSL { get; set; }

    }
}
