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
        List<SimpleDataRow> getSimpleAggregate(int id, DateTime beginDate, DateTime endDate);
        List<ReportDefinition> getList();
    }
    public class ReportsRepository : RepositoryBase<ReportDefinition>, IReportsRepository
    {


        public ReportsRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {}

        public List<SimpleDataRow> getSimpleAggregate(int id, DateTime beginDate, DateTime endDate)
        {


            var rdef = dbset.Single(a => a.ID == id);
            return db.Get().Database.SqlQuery<SimpleDataRow>(rdef.sqlquery,
                new SqlParameter { ParameterName = "startDate", Value = beginDate },
                new SqlParameter { ParameterName = "endDate", Value = endDate })
                .ToList();
        }

        public List<ReportDefinition> getList()
        {
            return db.Get().ReportDefinitions.AsEnumerable().ToList();
        }
    }
}
