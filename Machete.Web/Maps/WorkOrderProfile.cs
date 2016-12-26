using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class WorkOrderProfile : Profile
    {
        public WorkOrderProfile()
        {
            CreateMap<Domain.WorkOrder, ViewModel.WorkOrder>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                    Machete.Web.Resources.WorkOrders.tabprefix +
                    Convert.ToString(d.paperOrderNum.HasValue ? d.paperOrderNum : d.ID) +
                    " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID, opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum.HasValue ? d.paperOrderNum : d.ID)))
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork)))
                .ForMember(v => v.dateupdatedstring, opt => opt.MapFrom(d => System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", d.dateupdated)))
                .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource ? Shared.True : Shared.False))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)));
            CreateMap<Domain.WorkOrder, Service.DTO.WorkOrderList>()
                .ForMember(v => v.WAcount, opt => opt.MapFrom(d => d.workAssignments.Count()))
                .ForMember(v => v.emailSentCount, opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Domain.Email.iSent || e.statusID == Domain.Email.iReadyToSend).Count()))
                .ForMember(v => v.emailErrorCount, opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Domain.Email.iTransmitError).Count()))
            ;
            CreateMap<Service.DTO.WorkOrderList, ViewModel.WorkOrderList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                    Machete.Web.Resources.WorkOrders.tabprefix +
                    Convert.ToString(d.paperOrderNum.HasValue ? d.paperOrderNum : d.ID) +
                    " @ " + d.workSiteAddress1))
                .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                .ForMember(v => v.WOID, opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum.HasValue ? d.paperOrderNum : d.ID)))
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                ;

        }
    }
}