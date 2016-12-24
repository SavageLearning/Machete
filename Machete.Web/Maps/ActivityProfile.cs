using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Domain.Activity, ViewModel.Activity>();
            CreateMap<Domain.Activity, Service.DTO.ActivityList>();
            CreateMap<Service.DTO.ActivityList, ViewModel.ActivityList>();
        }
    }
}