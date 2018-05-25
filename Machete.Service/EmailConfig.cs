using Machete.Domain;
using System;
using System.Configuration;
using System.Text;

namespace Machete.Service
{
    public interface IEmailConfig
    {
        string host { get; set; }
        int port { get; set; }
        string userName { get; set; }
        string password { get; set; }
        bool enableSSL { get; set; }
        bool IsComplete { get; }
        string fromAddress { get; set; }
    }

    public class EmailConfig : IEmailConfig
    {
        public string host { get; set; }
        public int port { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool enableSSL { get; set; }
        public bool enableSimpleEmail { get; set; }
        public string fromAddress { get; set; }

        public EmailConfig(IConfigService cfg)
        {
            port = 0;
            enableSSL = false;
            enableSimpleEmail = false;
            host = cfg.getConfig(Cfg.EmailServerHostName);
            port = Convert.ToInt16(cfg.getConfig(Cfg.EmailServerPort));
            enableSSL = Convert.ToBoolean(cfg.getConfig(Cfg.EmailEnableSSL));
            userName = cfg.getConfig(Cfg.SmtpUser);
            password = cfg.getConfig(Cfg.SmtpPassword);
            //enableSimpleEmail = Convert.ToBoolean(ConfigurationManager.AppSettings[""]);
            fromAddress = cfg.getConfig(Cfg.EmailFromAddress);
        }

        public bool IsComplete
        {
            get
            {
                if (host == null) return false;
                if (port == 0) return false;
                if (userName == null) return false;
                if (password == null) return false;
                if (fromAddress == null) return false;
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
