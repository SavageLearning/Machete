using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;

namespace Machete.Web.ViewModel
{
    public class WorkOrderGroupPrintView
    {
        public IEnumerable<WorkOrder> orders { get; set; }
    }
}