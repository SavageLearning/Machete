using Machete.Service;
using System;

namespace Machete.Web.ViewModel
{
    public class ReportPrintView
    {
        public string typeOfReport { get; set; }
        public DateTime date { get; set; }
    }

    public class DailyReportPrintView : ReportPrintView
    {
        public dataTableResult<DailySumData> report { get; set; }
    }

    public class WeeklyReportPrintView : ReportPrintView
    {
        public dataTableResult<WeeklySummaryData> report { get; set; }
    }

    public class MonthlyReportPrintView : ReportPrintView
    {
        public dataTableResult<MonthlySummaryData> report { get; set; }
    }

    public class JzcReportPrintView : ReportPrintView
    {
        public dataTableResult<ZipModel> report { get; set; }
    }
}