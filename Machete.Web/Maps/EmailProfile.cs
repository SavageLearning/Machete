using AutoMapper;
using Machete.Web.ViewModel;

namespace Machete.Web.Maps
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<Domain.Email, EmailView>()
                .ForMember(ev => ev.status, opt => opt.Ignore()) // populated from Default after mapping
                .ForMember(ev => ev.templates, opt => opt.Ignore()) // populated from Default after mapping
                .ForMember(ev => ev.def, opt => opt.Ignore()) // populated from Default after mapping
                .ForMember(ev => ev.woid, opt => opt.Ignore()) // populated from Default after mapping
                .ForMember(ev => ev.idString, opt => opt.Ignore()) // set in class

                .ForMember(ev => ev.statusID, opt => opt.MapFrom(e => e.statusID));
            CreateMap<EmailView, Domain.Email>()
                .ForMember(e => e.WorkOrders, opt => opt.Ignore())
                .ForMember(e => e.RowVersion, opt => opt.Ignore()) // never used

                .ForMember(e => e.updatedby, opt => opt.Ignore())
                .ForMember(e => e.createdby, opt => opt.Ignore())
                .ForMember(e => e.datecreated, opt => opt.Ignore())
                .ForMember(e => e.dateupdated, opt => opt.Ignore());
            CreateMap<Domain.Email, Email>()
                .ForMember(e => e.idString, opt => opt.Ignore());
            CreateMap<Domain.Email, Service.DTO.EmailList>();
            CreateMap<Service.DTO.EmailList, EmailList>();
        }
    }
}