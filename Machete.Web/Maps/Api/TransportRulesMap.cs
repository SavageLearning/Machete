namespace Machete.Web.Maps.Api
{
    public class TransportRulesMap : MacheteProfile
    {
        public TransportRulesMap()
        {
            CreateMap<Domain.TransportRule, Machete.Web.ViewModel.Api.TransportRule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
            CreateMap<Domain.TransportCostRule, Machete.Web.ViewModel.Api.TransportCostRule>();
        }
    }
    public class TransportProvidersMap : MacheteProfile
    {
        public TransportProvidersMap()
        {
            CreateMap<Domain.TransportProvider, Machete.Web.ViewModel.Api.TransportProvider>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.text, opt => opt.MapFrom(d => d.text_EN));
            CreateMap<Domain.TransportProviderAvailability, Machete.Web.ViewModel.Api.TransportProviderAvailability>();

            CreateMap<Machete.Web.ViewModel.Api.TransportProviderAvailability, Domain.TransportProviderAvailability>();
        }
    }
}