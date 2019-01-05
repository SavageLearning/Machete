using System;

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
                .ForMember(v => v.fileCount, opt => opt.MapFrom(d => d.JoinEventImages.Count));
            CreateMap<Service.DTO.EventList, ViewModel.EventList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Event/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.dateFrom.ToShortDateString() + " " + getCI() == "ES" ? d.eventTypeES : d.eventTypeEN))
                .ForMember(v => v.type, opt => opt.MapFrom(d => getCI() == "ES" ? d.eventTypeES : d.eventTypeEN ))

                ;
            CreateMap<Domain.JoinEventImage, ViewModel.JoinEventImage>()
                .ForMember(v => v.idString, opt => opt.Ignore());

            CreateMap<Domain.Image, ViewModel.Image>()
                .ForMember(v => v.idString, opt => opt.Ignore());

        }
    }
}
