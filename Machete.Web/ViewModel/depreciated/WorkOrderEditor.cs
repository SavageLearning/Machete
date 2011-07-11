using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Machete.Domain;

namespace Machete.Web.ViewModel
{
    public class WorkOrderEditor
    {
        public Employer employer { get; set; }
        public WorkOrder order { get; set; }
        public IEnumerable<WorkAssignment> workAssignments { get; set; }
    }
}