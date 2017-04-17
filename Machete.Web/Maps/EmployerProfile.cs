﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class EmployerProfile : Profile
    {
        public EmployerProfile()
        {
            CreateMap<Domain.Employer, ViewModel.Employer>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Employer/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.name))
                .ForMember(v => v.active, opt => opt.MapFrom(d => Convert.ToString(d.active)))
                //.ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                //.ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource.ToString()));
            CreateMap<Domain.Employer, Service.DTO.EmployerList>();
            CreateMap<Service.DTO.EmployerList, ViewModel.EmployerList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Employer/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.name))
                .ForMember(v => v.active, opt => opt.MapFrom(d => Convert.ToString(d.active)))
                .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource.ToString()));
        }
    }
}