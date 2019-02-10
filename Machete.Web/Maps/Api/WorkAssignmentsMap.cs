namespace Machete.Web.Maps.Api
{
    public class WorkAssignmentsMap : MacheteProfile
    {
        public WorkAssignmentsMap()
        {
            CreateMap<Service.DTO.WorkAssignmentsList, Machete.Web.ViewModel.Api.WorkAssignment>();
            CreateMap<Domain.WorkAssignment, Machete.Web.ViewModel.Api.WorkAssignment>()
                .ForMember(v => v.skill, opt => opt.MapFrom(d => d.skillEN))
                .ForMember(v => v.requiresHeavyLifting, opt => opt.MapFrom(d => d.weightLifted));
            CreateMap<Machete.Web.ViewModel.Api.WorkAssignment, Domain.WorkAssignment>()
                .ForMember(v => v.weightLifted, opt => opt.MapFrom(d => d.requiresHeavyLifting));
        }
    }
}
