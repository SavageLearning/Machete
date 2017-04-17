﻿using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class ConfigProfile : Profile
    {
        public ConfigProfile()
        {
            CreateMap<Domain.Lookup, ViewModel.Config>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Config/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.category + " " + d.text_EN))
                .ForMember(v => v.recordid,             opt => opt.MapFrom(d => Convert.ToString(d.ID)));
            ;
            CreateMap<Domain.Lookup, Service.DTO.ConfigList>()
                //.ForMember(v => v.selected, opt => opt.MapFrom(d => d.selected ? Shared.yes : Shared.no ))
                //.ForMember(v => v.level, opt => opt.MapFrom(d => d.level != null ? Convert.ToString(d.level) : "" ))
                //.ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))

            ;
            CreateMap<Service.DTO.ConfigList, ViewModel.ConfigList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Config/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.category + " " + d.text_EN))
                ;
        }
    }

    //var result = from p in list
    //             select new
    //             {
    //                 tabref = "/Config/Edit/" + Convert.ToString(p.ID),
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
    //    if (per != null) return "/Config/Edit/" + Convert.ToString(per.ID);
    //    else return null;
    //}
    //private string _getTabLabel(Lookup per)
    //{
    //    if (per != null) return per.text_EN;
    //    else return null;
    //}
}