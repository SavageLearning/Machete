using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class WorkerSignin : Record
    {
        public int ID { get; set; }
        public virtual Worker worker {get; set;}
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        public int dwccardnum { get; set; }
        public int? WorkerID { get; set; }
        public int? WorkAssignmentID { get; set; }
        public DateTime dateforsignin { get; set; } 
    }
    public class WorkerSigninView
    {
        public Person person { get; set; }
        public WorkerSignin signin { get; set; }
        public WorkerSigninView(Person p, WorkerSignin s)
        {
            person = p;
            signin = s;
        }
        public WorkerSigninView() { }
    }
}
