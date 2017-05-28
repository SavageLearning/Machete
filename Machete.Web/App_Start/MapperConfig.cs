using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Machete.Web.Maps;

namespace Machete.Web
{
    public class MapperConfig
    {
        MapperConfiguration cfg;
        Mapper map;
        public MapperConfig()
        {
            cfg = new MapperConfiguration(c => {
                c.CreateMap<jQueryDataTableParam, viewOptions>()
                    .ForMember(vo => vo.CI, opt => opt.Ignore())
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
                    .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 == "asc" ? false : true));

                #region WoCombined
                // Splitting Combined into parts
                c.CreateMap<EmployerWoCombined, Domain.Employer>();
                // Splitting Combined into parts
                c.CreateMap<EmployerWoCombined, Domain.WorkOrder>();
                // re-combineing to view model object
                c.CreateMap<Domain.Employer, EmployerWoCombined>();
                // re-combineing to view model object
                c.CreateMap<Domain.WorkOrder, EmployerWoCombined>()
                    .ForMember(wo => wo.wo_city, opt => opt.MapFrom(e => e.city))
                    .ForMember(wo => wo.wo_state, opt => opt.MapFrom(e => e.state))
                    .ForMember(wo => wo.wo_phone, opt => opt.MapFrom(e => e.phone))
                    .ForMember(wo => wo.wo_zipcode, opt => opt.MapFrom(e => e.zipcode));
                #endregion

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
                c.AddProfile<ConfigProfile>();
                c.AddProfile<ReportDefinitionsMap>();
                c.AddProfile<EmployersMap>();
                c.AddProfile<WorkOrdersMap>();
                c.AddProfile<WorkAssignmentsMap>();
            });
        }

        public IMapper getMapper()
        {
            return map ?? (map = new Mapper(cfg));
        }
    }
}