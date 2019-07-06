using AutoMapper;
using System;
using System.Globalization;

namespace Machete.Web.Maps
{
    public class EmployerProfile : Profile
    {
        public EmployerProfile()
        {
            CreateMap<Domain.Employer, ViewModel.Employer>()
                .ForMember(e => e.WorkOrders, opt => opt.Ignore())
                .ForMember(e => e.def, opt => opt.Ignore())
                .ForMember(e => e.idString, opt => opt.Ignore())
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Employer/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.name))
                .ForMember(v => v.active, opt => opt.MapFrom(d => Convert.ToString(d.active)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated, CultureInfo.InvariantCulture)))
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource.ToString()));
            
            CreateMap<Service.DTO.EmployersList, ViewModel.EmployerList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Employer/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.name))
                .ForMember(v => v.active, opt => opt.MapFrom(d => Convert.ToString(d.active)))
                .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated, CultureInfo.InvariantCulture)))
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource.ToString()));
        }
    }
}