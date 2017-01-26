using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Domain.Event, ViewModel.Event>();
            CreateMap<Domain.Event, Service.DTO.EventList>();
            CreateMap<Service.DTO.EventList, ViewModel.EventList>();
        }
    }
}