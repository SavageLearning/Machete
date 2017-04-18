using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Data
{
    public interface IReportsRepository : IRepository<ReportDefinition>
    {
        List<SimpleDataRow> getJobsDispatchedCount(DateTime beginDate, DateTime endDate);
    }
    public class ReportsRepository : RepositoryBase<ReportDefinition>, IReportsRepository
    {


        public ReportsRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {}

        public List<SimpleDataRow> getJobsDispatchedCount(DateTime beginDate, DateTime endDate)
        {
            return db.Get().Database.SqlQuery<SimpleDataRow>(@"
                SELECT
                  convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), min(wa.skillid)) as id,
                  lskill.text_en  AS label,
                  count(lskill.text_en) value
                FROM [dbo].WorkAssignments as WA 
                join [dbo].lookups as lskill on (wa.skillid = lskill.id)
                join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
                join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
                WHERE wo.dateTimeOfWork < (@endDate) 
                and wo.dateTimeOfWork > (@startDate)
                and lstatus.text_en = 'Completed'
                group by lskill.text_en
                ",
                new SqlParameter { ParameterName = "startDate", Value = beginDate },
                new SqlParameter { ParameterName = "endDate", Value = endDate })
                .ToList();
        }
    }
}
