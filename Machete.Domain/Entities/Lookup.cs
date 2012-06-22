using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Lookup : Record
    {
        //public int ID { get; set; }
        [LocalizedDisplayName("category", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        [Required(ErrorMessageResourceName = "categoryRequired", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string category { get; set; } //Race, Language, M-Status
        //
        [LocalizedDisplayName("text_EN", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        [Required(ErrorMessageResourceName = "text_ENRequired", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string text_EN { get; set; }
        //
        [LocalizedDisplayName("text_ES", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        [Required(ErrorMessageResourceName = "text_ESRequired", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string text_ES { get; set; }
        //
        [LocalizedDisplayName("selected", NameResourceType = typeof(Resources.Lookup))]
        public bool selected { get; set; }
        //
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
        public int? typeOfWorkID { get; set; } // 1 DWC, 2 HHH
        [LocalizedDisplayName("speciality", NameResourceType = typeof(Resources.Lookup))]
        public bool speciality { get; set; }
        [LocalizedDisplayName("ltrCode", NameResourceType = typeof(Resources.Lookup))]
        [StringLength(1, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Lookup))]
        public string ltrCode { get; set; }
    }


}
