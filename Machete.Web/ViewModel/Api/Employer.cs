using System;

namespace Machete.Web.ViewModel.Api
{
    public class EmployerVM :RecordVM
    {
        public bool active { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public bool? blogparticipate { get; set; }
        public bool business { get; set; }
        public string businessname { get; set; }
        public string cellphone { get; set; }
        public string city { get; set; }
        public string driverslicense { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public bool? isOnlineProfileComplete { get; set; }
        public string licenseplate { get; set; }
        public string name { get; set; }
        public string notes { get; set; }
        public string onlineSigninID { get; set; }
        public bool onlineSource { get; set; }
        public string phone { get; set; }
        public bool receiveUpdates { get; set; }
        public int? referredby { get; set; }
        public string referredbyOther { get; set; }
        public bool returnCustomer { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
    }

    public class EmployersList
    {
        public int id { get; set; }
        public bool active { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public string zipcode { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }
        public bool onlineSource { get; set; }
    }
}