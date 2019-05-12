using System;
using System.Collections.Generic;

namespace Machete.Service
{
    // update 2019-1-11; heavily refactored to remove as much code as possible, reason being it was slowing down
    // the IDE with improvement suggestions (and probably slowing the build and tests). Hopefully better now.

    public class AverageWageModel
    {
        public DateTime date { get; set; }
        public double hours { get; set; }
        public double wages { get; set; }
        public double avg { get; set; }
    }

    public class MemberDateModel
    {
        public int dwcnum { get; set; }
        public string zip { get; set; }
        public DateTime memDate { get; set; }
        public DateTime expDate { get; set; }
    }

    public class TypeOfDispatchModel : ReportUnit
    {
        public int dwcList { get; set; }
        public int dwcPropio { get; set; }
        public int hhhList { get; set; }
        public int hhhPropio { get; set; }
    }

    public class StatusUnit : ReportUnit
    {
        public int? expiredOnDate { get; set; }
        public int? enrolledOnDate { get; set; }
    }

    public class PlacementUnit : ReportUnit
    {
        public int? undupCount { get; set; }
        public int? permCount { get; set; }
        public int? tempCount { get; set; }
    }

    /// <summary>
    /// This is the basic unit of the Report service layer.
    /// All of the values are nullable to make the unit as
    /// extensible as possible.
    /// </summary>
    public class ReportUnit
    {
        public DateTime? date { get; set; }
        public int? count { get; set; }
        public string info { get; set; }
        public string zip { get; set; }
        public string activityType { get; set; }
    }

    public class DailySumData : TypeOfDispatchModel
    {
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int cancelledJobs { get; set; }
        public int totalAssignments { get; set; }
    }

    public class WeeklySummaryData
    {
        public DayOfWeek dayofweek { get; set; }
        public DateTime date { get; set; }
        public int totalSignins { get; set; }
        public int noWeekJobs { get; set; }
        public double weekEstDailyHours { get; set; }
        public double weekEstPayment { get; set; }
        public double weekHourlyWage { get; set; }
        public IEnumerable<ReportUnit> topJobs { get; set; }
    }

    public class MonthlySummaryData
    {
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int dispatched { get; set; }
        public int tempDispatched { get; set; }
        public int permanentPlacements { get; set; }
        public int undupDispatched { get; set; }
        public double totalHours { get; set; }
        public double totalIncome { get; set; }
        public double avgIncomePerHour { get; set; }
        public int stillHere { get; set; }
        public int newlyEnrolled { get; set; }
        public int peopleWhoLeft { get; set; }
        public int peopleWhoWentToClass { get; set; }
    }

    public class YearSumData
    {
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int totalSignins { get; set; }
        public int uniqueSignins { get; set; }
        public int dispatched { get; set; }
        public int tempDispatched { get; set; }
        public int permanentPlacements { get; set; }
        public int undupDispatched { get; set; }
        public double totalHours { get; set; }
        public double totalIncome { get; set; }
        public double avgIncomePerHour { get; set; }
        public int stillHere { get; set; }
        public int newlyEnrolled { get; set; }
        public int peopleWhoLeft { get; set; }
        public int peopleWhoWentToClass { get; set; }
    }

    public class ZipModel
    {
        public string zips { get; set; }
        public int jobs { get; set; }
        public int emps { get; set; }
        public string skills { get; set; }
    }

    public class NewWorkerData
    {
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public int singleAdults { get; set; }
        public int familyHouseholds { get; set; }
        public int newSingleAdults { get; set; }
        public int newFamilyHouseholds { get; set; }
        public int zipCompleteness { get; set; }
    }
    
    public class ActivityData
    {
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public int safety { get; set; }
        public int skills { get; set; }
        public int esl { get; set; }
        public int basGarden { get; set; }
        public int advGarden { get; set; }
        public int finEd { get; set; }
        public int osha { get; set; }
        public IEnumerable<ReportUnit> drilldown { get; set; }
    }
}
