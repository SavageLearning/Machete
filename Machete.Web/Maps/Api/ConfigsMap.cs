using AutoMapper;
using Machete.Domain;
using Machete.Web.Controllers.Api;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
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