using System;
using System.Globalization;
using Machete.Domain;
using Machete.Service.DTO;
using Machete.Web.Resources;
using WorkOrder = Machete.Domain.WorkOrder;

namespace Machete.Web.Maps
{
    public class WorkOrderProfile : MacheteProfile
    {
        public WorkOrderProfile()
        {

            CreateMap<WorkOrder, ViewModel.WorkOrder>()
                .ForMember(v => v.tabref,               opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel,             opt => opt.MapFrom(d =>
                                                            WorkOrders.tabprefix +
                                                            Convert.ToString(d.paperOrderNum) +
                                                            " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID,                  opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID,                 opt => opt.MapFrom(d => string.Format("{0,5:D5}", d.paperOrderNum)))
                .ForMember(v => v.dateTimeofWork,       opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork, CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdatedstring,    opt => opt.MapFrom(d => string.Format("{0:MM/dd/yyyy HH:mm:ss}", d.dateupdated)))
                .ForMember(v => v.onlineSource,         opt => opt.MapFrom(d => d.onlineSource ? Shared.True : Shared.False))
                .ForMember(v => v.recordid,             opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.transportMethod,      opt => opt.MapFrom(d => getCI() == "ES" ? d.transportMethodES : d.transportMethodEN))
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                .ForMember(v => v.datecreatedstring, opt => opt.Ignore())
                .ForMember(v => v.workAssignments, opt => opt.MapFrom(d => d.workAssignments)).MaxDepth(3)  
                ;
            //
            //
            CreateMap<WorkOrdersList, ViewModel.WorkOrdersList>()
                .ForMember(v => v.tabref,            opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel,          opt => opt.MapFrom(d =>
                                                            WorkOrders.tabprefix +
                                                            Convert.ToString(d.paperOrderNum) +
                                                            " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID,               opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID,              opt => opt.MapFrom(d => string.Format("{0,5:D5}", d.paperOrderNum)))
                .ForMember(v => v.dateTimeofWork,    opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork, CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated,       opt => opt.MapFrom(d => Convert.ToString(d.dateupdated, CultureInfo.InvariantCulture)))
                .ForMember(v => v.status,            opt => opt.MapFrom(d => getCI() == "ES" ? d.statusES : d.statusEN))
                .ForMember(v => v.transportMethod, opt => opt.MapFrom(d => getCI() == "ES" ? d.transportMethodES : d.transportMethodEN))
                .ForMember(v => v.displayState,      opt => opt.MapFrom(d => computeOrderStatus(d)))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID.ToString()))
                ;
            CreateMap<WorkerAssignedList, ViewModel.WorkerAssignedList>()
                .ForMember(v => v.skill, opt => opt.MapFrom(d => getCI() == "ES" ? d.skillES : d.skillEN))
                ;
        }
        public string computeOrderStatus(WorkOrdersList d)
        {
            if (d.statusID == WorkOrder.iActive) return LOrderStatus.Active;
            if (d.statusID == WorkOrder.iCancelled) return LOrderStatus.Cancelled;
            if (d.statusID == WorkOrder.iExpired) return LOrderStatus.Expired;
            if (d.statusID == WorkOrder.iPending) return LOrderStatus.Pending;
            if (d.statusID == WorkOrder.iCompleted)
            {
                // if wo is completed, but 1 (or more) wa aren't assigned - the wo is still unassigned
                if (d.WAUnassignedCount > 0) return LOrderStatus.Unassigned;
                // if wo is completed, but 1 (or more) assigned worker(s) never signed in, then the wo has been orphaned
                if (d.WAUnassignedCount > 0) return LOrderStatus.Orphaned;
                return LOrderStatus.Completed;
            }

            return "unknown";
        }

    }
}
