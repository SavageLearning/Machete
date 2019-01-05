using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Activity : Record
    {
        public Activity()
        {
            idString = "activity";
        }

        public string idChild
        {
            get
            {
                return "asi" + this.ID + "-";
            }
        }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }

        public virtual ICollection<ActivitySignin> Signins { get; set; }
        //
        [LocalizedDisplayName("name", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public int nameID { get; set; }
        // This is the simplest hack i can come up with to keep search on these two Lookups (previously implement in the app)
        // but push it to the database. 
        [StringLength(50)]
        public string nameEN { get; set; }
        [StringLength(50)]
        public string nameES { get; set; }
        //
        [LocalizedDisplayName("type", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "typerequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public int typeID { get; set; }
        //
        [StringLength(50)]
        public string typeEN { get; set; }
        //
        [StringLength(50)]
        public string typeES { get; set; }
        //
        [LocalizedDisplayName("dateStart", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "dateStartrequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public DateTime dateStart { get; set; }
        //
        [LocalizedDisplayName("dateEnd", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "dateEndrequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public DateTime dateEnd { get; set; }

        [LocalizedDisplayName("recurring", NameResourceType = typeof(Resources.Activity))]
        public bool recurring { get; set; }
        public int firstID { get; set; }
        //
        [LocalizedDisplayName("teacher", NameResourceType = typeof(Resources.Activity))]
        [Required(ErrorMessageResourceName = "teacherrequired", ErrorMessageResourceType = typeof(Resources.Activity))]
        public string teacher { get; set; }
        //
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.Activity))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Activity))]
        public string notes { get; set; }
    }

    public class ActivityList
    {
        public int ID { get; set; }
        public string AID { get; set; }         // duplicate names for ids
        public string recordid { get; set; }    // because legacy reasons
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string count { get; set; }
        public string teacher { get; set; }
        public string dateStart { get; set; }
        public string dateEnd { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }
    }
}