using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.Models;
using Machete.Domain;

namespace Machete.Web.ViewModel
{
    public class WorkAssignmentIndex
    {
        public Filter filter { get; set; }
        public Employer employer { get; set; }
        public WorkOrder workOrder { get; set; }
        public WorkAssignment workAssignment { get; set; }
        public IEnumerable<WorkAssignment> workAssignments { get; set; }
    }
}