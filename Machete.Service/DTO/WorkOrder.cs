using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class WorkOrder : Domain.WorkOrder
    {
        public int WAcount { get; set; }
        public int emailSentCount { get; set; }
        public int emailErrorCount { get; set; }
    }
}
