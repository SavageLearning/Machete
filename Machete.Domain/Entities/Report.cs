using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Domain.Entities
{
    public class TypeOfDispatchReport
    {
        public DateTime date { get; set; }
        public int dwcList { get; set; }
        public int dwcPropio { get; set; }
        public int hhhList { get; set; }
        public int hhhPropio { get; set; }
    }

    public class AverageWages
    {
        public DateTime date { get; set; }
        public int hours { get; set; }
        public double wages { get; set; }
        public double avg { get; set; }
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
    }


    public class reportUnit
    {
        public DateTime? date { get; set; }
        public int? count { get; set; }
        public string info { get; set; }
    }

}