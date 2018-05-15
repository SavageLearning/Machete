using AutoMapper;
using Machete.Api.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api
{
    public class MapperConfig
    {
        MapperConfiguration cfg;
        Mapper map;
        public MapperConfig()
        {
            cfg = new MapperConfiguration(c =>
            {
                c.AddProfile<EmployersMap>();
                c.AddProfile<LookupsMap>();
                c.AddProfile<ReportDefinitionsMap>();
                c.AddProfile<WorkAssignmentsMap>();
                c.AddProfile<WorkOrdersMap>();
                c.AddProfile<Service.WorkOrderMap>();
                c.AddProfile<Service.EmployersMap>();
                c.AddProfile<TransportRulesMap>();
                c.AddProfile<TransportProvidersMap>();
                c.AddProfile<ScheduleRulesMap>();
            });
        }
        public IMapper getMapper()
        {
            return map ?? (map = new Mapper(cfg));
        }
    }
}
