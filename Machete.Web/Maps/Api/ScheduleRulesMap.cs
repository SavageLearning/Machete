using AutoMapper;
using ScheduleRuleViewModel = Machete.Web.ViewModel.Api.ScheduleRule;

namespace Machete.Web.Maps.Api
{
    public class ScheduleRulesMap : Profile
    {
        public ScheduleRulesMap()
        {
            CreateMap<Domain.ScheduleRule, ScheduleRuleViewModel>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
        }
    }
}
