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
            new Config { key = Cfg.WorkCenterDescription,         category = "OnlineOrders", publicConfig = true, value = "<p>Casa Latina is nonprofit organization that empowers Latino immigrants through educational and economic opportunities. Our employment program connects immigrants with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! <a href=\"http://casa-latina.org/get-involved/hire-worker \" target=\"_blank\">Learn more about Casa Latina</a>.</p>"}
        };

        public static void Initialize(MacheteContext context)
        {
            foreach (var c in list)
            {
                c.datecreated = DateTime.Now;
                c.dateupdated = DateTime.Now;
                c.createdby = "Init T. Script";
                c.updatedby = "Init T. Script";
                context.Configs.Add((Config) c.Clone());
                context.SaveChanges();
            }
        }

        public static void SetWindowsTimeZones(MacheteContext context, string tenantTimeZone)
        {
            Func<Config, bool> configIsMicrosoftTimezone = config => config.key == Cfg.MicrosoftTimeZoneIndex;

            if (!context.Configs.Any(configIsMicrosoftTimezone))
            {
                ;
                var configEntry = new Config()
                {
                    key = Cfg.MicrosoftTimeZoneIndex,
                    category = "Tenants",
                    publicConfig = true,
                    value = GetWindowsTimeZones(tenantTimeZone),
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script",
                };
                context.Configs.Add(configEntry);
                context.SaveChanges();
            }
        }

        public static string GetWindowsTimeZones(string tenantTimeZone)
        {   
            /* 
                These timezones are for SQLSERVER to compare against system UTC dates
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
                case "America/Chicago":
                    windowsTZ = "Central Standard Time";
                    break;
                case "America/Denver":
                    windowsTZ = "Mountain Standard Time";                    
                    break;
                case "America/Phoenix":
                    windowsTZ = "Mountain Standard Time";                    
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
