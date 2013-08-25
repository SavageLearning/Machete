using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Domain.Entities
{
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
