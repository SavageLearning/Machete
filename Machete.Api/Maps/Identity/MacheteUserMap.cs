using AutoMapper;
using Machete.Service;
using Machete.Service.Identity;
using Machete.Api.ViewModel.Identity;

namespace Machete.Api.Maps.Identity
{
    public class MacheteUserMap : Profile
    {
        public MacheteUserMap()
        {
            CreateMap<RegistrationViewModel, MacheteUser>()
                .ForMember(mu => mu.UserName, map => map.MapFrom(vm => vm.FirstName + "." + vm.LastName))
                .ForMember(mu => mu.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(mu => mu.LastName, map => map.MapFrom(vm => vm.LastName))
                .ForMember(mu => mu.Email, map => map.MapFrom(vm => vm.Email))
                ;
        }
    }
}
