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
                ID = 1, 
                name = "JobsDispatched",
                description = "desc",
                sqlquery = @"SELECT lskill.text_en  AS workType,
   count(lskill.text_en) subtotal
FROM [dbo].WorkAssignments as WA
join [dbo].lookups as lskill on (wa.skillid = lskill.id)
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id)
WHERE wo.dateTimeOfWork < (@end)
and wo.dateTimeOfWork > (@start)
and lstatus.text_en = 'Completed'
group by lskill.text_en
order by subtotal desc"
       
            }
        };
        public static void Initialize(MacheteContext context)
        {
            _cache.ForEach(u => {
                u.datecreated = DateTime.Now;
                u.dateupdated = DateTime.Now;
                u.createdby = "Init T. Script";
                u.updatedby = "Init T. Script";
                context.ReportDefinitions.Add(u);
            });
            context.Commit();
        }
    }

}
