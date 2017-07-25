using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Api.ViewModel;
using Newtonsoft.Json;

namespace Machete.Api.Maps
{
    public class WorkOrdersMap : MacheteProfile
    {
        public WorkOrdersMap()
        {
            CreateMap<Service.DTO.WorkOrdersList, WorkOrder>();
            CreateMap<Domain.WorkOrder, WorkOrder>();
            CreateMap<WorkOrder, Domain.WorkOrder>()
                .ForMember(v => v.datecreated, opt => opt.Ignore())
                .ForMember(v => v.dateupdated, opt => opt.Ignore())
                .ForMember(v => v.createdby, opt => opt.Ignore())
                .ForMember(v => v.updatedby, opt => opt.Ignore())
                .ForMember(v => v.idPrefix, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                .ForMember(v => v.ID, opt => opt.Ignore())
                ;
        }

    }
}