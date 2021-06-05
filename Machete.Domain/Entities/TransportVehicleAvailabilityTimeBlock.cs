using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportVehicleAvailabilityTimeBlock : Record
    {
        public virtual TransportVehicleAvailability TransportVehicleAvailability { get; set; }
        public int TransportVehicleAvailabilityID { get; set; }
        [StringLength(5)]
        public string StartTime { get; set; }
        [StringLength(5)]
        public string EndTime { get; set; }
    }

    public class TransportVehicleAvailabilityOverrideTimeBlock : Record
    {
        public virtual TransportVehicleAvailabilityOverride ParentOverride { get; set; }
        [StringLength(5)]
        public string StartTime { get; set; }
        [StringLength(5)]
        public string EndTime { get; set; }
    }
}
