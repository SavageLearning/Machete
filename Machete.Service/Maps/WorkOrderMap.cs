using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using DTO = Machete.Service.DTO;

namespace Machete.Service
{
    public class WorkOrderMap : Profile
    {
        public WorkOrderMap()
        {
            CreateMap<Domain.WorkOrder, Service.DTO.WorkOrdersList>()
                .ForMember(v => v.WAcount,              opt => opt.MapFrom(d => d.workAssignments.Count()))
                .ForMember(v => v.WAUnassignedCount,    opt => opt.MapFrom(d => d.workAssignments.Count(wa => wa.workerAssignedID == null)))
                .ForMember(v => v.WAOrphanedCount,      opt => opt.MapFrom(d => d.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null)))
                .ForMember(v => v.emailSentCount,       opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Domain.Email.iSent || e.statusID == Domain.Email.iReadyToSend).Count()))
                .ForMember(v => v.emailErrorCount,      opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Domain.Email.iTransmitError).Count()))
                .ForMember(v => v.workers,              opt => opt.MapFrom(d => d.workAssignments.Where(wa => wa.workerAssigned != null )))
            ;
            //
            CreateMap<Domain.WorkAssignment, DTO.WorkerAssignedList>()
                .ForMember(v => v.WID, opt => opt.MapFrom(d => d.workerAssigned.dwccardnum))
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.workerAssigned.Person.fullName))
                .ForMember(v => v.wage, opt => opt.MapFrom(d => d.hourlyWage))
                ;       
        } 
    }
}