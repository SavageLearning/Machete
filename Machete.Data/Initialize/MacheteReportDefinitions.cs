using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Data
{
    public static class MacheteReportDefinitions
    {

        public static List<ReportDefinition> cache { get { return _cache; } }

        private static List<ReportDefinition> _cache = new List<ReportDefinition>
        {
            new ReportDefinition {
                name = "DispatchesByJob",
                category = "Dispatches",
                description = "The number of completed dispatches, grouped by job (skill ID)",
                sqlquery = @"SELECT
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
group by lskill.text_en"
            },
            new ReportDefinition {
                name = "DispatchesByMonth",
                description = "The number of completed dispatches, grouped by month",
                category = "Dispatches",
                sqlquery = @"SELECT
  convert(varchar(23), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), month(min(wo.datetimeofwork))) as id,
  convert(varchar(7), min(wo.datetimeofwork), 126)  AS label,
  count(*) value

from workassignments wa
join workorders wo on wo.id = wa.workorderid
join lookups l on wo.status = l.id
where  datetimeofwork >= @startDate
and datetimeofwork < @endDate
and l.text_en = 'Completed'
and wa.workerassignedid is not null
group by month(wo.datetimeofwork)",


            }
        };
        public static void Initialize(MacheteContext context)
        {

            _cache.ForEach(u => {
                try
                {
                    context.ReportDefinitions.First(a => a.name == u.name);
                }
                catch
                {
                    u.datecreated = DateTime.Now;
                    u.dateupdated = DateTime.Now;
                    u.createdby = "Init T. Script";
                    u.updatedby = "Init T. Script";
                    context.ReportDefinitions.Add(u);
                }
            });
            context.Commit();
        }
    }

}
