using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class WorkerSignin : Signin
    {
        public WorkerSignin()
        {
            idString = "wsi";
        }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
        public bool memberInactive { get; set; }
        public bool memberSanctioned { get; set; }
        public bool memberExpired { get; set; }
        public bool memberExpelled { get; set; }
        public string imageRef { get; set; }
        public string message { get; set; }

        public int? WorkAssignmentID { get; set; }
        public DateTime? lottery_timestamp { get; set; }
        public int? lottery_sequence { get; set; }
        public virtual Worker worker { get; set; }
        public int? WorkerID { get; set; }
    }

    public class WorkerSigninList
    {
        public int ID { get; set; }
        public int WSIID { get; set; }
        public string recordid { get; set; }
        public string fullname { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public int dwccardnum { get; set; }
        public DateTime dateforsignin { get; set; }
        public string dateforsigninstring { get; set; }
        public int WAID { get; set; }
        public string memberStatus { get; set; }
        public bool memberInactive { get; set; }
        public bool memberSanctioned { get; set; }
        public bool memberExpired { get; set; }
        public bool memberExpelled { get; set; }
        public int? imageID { get; set; }
        public int? lotterySequence { get; set; }
        public string expirationDate { get; set; }
        public string skillCodes { get; set; }
        public string program { get; set; }
        public string imageRef { get; set; }

    }
}