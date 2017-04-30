using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
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
    public interface IReportsV2Service : IService<ReportDefinition>
    {
        List<Data.SimpleDataRow> getSimpleAggregate(DTO.SearchOptions o);
        List<ReportDefinition> getList();
        ReportDefinition Get(string idOrName);
    }

    public class ReportsV2Service : ServiceBase<ReportDefinition>, IReportsV2Service
    {
        protected readonly IReportsRepository repo;

        public ReportsV2Service(IReportsRepository repo, IUnitOfWork unitOfWork) : base(repo, unitOfWork)
        {
            this.repo = repo;
        }

        public List<Data.SimpleDataRow> getSimpleAggregate(DTO.SearchOptions o)
        {
            int id = 0;
            if (!Int32.TryParse(o.idOrName, out id))
            {
                id = repo.GetMany(r => string.Equals(r.name, o.idOrName, StringComparison.OrdinalIgnoreCase)).First().ID;
            }
            return repo.getSimpleAggregate(id, 
                o.beginDate ?? new DateTime(1753,1,1),
                o.endDate ?? DateTime.MaxValue);
        }

        public List<ReportDefinition> getList()
        {
            return repo.getList();
        }

        public ReportDefinition Get(string idOrName)
        {
            int id = 0;
            Domain.ReportDefinition result;
            // accept vanityname or ID
            if (Int32.TryParse(idOrName, out id))
            {
                result = repo.GetById(id);
            }
            else
            {
                result = repo.GetMany(r => string.Equals(r.name, idOrName, StringComparison.OrdinalIgnoreCase)).First();
            }

            return result;
        }
    }
}
