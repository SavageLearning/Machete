using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Machete.Domain;
using Machete.Service.DTO;

namespace Machete.Api.Helpers
{
    internal static class MapperHelpers
    {
        public static DateTime ToUtcDateTime(this string dateString, TimeZoneInfo ClientTimeZoneInfo) =>
            DateTime.SpecifyKind(Convert.ToDateTime(dateString), DateTimeKind.Unspecified).ClientToUtc(ClientTimeZoneInfo);

        public static DateTime? ToUtcDatetime(this DateTime? date, TimeZoneInfo ClientTimeZoneInfo) =>
            date?.ClientToUtc(ClientTimeZoneInfo);

        public static DateTime ClientToUtc(this DateTime date, TimeZoneInfo ClientTimeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeToUtc(date, ClientTimeZoneInfo);
        }

        public static string NormalizeName(string commonName)
        {
            var name = commonName ?? String.Empty;
            TextInfo tInfo = new CultureInfo("en-US", false).TextInfo;
            var commonNameTitleCase = tInfo.ToTitleCase(name);
            return String.Concat(commonNameTitleCase.Where(c => !Char.IsWhiteSpace(c)));
        }
    }
}