using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.ViewModel;
using Newtonsoft.Json;

namespace Machete.Web.Maps
{
    public class WorkAssignmentsMap : MacheteProfile
    {
        public WorkAssignmentsMap()
        {
            CreateMap<Service.DTO.WorkAssignmentsList, ViewModel.Api.WorkAssignment>();
            CreateMap<Domain.WorkAssignment, ViewModel.Api.WorkAssignment>();
        }

    }
}