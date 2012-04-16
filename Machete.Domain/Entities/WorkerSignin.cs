using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class WorkerSignin : Signin { }
    public abstract class Signin : Record
    {
        //public int ID { get; set; }
        public virtual Worker worker {get; set;}
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(Resources.Worker))]
        public int dwccardnum { get; set; }
        public int? WorkerID { get; set; }
        public int? memberStatus { get; set; }
        public int? WorkAssignmentID { get; set; }
        public DateTime dateforsignin { get; set; }
        public DateTime? lottery_timestamp { get; set; }
        public int? lottery_sequence { get; set; }
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
        public int? skill1 { get; set; }
        public int? skill2 { get; set; }
        public int? skill3 { get; set; }
        public int signinID { get; set; }
        public int englishlevel { get; set; }
        public DateTime dateforsignin { get; set; }
        public int? waid { get; set; }
        public int? imageID { get; set; }
        public DateTime expirationDate { get; set; }
        public int memberStatus { get; set; }
        public int? lotterySequence { get; set; }
        public Person p { get; set; }
        public Worker w { get; set; }
        public Signin s { get; set; }
        public int typeOfWorkID { get; set; }

        public WorkerSigninView(Person per, Signin sign)
        {
            p = per;
            w = p.Worker;
            s = sign;
            ID = s.ID;
            firstname1 = p == null ? null : p.firstname1;
            firstname2 = p == null ? null : p.firstname2;
            lastname1 = p == null ? null : p.lastname1;
            lastname2 = p == null ? null : p.lastname2;
            dateforsignin = s.dateforsignin;
            dwccardnum = s.dwccardnum;
            signinID = s.ID;
            dateupdated = s.dateupdated;
            datecreated = s.datecreated;
            Createdby = s.Createdby;
            Updatedby = s.Updatedby;
            waid = s.WorkAssignmentID;
            skill1 = p == null ? null : p.Worker.skill1;
            skill2 = p == null ? null : p.Worker.skill2;
            skill3 = p == null ? null : p.Worker.skill3;
            imageID = p == null ? null : p.Worker.ImageID;
            englishlevel = p == null ? 0 : p.Worker.englishlevelID;
            lotterySequence = s.lottery_sequence;
            expirationDate = p == null ? DateTime.MinValue : p.Worker.memberexpirationdate;
            memberStatus = p == null ? 0 : p.Worker.memberStatus;
            typeOfWorkID = p == null ? 0 : p.Worker.typeOfWorkID;
        }
        public WorkerSigninView() { }
    }
}
