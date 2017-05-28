using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.ViewModel;
using Newtonsoft.Json;

namespace Machete.Web.Maps
{
    public class WorkOrdersMap : MacheteProfile
    {
        public WorkOrdersMap()
        {
            CreateMap<Service.DTO.WorkOrdersList, ViewModel.Api.WorkOrder>();
            CreateMap<Domain.WorkOrder, ViewModel.Api.WorkOrder>();
        }

    }
}