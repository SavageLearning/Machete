using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.Maps
{
    public class TransportRulesMap : MacheteProfile
    {
        public TransportRulesMap()
        {
            CreateMap<Domain.TransportRule, ViewModel.TransportRule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
            CreateMap<Domain.TransportCostRule, ViewModel.TransportCostRule>();
        }
    }
    public class TransportProvidersMap : MacheteProfile
    {
        public TransportProvidersMap()
        {
            CreateMap<Domain.TransportProvider, ViewModel.TransportProvider>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.text, opt => opt.MapFrom(d => d.text_EN));
            CreateMap<Domain.TransportProviderAvailability, ViewModel.TransportProviderAvailability>();

            CreateMap<ViewModel.TransportProviderAvailability, Domain.TransportProviderAvailability>();
        }
    }
}