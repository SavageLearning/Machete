namespace Machete.Web.Maps.Api
{
    public class ScheduleRulesMap : MacheteProfile
    {
        public ScheduleRulesMap()
        {
            CreateMap<Domain.ScheduleRule, Machete.Web.ViewModel.Api.ScheduleRule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
        }
    }
}
