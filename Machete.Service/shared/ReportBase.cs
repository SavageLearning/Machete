using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Data.Objects.SqlClient;

using Machete.Data;
using Machete.Data.Infrastructure;

using Machete.Domain;
using Machete.Domain.Entities;

using Machete.Service.shared;

using NLog;


namespace Machete.Service.shared
{
    public interface IReportBase
    {
        IQueryable<reportUnit> CountSignins(DateTime beginDate, DateTime endDate);
        IQueryable<reportUnit> CountUniqueSignins(DateTime beginDate, DateTime endDate);
        IQueryable<reportUnit> CountAssignments(DateTime beginDate, DateTime endDate);
        IQueryable<reportUnit> CountCancelled(DateTime beginDate, DateTime endDate);
        IQueryable<TypeOfDispatchReport> CountTypeofDispatch(DateTime beginDate, DateTime endDate);
        IQueryable<AverageWages> HourlyWageAverage(DateTime beginDate, DateTime endDate);
        IQueryable<reportUnit> ListJobs(DateTime beginDate, DateTime endDate);
        IQueryable<reportUnit> ListZipCodes(DateTime beginDate, DateTime endDate);
    }

    public abstract class ReportBase
    {

        // Repository declarations
        protected readonly IWorkOrderRepository woRepo;
        protected readonly IWorkAssignmentRepository waRepo;
        protected readonly IWorkerRepository wRepo;
        protected readonly IWorkerSigninRepository wsiRepo;
        protected readonly IWorkerRequestRepository wrRepo;
        protected readonly ILookupRepository lookRepo;
        protected readonly ILookupCache lookCache;

        protected ReportBase(IWorkOrderRepository woRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo,
                             IWorkerSigninRepository wsiRepo,
                             IWorkerRequestRepository wrRepo,
                             ILookupRepository lookRepo,
                             ILookupCache lookCache)
        {
            this.woRepo = woRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
            this.wsiRepo = wsiRepo;
            this.wrRepo = wrRepo;
            this.lookRepo = lookRepo;
            this.lookCache = lookCache;
        }

        // Anything else?

    }

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

    public class reportUnit
    {
        public DateTime? date { get; set; }
        public int? count { get; set; }
        public string info { get; set; }
    }
}