using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;



namespace Machete.Domain
{
    public class Worker : Record
    {
        public static int iActive { get; set; }
        public static int iInactive { get; set; }
        public static int iSanctioned { get; set; }
        public static int iExpired { get; set; }
        public static int iExpelled { get; set; }
        public static int iDWC { get; set; } //TODO: Casa specific configuration. needs real abstraction.
        public static int iHHH { get; set; } //TODO: Casa specific configuration. needs real abstraction.

        public Worker()
        {
            idString = "worker";
        }
        //public int ID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<WorkerSignin> workersignins { get; set; }
        public virtual ICollection<WorkAssignment> workAssignments { get; set; }
        //
        [Required(ErrorMessageResourceName = "typeOfWorkID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("typeOfWorkID", NameResourceType = typeof(Resources.Worker))]
        public int typeOfWorkID { get; set; }
        //
        [Required(ErrorMessageResourceName = "dateOfMembership", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dateOfMembership", NameResourceType = typeof(Resources.Worker))]
        public DateTime dateOfMembership { get; set; }
        //
        [Required(ErrorMessageResourceName = "dateOfBirth", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dateOfBirth", NameResourceType = typeof(Resources.Worker))]
        public DateTime dateOfBirth {get; set;}
        //
        [Required(ErrorMessageResourceName = "memberStatus", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("memberStatus", NameResourceType = typeof(Resources.Worker))]
        public int memberStatus { get; set; }
        //
        [LocalizedDisplayName("memberReactivateDate", NameResourceType = typeof(Resources.Worker))]
        public DateTime? memberReactivateDate { get; set; }
        //
        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Worker))]
        public bool active { get; set; }
        //
        [LocalizedDisplayName("homeless", NameResourceType = typeof(Resources.Worker))]
        public bool? homeless { get; set; }
        //
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("raceID", NameResourceType = typeof(Resources.Worker))]
        public int RaceID { get; set; }
        //
        [LocalizedDisplayName("raceother", NameResourceType = typeof(Resources.Worker))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        public string raceother { get; set; }
        //
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [Required(ErrorMessageResourceName = "height", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("height", NameResourceType = typeof(Resources.Worker))]
        public string height { get; set; }
        //
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [Required(ErrorMessageResourceName = "weight", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("weight", NameResourceType = typeof(Resources.Worker))]
        public string weight { get; set; }
        //
        [Required(ErrorMessageResourceName = "englishlevelID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("englishlevelID", NameResourceType = typeof(Resources.Worker))]
        public int englishlevelID { get; set; }
        //
        [LocalizedDisplayName("recentarrival", NameResourceType = typeof(Resources.Worker))]
        public bool recentarrival { get; set; }
        //
        [Required(ErrorMessageResourceName = "dateinUSA", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dateinUSA", NameResourceType = typeof(Resources.Worker))]
        public DateTime? dateinUSA { get; set; }
        //
        [Required(ErrorMessageResourceName = "dateinseattle", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dateinseattle", NameResourceType = typeof(Resources.Worker))]
        public DateTime? dateinseattle { get; set; }
        //
        [LocalizedDisplayName("disabled", NameResourceType = typeof(Resources.Worker))]
        public bool disabled { get; set; }
        //
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("disabilitydesc", NameResourceType = typeof(Resources.Worker))]
        public string disabilitydesc { get; set; }
        //
        [Required(ErrorMessageResourceName = "maritalstatus", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("maritalstatus", NameResourceType = typeof(Resources.Worker))]
        public int maritalstatus { get; set; }
        //
        [LocalizedDisplayName("livewithchildren", NameResourceType = typeof(Resources.Worker))]
        public bool livewithchildren { get; set; }
        //
        [LocalizedDisplayName("numofchildren", NameResourceType = typeof(Resources.Worker))]
        [Required(ErrorMessageResourceName = "numofchildren", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^([0-9]|[0-1]\\d|20)$", ErrorMessageResourceName = "numofchildrenRxError", ErrorMessageResourceType = typeof(Resources.Worker))]
        public int numofchildren { get; set; }
        //
        [Required(ErrorMessageResourceName = "incomeID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("incomeID", NameResourceType = typeof(Resources.Worker))]
        public int incomeID { get; set; }
        //
        [LocalizedDisplayName("livealone", NameResourceType = typeof(Resources.Worker))]
        public bool livealone { get; set; }
        //
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("emcontUSAname", NameResourceType = typeof(Resources.Worker))]
        public string emcontUSAname { get; set; }
        //
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("emcontUSArelation", NameResourceType = typeof(Resources.Worker))]
        public string emcontUSArelation { get; set; }
        //
        [StringLength(14, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("emcontUSAphone", NameResourceType = typeof(Resources.Worker))]
        public string emcontUSAphone { get; set; }
        // TODO: require unique number when EF supports it
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        public int dwccardnum { get; set; }
        //
        [Required(ErrorMessageResourceName = "neighborhoodID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("neighborhoodID", NameResourceType = typeof(Resources.Worker))]
        public int neighborhoodID { get; set; }
        //
        [LocalizedDisplayName("immigrantrefugee", NameResourceType = typeof(Resources.Worker))]
        public bool immigrantrefugee { get; set; }
        //  
        [Required(ErrorMessageResourceName = "countryoforiginID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("countryoforiginID", NameResourceType = typeof(Resources.Worker))]
        public int countryoforiginID { get; set; }
        //
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("emcontoriginname", NameResourceType = typeof(Resources.Worker))]
        public string emcontoriginname { get; set; }
        //
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("emcontoriginrelation", NameResourceType = typeof(Resources.Worker))]
        public string emcontoriginrelation { get; set; }
        //
        [StringLength(14, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("emcontoriginphone", NameResourceType = typeof(Resources.Worker))]
        public string emcontoriginphone { get; set; }
        //
        [Required(ErrorMessageResourceName = "memberexpirationdate", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("memberexpirationdate", NameResourceType = typeof(Resources.Worker))]
        public DateTime memberexpirationdate { get; set; }
        //
        [LocalizedDisplayName("driverslicense", NameResourceType = typeof(Resources.Worker))]
        public bool driverslicense { get; set; }
        //
        [LocalizedDisplayName("licenseexpirationdate", NameResourceType = typeof(Resources.Worker))]
        public DateTime? licenseexpirationdate { get; set; }
        //
        [LocalizedDisplayName("carinsurance", NameResourceType = typeof(Resources.Worker))]
        public bool? carinsurance { get; set; }
        //
        [LocalizedDisplayName("insuranceexpiration", NameResourceType = typeof(Resources.Worker))]
        public DateTime? insuranceexpiration { get; set; }
        //
        public int? ImageID { get; set; }
        //
        [LocalizedDisplayName("skill1", NameResourceType = typeof(Resources.Worker))]
        public int? skill1 { get; set; }
        //
        [LocalizedDisplayName("skill2", NameResourceType = typeof(Resources.Worker))]
        public int? skill2 { get; set; }
        //
        [LocalizedDisplayName("skill3", NameResourceType = typeof(Resources.Worker))]
        public int? skill3 { get; set; }
        //
        public bool isActive 
        {
            get { return this.memberStatus == iActive ? true : false; }
        }
        public bool isInactive
        {
            get { return this.memberStatus == iInactive ? true : false; }
        }
        public bool isSanctioned
        {
            get { return this.memberStatus == iSanctioned ? true : false; }
        }
        public bool isExpired
        {
            get { return this.memberStatus == iExpired ? true : false; }
        }
        public bool isExpelled
        {
            get { return this.memberStatus == iExpelled ? true : false; }
        }
    }
}

