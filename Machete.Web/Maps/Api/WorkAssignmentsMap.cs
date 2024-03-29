﻿using AutoMapper;
using Machete.Domain;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
{
    public class WorkAssignmentsMap : Profile
    {
        public WorkAssignmentsMap()
        {
            CreateMap<Service.DTO.WorkAssignmentsList, WorkAssignmentVM>();
            CreateMap<WorkAssignment, WorkAssignmentVM>()
                .ForMember(v => v.skill, opt => opt.MapFrom(d => d.skillEN))
                .ForMember(v => v.requiresHeavyLifting, opt => opt.MapFrom(d => d.weightLifted));
            CreateMap<WorkAssignmentVM, Domain.WorkAssignment>()
                .ForMember(d => d.skillEN, opt => opt.MapFrom(v => v.skill))
                .ForMember(d => d.weightLifted, opt => opt.MapFrom(v => v.requiresHeavyLifting));
            CreateMap<WorkAssignmentVM, Domain.WorkAssignment>()
                .ForMember(v => v.weightLifted, opt => opt.MapFrom(d => d.requiresHeavyLifting));
        }
    }
}
