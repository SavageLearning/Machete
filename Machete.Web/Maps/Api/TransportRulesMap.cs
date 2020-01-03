using AutoMapper;
using Machete.Domain;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
{
    public class TransportRulesMap : Profile
    {
        public TransportRulesMap()
        {
            CreateMap<TransportRule, TransportRuleVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.costRules, opt => opt.Ignore());
            CreateMap<TransportCostRule, TransportCostRuleVM>();
        }
    }
}
