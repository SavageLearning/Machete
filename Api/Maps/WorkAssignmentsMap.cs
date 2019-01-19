using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Api.ViewModel;
using Machete.Api.ViewModels;
using Newtonsoft.Json;

namespace Machete.Api.Maps
{
    public class WorkAssignmentsMap : MacheteProfile
    {
        public WorkAssignmentsMap()
        {
            CreateMap<Service.DTO.WorkAssignmentsList, WorkAssignment>();
            CreateMap<Domain.WorkAssignment, WorkAssignment>()
                .ForMember(v => v.skill, opt => opt.MapFrom(d => d.skillEN))
                .ForMember(v => v.requiresHeavyLifting, opt => opt.MapFrom(d => d.weightLifted));
            CreateMap<WorkAssignment, Domain.WorkAssignment>()
                .ForMember(v => v.weightLifted, opt => opt.MapFrom(d => d.requiresHeavyLifting));
        }
    }
}
