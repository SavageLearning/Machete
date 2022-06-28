using AutoMapper;
using Machete.Domain;
using Machete.Api.ViewModel;

namespace Machete.Api.Maps
{
    public class ScheduleRulesMap : Profile
    {
        public ScheduleRulesMap()
        {
            CreateMap<ScheduleRule, ScheduleRuleVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
            CreateMap<ScheduleRuleVM, ScheduleRule>()
                .ForMember(v => v.ID, opt => opt.MapFrom(d => d.id));

        }
    }
}
