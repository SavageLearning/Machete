using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Worker : Record
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }

        //public static int iActive { get; set; }
        //public static int iInactive { get; set; }
        //public static int iSanctioned { get; set; }
        //public static int iExpired { get; set; }
        //public static int iExpelled { get; set; }

        public Worker()
        {
            idString = "worker";
        }
        //public int ID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<WorkerSignin> workersignins { get; set; }
        public virtual ICollection<WorkAssignment> workAssignments { get; set; }
        //
        [StringLength(100)]
        public string fullNameAndID { get; set; }

        [Required(ErrorMessageResourceName = "typeOfWorkID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("typeOfWorkID", NameResourceType = typeof(Resources.Worker))]
        public int typeOfWorkID { get; set; }
        // typeOfWork is really 'program'; as in, what program at the Center does the worker belong to
        // Program is the letter code displayed instead of the full program name
        public string typeOfWork { get; set; }
        //
        [Required(ErrorMessageResourceName = "dateOfMembership", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dateOfMembership", NameResourceType = typeof(Resources.Worker))]
        public DateTime dateOfMembership { get; set; }
        //
        [LocalizedDisplayName("dateOfBirth", NameResourceType = typeof(Resources.Worker))]
        public DateTime? dateOfBirth { get; set; }

        [Required(ErrorMessageResourceName = "memberStatus", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("memberStatus", NameResourceType = typeof(Resources.Worker))]
        public int memberStatusID { get; set; }
        [StringLength(50)]
        public string memberStatusEN { get; set; }
        [StringLength(50)]
        public string memberStatusES { get; set; }
        //
        [LocalizedDisplayName("memberReactivateDate", NameResourceType = typeof(Resources.Worker))]
        public DateTime? memberReactivateDate { get; set; }

        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Worker))]
        public bool? active { get; set; }

        [LocalizedDisplayName("homeless", NameResourceType = typeof(Resources.Worker))]
        public bool? homeless { get; set; }
        //
        public int? housingType { get; set; }

        [LocalizedDisplayName("raceID", NameResourceType = typeof(Resources.Worker))]
        public int? RaceID { get; set; }

        [LocalizedDisplayName("raceother", NameResourceType = typeof(Resources.Worker))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        public string raceother { get; set; }
        //
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("height", NameResourceType = typeof(Resources.Worker))]
        public string height { get; set; }
        //
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("weight", NameResourceType = typeof(Resources.Worker))]
        public string weight { get; set; }
        //
        [LocalizedDisplayName("englishlevelID", NameResourceType = typeof(Resources.Worker))]
        public int englishlevelID { get; set; }
        //
        [LocalizedDisplayName("recentarrival", NameResourceType = typeof(Resources.Worker))]
        public bool? recentarrival { get; set; }

        [LocalizedDisplayName("dateinUSA", NameResourceType = typeof(Resources.Worker))]
        public DateTime? dateinUSA { get; set; }
        //
        [LocalizedDisplayName("dateinseattle", NameResourceType = typeof(Resources.Worker))]
        public DateTime? dateinseattle { get; set; }
        //
        [LocalizedDisplayName("disabled", NameResourceType = typeof(Resources.Worker))]
        public bool? disabled { get; set; }

        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("disabilitydesc", NameResourceType = typeof(Resources.Worker))]
        public string disabilitydesc { get; set; }

        [LocalizedDisplayName("maritalstatus", NameResourceType = typeof(Resources.Worker))]
        public int? maritalstatus { get; set; }

        [LocalizedDisplayName("livewithchildren", NameResourceType = typeof(Resources.Worker))]
        public bool? livewithchildren { get; set; }

        [LocalizedDisplayName("liveWithSpouse", NameResourceType = typeof(Resources.Worker))]
        public bool? liveWithSpouse { get; set; }

        [LocalizedDisplayName("livealone", NameResourceType = typeof(Resources.Worker))]
        public bool? livealone { get; set; }

        [StringLength(1000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("liveWithDescription", NameResourceType = typeof(Resources.Worker))]
        public string liveWithDescription { get; set; }

        [LocalizedDisplayName("numofchildren", NameResourceType = typeof(Resources.Worker))]
        [RegularExpression("^([0-9]|[0-1]\\d|20)$", ErrorMessageResourceName = "numofchildrenRxError", ErrorMessageResourceType = typeof(Resources.Worker))]
        public int? numofchildren { get; set; }

        [LocalizedDisplayName("americanBornChildren", NameResourceType = typeof(Resources.Worker))]
        public int? americanBornChildren { get; set; }

        [LocalizedDisplayName("numChildrenUnder18", NameResourceType = typeof(Resources.Worker))]
        public int? numChildrenUnder18 { get; set; }

        [LocalizedDisplayName("educationLevel", NameResourceType = typeof(Resources.Worker))]
        public int? educationLevel { get; set; }

        [LocalizedDisplayName("farmLaborCharacteristics", NameResourceType = typeof(Resources.Worker))]
        public int? farmLaborCharacteristics { get; set; }

        [LocalizedDisplayName("wageTheftVictim", NameResourceType = typeof(Resources.Worker))]
        public bool? wageTheftVictim { get; set; }

        [LocalizedDisplayName("wageTheftRecoveryAmount", NameResourceType = typeof(Resources.Worker))]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public double? wageTheftRecoveryAmount { get; set; }

        [LocalizedDisplayName("incomeID", NameResourceType = typeof(Resources.Worker))]
        public int? incomeID { get; set; }

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
        [LocalizedDisplayName("neighborhoodID", NameResourceType = typeof(Resources.Worker))]
        public int? neighborhoodID { get; set; }

        [LocalizedDisplayName("immigrantrefugee", NameResourceType = typeof(Resources.Worker))]
        public bool? immigrantrefugee { get; set; }

        [LocalizedDisplayName("countryoforiginID", NameResourceType = typeof(Resources.Worker))]
        public int? countryoforiginID { get; set; }

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
        public bool? driverslicense { get; set; }

        [LocalizedDisplayName("licenseexpirationdate", NameResourceType = typeof(Resources.Worker))]
        public DateTime? licenseexpirationdate { get; set; }
        //
        [LocalizedDisplayName("carinsurance", NameResourceType = typeof(Resources.Worker))]
        public bool? carinsurance { get; set; }
        //
        [LocalizedDisplayName("insuranceexpiration", NameResourceType = typeof(Resources.Worker))]
        public DateTime? insuranceexpiration { get; set; }
        //
        [LocalizedDisplayName("lastPaymentDate", NameResourceType = typeof(Resources.Worker))]
        public DateTime? lastPaymentDate { get; set; }

        [LocalizedDisplayName("lastPaymentAmount", NameResourceType = typeof(Resources.Worker))]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public double? lastPaymentAmount { get; set; }

        [LocalizedDisplayName("ownTools", NameResourceType = typeof(Resources.Worker))]
        public bool? ownTools { get; set; }

        [LocalizedDisplayName("healthInsurance", NameResourceType = typeof(Resources.Worker))]
        public bool? healthInsurance { get; set; }

        [LocalizedDisplayName("usVeteran", NameResourceType = typeof(Resources.Worker))]
        public bool? usVeteran { get; set; }

        [LocalizedDisplayName("healthInsuranceDate", NameResourceType = typeof(Resources.Worker))]
        public DateTime? healthInsuranceDate { get; set; }

        [LocalizedDisplayName("vehicleTypeID", NameResourceType = typeof(Resources.Worker))]
        public int? vehicleTypeID { get; set; }

        [LocalizedDisplayName("incomeSourceID", NameResourceType = typeof(Resources.Worker))]
        public int? incomeSourceID { get; set; }

        [StringLength(1000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("introToCenter", NameResourceType = typeof(Resources.Worker))]
        public string introToCenter { get; set; }

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

        public string skillCodes { get; set; }
        //
        [LocalizedDisplayName("workerRating", NameResourceType = typeof(Resources.Worker))]
        public float? workerRating { get; set; }

        [LocalizedDisplayName("lgbtq", NameResourceType = typeof(Resources.Worker))]
        public bool? lgbtq { get; set; }

    }

    public class WorkerList
    {
        public int ID { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string WID { get; set; }
        public string recordid { get; set; }
        public string dwccardnum { get; set; }
        public string active { get; set; }
        public string memberStatus { get; set; }
        public int memberStatusID { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string memberexpirationdate { get; set; }
    }
}