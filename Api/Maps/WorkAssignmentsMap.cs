using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Api.ViewModel;
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
                .ForMember(v => v.weightLifted, opt => opt.MapFrom(d => d.requiresHeavyLifting))
                // hard-coding onlineorders' days to 1 since no one is currently using the multi-day orders
                .ForMember(v => v.days, opt => opt.UseValue(1));
        }

    }
}