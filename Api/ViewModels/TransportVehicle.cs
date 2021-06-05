using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
{
    public class TransportVehicle : BaseModel
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
        //public List<TransportCostRule> costRules { get; set; }
    }

    public class TransportVehicleSchedule : BaseModel
    {
        public int Capacity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TransportVehicleId { get; set; }
        public string TransportVehicleName { get; set; }
    }
}