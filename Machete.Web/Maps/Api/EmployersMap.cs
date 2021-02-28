using AutoMapper;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
{
    public class EmployersMap : Profile
    {
        public EmployersMap()
        {
            CreateMap<Service.DTO.EmployersList, EmployerVM>();
            CreateMap<Domain.Employer, EmployerVM>();
            CreateMap<EmployerVM, Domain.Employer>()
                .ForMember(v => v.datecreated, opt => opt.Ignore())
                .ForMember(v => v.dateupdated, opt => opt.Ignore())
                .ForMember(v => v.createdby, opt => opt.Ignore())
                .ForMember(v => v.updatedby, opt => opt.Ignore())
                .ForMember(v => v.ID, opt => opt.Ignore())
                .ForMember(v => v.onlineSigninID, opt => opt.Ignore())
                .ForMember(v => v.licenseplate, opt => opt.Ignore())
                .ForMember(v => v.driverslicense, opt => opt.Ignore())
                .ForMember(v => v.isOnlineProfileComplete, opt => opt.Ignore())
                .ForMember(v => v.email, opt => opt.Ignore())
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource))
                ;
        }

    }
}