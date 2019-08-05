using System;
using AutoMapper;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;

namespace Machete.Web.Maps
{
    public static class MvcMapperConfiguration
    {
        public static void ConfigureMvc(this IMapperConfigurationExpression config)
        {
            config.CreateMap<SearchOptions, Data.DTO.SearchOptions>()
                .ForMember(v => v.beginDate, opt => opt.MapFrom(d => d.beginDate ?? new DateTime(1753, 1, 1)))
                .ForMember(v => v.endDate, opt => opt.MapFrom(d => d.endDate ?? DateTime.MaxValue))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.dwccardnum ?? 0));
            config.CreateMap<jQueryDataTableParam, viewOptions>()
                .ForMember(vo => vo.CI, opt => opt.Ignore())
                .ForMember(vo => vo.employerGuid, opt => opt.Ignore()) // API-only option
                .ForMember(vo => vo.personID, opt => opt.MapFrom(dt => dt.personID ?? 0))
                .ForMember(vo => vo.emailID,
                    opt => opt.MapFrom(dt =>
                        string.IsNullOrEmpty(dt.searchColName("emailID"))
                            ? null
                            : (int?) Convert.ToInt32(dt.searchColName("emailID"))))
                .ForMember(vo => vo.onlineSource,
                    opt => opt.MapFrom(dt =>
                        string.IsNullOrEmpty(dt.searchColName("onlineSource"))
                            ? null
                            : dt.searchColName("onlineSource")))
                .ForMember(vo => vo.status,
                    opt => opt.MapFrom(dt =>
                        string.IsNullOrEmpty(dt.searchColName("status"))
                            ? null
                            : (int?) Convert.ToInt32(dt.searchColName("status"))))
                .ForMember(vo => vo.EmployerID,
                    opt => opt.MapFrom(dt =>
                        string.IsNullOrEmpty(dt.searchColName("EID"))
                            ? null
                            : (int?) Convert.ToInt32(dt.searchColName("EID"))))
                .ForMember(vo => vo.sortColName, opt => opt.MapFrom(dt => dt.sortColName()))
                .ForMember(vo => vo.dwccardnum, opt => opt.MapFrom(dt => Convert.ToInt32(dt.dwccardnum)))
                .ForMember(vo => vo.woid, opt => opt.MapFrom(dt => Convert.ToInt32(dt.searchColName("WOID"))))
                .ForMember(vo => vo.date, opt => opt.MapFrom(dt => MapperHelpers.DataTablesToUtc(dt.todaysdate)))
                .ForMember(vo => vo.displayStart, opt => opt.MapFrom(dt => dt.iDisplayStart))
                .ForMember(vo => vo.displayLength, opt => opt.MapFrom(dt => dt.iDisplayLength))
                .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 != "asc"));

            config.AddProfile<EmailProfile>();
            config.AddProfile<EmployerProfile>();
            config.AddProfile<WorkOrderProfile>();
            config.AddProfile<WorkAssignmentProfile>();
            config.AddProfile<WorkerSigninProfile>();
            config.AddProfile<PersonProfile>();
            config.AddProfile<WorkerProfile>();
            config.AddProfile<ActivityProfile>();
            config.AddProfile<ActivitySigninProfile>();
            config.AddProfile<EventProfile>();
            config.AddProfile<LookupProfile>();
            config.AddProfile<WorkOrderMap>();
            config.AddProfile<EmployersMap>();
            config.AddProfile<WorkerRequestProfile>();
        }
    }
}
