using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class WorkAssignmentsList
    {
        public int ID { get; set; }
        public int workOrderID { get; set; }
        public string employername { get; set; }
        public int englishLevelID { get; set; }
        public int? skillID { get; set; }
        public string skillEN { get; set; }
        public string skillES { get; set; }
        public double hourlyWage { get; set; }
        public double hours { get; set; }
        public int? hourRange { get; set; }
        public int days { get; set; }
        public int? paperOrderNum { get; set; }
        public string description { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }
        public DateTime dateTimeofWork { get; set; }
        public int? WOstatus { get; set; }
        public double earnings { get; set; }
        public double maxEarnings { get; set; }
        public int? workerSigninID { get; set; }
        public int? workerAssignedID { get; set; }
        public int? workerAssignedDWCCardnum { get; set; }
        public string assignedWorker { get; set; }
        public int? pseudoID { get; set; }
        public string fullWAID { get; set; }
        public List<string> requestedList { get; set; }
        public double timeZoneOffset { get; set; }
    }
}
