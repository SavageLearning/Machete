using AutoMapper;
using Machete.Domain;
using Machete.Api.ViewModel;

namespace Machete.Api.Maps
{
    public class TransportRulesMap : Profile
    {
        public TransportRulesMap()
        {
            CreateMap<TransportRule, TransportRuleVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.costRules, opt => opt.Ignore());
            CreateMap<TransportCostRule, TransportCostRuleVM>();

            CreateMap<TransportRuleVM, TransportRule>()
                .ForMember(v => v.ID, opt => opt.MapFrom(d => d.id));
            CreateMap<TransportCostRuleVM, TransportCostRule>();
        }
    }
}
