using Machete.Data;
using Machete.Service.DTO.Reports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Machete.Service.DTO;

namespace Machete.Service
{
    public interface IReportsV2Service
    {
        List<Data.SimpleDataRow> getJobsDispatchedCount(DTO.SearchOptions o);
    }

    public class ReportsV2Service : IReportsV2Service
    {
        protected readonly IReportsRepository repo;

        public ReportsV2Service(IReportsRepository repo)
        {
            this.repo = repo;
        }

        public List<Data.SimpleDataRow> getJobsDispatchedCount(DTO.SearchOptions o)
        {
            return repo.getJobsDispatchedCount(o.beginDate ?? new DateTime(1753,1,1),
                o.endDate ?? DateTime.MaxValue);
        }
    }
}
