using AutoMapper;
using TransportRuleViewModel = Machete.Web.ViewModel.Api.TransportRule;
using TransportCostRuleViewModel = Machete.Web.ViewModel.Api.TransportCostRule;

namespace Machete.Web.Maps.Api
{
    public class TransportRulesMap : Profile
    {
        public TransportRulesMap()
        {
            CreateMap<Domain.TransportRule, Machete.Web.ViewModel.Api.TransportRule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));
                //.ForMember(v => v.costRules, opt => opt.Ignore()); // bug involving lazy-loading, which disappeared???
            CreateMap<Domain.TransportCostRule, TransportCostRuleViewModel>();
        }
    }
}
