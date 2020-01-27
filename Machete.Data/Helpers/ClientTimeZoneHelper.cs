using System;

namespace Machete.Data.Helpers
{
    public static class ClientTimeZoneHelper
    {
        public static DateTime DateBasedOn(this DateTime date, TimeZoneInfo clientTimeZoneInfo) =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(date, DateTimeKind.Unspecified), clientTimeZoneInfo).Date;        
    }
}