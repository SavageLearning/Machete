using System;
using System.Threading;
using AutoMapper;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;

namespace Machete.Web.Maps
{
    public class MapperConfigurationFactory : Profile
    {
        private readonly MapperConfiguration _config;

        public MapperConfiguration Config => _config;

        public MapperConfigurationFactory()
        {
            _config = new MapperConfiguration(c => {
                c.CreateMap<SearchOptions, Data.DTO.SearchOptions>()
                 .ForMember(v => v.beginDate, opt => opt.MapFrom(d => d.beginDate ?? new DateTime(1753, 1, 1)))
                 .ForMember(v => v.endDate, opt => opt.MapFrom(d => d.endDate ?? DateTime.MaxValue))
                 .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.dwccardnum ?? 0));
                c.CreateMap<jQueryDataTableParam, viewOptions>()
                 .ForMember(vo => vo.CI, opt => opt.Ignore())
                 .ForMember(vo => vo.employerGuid, opt => opt.Ignore()) // API-only option
                 .ForMember(vo => vo.authenticated, opt => opt.Ignore())
                 .ForMember(vo => vo.personID, opt => opt.MapFrom(dt => dt.personID ?? 0))
                 .ForMember(vo => vo.emailID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("emailID")) ? null : (int?)Convert.ToInt32(dt.searchColName("emailID"))))
                 .ForMember(vo => vo.onlineSource, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("onlineSource")) ? null : dt.searchColName("onlineSource")))
                 .ForMember(vo => vo.status, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("status")) ? null : (int?)Convert.ToInt32(dt.searchColName("status"))))
                 .ForMember(vo => vo.EmployerID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("EID")) ? null : (int?)Convert.ToInt32(dt.searchColName("EID"))))
                 .ForMember(vo => vo.sortColName, opt => opt.MapFrom(dt => dt.sortColName()))
                 .ForMember(vo => vo.dwccardnum, opt => opt.MapFrom(dt => Convert.ToInt32(dt.dwccardnum)))
                 .ForMember(vo => vo.woid, opt => opt.MapFrom(dt => Convert.ToInt32(dt.searchColName("WOID"))))
                 .ForMember(vo => vo.date, opt => opt.MapFrom(dt => dt.todaysdate == null ? null : (DateTime?)DateTime.Parse(dt.todaysdate)))
                 .ForMember(vo => vo.displayStart, opt => opt.MapFrom(dt => dt.iDisplayStart))
                 .ForMember(vo => vo.displayLength, opt => opt.MapFrom(dt => dt.iDisplayLength))
                 .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 != "asc"));

                c.AddProfile<EmailProfile>();
                c.AddProfile<EmployerProfile>();
                c.AddProfile<WorkOrderProfile>();
                c.AddProfile<WorkAssignmentProfile>();
                c.AddProfile<WorkerSigninProfile>();
                c.AddProfile<PersonProfile>();
                c.AddProfile<WorkerProfile>();
                c.AddProfile<ActivityProfile>();
                c.AddProfile<ActivitySigninProfile>();
                c.AddProfile<EventProfile>();
                c.AddProfile<LookupProfile>();
                c.AddProfile<WorkOrderMap>();
                c.AddProfile<EmployersMap>();
                c.AddProfile<WorkerRequestProfile>();
            });
        }
    }
    public class MacheteProfile : Profile
    {
        protected static string getCI()
        {
            return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();
        }
    }
}
