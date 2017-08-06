using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
{
    public class WorkOrder
    {
        public string createdby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public int id { get; set; }
        public string idPrefix { get; }
        public string idString { get; set; }
        public string updatedby { get; set; }

        public string additionalNotes { get; set; }
        public string city { get; set; }
        public string contactName { get; set; }
        public DateTime dateTimeofWork { get; set; }
        public string description { get; set; }
        public bool? disclosureAgreement { get; set; }
        public int EmployerID { get; set; }
        public bool englishRequired { get; set; }
        public string englishRequiredNote { get; set; }
        public bool lunchSupplied { get; set; }
        public bool onlineSource { get; set; }
        public int? paperOrderNum { get; set; }
        public string paypalErrors { get; set; }
        public double? paypalFee { get; set; }
        public string paypalPayerId { get; set; }
        public string paypalToken { get; set; }
        public string paypalTransactID { get; set; }
        public bool permanentPlacement { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string statusEN { get; set; }
        public string statusES { get; set; }
        public int statusID { get; set; }
        public bool timeFlexible { get; set; }
        public double timeZoneOffset { get; set; }
        public double transportFee { get; set; }
        public double transportFeeExtra { get; set; }
        public string transportMethodEN { get; set; }
        public string transportMethodES { get; set; }
        public int transportMethodID { get; set; }
        public string transportTransactID { get; set; }
        public int? transportTransactType { get; set; }
        public int typeOfWorkID { get; set; }
        public int waPseudoIDCounter { get; set; }
        public string workSiteAddress1 { get; set; }
        public string workSiteAddress2 { get; set; }
        public string zipcode { get; set; }


        public string EID { get; set; }
        public string WOID { get; set; }
        public string recordid { get; set; }
        public string dateupdatedstring { get; set; }
        public string datecreatedstring { get; set; }
        public string transportMethod { get; set; }
        public Collection<WorkAssignment> workAssignments { get; set; }
    }
}