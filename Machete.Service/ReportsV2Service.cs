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
        List<SimpleDataRow> getJobsDispatchedCount(DTO.SearchOptions o);
    }

    public class ReportsV2Service : IReportsV2Service
    {
        protected readonly IWorkOrderRepository woRepo;
        protected readonly IWorkAssignmentRepository waRepo;
        protected readonly IWorkerRepository wRepo;

        public ReportsV2Service(IWorkOrderRepository woRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo
                             )
        {
            this.woRepo = woRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
        }

        public List<SimpleDataRow> getJobsDispatchedCount(DTO.SearchOptions o)
        {
            return waRepo.GetAllQ()
                .Where(wa => DbFunctions.TruncateTime(wa.workOrder.dateTimeofWork) >= o.beginDate
                          && DbFunctions.TruncateTime(wa.workOrder.dateTimeofWork) < o.endDate)
                .GroupBy(wa =>  new { wa.skillEN, wa.skillID } )
                .Select(g => new SimpleDataRow
                {
                    id = DbFunctions.TruncateTime(o.beginDate) + "-" + DbFunctions.TruncateTime(o.endDate) + "-" + g.Key.skillID,
                    label = g.Key.skillEN,
                    value = g.Count().ToString()
                }).ToList();
        }
    }
}
