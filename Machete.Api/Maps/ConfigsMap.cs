using AutoMapper;
using Machete.Domain;
using Machete.Api.Controllers;
using Machete.Api.ViewModel;

namespace Machete.Api.Maps
{
    public class ConfigsMap : Profile
    {
        public ConfigsMap()
        {
            CreateMap<Config, ConfigVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                ;
            CreateMap<ConfigVM, Config>();
        }
    }
}