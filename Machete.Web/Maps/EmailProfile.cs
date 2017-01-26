using AutoMapper;
using Machete.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<Domain.Email, EmailView>()
                .ForMember(ev => ev.statusID, opt => opt.MapFrom(e => e.statusID));
            CreateMap<EmailView, Domain.Email>()
                .ForMember(e => e.updatedby, opt => opt.Ignore())
                .ForMember(e => e.createdby, opt => opt.Ignore())
                .ForMember(e => e.datecreated, opt => opt.Ignore())
                .ForMember(e => e.dateupdated, opt => opt.Ignore());
            CreateMap<Domain.Email, ViewModel.Email>();
            CreateMap<Domain.Email, Service.DTO.EmailList>();
            CreateMap<Service.DTO.EmailList, ViewModel.EmailList>();
        }
    }
}