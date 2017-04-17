using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class EventList
    {
        public int ID { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public string eventTypeEN { get; set; }
        public string eventTypeES { get; set; }
        public string notes { get; set; }
        public int fileCount { get; set; }
        public int eventTypeID { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }

    }
}
