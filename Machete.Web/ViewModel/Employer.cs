

namespace Machete.Web.ViewModel
{

    public class Employer
    {
        public int ID { get; set; }
        public string tabref { get { return "/Employer/Edit/" + ID.ToString(); } }
        public string tablabel { get { return name; } }
        public string active { get; set; }
        public string EID { get; set; }
        public string recordid { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string driverslicense { get; set; }
        public string licenseplate { get; set; }
        public string dateupdated { get; set; }
        public string Updatedby { get; set; }
        public string onlineSource { get; set; }
    }

}