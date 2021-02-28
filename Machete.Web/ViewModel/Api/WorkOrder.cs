using System.Collections.ObjectModel;

namespace Machete.Web.ViewModel.Api
{
    public class WorkOrderVM : RecordVM
    {
        public string city { get; set; }
        public string contactName { get; set; }
        public string dateTimeofWork { get; set; }
        public string description { get; set; }
        public bool? disclosureAgreement { get; set; }
        public int EmployerID { get; set; }
        public bool englishRequired { get; set; }
        public string englishRequiredNote { get; set; }
        public bool onlineSource { get; set; }
        public int? paperOrderNum { get; set; }
        public string paypalErrors { get; set; }
        public double? ppFee { get; set; }
        public string ppPayerID { get; set; }
        public string ppPaymentToken { get; set; }
        public string ppPaymentID { get; set; }
        public string ppState { get; set; }
        public string ppResponse { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string statusEN { get; set; }
        public int statusID { get; set; }
        public bool timeFlexible { get; set; }
        public double timeZoneOffset { get; set; }
        public double transportFee { get; set; }
        public string transportMethodEN { get; set; }
        public int transportProviderID { get; set; }
        public string workSiteAddress1 { get; set; }
        public string workSiteAddress2 { get; set; }
        public string zipcode { get; set; }
        public Collection<WorkAssignmentVM> workAssignments { get; set; }
    }
}