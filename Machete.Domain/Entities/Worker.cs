using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;



namespace Machete.Domain
{
    public class Worker
    {
        public int ID { get; set; }
        public virtual Person Person { get; set; }
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
        public byte englishlevelID { get; set; }
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
        [StringLength(1, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        public string maritalstatus { get; set; }
        //
        [LocalizedDisplayName("livewithchildren", NameResourceType = typeof(Resources.Worker))]
        public bool livewithchildren { get; set; }
        //
        [LocalizedDisplayName("numofchildren", NameResourceType = typeof(Resources.Worker))]
        [Required(ErrorMessageResourceName = "numofchildren", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^([0-9]|[0-1]\\d|20)$", ErrorMessageResourceName = "numofchildrenRxError", ErrorMessageResourceType = typeof(Resources.Worker))]
        public byte numofchildren { get; set; }
        //
        [Required(ErrorMessageResourceName = "incomeID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("incomeID", NameResourceType = typeof(Resources.Worker))]
        public byte incomeID { get; set; }
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
        // TODO: require unique number
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        public int dwccardnum { get; set; }
        //
        [Required(ErrorMessageResourceName = "neighborhoodID", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("neighborhoodID", NameResourceType = typeof(Resources.Worker))]
        public byte neighborhoodID { get; set; }
        //
        [LocalizedDisplayName("immigrantrefugee", NameResourceType = typeof(Resources.Worker))]
        public bool immigrantrefugee { get; set; }
        //  
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
        [Required(ErrorMessageResourceName = "countryoforigin", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("countryoforigin", NameResourceType = typeof(Resources.Worker))]
        public string countryoforigin { get; set; }
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
        //TODO: how to handle imaage from SQL?
        [Required(ErrorMessageResourceName = "memberexpirationdate", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("memberexpirationdate", NameResourceType = typeof(Resources.Worker))]
        public DateTime memberexpirationdate { get; set; }
        //
        //[StringLength(16, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Worker))]
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
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public string Createdby { get; set; }
        public string Updatedby { get; set; }
    }
}

