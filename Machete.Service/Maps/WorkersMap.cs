using AutoMapper;
using Machete.Domain;

namespace Machete.Service.Maps
{
    public class WorkersMap : Profile
    {
        public WorkersMap()
        {
            CreateMap<Person, Service.DTO.WorkforceList>()
                .ForMember(v => v.firstname1, opt => opt.MapFrom(d => d.firstname1))
                .ForMember(v => v.firstname2, opt => opt.MapFrom(d => d.firstname2))
                .ForMember(v => v.lastname1, opt => opt.MapFrom(d => d.lastname1))
                .ForMember(v => v.lastname2, opt => opt.MapFrom(d => d.lastname2))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.Worker.dwccardnum))
                .ForMember(v => v.memberexpirationdate, opt => opt.MapFrom(d => d.Worker.memberexpirationdate))
                .ForMember(v => v.memberStatusEN, opt => opt.MapFrom(d => d.Worker.memberStatusEN))
                .ForMember(v => v.driverslicense, opt => opt.MapFrom(d => d.Worker.driverslicense))
                .ForMember(v => v.carinsurance, opt => opt.MapFrom(d => d.Worker.carinsurance))
                .ForMember(v => v.insuranceexpiration, opt => opt.MapFrom(d => d.Worker.insuranceexpiration))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.Worker.dateupdated))
                .ForMember(v => v.memberStatusID, opt => opt.MapFrom(d => d.Worker.memberStatusID))
                .ForMember(v => v.skillCodes, opt => opt.MapFrom(d => d.Worker.skillCodes))
                .ForMember(v => v.zipCode, opt => opt.MapFrom(d => d.zipcode))
                ;
        }
    }
}