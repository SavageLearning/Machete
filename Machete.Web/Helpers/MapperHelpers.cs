using System;
using System.Globalization;
using Machete.Domain;
using Machete.Service.DTO;

namespace Machete.Web.Helpers
{
    internal static class MapperHelpers
    {
        public static TimeZoneInfo ClientTimeZoneInfo { get; set; }
        public static IDefaults Defaults { get; set; }

        public static DateTime UtcToClient(this DateTime date) =>
            TimeZoneInfo.ConvertTimeFromUtc(date, ClientTimeZoneInfo);

        public static string UtcToClientString(this DateTime date) =>
            date.UtcToClient().ToString(CultureInfo.InvariantCulture);
            
        public static string ComputeOrderStatus(WorkOrdersList d)
        {
            if (d.statusID == WorkOrder.iActive) return LOrderStatus.Active;
            if (d.statusID == WorkOrder.iCancelled) return LOrderStatus.Cancelled;
            if (d.statusID == WorkOrder.iExpired) return LOrderStatus.Expired;
            if (d.statusID == WorkOrder.iPending) return LOrderStatus.Pending;
            if (d.statusID == WorkOrder.iCompleted)
            {
                // if wo is completed, but 1 (or more) wa aren't assigned - the wo is still unassigned
                if (d.WAUnassignedCount > 0) return LOrderStatus.Unassigned;
                // if wo is completed, but 1 (or more) assigned worker(s) never signed in, then the wo has been orphaned
                if (d.WAUnassignedCount > 0) return LOrderStatus.Orphaned;
                return LOrderStatus.Completed;
            }
            return "unknown";
        }
    }
}
