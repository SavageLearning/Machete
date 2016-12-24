using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class WorkerProfile : Profile
    {
        public WorkerProfile()
        {
            CreateMap<Domain.Worker, Service.DTO.WorkerList>()
                 .ForMember(v => v.firstname1, opt => opt.MapFrom(d => d.Person.firstname1))
                 .ForMember(v => v.firstname2, opt => opt.MapFrom(d => d.Person.firstname2))
                 .ForMember(v => v.lastname1, opt => opt.MapFrom(d => d.Person.lastname1))
                 .ForMember(v => v.lastname2, opt => opt.MapFrom(d => d.Person.lastname2))
            ;
            CreateMap<Domain.Worker, ViewModel.Worker>();
            CreateMap<Service.DTO.WorkerList, ViewModel.WorkerList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Worker/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                .ForMember(v => v.WID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => Convert.ToString(d.dwccardnum)))
                .ForMember(v => v.active, opt => opt.MapFrom(d => d.active ? Shared.True : Shared.False))
                .ForMember(v => v.wkrStatus, opt => opt.MapFrom(d => WorkerStatus(d)))
                .ForMember(v => v.memberexpirationdate, opt => opt.MapFrom(d => Convert.ToString(d.memberexpirationdate)))
                ;
        }
        public static string WorkerStatus(Service.DTO.WorkerList wkr)
        {
            if (wkr.memberStatus == Domain.Worker.iActive) return "active";
            if (wkr.memberStatus == Domain.Worker.iInactive) return "inactive";
            if (wkr.memberStatus == Domain.Worker.iExpired) return "expired";
            if (wkr.memberStatus == Domain.Worker.iSanctioned) return "sanctioned";
            if (wkr.memberStatus == Domain.Worker.iExpelled) return "expelled";
            return null;
        }
    }
}