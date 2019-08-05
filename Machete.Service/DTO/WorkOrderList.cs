using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class WorkOrdersList
    {
        public int ID { get; set; }
        public int EmployerID { get; set; }
        public DateTime dateTimeofWork { get; set; }
        public int? paperOrderNum { get; set; }
        public int statusID { get; set; }
        public string statusEN { get; set; }
        public string statusES { get; set; }
        public int transportMethodID { get; set; }
        public int transportProviderID { get; set; }
        public string transportMethodEN { get; set; }
        public string transportMethodES { get; set; }
        public string contactName { get; set; }
        public string workSiteAddress1 { get; set; }
        public string zipcode { get; set; }
        public bool onlineSource { get; set; }
        public string updatedby { get; set; }
        public DateTime dateupdated { get; set; }
        public DateTime datecreated { get; set; }
        public string city { get; set; }
        public int WAcount { get; set; }
        public int emailSentCount { get; set; }
        public int emailErrorCount { get; set; }
        public int WAUnassignedCount { get; set; }
        public int WAOrphanedCount { get; set; }
        public IEnumerable<WorkerAssignedList> workers { get; set; }
    }


    public class WorkerAssignedList
    {
        public int WID { get; set; }
        public string name { get; set; }
        public string skillEN { get; set; }
        public string skillES { get; set; }
        public double hours { get; set; }
        public double wage { get; set; }
    }
}
