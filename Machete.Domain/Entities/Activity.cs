using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Activity : Record
    {
        public virtual ICollection<ActivitySignin> Signins { get; set; }
        //
        [LocalizedDisplayName("name", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public int name { get; set; } // lookup
        //
        [LocalizedDisplayName("type", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "typerequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public int type { get; set; }
        //
        [LocalizedDisplayName("dateStart", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "dateStartrequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public DateTime dateStart { get; set; }
        //
        [LocalizedDisplayName("dateEnd", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "dateEndrequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public DateTime dateEnd { get; set; }
        //
        [LocalizedDisplayName("teacher", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "teacherrequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public string teacher { get; set; }
        //
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.Activity))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Activity))]
        public string notes { get; set; }
    }

    public class ActivitySignin: Signin
    {
        public virtual Activity Activity { get; set; }
        public int ActivityID { get; set; }
    }
}
