using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class WorkOrder : Record
    {
        public IDefaults def { get; set; }

        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string EID { get; set; }
        public string WOID { get; set; }
        public string recordid { get; set; }
        public string dateupdatedstring { get; set; }
        public string datecreatedstring { get; set; }
        public string transportMethod { get; set; }


        public static int iActive { get; set; }
        public static int iPending { get; set; }
        public static int iCompleted { get; set; }
        public static int iCancelled { get; set; }
        public static int iExpired { get; set; }

        //public int ID { get; set; }
        public int EmployerID { get; set; }
        public virtual Employer Employer { get; set; }

        public virtual ICollection<WorkAssignment> workAssignments { get; set; }

        [LocalizedDisplayName("workerRequests", NameResourceType = typeof(Resources.WorkOrder))]
        public virtual ICollection<WorkerRequest> workerRequests { get; set; }
        public virtual ICollection<Email> Emails { get; set; }

        // Constructor
        public WorkOrder()
        {
            idString = "WO";
        }
        public Double timeZoneOffset { get; set; }

        // Flag identifying if source of work order was online web form
        [LocalizedDisplayName("onlineSource", NameResourceType = typeof(Resources.WorkOrder))]
        public bool onlineSource { get; set; }

        // Reference to historical paper record order number
        [LocalizedDisplayName("paperOrderNum", NameResourceType = typeof(Resources.WorkOrder))]
        [RegularExpression(@"^$|^[\d]{1,5}$", ErrorMessageResourceName = "paperOrderNumFormat", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int? paperOrderNum { get; set; }

        // Counter to track next work assignment number associated with this work order
        public int waPseudoIDCounter { get; set; }

        // Work site contact name - may be different than employer name
        [LocalizedDisplayName("contactName", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "contactNamerequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string contactName { get; set; }

        // Work order status
        [LocalizedDisplayName("status", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "statusrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int statusID { get; set; }
        [StringLength(50)]
        public string statusEN { get; set; }
        [StringLength(50)]
        public string statusES { get; set; }

        // Work site address, 1
        [LocalizedDisplayName("workSiteAddress1", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "workSiteAddress1required", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string workSiteAddress1 { get; set; }

        // Work site address, 2
        [LocalizedDisplayName("workSiteAddress2", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string workSiteAddress2 { get; set; }

        // Work site city
        [LocalizedDisplayName("city", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "cityrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string city { get; set; }

        // Work site state
        [LocalizedDisplayName("state", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "staterequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string state { get; set; }

        // Work site zipcode
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string zipcode { get; set; }

        // Work site phone
        [LocalizedDisplayName("phone", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string phone { get; set; }

        // Work program (e.g. HHH, DWC, etc)
        // TODO: investigate deleting this - it doesn't appear in the WO interface
        [LocalizedDisplayName("typeOfWorkID", NameResourceType = typeof(Resources.WorkOrder))]
        //[Required(ErrorMessageResourceName = "typeOfWorkIDrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int typeOfWorkID { get; set; }

        // Flag indicating if english is required for at least one worker
        [LocalizedDisplayName("englishRequired", NameResourceType = typeof(Resources.WorkOrder))]
        public bool englishRequired { get; set; }

        // Description of english skills required
        [LocalizedDisplayName("englishRequiredNote", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string englishRequiredNote { get; set; }

        // Flag indicating if lunch is supplied
        [LocalizedDisplayName("lunchSupplied", NameResourceType = typeof(Resources.WorkOrder))]
        public bool lunchSupplied { get; set; }

        // Flag indicating if permanent placement is desired
        [LocalizedDisplayName("permanentPlacement", NameResourceType = typeof(Resources.WorkOrder))]
        public bool permanentPlacement { get; set; }

        // Method of transportation for worker to arrive at work site
        [LocalizedDisplayName("transportMethodID", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportMethodIDrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int transportMethodID { get; set; }
        public string transportMethodEN { get; set; }
        public string transportMethodES { get; set; }

        [Required(ErrorMessageResourceName = "transportMethodIDrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int transportProviderID { get; set; }

        // Transportation fee charged for worker transportation 
        [LocalizedDisplayName("transportFee", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportFeeRequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public double transportFee { get; set; }

        // Extra transportation fee charged for worker transportation 
        [LocalizedDisplayName("transportFeeExtra", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportFeeExtraRequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public double transportFeeExtra { get; set; }

        // Transportation fee payment method
        [LocalizedDisplayName("transportTransactType", NameResourceType = typeof(Resources.WorkOrder))]
        public int? transportTransactType { get; set; }

        // Transportation transaction fee ID associated with payment (e.g. check number)
        [LocalizedDisplayName("transportTransactID", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string transportTransactID { get; set; }

        // Work order description
        [LocalizedDisplayName("description", NameResourceType = typeof(Resources.WorkOrder))]
        //[Required(ErrorMessageResourceName = "descriptionRequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string description { get; set; }

        // Date / time of work
        [LocalizedDisplayName("dateTimeofWork", NameResourceType = typeof(Resources.WorkOrder))]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceName = "dateTimeofWorkrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public DateTime dateTimeofWork { get; set; }

        // Flag indicating if the time is flexible
        [LocalizedDisplayName("timeFlexible", NameResourceType = typeof(Resources.WorkOrder))]
        public bool timeFlexible { get; set; }

        [LocalizedDisplayName("additionalNotes", NameResourceType = typeof(Resources.WorkAssignment))]
        [StringLength(1000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkAssignment))]
        public string additionalNotes { get; set; }

        [LocalizedDisplayName("disclosureAgreement", NameResourceType = typeof(Resources.WorkOrder))]
        public bool? disclosureAgreement { get; set; }

        // Fee charged by PayPal for online payments
        public double? ppFee { get; set; }

        [StringLength(5000)]
        public string ppResponse { get; set; }

        [StringLength(25)]
        public string ppPaymentToken { get; set; }

        [StringLength(50)]
        public string ppPaymentID { get; set; }

        [StringLength(25)]
        public string ppPayerID { get; set; }

        [StringLength(20)]
        public string ppState { get; set; }

    }

    public class WorkOrdersList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string EID { get; set; }
        public string WOID { get; set; }
        public string dateTimeofWork { get; set; }
        public string status { get; set; }
        public int statusID { get; set; }
        public string displayState { get; set; }
        public string transportMethod { get; set; }
        public int WAcount { get; set; }
        public string contactName { get; set; }
        public string workSiteAddress1 { get; set; }
        public string zipcode { get; set; }
        public string onlineSource { get; set; }
        public string emailSentCount { get; set; }
        public string emailErrorCount { get; set; }
        public string updatedby { get; set; }
        public string dateupdated { get; set; }
        public string recordid { get; set; }
        public IEnumerable<WorkerAssignedList> workers { get; set; }
    }

    public class WorkerAssignedList
    {
        public int WID { get; set; }
        public string name { get; set; }
        public string skill { get; set; }
        public double hours { get; set; }
        public double wage { get; set; }
    }

    //workers = showWorkers? // Note: Workers appears to not be used
    //    from w in wo.workAssignments
    //    select new
    //                    {
    //                        WID = w.workerAssigned != null ? (int?)w.workerAssigned.dwccardnum : null,
    //                        name = w.workerAssigned != null ? w.workerAssigned.Person.fullName() : null,
    //                        skill = lcache.textByID(w.skillID, currentCulture.TwoLetterISOLanguageName),
    //                        hours = w.hours,
    //                        wage = w.hourlyWage
    //} : null
}