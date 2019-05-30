using AutoMapper;
using Machete.Data;
using Machete.Data.Identity;
using Machete.Web.ViewModel.Api.Identity;

namespace Machete.Web.Maps.Api.Identity {
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
