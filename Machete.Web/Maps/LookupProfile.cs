using AutoMapper;
using System;
using Machete.Service;

namespace Machete.Web.Maps
{
    public class LookupProfile : Profile
    {
        public LookupProfile()
        {
            CreateMap<Domain.Lookup, ViewModel.Lookup>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Lookup/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.category + " " + d.text_EN))
                .ForMember(v => v.recordid,             opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                 .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore());
            
            CreateMap<Domain.Lookup, Service.DTO.LookupList>()
                //.ForMember(v => v.selected, opt => opt.MapFrom(d => d.selected ? Shared.yes : Shared.no ))
                //.ForMember(v => v.level, opt => opt.MapFrom(d => d.level != null ? Convert.ToString(d.level) : "" ))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => SqlFunctions.StringConvert(d.ID)))
                .ForMember(v => v.tabref, opt => opt.Ignore())
                .ForMember(v => v.tablabel, opt => opt.Ignore());

            CreateMap<Service.DTO.LookupList, ViewModel.LookupList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Lookup/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.category + " " + d.text_EN));
        }
    }
}
