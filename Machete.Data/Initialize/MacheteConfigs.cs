using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Machete.Domain;
using System.Data.SqlClient;

namespace Machete.Data
{
    public static class MacheteConfigs
    {
        private static List<Config> list = new List<Config>
        {
            new Config { key = Cfg.OrganizationName,              category = "General", publicConfig = true, value = "Test Center" },
            new Config { key = Cfg.EmailServerHostName,           category = "Emails", publicConfig = false, value = "email.com" },
            new Config { key = Cfg.EmailServerPort,               category = "Emails", publicConfig = false, value = "587" },
            new Config { key = Cfg.EmailEnableSSL,                category = "Emails", publicConfig = false, value = "true" },
            new Config { key = Cfg.SmtpUser,                      category = "Emails", publicConfig = false, value = "user" },
            new Config { key = Cfg.SmtpPassword,                  category = "Emails", publicConfig = false, value = "secret" },
            new Config { key = Cfg.EmailFromAddress,              category = "Emails", publicConfig = false, value = "user@email.com" },
            new Config { key = Cfg.EmailEnableSimple,             category = "Emails", publicConfig = false, value = "true" },
            new Config { key = Cfg.TimeZoneDifferenceFromPacific, category = "General", publicConfig = true, value = "0" },
            new Config { key = Cfg.OrganizationAddress,           category = "General", publicConfig = true, value = "123 main st, seattle, wa 98101" },
            new Config { key = Cfg.PaypalId,                      category = "PayPal", publicConfig = true, value = "id" },
            new Config { key = Cfg.PaypalUrl,                     category = "PayPal", publicConfig = true, value = "url" },
            new Config { key = Cfg.PaypalSecret,                  category = "PayPal", publicConfig = false, value = "secreet"},
            new Config { key = Cfg.PaypalEnvironment,             category = "PayPal", publicConfig = true, value = "sandbox" },
            new Config { key = Cfg.WorkCenterDescription,         category = "OnlineOrders", publicConfig = true, value = "<p>Casa Latina is nonprofit organization that empowers Latino immigrants through educational and economic opportunities. Our employment program connects immigrants with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! <a href=\"http://casa-latina.org/get-involved/hire-worker \" target=\"_blank\">Learn more about Casa Latina</a>.</p>"},
            new Config { key = Cfg.MicrosoftTimeZoneIndex,        category = "Tenants", publicConfig = true }
        };

        public static void Initialize(MacheteContext context, string tenantTimeZone)
        {
            foreach (var c in list)
            {
                if (c.key == Cfg.MicrosoftTimeZoneIndex)
                    c.value = GetWindowsTimeZones(tenantTimeZone);
                c.datecreated = DateTime.Now;
                c.dateupdated = DateTime.Now;
                c.createdby = "Init T. Script";
                c.updatedby = "Init T. Script";
                context.Configs.Add((Config) c.Clone());
                context.SaveChanges();
            }
        }

        public static string GetWindowsTimeZones(string tenantTimeZone)
        {   
            /* 
                These timezones are for SQLSERVER to compare against system UTC dates
                !! This seed is only valid in development environments. Ideally we would check
                for INA (linux) timezones for Windows TZ equivalent through the Bing api 
            */
            string windowsTZ = "";
            switch (tenantTimeZone)
            {
                case "America/Los_Angeles":
                    windowsTZ = "Pacific Standard Time";
                    break;
                case "America/New_York":
                    windowsTZ = "US Eastern Standard Time";
                    break;
                case "UTC":
                    windowsTZ = "UTC";
                    break;                    
                default: 
                    windowsTZ = "";
                    break;
            }
            return windowsTZ;
        }
    }
}
