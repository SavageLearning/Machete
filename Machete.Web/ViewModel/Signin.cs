using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Web.ViewModel
{
    public abstract class Signin : Record
    {
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(Resources.Worker))]
        public virtual int dwccardnum { get; set; }
        public int? memberStatusID { get; set; }
        public DateTime dateforsignin { get; set; }
        public Double timeZoneOffset { get; set; }
    }
}