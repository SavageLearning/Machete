using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Machete.Service;

namespace Machete.Web.ViewModel
{
    public class ReportPrintView
    {
        public string typeOfReport { get; set; }
        public DateTime date { get; set; }
    }

    public class DailyReportPrintView : ReportPrintView
    {
        public dataTableResult<dailyData> report { get; set; }
    }

    public class WeeklyReportPrintView : ReportPrintView
    {
        public dataTableResult<weeklyData> report { get; set; }
    }

    public class MonthlyReportPrintView : ReportPrintView
    {
        public dataTableResult<monthlyData> report { get; set; }
    }

    public class JzcReportPrintView : ReportPrintView
    {
        public dataTableResult<jzcData> report { get; set; }
    }
}