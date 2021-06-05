using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportVehicleAvailability : Record
    {
        public virtual TransportVehicle TransportVehicle { get; set; } 
        public int TransportVehicleID { get; set; }
        public int Day { get; set; }
        public virtual ICollection<TransportVehicleAvailabilityTimeBlock> TimeBlocks { get; set; }
    }

    public class TransportVehicleAvailabilityOverride : Record
    {
        public DateTime OverrideDate { get; set; }        
    }
}
