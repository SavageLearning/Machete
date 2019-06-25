using System;
using System.Globalization;
using AutoMapper;
using Machete.Web.Helpers;
using static Machete.Web.Helpers.Extensions;

namespace Machete.Web.Maps
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Domain.Activity, ViewModel.Activity>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d =>
                    d.recurring ?  "/Activity/CreateMany/" + Convert.ToString(d.ID) : "/Activity/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                    d.recurring ? "Recurring Event with " + d.teacher : d.nameEN + " with " + d.teacher))
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                ;
            CreateMap<Domain.Activity, Service.DTO.ActivityList>()
                .ForMember(v => v.count, opt => opt.MapFrom(d => d.Signins.Count))
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
                .ForMember(v => v.dateStart, opt => opt.MapFrom(d => d.dateStart.UtcToClientString()))
                .ForMember(v => v.dateEnd, opt => opt.MapFrom(d => d.dateEnd.UtcToClientString()))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated, CultureInfo.InvariantCulture))) // not used?
                ;
            CreateMap<Domain.Activity, ViewModel.ActivitySchedule>()
                .ForMember(v => v.firstID, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.nameID))
                .ForMember(v => v.type, opt => opt.MapFrom(d => d.typeID))
                .ForMember(v => v.idString, opt => opt.MapFrom(unused => "activity"))
                .ForMember(v => v.dateStart, opt => opt.MapFrom(activity => activity.dateStart.UtcToClient()))
                .ForMember(v => v.dateEnd,opt => opt.MapFrom(activity => activity.dateEnd.UtcToClient()))
                .ForMember(v => v.def, opt => opt.MapFrom(unused => MapperHelpers.Defaults))
                .ForMember(v => v.sunday, opt => opt.Ignore())
                .ForMember(v => v.monday, opt => opt.Ignore())
                .ForMember(v => v.tuesday, opt => opt.Ignore())
                .ForMember(v => v.wednesday, opt => opt.Ignore())
                .ForMember(v => v.thursday, opt => opt.Ignore())
                .ForMember(v => v.friday, opt => opt.Ignore())
                .ForMember(v => v.saturday, opt => opt.Ignore())
                .ForMember(v => v.stopDate, opt => opt.Ignore())
                ;
        }
    }
}
