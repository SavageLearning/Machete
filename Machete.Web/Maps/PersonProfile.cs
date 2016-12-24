using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Domain.Person, Service.DTO.PersonList>();
            CreateMap<Domain.Person, ViewModel.Person>();
            CreateMap<Service.DTO.PersonList, ViewModel.PersonList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Person/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => Convert.ToString(d.dwccardnum)))
                .ForMember(v => v.active, opt => opt.MapFrom(d => d.active ? Shared.True : Shared.False))
                .ForMember(v => v.status, opt => opt.MapFrom(d => d.active))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                ;
        }
    }
}