using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Machete.Domain;
using Machete.Service.DTO;

namespace Machete.Web.Helpers
{
    internal static class MapperHelpers
    {
        public static TimeZoneInfo ClientTimeZoneInfo { get; set; }
        
        public static IDefaults Defaults { get; set; }
        
        public static IList<string> UserNames { get; set; }
        
        public static DateTime StartDate { get; set; }
        
        public static DateTime EndDate { get; set; }

        public static DateTime UtcToClient(this DateTime date) => // Specify Utc because the method says it explicitly
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(date, DateTimeKind.Utc), ClientTimeZoneInfo);

        public static string UtcToClientString(this DateTime date) =>
            date.UtcToClient().ToString(CultureInfo.InvariantCulture);
            
        public static string UtcToClientString(this DateTime date, string s) => 
            date.UtcToClient().ToString(s, CultureInfo.InvariantCulture);
        
        public static DateTime ClientToUtc(this DateTime date) =>
            TimeZoneInfo.ConvertTimeToUtc(date, ClientTimeZoneInfo);            
        
        public static DateTime? DataTablesToUtc(string date)
        {
            if (string.IsNullOrEmpty(date)) return null;
            var dateTime = (DateTime?) DateTime.Parse(date);
            return dateTime.ToUtcDatetime();
        }

        public static DateTime ToUtcDateTime(this string dateString) => 
            DateTime.SpecifyKind(Convert.ToDateTime(dateString), DateTimeKind.Unspecified).ClientToUtc();

        public static DateTime? ToUtcDatetime(this DateTime? date) =>
            date?.ClientToUtc();

        // NOTE: Keeping this block to remember what the orphaned/unassigned criteria was
        // 
        // public static string ComputeOrderStatus(WorkOrdersList d)
        // {
        //     if (d.statusID == WorkOrder.iActive) return LOrderStatus.Active;
        //     if (d.statusID == WorkOrder.iCancelled) return LOrderStatus.Cancelled;
        //     if (d.statusID == WorkOrder.iExpired) return LOrderStatus.Expired;
        //     if (d.statusID == WorkOrder.iPending) return LOrderStatus.Pending;
        //     if (d.statusID == WorkOrder.iCompleted)
        //     {
        //         // if wo is completed, but 1 (or more) wa aren't assigned - the wo is still unassigned
        //         if (d.WAUnassignedCount > 0) return LOrderStatus.Unassigned;
        //         // if wo is completed, but 1 (or more) assigned worker(s) never signed in, then the wo has been orphaned
        //         if (d.WAUnassignedCount > 0) return LOrderStatus.Orphaned;
        //         return LOrderStatus.Completed;
        //     }
        //     return "unknown";
        // }

        //
        // Summary:
        //     converts into a string ID, no spaces and camelcased. Used for report name identifier
        // 
        //
        public static string ToNameAsId(string commonName)
        {
            TextInfo tInfo = new CultureInfo("en-US", false).TextInfo;
            var commonNameTitleCase = tInfo.ToTitleCase(commonName);
            return String.Concat(commonNameTitleCase.Where(c => !Char.IsWhiteSpace(c)));
        }
    }
}
