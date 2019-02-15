using System.Globalization;
using AutoMapper;
using WorkOrderViewModel = Machete.Web.ViewModel.Api.WorkOrder;

namespace Machete.Web.Maps.Api
{
    public class WorkOrdersMap : Profile
    {
        public WorkOrdersMap()
        {
            CreateMap<Service.DTO.WorkOrdersList, WorkOrderViewModel>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            CreateMap<Domain.WorkOrder, WorkOrderViewModel>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            //CreateMap<Domain.WorkOrder, Service.DTO.WorkOrdersList>()
            //    .ForMember(v => v.workers, opt => opt.MapFrom(d => d.workerRequests ?? new List<Domain.WorkerRequest>()))
            //    ;
            CreateMap<WorkOrderViewModel, Domain.WorkOrder>()
                .ForMember(v => v.datecreated, opt => opt.Ignore())
                .ForMember(v => v.dateupdated, opt => opt.Ignore())
                .ForMember(v => v.createdby, opt => opt.Ignore())
                .ForMember(v => v.updatedby, opt => opt.Ignore())
                .ForMember(v => v.ID, opt => opt.Ignore())
                ;
        }

    }
}