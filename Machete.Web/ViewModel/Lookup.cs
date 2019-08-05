using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Lookup : Record
    {
        public IDefaults def { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string recordid { get; set; }

        public Lookup()
        {
            idString = "lookup";
        }
        //public int ID { get; set; }
        [LocalizedDisplayName("category", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        [Required(ErrorMessageResourceName = "categoryRequired", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string category { get; set; } //Race, Language, M-Status

        [LocalizedDisplayName("text_EN", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        [Required(ErrorMessageResourceName = "text_ENRequired", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string text_EN { get; set; }

        [LocalizedDisplayName("text_ES", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        [Required(ErrorMessageResourceName = "text_ESRequired", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string text_ES { get; set; }

        [LocalizedDisplayName("selected", NameResourceType = typeof(Resources.Lookup))]
        public bool selected { get; set; }

        // Skill specific fields
        [LocalizedDisplayName("subcategory", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string subcategory { get; set; } // used in skills; allows hierarchy for skill match

        [LocalizedDisplayName("level", NameResourceType = typeof(Resources.Lookup))]
        public int? level { get; set; }      //progression, 0 if unused

        [LocalizedDisplayName("wage", NameResourceType = typeof(Resources.Lookup))]
        public double? wage { get; set; }

        [LocalizedDisplayName("minHour", NameResourceType = typeof(Resources.Lookup))]
        public int? minHour { get; set; }

        [LocalizedDisplayName("fixedJob", NameResourceType = typeof(Resources.Lookup))]
        public bool? fixedJob { get; set; }

        [LocalizedDisplayName("sortorder", NameResourceType = typeof(Resources.Lookup))]
        public int? sortorder { get; set; }

        [LocalizedDisplayName("typeOfWorkID", NameResourceType = typeof(Resources.Lookup))]
        public int? typeOfWorkID { get; set; }

        [LocalizedDisplayName("speciality", NameResourceType = typeof(Resources.Lookup))]
        public bool speciality { get; set; }

        [LocalizedDisplayName("ltrCode", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(3, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string ltrCode { get; set; }

        [LocalizedDisplayName("emailTemplate", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(8000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string emailTemplate { get; set; }

        [LocalizedDisplayName("skillDescriptionEn", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(300, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string skillDescriptionEn { get; set; }

        [LocalizedDisplayName("skillDescriptionEs", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(300, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string skillDescriptionEs { get; set; }

        [LocalizedDisplayName("minimumCost", NameResourceType = typeof(Resources.Lookup))]
        public double? minimumCost { get; set; }

        /// <summary>
        /// Set only for records that correspond to internal component or status
        /// </summary>
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string key { get; set; }

        [LocalizedDisplayName("active", NameResourceType = typeof(Resources.Lookup))]
        public bool active { get; set; }
    }

    public class LookupList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string category { get; set; }
        public string selected { get; set; }
        public string text_EN { get; set; }
        public string text_ES { get; set; }
        public string subcategory { get; set; }
        public string level { get; set; }
        public string ltrCode { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }
        public string recordid { get; set; }    
        public bool active {get; set; }
    }
}