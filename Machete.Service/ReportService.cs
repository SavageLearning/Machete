using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Domain.Entities;
using Machete.Data;
using System.Data.Objects;
using Machete.Data.Infrastructure;
using System.Text.RegularExpressions;
using System.Data.Objects.SqlClient;
using System.Globalization;
using NLog;

namespace Machete.Service
{
    // Other interfaces seem to implement IService, which is a tool for writing a type of record.
    // See [workingDirectory]\Machete.Service\shared\ServiceBase.cs -- didn't do that here.

    public interface IReportService
    {
        IQueryable<MonthlyWithDetailReport> MonthlyWithDetail(DateTime beginDate, DateTime endDate);
    }

    public class ReportService : IReportService
    {
        //private readonly IReportService reportServ;

        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;

        //private readonly ILookupRepository lRepo; //meh? necesary?

        public ReportService(IWorkerSigninRepository wsiRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo)
        {
            this.wsiRepo = wsiRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
        }

        //not sure how to do this yet
        //public IQueryable<MonthlyWithDetailReport> MonthlyWithDetail()
        //{
        //    return MonthlyWithDetail(null);
        //}

        public IQueryable<MonthlyWithDetailReport> MonthlyWithDetail(DateTime beginDate, DateTime endDate) //no drilldown
        {
            IQueryable<MonthlyWithDetailReport> query;
            query = 
                //uses workersignins, workassignments, workers
                // stuck here ^ ->
            var q = db.WorkerSignins
                .Where(wsi => wsi.dateforsignin >= beginDate &&
                              wsi.dateforsignin <= endDate)
                .GroupJoin(db.WorkAssignments, // necessary for left outer join
                      wsi => wsi.WorkAssignmentID,
                      wa => wa.ID,
                      (wsi, wa) => new
                      {
                          wsi,
                          // In SQL, the transformations below would be done in the select; makes this unusual
                          // checkign for null b/c it will be null if there's no match in the left outer join
                          wahours = wa.FirstOrDefault().hours == null ? 0 : wa.FirstOrDefault().hours,
                          wadays = wa.FirstOrDefault().days == null ? 0 : wa.FirstOrDefault().days,
                          wawage = wa.FirstOrDefault().hourlyWage == null ? 0 : wa.FirstOrDefault().hourlyWage
                      })
                .Join(db.Workers, // inner join
                    x => x.wsi.WorkerID,
                    w => w.ID,
                    (xx, ww) => new
                    {
                        xx,
                        // these are the analogous 'case' statements in the SQL. im transforming them to 1s or 0s 
                        // so that I can sum them below. This is the first half of 'make SQL do the math'
                        DWC = (ww.typeOfWorkID == 20 ? 1 : 0),
                        HHH = (ww.typeOfWorkID == 21 ? 1 : 0),
                        DWCdispatched = (ww.typeOfWorkID == 20 && xx.wsi.WorkAssignmentID != null ? 1 : 0),
                        HHHdispatched = (ww.typeOfWorkID == 21 && xx.wsi.WorkAssignmentID != null ? 1 : 0),
                        totalHours = (xx.wahours * xx.wadays),
                        totalIncome = (xx.wahours * xx.wadays * xx.wawage),
                    })
                .GroupBy(a => a.xx.wsi.dateforsignin)
                .Select(group => new mwdData
                {
                    date = group.Key,
                    // and the second half of 'make SQL do the math'. all the above stuff was to get to
                    // this group by. SQL server will execute all of the resulting SQL in <1 second. 
                    totalSignins = group.Count(),
                    totalDWCSignins = group.Sum(b => b.DWC),
                    totalHHHSignins = group.Sum(b => b.HHH),
                    dispatchedDWCSignins = group.Sum(b => b.DWCdispatched),
                    dispatchedHHHSignins = group.Sum(b => b.HHHdispatched),
                    totalHours = group.Sum(b => b.totalHours),
                    totalIncome = group.Sum(b => b.totalIncome)//,
                    // this last field isnt working. it can be calculated at the MVC Controller layer, before the data goes out
                    // it's just: totalHours * totalIncome
                    //avgIncomePerHour = group.Sum(b => b.totalIncome / b.totalHours)
                })
                .OrderBy(a => a.date);

            return q;
        }

        public class mwdData
        {
            /// A class containing all of the data for the Monthly Report with Details
            /// DateTime date, int totalDWCSignins, int totalHHHSignins
            /// dispatchedDWCSignins, int dispatchedHHHSignins
            /// int totalHours, double totalIncome, ...
            /// double totalAverage (commented out, not working)
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

}