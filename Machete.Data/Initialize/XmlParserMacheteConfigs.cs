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
    public static class XmlParserMacheteConfigs
    {
        private static List<ConfigMap> list = new List<ConfigMap>
        {
            new ConfigMap { old ="OrganizationName", cur = "OrganizationName", cat = "General" },
            new ConfigMap { old ="EmailServerHostName", cur = "EmailServerHostName", cat = "Emails" },
            new ConfigMap { old ="EmailServerPort", cur = "EmailServerPort", cat = "Emails" },
            new ConfigMap { old ="EmailEnableSSL", cur = "EmailEnableSSL", cat = "Emails" },
            new ConfigMap { old ="SmtpUser", cur = "SmtpUser", cat = "Emails" },
            new ConfigMap { old ="SmtpPassword", cur = "SmtpPassword", cat = "Emails" },
            new ConfigMap { old ="EmailFromAddress", cur = "EmailFromAddress", cat = "Emails" },
            new ConfigMap { old ="EmailEnableSimple", cur = "EmailEnableSimple", cat = "Emails" },
            new ConfigMap { old ="CenterUsesDuplicateSigninList", cur = "CenterUsesDuplicateSigninList", cat = "DailyList" },
            new ConfigMap { old ="CenterUsesDuplicateDailyList", cur = "CenterUsesDuplicateDailyList", cat = "DailyList" },
            new ConfigMap { old ="TimeZoneDifferenceFromPacific", cur = "TimeZoneDifferenceFromPacific", cat = "General" },
            new ConfigMap { old ="OrganizationAddress", cur = "OrganizationAddress", cat = "General" },
            new ConfigMap { old ="OnlineAdvanceHoursSaturday", cur = "OnlineAdvanceHoursSaturday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineAdvanceHoursSunday", cur = "OnlineAdvanceHoursSunday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineAdvanceHoursMonday", cur = "OnlineAdvanceHoursMonday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineAdvanceHoursTuesday", cur = "OnlineAdvanceHoursTuesday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineAdvanceHoursWednesday", cur = "OnlineAdvanceHoursWednesday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineAdvanceHoursThursday", cur = "OnlineAdvanceHoursThursday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineAdvanceHoursFriday", cur = "OnlineAdvanceHoursFriday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursSaturday", cur = "OnlineEarliestHoursSaturday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursSunday", cur = "OnlineEarliestHoursSunday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursMonday", cur = "OnlineEarliestHoursMonday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursTuesday", cur = "OnlineEarliestHoursTuesday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursWednesday", cur = "OnlineEarliestHoursWednesday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursThursday", cur = "OnlineEarliestHoursThursday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineEarliestHoursFriday", cur = "OnlineEarliestHoursFriday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursSaturday", cur = "OnlineLatestHoursSaturday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursSunday", cur = "OnlineLatestHoursSunday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursMonday", cur = "OnlineLatestHoursMonday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursTuesday", cur = "OnlineLatestHoursTuesday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursWednesday", cur = "OnlineLatestHoursWednesday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursThursday", cur = "OnlineLatestHoursThursday", cat = "OnlineOrders" },
            new ConfigMap { old ="OnlineLatestHoursFriday", cur = "OnlineLatestHoursFriday", cat = "OnlineOrders" },
            new ConfigMap { old ="ServiceableZipcodesTransportBusInsideZone", cur = "ServiceableZipcodesTransportBusInsideZone", cat = "OnlineOrders" },
            new ConfigMap { old ="ServiceableZipcodesTransportBusOutsideZone", cur = "ServiceableZipcodesTransportBusOutsideZone", cat = "OnlineOrders" },
            new ConfigMap { old ="ServiceableZipcodesTransportVanInsideZone", cur = "ServiceableZipcodesTransportVanInsideZone", cat = "OnlineOrders" },
            new ConfigMap { old ="ServiceableZipcodesTransportVanOutsideZone", cur = "ServiceableZipcodesTransportVanOutsideZone", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportVanInsideZoneCostOneWorker", cur = "TransportVanInsideZoneCostOneWorker", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportVanInsideZoneCostMultipleWorkers", cur = "TransportVanInsideZoneCostMultipleWorkers", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportVanOutsideZoneCost", cur = "TransportVanOutsideZoneCost", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportBusOutsideZoneCost", cur = "TransportBusOutsideZoneCost", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportBusInsideZoneCost", cur = "TransportBusInsideZoneCost", cat = "OnlineOrders" },
            new ConfigMap { old ="HirerRegisterInstructions_EN", cur = "HirerRegisterInstructions_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="HirerRegisterInstructions_ES", cur = "HirerRegisterInstructions_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="WorkCenterDescription_EN", cur = "WorkCenterDescription_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="WorkCenterDescription_ES", cur = "WorkCenterDescription_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="LogOnTitle_EN", cur = "LogOnTitle_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="LogOnTitle_ES", cur = "LogOnTitle_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="CreateWOHeader_EN", cur = "CreateWOHeader_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="CreateWOHeader_ES", cur = "CreateWOHeader_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="BusinessHours_EN", cur = "BusinessHours_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="BusinessHours_ES", cur = "BusinessHours_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="PreviouslyHired_EN", cur = "PreviouslyHired_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="PreviouslyHired_ES", cur = "PreviouslyHired_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="ReceiveUpdates_EN", cur = "ReceiveUpdates_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="ReceiveUpdates_ES", cur = "ReceiveUpdates_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="WeightLifted_EN", cur = "WeightLifted_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="WeightLifted_ES", cur = "WeightLifted_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportationMethod_EN", cur = "TransportationMethod_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportationMethod_ES", cur = "TransportationMethod_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportationCalendar_EN", cur = "TransportationCalendar_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="TransportationCalendar_ES", cur = "TransportationCalendar_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="Disclaimer_EN", cur = "Disclaimer_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="Disclaimer_ES", cur = "Disclaimer_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="Submission_EN", cur = "Submission_EN", cat = "OnlineOrders" },
            new ConfigMap { old ="Submission_ES", cur = "Submission_ES", cat = "OnlineOrders" },
            new ConfigMap { old ="PayPalPayment_EN", cur = "PayPalPayment_EN", cat = "PayPal" },
            new ConfigMap { old ="PayPalPayment_ES", cur = "PayPalPayment_ES", cat = "PayPal" },
            new ConfigMap { old ="PayPalAccountID", cur = "PayPalAccountID", cat = "PayPal" },
            new ConfigMap { old ="HostingEndpoint", cur = "HostingEndpoint", cat = "PayPal" },
            new ConfigMap { old ="PaypalDescription", cur = "PaypalDescription", cat = "PayPal" }
        };

        public static void Initialize(MacheteContext context)
        {
            string cfgValue;

            list.ForEach(u =>
            {
                try
                {
                    cfgValue = ConfigurationManager.AppSettings[u.old];
                }
                catch
                {
                    cfgValue = "NULL";
                }

                var o = new Config
                {
                    key = u.cur,
                    value = cfgValue ?? "NULL",  // <--- offending line
                    category = u.cat,
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script"
                };
                context.Configs.Add(o);
                context.Commit();

            });
            var offset = ConfigurationManager.AppSettings["TimeZoneDifferenceFromPacific"];
            if (offset == null) offset = "0";
            context.Database.ExecuteSqlCommand(@"update dbo.workersignins set timeZoneOffset = @timezone",
                new SqlParameter { ParameterName = "timezone", Value = offset });
            context.Database.ExecuteSqlCommand(@"update dbo.activitysignins set timeZoneOffset = @timezone",
                new SqlParameter { ParameterName = "timezone", Value = offset });
            context.Database.ExecuteSqlCommand(@"update dbo.workorders set timeZoneOffset = @timezone",
                new SqlParameter { ParameterName = "timezone", Value = offset });

        }

    }

    public class ConfigMap
    {
        public string old;
        public string cur; // current (new)
        public string cat; // category
    }
}
