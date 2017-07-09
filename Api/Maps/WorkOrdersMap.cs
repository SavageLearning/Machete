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
        }

    }
}