using System;
using System.Collections.Generic;
using System.Linq;
using Machete.Domain;
using Machete.Domain.EmbeddedAttributes;
using Newtonsoft.Json;
using TimeZoneConverter;

namespace Machete.Service
{
    public static class MacheteConfigs
    {
        private static List<Config> list = new List<Config>
        {
            new Config { key = Cfg.OrganizationName,              category = "General", publicConfig = true, value = "Test Center", description = "Will be displayed in the employer's portal and throughout the app" },
            new Config { key = Cfg.EmailServerHostName,           category = "Emails", publicConfig = false, value = "email.com" },
            new Config { key = Cfg.EmailServerPort,               category = "Emails", publicConfig = false, value = "587" },
            new Config { key = Cfg.EmailEnableSSL,                category = "Emails", publicConfig = false, value = "true" },
            new Config { key = Cfg.SmtpUser,                      category = "Emails", publicConfig = false, value = "user" },
            new Config { key = Cfg.SmtpPassword,                  category = "Emails", publicConfig = false, value = "secret" },
            new Config { key = Cfg.EmailFromAddress,              category = "Emails", publicConfig = false, value = "user@email.com" },
            new Config { key = Cfg.EmailEnableSimple,             category = "Emails", publicConfig = false, value = "true" },
            new Config { key = Cfg.TimeZoneDifferenceFromPacific, category = "General", publicConfig = true, value = "0" },
            new Config { key = Cfg.OrganizationAddress,           category = "General", publicConfig = true, value = "123 main st, seattle, wa 98101", description = "Will be displayed in work order summaries, etc" },
            new Config { key = Cfg.PaypalId,                      category = "PayPal", publicConfig = true, value = "id" },
            new Config { key = Cfg.PaypalUrl,                     category = "PayPal", publicConfig = true, value = "url" },
            new Config { key = Cfg.PaypalSecret,                  category = "PayPal", publicConfig = false, value = "secreet"},
            new Config { key = Cfg.PaypalEnvironment,             category = "PayPal", publicConfig = true, value = "sandbox" },
            new Config { key = Cfg.WorkCenterDescription,         category = "OnlineOrders", publicConfig = true, value = 
            "<p>Casa Latina is nonprofit organization that empowers Latino immigrants through educational and economic opportunities. Our employment program connects immigrants with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! <a href=\"http://casa-latina.org/get-involved/hire-worker \" target=\"_blank\">Learn more about Casa Latina</a>.</p>"
            , description = "Will be visible in the welcome page for all users"},
            new Config { key = Cfg.OnlineOrdersIntroMessage,         category = "OnlineOrders", publicConfig = true, value = "OnlineOrdersIntroMessage", description = "Displays in the 1st step of the online hiring. Include your center's basic information and how you program works, contact number, etc. Links to your website will also work." },
            new Config { key = Cfg.OnlineOrdersTerms,             category = "OnlineOrders", publicConfig = true, value = JsonConvert.SerializeObject(new[] 
                                                                                                                                {
                                                                                                                                    new Confirm() { name = "term1", description = "This is the term one" },
                                                                                                                                    new Confirm() { name = "term2", description = "This is the term two" }
                                                                                                                                }), description = "The terms that an employer has to accept before proceding with creating a work order"},
            new Config { key = Cfg.OnlineOrdersTransportDetailsLink, category = "OnlineOrders", publicConfig = true, value = "OnlineOrdersTransportDetailsLink", description = "Will display if your transport provider cost rules have any fees. Include the link to your website with transport cost info "},
            new Config { key = Cfg.OrderConfirmTransportFeesNotice, category = "OnlineOrders", publicConfig = true, value = "OrderConfirmTransportFeesNotice", description = "Will display in the last step if employer chooses a transport method with fees. Inlcude text on why the fee applies"},
            new Config { key = Cfg.DisableOnlineOrders,           category = "OnlineOrders", publicConfig = true, value = "TRUE", description = "Enter either TRUE or FALSE. Enter TRUE to turn off access to the online hiring portal"},
            new Config { key = Cfg.DisableOnlineOrdersBanner,     category = "OnlineOrders", publicConfig = true, value = "Online orders are currently disabled. Please call the center.", description = "The message for employers when the online hring portal is turned off"},
            new Config { key = Cfg.DisableOnlineOrdersBannerInfoUrl,     category = "OnlineOrders", publicConfig = true, value = "DisableOnlineOrdersBannerInfoUrl", description = "The URL to your center's website explaining why online hiring is turned off"},
            new Config { key = Cfg.MicrosoftTimeZoneIndex, category = "Tenants", publicConfig = true }
        };

        /// <summary>
        /// Checks if a config exists, if it doesn't, it creates one
        /// This method should always check if tenant has all configs,
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenantTimeZone"></param>
        public static void Synchronize(MacheteContext context, string tenantTimeZone)
        {
            foreach (var c in list)
            {
                if (!context.Configs.Any(config => config.key == c.key))
                {
                    if (c.key == Cfg.MicrosoftTimeZoneIndex)
                        c.value = TZConvert.IanaToWindows(tenantTimeZone); 
                    c.datecreated = DateTime.Now;
                    c.dateupdated = DateTime.Now;
                    c.createdby = "Init T. Script";
                    c.updatedby = "Init T. Script";
                    context.Configs.Add((Config) c.Clone());
                    context.SaveChanges();
                }
            }
        }
    }
}
