using Machete.Web.Resources;
using System;
using System.Globalization;

namespace Machete.Web.Maps
{
    public class PersonProfile : MacheteProfile
    {
        public PersonProfile()
        {
            CreateMap<Domain.Person, Service.DTO.PersonList>()
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.Worker.dwccardnum))
                //.ForMember(v => v.workerStatus, opt => opt.MapFrom(d => getCI() == "ES" ? d.Worker.memberStatusES : d.Worker.memberStatusEN))
                .ForMember(v => v.memberStatusID, opt => opt.MapFrom(d => d.Worker.memberStatusID))
                .ForMember(v => v.memberStatusEN, opt => opt.MapFrom(d => d.Worker.memberStatusEN))
                .ForMember(v => v.memberStatusES, opt => opt.MapFrom(d => d.Worker.memberStatusES))
                ;
            CreateMap<Domain.Person, ViewModel.Person>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Person/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                .MaxDepth(3)
                ;
            CreateMap<Service.DTO.PersonList, ViewModel.PersonList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Person/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => Convert.ToString(d.dwccardnum)))
                .ForMember(v => v.active, opt => opt.MapFrom(d => d.active ? Shared.True : Shared.False))
                .ForMember(v => v.status, opt => opt.MapFrom(d => d.active))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.workerStatus, opt => opt.MapFrom(d => getCI() == "ES" ? d.memberStatusES : d.memberStatusEN))

                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated, CultureInfo.InvariantCulture)))
                ;
        }
    }
}
