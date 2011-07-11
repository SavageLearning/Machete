using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class WorkerRequest : Record
    {
        public int ID { get; set; }
        public int WorkOrderID { get; set; }
        public virtual WorkOrder workOrder { get; set; }
        public int WorkerID { get; set; }
        public virtual Worker workerRequested { get; set; }
        public string fullName
        {
            get {
                Person p = this.workerRequested.Person;
                    return p.firstname1 + " " +
                         p.firstname2 + " " +
                         p.lastname1 + " " +
                         p.lastname2;
                }
        }
    }
}
