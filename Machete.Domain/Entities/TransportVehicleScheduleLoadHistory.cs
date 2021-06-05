using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportVehicleScheduleLoadHistory : Record
    {
        public DateTime ExecutionTime { get; set; }
        public int TransportVehicleId { get; set; }
        public virtual TransportVehicle TransportVehicle {get;set; }
    }
}
