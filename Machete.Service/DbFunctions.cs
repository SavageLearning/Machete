using System;

namespace Machete.Service
{
    public static class DbFunctions
    {
        public static DateTime AddMonths(DateTime date, int interval)
        {
            return date.AddMonths(interval);
        }

        public static int DiffMinutes(DateTime userDate, DateTime recordDate)
        {
            return (userDate - recordDate).Minutes;
        }
        
        public static int DiffHours(DateTime userDate, DateTime recordDate)
        {
            return (userDate - recordDate).Hours;
        }

        public static int DiffDays(DateTime userDate, DateTime recordDate)
        {
            return (userDate - recordDate).Days;
        }

        public static int DiffMonths(DateTime userDate, DateTime recordDate)
        {
            var value = 0;            
            if (userDate.Year == recordDate.Year && userDate.Month == recordDate.Month) return 0;
            
            var daysInUserMonth = DateTime.DaysInMonth(userDate.Year, userDate.Month);
            var daysRemainingInUserMonth = daysInUserMonth - userDate.Day;
            var daysInRecordMonth = DateTime.DaysInMonth(recordDate.Year, recordDate.Month);
            var daysRemainingInRecordMonth = daysInRecordMonth - recordDate.Day;

            // we expect negative numbers, if the user date exceeds the record date
            //var totalSpan = (recordDate - userDate).Days;

            if (recordDate > userDate) // then we have not passed the date.
            {
                var looseDays = daysRemainingInUserMonth + recordDate.Day;
                if (looseDays > daysInUserMonth) value++;
                //totalSpan -= looseDays;
            } else { // we have passed the date.
                var looseDays = daysRemainingInRecordMonth + userDate.Day;
                if (looseDays > daysInRecordMonth) value--;
                //totalSpan += looseDays;
            }
            // we can now ignore the question of whether there is an extra month lost in the days.
            var yearMonths = (recordDate.Year - userDate.Year) * 12;
            var months = (recordDate.Month - userDate.Month);

            value += yearMonths;
            value += months;
            
            return value;
        }
    }

    public static class SqlFunctions {
        public static string StringConvert(decimal wsiDwccardnum)
        {
            return wsiDwccardnum.ToString();
        }
    }
}
