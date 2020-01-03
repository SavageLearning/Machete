using System;
using System.Globalization;
using AutoMapper;
using Machete.Domain;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using WorkOrder = Machete.Domain.WorkOrder;
using static Machete.Web.Helpers.Extensions;

namespace Machete.Web.Maps
{
    public class WorkOrderProfile : Profile
    {
        public WorkOrderProfile()
        {
            CreateMap<WorkOrder, ViewModel.WorkOrderMVC>()
                .ForMember(v => v.tabref,               opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel,             opt => opt.MapFrom(d =>
                                                            WorkOrders.tabprefix +
                                                            Convert.ToString(d.paperOrderNum) +
                                                            " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID,                  opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID,                 opt => opt.MapFrom(d => string.Format("{0,5:D5}", d.paperOrderNum)))
                .ForMember(v => v.dateTimeofWork,       opt => opt.MapFrom(d => d.dateTimeofWork.UtcToClientString()))
                .ForMember(v => v.dateupdatedstring,    opt => opt.MapFrom(d => string.Format("{0:MM/dd/yyyy HH:mm:ss}", d.dateupdated)))
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource))
                .ForMember(v => v.recordid,             opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.transportMethod,      opt => opt.MapFrom(d => getCI() == "ES" ? d.transportMethodES : d.transportMethodEN))
                .ForMember(v => v.def, opt => opt.MapFrom(d => MapperHelpers.Defaults))
                .ForMember(v => v.idString, opt => opt.Ignore())
                .ForMember(v => v.datecreatedstring, opt => opt.Ignore())
                .ForMember(v => v.workAssignments, opt => opt.MapFrom(d => d.workAssignments)).MaxDepth(3)
                //.ForMember(v => v.workAssignments, opt => opt.Condition(d => d.workAssignments.Count > 0)).MaxDepth(3)
                .ForMember(v => v.workerRequestsAAA, opt => opt.MapFrom(d => d.workerRequestsDDD))
                //.ForMember(v => v.workerRequestsAAA, opt => opt.Condition(d => d.workerRequestsDDD.Count > 0))
                ;
            //
            //
            CreateMap<WorkOrdersList, ViewModel.WorkOrdersList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => $"/WorkOrder/Edit/{d.ID.ToString()}"))
                .ForMember(v => v.tablabel, opt =>
                    opt.MapFrom(d =>
                        $"{WorkOrders.tabprefix}{d.paperOrderNum.ToString()} @ {d.workSiteAddress1}"
                    )
                )
                .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID, opt => opt.MapFrom(d => $"{d.paperOrderNum,5:D5}"))
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.UtcToClientString()))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.UtcToClientString()))
                .ForMember(v => v.status, opt => opt.MapFrom(d => getCI() == "ES" ? d.statusES : d.statusEN))
                .ForMember(v => v.transportMethod, opt => opt.MapFrom(d => getCI() == "ES" ? d.transportMethodES : d.transportMethodEN))
                .ForMember(v => v.displayState,      opt => opt.MapFrom(d => d.statusEN))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID.ToString()))
                ;
            CreateMap<WorkerAssignedList, ViewModel.WorkerAssignedList>()
                .ForMember(v => v.skill, opt => opt.MapFrom(d => getCI() == "ES" ? d.skillES : d.skillEN))
                ;
        }
    }
}
