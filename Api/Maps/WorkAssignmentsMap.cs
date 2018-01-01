using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Api.ViewModel;
using Newtonsoft.Json;

namespace Machete.Api.Maps
{
    public class WorkAssignmentsMap : MacheteProfile
    {
        public WorkAssignmentsMap()
        {
            CreateMap<Service.DTO.WorkAssignmentsList, WorkAssignment>();
            CreateMap<Domain.WorkAssignment, WorkAssignment>();
            CreateMap<WorkAssignment, Domain.WorkAssignment>();
        }

    }
}