using Machete.Web.Helpers;

namespace Machete.Web.ViewModel
{
    public class Employer : Domain.Employer
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }
        public new string active { get; set; }
        public new string onlineSource { get; set; }
        public new string blogparticipate { get; set; }
        public new string business { get; set; }
        public new string isOnlineProfileComplete { get; set; }
        public new string receiveUpdates { get; set; }
        public new string referredby { get; set; }
        public new string returnCustomer { get; set; }
    }

    public class EmployerList 
    {
        public int ID { get; set; }
        public string EID { get; set; }         // duplicate names for ids
        public string recordid { get; set; }    // because legacy reasons
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string active { get; set; }
        public string dateupdated { get; set; }
        public string onlineSource { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public string zipcode { get; set; }
        public string updatedby { get; set; }
    }
}