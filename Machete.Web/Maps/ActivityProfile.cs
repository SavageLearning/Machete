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
            CreateMap<Domain.Activity, ViewModel.Activity>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => d.recurring ?
                    "/Activity/CreateMany/" + Convert.ToString(d.ID) :
                    "/Activity/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.recurring ?
                    "Recurring Event with " + d.teacher :
                    d.nameEN + " with " + d.teacher)) // hardcoded english; skipping for now
                    

                ;
            CreateMap<Domain.Activity, Service.DTO.ActivityList>()
                .ForMember(v => v.count, opt => opt.MapFrom(d => d.Signins.Count()))
                ;
            CreateMap<Service.DTO.ActivityList, ViewModel.ActivityList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => d.recurring ?
                    "/Activity/CreateMany/" + Convert.ToString(d.ID) :
                    "/Activity/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.recurring ?
                    "Recurring Event with " + d.teacher :
                    d.nameEN + " with " + d.teacher)) // hardcoded english; skipping for now
                .ForMember(v => v.AID, opt => opt.MapFrom(d => d.ID.ToString()))  
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID.ToString()))  
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.nameEN))
                .ForMember(v => v.type, opt => opt.MapFrom(d => d.typeEN))
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
        //private object dtResponse( Domain.Activity p)
        //{
        //    return new
        //    {
        //        tabref = EditTabRef(p),
        //        tablabel = EditTabLabel(p),
        //        name = lcache.textByID(p.name, CI.TwoLetterISOLanguageName),
        //        type = lcache.textByID(p.type, CI.TwoLetterISOLanguageName),
        //        count = p.Signins.Count(),
        //        teacher = p.teacher,
        //        dateStart = p.dateStart.ToString(),
        //        dateEnd = p.dateEnd.ToString(),
        //        AID = Convert.ToString(p.ID),
        //        recordid = Convert.ToString(p.ID),
        //        dateupdated = Convert.ToString(p.dateupdated),
        //        updatedby = p.updatedby
        //    };
        //}
        //private string EditTabRef(Domain.Activity act)
        //{
        //    if (act == null) return null;
        //    return "/Activity/Edit/" + Convert.ToString(act.ID);
        //}
        //private string EditTabLabel(Domain.Activity act)
        //{
        //    if (act == null) return null;
        //    return lcache.textByID(act.name, CI.TwoLetterISOLanguageName) + " with " +
        //            act.teacher;
        //}


        //private string CreateManyTabRef(Domain.Activity act)
        //{
        //    if (act == null) return null;
        //    return "/Activity/CreateMany/" + Convert.ToString(act.ID);
        //}
        //private string CreateManyTabLabel(Domain.Activity act)
        //{
        //    if (act == null) return null;
        //    return "Recurring Event with " + act.teacher;
        //}
    }
}