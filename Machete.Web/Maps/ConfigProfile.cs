using AutoMapper;
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
            CreateMap<Domain.Lookup, ViewModel.Config>();
            CreateMap<Domain.Lookup, Service.DTO.ConfigList>();
            CreateMap<Service.DTO.ConfigList, ViewModel.ConfigList>();
        }
    }
}