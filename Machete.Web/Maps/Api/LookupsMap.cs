using AutoMapper;
using LookupViewModel = Machete.Web.ViewModel.Api.Lookup;

namespace Machete.Web.Maps.Api
{
    public class LookupsMap : Profile
    {
        public LookupsMap()
        {
            CreateMap<Domain.Lookup, LookupViewModel>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                ;
        }
    }
}