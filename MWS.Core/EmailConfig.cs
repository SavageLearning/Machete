using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Core
{
    public class fooEmailConfig
    {
        public string host { get; set; }
        public int port { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool enableSSL { get; set; }

        public fooEmailConfig()
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
