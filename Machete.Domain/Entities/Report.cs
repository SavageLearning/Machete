using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Domain.Entities
{
    public class DailyCasaLatinaReport
    {
        public DateTime date { get; set; }
        public int dwcList { get; set; }
        public int dwcPropio { get; set; }
        public int hhhList { get; set; }
        public int hhhPropio { get; set; }
        public int totalSignins { get; set; }
        public int cancelledJobs { get; set; }
        public int dwcFuture { get; set; }
        public int dwcPropioFuture { get; set; }
        public int hhhFuture { get; set; }
        public int hhhPropioFuture { get; set; }
        public int futureTotal { get; set; }
    }

    public class WeeklyElCentroReport
    {
        public DateTime date { get; set; }
        public int totalSignins { get; set; }
        public int noWeekJobs { get; set; }
//        public string weekJobsSector { get; set; }
        public int weekEstDailyHours { get; set; }
        public double weekEstPayment { get; set; }
        public double weekHourlyWage { get; set; }
    }

    public class MonthlyWithDetailReport
    {
        /// A class containing all of the data for the Monthly Report with Details
        public DateTime date { get; set; }
        public int totalSignins { get; set; }
        public int totalDWCSignins { get; set; }
        public int totalHHHSignins { get; set; }
        public int dispatchedDWCSignins { get; set; }
        public int dispatchedHHHSignins { get; set; }
        public int totalHours { get; set; }
        public double totalIncome { get; set; }
        // public double avgIncomePerHour { get; set; }
    }
}
