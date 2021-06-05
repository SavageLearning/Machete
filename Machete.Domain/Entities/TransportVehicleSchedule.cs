using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportVehicleSchedule : Record
    {
        public virtual int TransportVehicleId { get; set; }
        public virtual TransportVehicle TransportVehicle { get; set; }

        public virtual ICollection<JoinTransportVehicleScheduleWorkOrder>
            JoinTransportVehicleScheduleWorkOrder { get; set; }

        public int Capacity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class JoinTransportVehicleScheduleWorkOrder : Record
    {
        public int TransportVehicleScheduleID;
        public virtual TransportVehicleSchedule TransportVehicleSchedule { get; set; }

        public int WorkOrderID;
        public virtual WorkOrder WorkOrder { get; set; }
    }
}
