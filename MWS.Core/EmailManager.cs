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
using System.Data.Entity.Infrastructure;

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
        public Stack<Exception> exceptionStack { get; set; }
        public Stack<Email> sentStack { get; set; }

        public EmailManager(IEmailService eServ, IUnitOfWork uow)
        {
            serv = eServ;
            db =uow;
        }

        public void ProcessQueue()
        {
            var cfg = LoadEmailConfig();
            var emaillist = serv.GetEmailsToSend();
            exceptionStack = new Stack<Exception>();
            sentStack = new Stack<Email>();
            foreach (var e in emaillist)
            {

                if (!setStatusToSending(e))
                {
                    // failed concurrency check; another process has changed the record, skipping
                    continue;
                }
                try
                {
                    SendEmail(e, cfg);
                    e.statusID = Email.iSent; // record sent
                    sentStack.Push(e);
                }
                catch (Exception ex)
                {
                    //  don't log the repeating message 
                    if (exceptionStack.Count() == 0 || 
                        exceptionStack.Peek().Message != ex.Message)
                    {
                        exceptionStack.Push(ex);
                    }
                    e.statusID = Email.iTransmitError;
                }
                finally
                {
                    e.transmitAttempts += 1;
                    db.Commit();
                }
            }
        }

        public bool setStatusToSending(Email em)
        {
            try
            {
                em.statusID = Email.iSending; // lock out edits
                db.Commit();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// String of exception messages, 1 per line
        /// </summary>
        /// <returns></returns>
        public string getExceptions()
        {
            var sb = new StringBuilder();
            if (exceptionStack.Count() > 0)
            {
                sb.AppendLine(new System.String('-', 40));
                foreach (Exception exx in exceptionStack)
                {
                    sb.AppendLine(exx.Message);
                }
                sb.AppendLine(new System.String('-', 40));
            }
            return sb.ToString();
        }

        public string getSent()
        {
            var sb = new StringBuilder();
            if (sentStack.Count() > 0)
            {
                sb.AppendLine(new System.String('-', 40));
                foreach (Email exx in sentStack)
                {
                    sb.AppendLine(
                        System.String.Format("EmailID: {2}, To: {0}, Subject: {1}, Attempts: {3}",
                        exx.emailTo, exx.subject, exx.ID, exx.transmitAttempts)
                        );
                }
                sb.AppendLine(new System.String('-', 40));
            }
            return sb.ToString();
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
            if (!cfg.IsComplete) throw new Exception("EmailConfig incomplete. Needs host, port, userName, & password");
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

        public EmailConfig()
        {
            port = 0;
            enableSSL = false;
            host = ConfigurationManager.AppSettings["EmailServerHostName"];
            port = Convert.ToInt16(ConfigurationManager.AppSettings["EmailServerPort"]);
            enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailEnableSSL"]);
            userName = ConfigurationManager.AppSettings["EmailAccount"];
            password = ConfigurationManager.AppSettings["EmailPassword"];
        }

        public bool IsComplete
        {
            get
            {
                if (host == null) return false;
                if (port == 0) return false;
                if (userName == null) return false;
                if (password == null) return false;
                return true;
            }
        }

        public string GetMissingConfigEntries()
        {
            var sb = new StringBuilder();
            if (host == null) sb.AppendLine("EmailConfig.host is null");
            if (port == 0) sb.AppendLine("EmailConfig.port is 0");
            if (userName == null) sb.AppendLine("EmailConfig.userName is null");
            if (password == null) sb.AppendLine("EmailConfig.password is null");
            return sb.ToString();
        }
    }
}
