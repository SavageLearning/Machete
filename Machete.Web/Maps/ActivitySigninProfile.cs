using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class ActivitySigninProfile : Profile
    {
        public ActivitySigninProfile()
        {
            CreateMap<Domain.ActivitySignin, ViewModel.ActivitySignin>();
            CreateMap<Domain.ActivitySignin, Service.DTO.ActivitySigninList>();
            CreateMap<Service.DTO.ActivitySigninList, ViewModel.ActivitySigninList>();
        }
    }
}