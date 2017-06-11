using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Machete.Web.Maps
{
    public class ActivityProfile : MacheteProfile
    {
        public ActivityProfile()
        {
            CreateMap<Domain.Activity, ViewModel.Activity>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Activity/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.recurring ?
                    "Recurring Event with " + d.teacher :
                    d.nameEN + " with " + d.teacher)) // hardcoded english; skipping for now
                ;
            CreateMap<Domain.Activity, Service.DTO.ActivityList>()
                .ForMember(v => v.count, opt => opt.MapFrom(d => d.Signins.Count()))
                ;
            CreateMap<Service.DTO.ActivityList, ViewModel.ActivityList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Activity/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.recurring ?
                    "Recurring Event with " + d.teacher :
                    d.nameEN + " with " + d.teacher)) // hardcoded english; skipping for now
                .ForMember(v => v.AID, opt => opt.MapFrom(d => d.ID.ToString()))  
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID.ToString()))  
                .ForMember(v => v.name, opt => opt.MapFrom(d => getCI() == "ES" ? d.nameES : d.nameEN))
                .ForMember(v => v.type, opt => opt.MapFrom(d => getCI() == "ES" ? d.typeES : d.typeEN))
                .ForMember(v => v.count, opt => opt.MapFrom(d => d.count.ToString()))
                .ForMember(v => v.dateStart, opt => opt.MapFrom(d => Convert.ToString(d.dateStart)))
                .ForMember(v => v.dateEnd, opt => opt.MapFrom(d => Convert.ToString(d.dateEnd)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                ;
            CreateMap<Domain.Activity, ViewModel.ActivitySchedule>()
                .ForMember(v => v.firstID, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.nameID))
                .ForMember(v => v.type, opt => opt.MapFrom(d => d.typeID))
                .ForMember(v => v.idString, opt => opt.UseValue("activity"))
                ;
        }
    }
}