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

namespace MWS.Core
{
    public class EmailManager
    {
        IEmailService serv;

        public EmailManager(IEmailService eServ)
        {
            serv = eServ;
        }

        public void ProcessQueue()
        {
            var cfg = LoadEmailConfig();
            var list = serv.GetAll().Where(e => e.status == Email.iReadyToSend);
            foreach (var e in list)
            {
                // attempt to send email
                SendEmail(e, cfg);
                // 
            }
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
