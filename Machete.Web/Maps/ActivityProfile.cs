using System;
using System.Globalization;

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
                .ForMember(v => v.dateStart, opt => opt.MapFrom(d => Convert.ToString(d.dateStart, CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateEnd, opt => opt.MapFrom(d => Convert.ToString(d.dateEnd, CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated, CultureInfo.InvariantCulture)))
                ;
            CreateMap<Domain.Activity, ViewModel.ActivitySchedule>()
                .ForMember(v => v.firstID, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.nameID))
                .ForMember(v => v.type, opt => opt.MapFrom(d => d.typeID))
                .ForMember(v => v.idString, opt => opt.MapFrom(src => "activity"))
                .ForMember(v => v.sunday, opt => opt.Ignore())
                .ForMember(v => v.monday, opt => opt.Ignore())
                .ForMember(v => v.tuesday, opt => opt.Ignore())
                .ForMember(v => v.wednesday, opt => opt.Ignore())
                .ForMember(v => v.thursday, opt => opt.Ignore())
                .ForMember(v => v.friday, opt => opt.Ignore())
                .ForMember(v => v.saturday, opt => opt.Ignore())
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.stopDate, opt => opt.Ignore())

                ;
        }
    }
}
