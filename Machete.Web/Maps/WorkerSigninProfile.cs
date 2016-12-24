using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class WorkerSigninProfile : Profile
    {
        public WorkerSigninProfile()
        {
            CreateMap<Domain.WorkerSignin, Service.DTO.WorkerSigninList>()
                .ForMember(v => v.lotterySequence, opt => opt.MapFrom(d => d.lottery_sequence))
                .ForMember(v => v.englishlevel, opt => opt.MapFrom(d => d == null ? 0 : d.worker.englishlevelID))
                .ForMember(v => v.waid, opt => opt.MapFrom(d => d.WorkAssignmentID))
                .ForMember(v => v.skill1, opt => opt.MapFrom(d => d == null ? null : d.worker.skill1))
                .ForMember(v => v.skill2, opt => opt.MapFrom(d => d == null ? null : d.worker.skill2))
                .ForMember(v => v.skill3, opt => opt.MapFrom(d => d == null ? null : d.worker.skill3))
                .ForMember(v => v.fullname, opt => opt.MapFrom(d =>
                    d.worker.Person.firstname1 + " " +
                    d.worker.Person.firstname2 + " " +
                    d.worker.Person.lastname1 + " " +
                    d.worker.Person.lastname2))
            ;
            CreateMap<Domain.WorkerSignin, ViewModel.WorkerSignin>()
            ;
            CreateMap<Service.DTO.WorkerSigninList, ViewModel.WorkerSigninList>()
            ;
        }
    }
}