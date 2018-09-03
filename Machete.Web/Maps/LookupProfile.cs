using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            ;
            CreateMap<Domain.Lookup, Service.DTO.LookupList>()
                //.ForMember(v => v.selected, opt => opt.MapFrom(d => d.selected ? Shared.yes : Shared.no ))
                //.ForMember(v => v.level, opt => opt.MapFrom(d => d.level != null ? Convert.ToString(d.level) : "" ))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Lookup/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.category + " " + d.text_EN))
            ;
            CreateMap<Service.DTO.LookupList, ViewModel.LookupList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Lookup/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.category + " " + d.text_EN))
                ;
        }
    }

    //var result = from p in list
    //             select new
    //             {
    //                 tabref = "/Lookup/Edit/" + Convert.ToString(p.ID),
    //                 tablabel = p.category + ' ' + p.text_EN,
    //                 category = p.category,
    //                 selected = p.selected,
    //                 text_EN = p.text_EN,
    //                 text_ES = p.text_ES,
    //                 subcategory = p.subcategory,
    //                 level = p.level,
    //                 ltrCode = p.ltrCode,
    //                 dateupdated = Convert.ToString(p.dateupdated),
    //                 updatedby = p.updatedby,
    //                 recordid = Convert.ToString(p.ID)
    //             };

    //private string _getTabRef(Lookup per)
    //{
    //    if (per != null) return "/Lookup/Edit/" + Convert.ToString(per.ID);
    //    else return null;
    //}
    //private string _getTabLabel(Lookup per)
    //{
    //    if (per != null) return per.text_EN;
    //    else return null;
    //}
}