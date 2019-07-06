using System;
using System.Globalization;
using AutoMapper;
using Machete.Web.Helpers;

namespace Machete.Web.Maps.Api
{
    public class WorkOrdersMap : Profile
    {
        public WorkOrdersMap()
        {
            CreateMap<Service.DTO.WorkOrdersList, ViewModel.Api.WorkOrder>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.UtcToClientString("o")))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            CreateMap<Domain.WorkOrder, ViewModel.Api.WorkOrder>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.UtcToClientString("o")))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            //CreateMap<Domain.WorkOrder, Service.DTO.WorkOrdersList>()
            //    .ForMember(v => v.workers, opt => opt.MapFrom(d => d.workerRequests ?? new List<Domain.WorkerRequest>()))
            //    ;
            CreateMap<ViewModel.Api.WorkOrder, Domain.WorkOrder>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToUtcDateTime()))
                .ForMember(v => v.datecreated, opt => opt.Ignore())
                .ForMember(v => v.dateupdated, opt => opt.Ignore())
                .ForMember(v => v.createdby, opt => opt.Ignore())
                .ForMember(v => v.updatedby, opt => opt.Ignore())
                .ForMember(v => v.ID, opt => opt.Ignore())
                ;
        }

    }
}
