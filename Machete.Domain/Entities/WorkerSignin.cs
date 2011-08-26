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
        //public int ID { get; set; }
        public virtual Worker worker {get; set;}
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        public int dwccardnum { get; set; }
        public int? WorkerID { get; set; }
        public int? WorkAssignmentID { get; set; }
        public DateTime dateforsignin { get; set; } 
    }
    public class WorkerSigninView : Record
    {
        public int dwccardnum { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string fullname
        {
            get
            {
                return firstname1 + " " +
                        firstname2 + " " +
                        lastname1 + " " +
                        lastname2;
            }
            set{}
        }
        public int signinID { get; set; }
        public DateTime dateforsignin { get; set; }
        public int? waid { get; set; }


        public WorkerSigninView(Person p, WorkerSignin s)
        {
            ID = s.ID;
            firstname1 = p.firstname1;
            firstname2 = p.firstname2;
            lastname1 = p.lastname1;
            lastname2 = p.lastname2;
            dateforsignin = s.dateforsignin;
            dwccardnum = s.dwccardnum;
            signinID = s.ID;
            dateupdated = s.dateupdated;
            datecreated = s.datecreated;
            Createdby = s.Createdby;
            Updatedby = s.Updatedby;
            waid = s.WorkAssignmentID;
        }
        public WorkerSigninView() { }
    }
}
