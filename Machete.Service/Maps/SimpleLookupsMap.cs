using AutoMapper;
using Machete.Service.DTO;

namespace Machete.Service.Maps
{
    public class SimpleLookupsMap : Profile
    {
        public SimpleLookupsMap()
        {
            CreateMap<Domain.Lookup, DTO.SimpleLookupList>();
        }
    }
}