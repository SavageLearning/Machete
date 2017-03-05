using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using DTO = Machete.Service.DTO;

namespace Machete.Web.Maps
{
    public class WorkOrderProfile : MacheteProfile
    {
        public WorkOrderProfile()
        {
            CreateMap<Domain.WorkOrder, ViewModel.WorkOrder>()
                .ForMember(v => v.tabref,               opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel,             opt => opt.MapFrom(d =>
                                                            Machete.Web.Resources.WorkOrders.tabprefix +
                                                            Convert.ToString(d.paperOrderNum) +
                                                            " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID,                  opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID,                 opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum)))
                .ForMember(v => v.dateTimeofWork,       opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork)))
                .ForMember(v => v.dateupdatedstring,    opt => opt.MapFrom(d => System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", d.dateupdated)))
                .ForMember(v => v.onlineSource,         opt => opt.MapFrom(d => d.onlineSource ? Shared.True : Shared.False))
                .ForMember(v => v.recordid,             opt => opt.MapFrom(d => Convert.ToString(d.ID)));
            //
            //
            CreateMap<Domain.WorkOrder, Service.DTO.WorkOrderList>()
                .ForMember(v => v.WAcount,              opt => opt.MapFrom(d => d.workAssignments.Where(wa => wa.workerAssigned != null).Count()))
                .ForMember(v => v.WAUnassignedCount,    opt => opt.MapFrom(d => d.workAssignments.Count(wa => wa.workerAssignedID == null)))
                .ForMember(v => v.WAOrphanedCount,      opt => opt.MapFrom(d => d.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null)))
                .ForMember(v => v.emailSentCount,       opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Domain.Email.iSent || e.statusID == Domain.Email.iReadyToSend).Count()))
                .ForMember(v => v.emailErrorCount,      opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Domain.Email.iTransmitError).Count()))
                .ForMember(v => v.workers,              opt => opt.MapFrom(d => d.workAssignments.Where(wa => wa.workerAssigned != null )))
            ;
            //
            //
            CreateMap<Service.DTO.WorkOrderList, ViewModel.WorkOrderList>()
                .ForMember(v => v.tabref,            opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel,          opt => opt.MapFrom(d =>
                                                            Machete.Web.Resources.WorkOrders.tabprefix +
                                                            Convert.ToString(d.paperOrderNum) +
                                                            " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID,               opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID,              opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum)))
                .ForMember(v => v.dateTimeofWork,    opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork)))
                .ForMember(v => v.dateupdated,       opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                .ForMember(v => v.status,            opt => opt.MapFrom(d => getCI() == "ES" ? d.statusES : d.statusEN))
                .ForMember(v => v.transportMethod, opt => opt.MapFrom(d => getCI() == "ES" ? d.transportMethodES : d.transportMethodEN))
                .ForMember(v => v.displayState,      opt => opt.MapFrom(d => computeOrderStatus(d)))
                ;

            CreateMap<Domain.WorkAssignment, DTO.WorkerAssignedList>()
                .ForMember(v => v.WID, opt => opt.MapFrom(d => d.workerAssigned.dwccardnum))
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.workerAssigned.Person.fullName))
                ;

            CreateMap<DTO.WorkerAssignedList, ViewModel.WorkerAssignedList>()
                .ForMember(v => v.skill, opt => opt.MapFrom(d => getCI() == "ES" ? d.skillES : d.skillEN))
                ;
        }
        public string computeOrderStatus(Service.DTO.WorkOrderList d)
        {
            if (d.statusID == Domain.WorkOrder.iActive) return Domain.LOrderStatus.Active;
            if (d.statusID == Domain.WorkOrder.iCancelled) return Domain.LOrderStatus.Cancelled;
            if (d.statusID == Domain.WorkOrder.iExpired) return Domain.LOrderStatus.Expired;
            if (d.statusID == Domain.WorkOrder.iPending) return Domain.LOrderStatus.Pending;
            if (d.statusID == Domain.WorkOrder.iCompleted)
            {
                // if wo is completed, but 1 (or more) wa aren't assigned - the wo is still unassigned
                if (d.WAUnassignedCount > 0) return Domain.LOrderStatus.Unassigned;
                // if wo is completed, but 1 (or more) assigned worker(s) never signed in, then the wo has been orphaned
                if (d.WAUnassignedCount > 0) return Domain.LOrderStatus.Orphaned;
                return Domain.LOrderStatus.Completed;
            }
            else
                return "unknown";
        }
        //tabref = wo.getTabRef(),
        //tablabel = Machete.Web.Resources.WorkOrders.tabprefix + wo.getTabLabel(),
        //EID = Convert.ToString(wo.EmployerID), // Note: Employer ID appears to be unused
        //WOID = System.String.Format("{0,5:D5}", wo.paperOrderNum), // TODO: investigate why PaperOrderNum is used - shouldn't this be the Order # from the WO Table?
        //dateTimeofWork = wo.dateTimeofWork.ToString(),
        //TODO: status = lcache.textByID(wo.status, CI.TwoLetterISOLanguageName),
        //TODO: WAcount = wo.workAssignments.Count(a => a.workOrderID == ID).ToString(),
        //contactName = wo.contactName,
        //workSiteAddress1 = wo.workSiteAddress1,
        //zipcode = wo.zipcode,
        //dateupdated = System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", wo.dateupdated), // Note: Date Updated appears to be unused
        //updatedby = wo.updatedby,
        //TODO: transportMethod = lcache.textByID(wo.transportMethodID, CI.TwoLetterISOLanguageName), // Used in Hirer a lot
        //TODO: displayState = _getDisplayState(wo), // Used to determine row color on /WorkOrder/Index.cshtml
        //onlineSource = wo.onlineSource ? Shared.True : Shared.False,
        //TODO: emailSentCount = wo.Emails.Where(e => e.statusID == Email.iSent || e.statusID == Email.iReadyToSend).Count(),
        //TODO: emailErrorCount = wo.Emails.Where(e => e.statusID == Email.iTransmitError).Count(),
        //recordid = wo.ID.ToString(), 
        //TODO: workers = showWorkers ? // Used in Datatables details
        //        from w in wo.workAssignments
        //        select new
        //        {
        //            WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
        //            name = w.workerAssigned != null ? w.workerAssigned.Person.fullName() : null,
        //            skill = lcache.textByID(w.skillID, CI.TwoLetterISOLanguageName),
        //            hours = w.hours,
        //            wage = w.hourlyWage
        //        } : null

        //private string _getDisplayState(WorkOrder wo)
        //{
        //    string status = lcache.textByID(wo.status, "en");
        //    if (wo.status == WorkOrder.iCompleted)
        //    {
        //        // If WO is completed, but 1 (or more) WA aren't assigned - the WO is still Unassigned
        //        if (wo.workAssignments.Count(wa => wa.workerAssignedID == null) > 0) return "Unassigned";
        //        // If WO is completed, but 1 (or more) Assigned Worker(s) never signed in, then the WO has been Orphaned
        //        if (wo.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null) > 0) return "Orphaned";
        //    }
        //    return status;
        //}
        //public string getPseudoWOID()
        //{
        //    return this.paperOrderNum.HasValue ? System.String.Format("{0,5:D5}", this.paperOrderNum) : System.String.Format("{0,5:D5}", this.ID);
        //}
    }
}