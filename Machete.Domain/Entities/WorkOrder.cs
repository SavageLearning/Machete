using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class WorkOrder : Record
    {
        public static int iActive {get; set;}
        public static int iPending {get; set;}
        public static int iCompleted {get; set;}
        public static int iCancelled {get; set;}
        public static int iExpired {get; set;}

        //public int ID { get; set; }
        public int EmployerID { get; set; }
        public virtual Employer Employer { get; set; }

        public virtual ICollection<WorkAssignment> workAssignments { get; set; }
        public virtual ICollection<WorkerRequest> workerRequests { get; set; }
        //
        public WorkOrder()
        {
            idString = "WO";
            this.waPseudoIDCounter = 0;
        }
        //
        [LocalizedDisplayName("onlineSource", NameResourceType = typeof(Resources.WorkOrder))]
        public bool onlineSource { get; set; }
        //
        [LocalizedDisplayName("paperOrderNum", NameResourceType = typeof(Resources.WorkOrder))]
        [RegularExpression(@"^$|^[\d]{1,5}$", ErrorMessageResourceName = "paperOrderNumFormat", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int? paperOrderNum { get; set; }
        // used to track next number assignments in this work order
        public int waPseudoIDCounter { get; set; }
        //
        [LocalizedDisplayName("contactName", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "contactNamerequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string contactName { get; set; }
        //
        [LocalizedDisplayName("status", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "statusrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int status { get; set; }
        //
        [LocalizedDisplayName("workSiteAddress1", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "workSiteAddress1required", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string workSiteAddress1 { get; set; }
        //
        [LocalizedDisplayName("workSiteAddress2", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string workSiteAddress2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "cityrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "staterequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string state { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string phone { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("typeOfWorkID", NameResourceType = typeof(Resources.WorkOrder))]
        //[Required(ErrorMessageResourceName = "typeOfWorkIDrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int typeOfWorkID { get; set; }
        //
        [LocalizedDisplayName("englishRequired", NameResourceType = typeof(Resources.WorkOrder))]
        public bool englishRequired { get; set; }
        //
        [LocalizedDisplayName("englishRequiredNote", NameResourceType = typeof(Resources.WorkOrder))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string englishRequiredNote { get; set; }
        //
        [LocalizedDisplayName("lunchSupplied", NameResourceType = typeof(Resources.WorkOrder))]
        public bool lunchSupplied { get; set; }
        //
        [LocalizedDisplayName("permanentPlacement", NameResourceType = typeof(Resources.WorkOrder))]
        public bool permanentPlacement { get; set; }
        //
        [LocalizedDisplayName("transportMethodID", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportMethodIDrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public int transportMethodID { get; set; }
        //
        [LocalizedDisplayName("transportFee", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportFeeRequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public double transportFee { get; set; }
        //
        [LocalizedDisplayName("transportFeeExtra", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportFeeExtraRequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public double transportFeeExtra { get; set; }
        //
        [LocalizedDisplayName("description", NameResourceType = typeof(Resources.WorkOrder))]
        //[Required(ErrorMessageResourceName = "descriptionRequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public string description { get; set; }
        //
        [LocalizedDisplayName("dateTimeofWork", NameResourceType = typeof(Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "dateTimeofWorkrequired", ErrorMessageResourceType = typeof(Resources.WorkOrder))]
        public DateTime dateTimeofWork { get; set; }
        //
        [LocalizedDisplayName("timeFlexible", NameResourceType = typeof(Resources.WorkOrder))]
        public bool timeFlexible { get; set; }

        //
        //
        //
        public string getPseudoWOID()
        {
            return this.paperOrderNum.HasValue ? System.String.Format("{0,5:D5}", this.paperOrderNum) : System.String.Format("{0,5:D5}", this.ID);
        }
        public string getTabRef()
        {
            return "/WorkOrder/Edit/" + Convert.ToString(this.ID);
        }

        public string getTabLabel()
        {
            return this.getPseudoWOID() + " @ " + this.workSiteAddress1;
        }


    }

    public class WorkOrderSummary
    {
        public DateTime? date { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }
}