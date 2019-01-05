using Machete.Web.Resources;
using System;
using System.Globalization;

namespace Machete.Web.Maps
{
    public class WorkerProfile : MacheteProfile
    {
        public WorkerProfile()
        {
            CreateMap<Domain.Worker, Service.DTO.WorkerList>()
                 .ForMember(v => v.firstname1, opt => opt.MapFrom(d => d.Person.firstname1))
                 .ForMember(v => v.firstname2, opt => opt.MapFrom(d => d.Person.firstname2))
                 .ForMember(v => v.lastname1, opt => opt.MapFrom(d => d.Person.lastname1))
                 .ForMember(v => v.lastname2, opt => opt.MapFrom(d => d.Person.lastname2))

            ;
            CreateMap<Domain.Worker, ViewModel.Worker>()
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Worker/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.Person.firstname1 + ' ' + d.Person.lastname1))
                //.ForMember(v => v.Person, opt => opt.Ignore())
                ;
            CreateMap<Service.DTO.WorkerList, ViewModel.WorkerList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Worker/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                .ForMember(v => v.WID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => Convert.ToString(d.dwccardnum)))
                .ForMember(v => v.active, opt => opt.MapFrom(d => d.active ?? false ? Shared.True : Shared.False))
                .ForMember(v => v.memberStatus, opt => opt.MapFrom(d => getCI() == "ES" ? d.memberStatusES : d.memberStatusEN))
                .ForMember(v => v.memberexpirationdate, opt => opt.MapFrom(d => Convert.ToString(d.memberexpirationdate, CultureInfo.InvariantCulture)))
                ;
        }
    }
}
