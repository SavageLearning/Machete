using AutoMapper;
using System.Linq;
using Machete.Domain;

namespace Machete.Service
{
    public class WorkOrderMap : Profile
    {
        public WorkOrderMap()
        {
            CreateMap<Domain.WorkOrder, Service.DTO.WorkOrdersList>()
                .ForMember(v => v.WAcount,              opt => opt.MapFrom(d => d.workAssignments.Count()))
                // .ForMember(v => v.WAUnassignedCount,    opt => opt.MapFrom(d => d.workAssignments.Count(wa => wa.workerAssignedID == null)))
                // .ForMember(v => v.WAOrphanedCount,      opt => opt.MapFrom(d => d.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null)))
                .ForMember(v => v.emailSentCount,       opt => opt.MapFrom(d => d.EmailWorkOrders.Count(ewos => ewos.Email.statusID == Email.iSent || ewos.Email.statusID == Email.iReadyToSend)))
                .ForMember(v => v.emailErrorCount,      opt => opt.MapFrom(d => d.EmailWorkOrders.Count(ewos => ewos.Email.statusID == Email.iTransmitError)))
                .ForMember(v => v.workers,              opt => opt.MapFrom(d => d.workAssignments.Where(wa => wa.workerAssignedDDD != null)))
            ;
            //
            CreateMap<Domain.WorkAssignment, DTO.WorkerAssignedList>()
                .ForMember(v => v.WID, opt => opt.MapFrom(d => d.workerAssignedDDD.dwccardnum))
                .ForMember(v => v.name, opt => opt.MapFrom(d => d.workerAssignedDDD.Person.fullName))
                .ForMember(v => v.wage, opt => opt.MapFrom(d => d.hourlyWage))
                ;       
        } 
    }
}
