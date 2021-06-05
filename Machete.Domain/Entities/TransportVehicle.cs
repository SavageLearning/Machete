using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain

{
    public class TransportVehicle : Record
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
        public int TransportProviderID { get; set; }
        public virtual TransportProvider TransportProvider { get; set; }
        public virtual ICollection<TransportVehicleAvailability> Availability { get; set; }
        public virtual ICollection<TransportVehicleAvailabilityOverride> AvailabilityOverrides { get; set; }
        public virtual ICollection<TransportVehicleScheduleLoadHistory> TransportVehicleScheduleLoads { get; set; }

        public TransportVehicle()
        {
            Active = false;
        }
    }
}
