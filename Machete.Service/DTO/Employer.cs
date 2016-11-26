using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class Employer : Record
    {
        public string active { get; set; }
        public string EID { get; set; }
        public string recordid { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string driverslicense { get; set; }
        public string licenseplate { get; set; }
        public string onlineSource { get; set; }

        public string blogparticipate { get; set; }
        public string business { get; set; }
        public string businessname { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string isOnlineProfileComplete { get; set; }
        public string notes { get; set; }
        public string onlineSigninID { get; set; }
        public string receiveUpdates { get; set; }
        public string referredby { get; set; }
        public string referredbyOther { get; set; }
        public string returnCustomer { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
    }
}
