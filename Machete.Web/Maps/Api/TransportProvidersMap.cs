using AutoMapper;
using Machete.Domain;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
{
    public class TransportProvidersMap : Profile
    {
        public TransportProvidersMap()
        {
            CreateMap<TransportProvider, TransportProviderVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.text, opt => opt.MapFrom(d => d.text_EN));
            CreateMap<TransportProviderAvailability, TransportProviderAvailabilityVM>();

            CreateMap<TransportProviderAvailabilityVM, TransportProviderAvailability>();
        }
    }
}
