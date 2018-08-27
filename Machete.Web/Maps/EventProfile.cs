using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class EventProfile : MacheteProfile
    {
        public EventProfile()
        {
            CreateMap<Domain.Event, ViewModel.Event>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Event/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.dateFrom.ToShortDateString() + " " + getCI() == "ES" ? d.eventTypeES : d.eventTypeEN))
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                ;
            CreateMap<Domain.Event, Service.DTO.EventList>()
                .ForMember(v => v.fileCount, opt => opt.MapFrom(d => d.JoinEventImages.Count()));
            CreateMap<Service.DTO.EventList, ViewModel.EventList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Event/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.dateFrom.ToShortDateString() + " " + getCI() == "ES" ? d.eventTypeES : d.eventTypeEN))
                .ForMember(v => v.type, opt => opt.MapFrom(d => getCI() == "ES" ? d.eventTypeES : d.eventTypeEN ))

                ;
            CreateMap<Domain.JoinEventImage, Web.ViewModel.JoinEventImage>()
                .ForMember(v => v.idString, opt => opt.Ignore());

            CreateMap<Domain.Image, Web.ViewModel.Image>()
                .ForMember(v => v.idString, opt => opt.Ignore());

        }
    }

    //var result = from p in list.query
    //             select new
    //             {
    //                 tabref = _getTabRef(p),
    //                 tablabel = _getTabLabel(p, CI.TwoLetterISOLanguageName),
    //                 recordid = p.ID,
    //                 notes = p.notes,
    //                 datefrom = p.dateFrom.ToShortDateString(),
    //                 dateto = p.dateTo == null ? "" : ((DateTime)p.dateTo).ToShortDateString(),
    //                 fileCount = p.JoinEventImages.Count(),
    //                 type = lcache.textByID(p.eventType, CI.TwoLetterISOLanguageName),
    //                 dateupdated = Convert.ToString(p.dateupdated),
    //                 updatedby = p.updatedby
    //             };

    //private string _getTabRef(Event evnt)
    //{
    //    return "/Event/Edit/" + Convert.ToString(evnt.ID);
    //}
    ////
    ////
    //private string _getTabLabel(Event evnt, string locale)
    //{
    //    return evnt.dateFrom.ToShortDateString() + " " + lcache.textByID(evnt.eventType, locale);
    //}
}