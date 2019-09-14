using AutoMapper;
using TransportProviderAvailabilityViewModel = Machete.Web.ViewModel.Api.TransportProviderAvailability;
using TransportProviderViewModel = Machete.Web.ViewModel.Api.TransportProvider;

namespace Machete.Web.Maps.Api
{
    public class TransportProvidersMap : Profile
    {
        public TransportProvidersMap()
        {
            CreateMap<Domain.TransportProvider, TransportProviderViewModel>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.text, opt => opt.MapFrom(d => d.text_EN));
            CreateMap<Domain.TransportProviderAvailabilities, TransportProviderAvailabilityViewModel>();

            CreateMap<TransportProviderAvailabilityViewModel, Domain.TransportProviderAvailabilities>();
        }
    }
}
