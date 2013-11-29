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
using System.IO;
//using System.Transactions;

namespace MWS.Core
{
    public interface IEmailManager
    {
        void ProcessQueue(EmailServerConfig cfg);
        bool SendEmail(Email email, EmailServerConfig cfg);
        EmailServerConfig LoadEmailConfig();
    }

    public class EmailQueueManager : IEmailManager
    {
        IEmailService serv;
        IUnitOfWork db;
        public Stack<Exception> exceptionStack { get; set; }
        public Stack<Email> sentStack { get; set; }

        public EmailQueueManager(IEmailService eServ, IUnitOfWork uow)
        {
            serv = eServ;
            db =uow;
        }

        public string getDiagnostics()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Total emails in database: {0}", serv.TotalCount().ToString());
            return sb.ToString();
        }

        public void ProcessQueue(EmailServerConfig cfg)
        {
            exceptionStack = new Stack<Exception>();
            sentStack = new Stack<Email>();
            //
            var emaillist = serv.GetEmailsToSend();
            foreach (var e in emaillist)
            {
                SendEmail(e, cfg);
            }
            db.Commit();
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
                sb.AppendLine();
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
                        System.String.Format("EmailID: {2}, To: {0}, Subject: {1}, Attempts: {3}, Status: {4}",
                        exx.emailTo, exx.subject, exx.ID, exx.transmitAttempts, exx.statusID)
                        );
                }
                sb.AppendLine(new System.String('-', 40));
            }
            return sb.ToString();
        }

        public bool SendEmail(Email e, EmailServerConfig cfg)
        {
            try
            {
                e.emailFrom = cfg.OutgoingAccount;
                var client = new SmtpClient(cfg.HostName, cfg.Port)
                {
                    Credentials = new NetworkCredential(cfg.OutgoingAccount, cfg.OutgoingPassword),
                    EnableSsl = cfg.EnableSSL
                };
                var msg = new MailMessage(e.emailFrom, e.emailTo);
                var a = Attachment.CreateAttachmentFromString(e.attachment, e.attachmentContentType);
                a.Name = "Order.html";
                msg.Subject = e.subject;
                msg.Body = e.body;
                msg.Attachments.Add(a
                    //Attachment.CreateAttachmentFromString(e.attachment, e.attachmentContentType)
                );
                client.Send(msg);

                //client.Send(e.emailFrom, e.emailTo, e.subject, e.body);
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
                e.lastAttempt = DateTime.Now;
                e.transmitAttempts += 1;
            }
            return true;
        }

        public EmailServerConfig LoadEmailConfig()
        {
            var cfg = new EmailServerConfig();
            if (!cfg.IsComplete) throw new Exception("EmailConfig incomplete. Needs host, port, userName, & password");
            return cfg;
        }
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
