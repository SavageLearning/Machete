using AutoMapper;
using Machete.Domain;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
{
    public class ScheduleRulesMap : Profile
    {
        public ScheduleRulesMap()
        {
            CreateMap<ScheduleRule, ScheduleRuleVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
        }
    }
}
