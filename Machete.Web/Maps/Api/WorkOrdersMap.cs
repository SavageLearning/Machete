using System;
using System.Globalization;
using AutoMapper;
using Machete.Domain;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Maps.Api
{
    public class WorkOrdersMap : Profile
    {
        public WorkOrdersMap()
        {
            CreateMap<Service.DTO.WorkOrdersList, WorkOrderVM>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            CreateMap<WorkOrder, WorkOrderVM>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            //CreateMap<WorkOrder, Service.DTO.WorkOrdersList>()
            //    .ForMember(v => v.workers, opt => opt.MapFrom(d => d.workerRequests ?? new List<WorkerRequest>()))
            //    ;
            CreateMap<WorkOrderVM, WorkOrder>()
                // Don't convert Date; JavaScript dates are already UTC + timezone offset. NEW api should get/return UTC.
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork)) //.ToUtcDateTime()))
                .ForMember(v => v.datecreated, opt => opt.Ignore())
                .ForMember(v => v.dateupdated, opt => opt.Ignore())
                .ForMember(v => v.createdby, opt => opt.Ignore())
                .ForMember(v => v.updatedby, opt => opt.Ignore())
                .ForMember(v => v.ID, opt => opt.Ignore())
                ;
        }

    }
}
