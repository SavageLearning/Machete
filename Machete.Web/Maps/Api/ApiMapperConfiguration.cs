using AutoMapper;
using Machete.Web.Maps.Api.Identity;

namespace Machete.Web.Maps.Api
{
    public static class ApiMapperConfiguration
    {
        public static void ConfigureApi(this IMapperConfigurationExpression c)
        {
            c.AddProfile<EmployersMap>();
            c.AddProfile<ConfigsMap>();
            c.AddProfile<LookupsMap>();
            c.AddProfile<ReportDefinitionsMap>();
            c.AddProfile<WorkAssignmentsMap>();
            c.AddProfile<WorkOrdersMap>();
            c.AddProfile<Service.WorkOrderMap>();
            c.AddProfile<Service.EmployersMap>();
            c.AddProfile<TransportRulesMap>();
            c.AddProfile<TransportProvidersMap>();
            c.AddProfile<ScheduleRulesMap>();

            c.AddProfile<MacheteUserMap>();
        }
    }
}
