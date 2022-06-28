using AutoMapper;
using Machete.Domain;
using Machete.Api.ViewModel;

namespace Machete.Api.Maps
{
    public class TransportProvidersMap : Profile
    {
        public TransportProvidersMap()
        {
            CreateMap<TransportProvider, TransportProviderVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.text, opt => opt.MapFrom(d => d.text_EN));
            CreateMap<TransportProviderVM, TransportProvider>()
                .ForMember(d => d.text_EN, opt => opt.MapFrom(v => v.text))
                .ForMember(d => d.ID, opt => opt.MapFrom(v => v.id));
            CreateMap<TransportProviderAvailability, TransportProviderAvailabilityVM>();

            CreateMap<TransportProviderAvailabilityVM, TransportProviderAvailability>();
        }
    }
}
