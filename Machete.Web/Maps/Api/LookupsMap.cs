using System.Linq;

namespace Machete.Web.Maps.Api
{
    public class LookupsMap : MacheteProfile
    {
        public LookupsMap()
        {
            CreateMap<Domain.Lookup, Machete.Web.ViewModel.Api.Lookup>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                ;
        }
    }
}