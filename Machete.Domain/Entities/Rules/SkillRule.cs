using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Eventually I plan to break out the Skills from the lookups. Lookups is horribly overloaded.
// RIght now I am leaving them in the Lookups, until after the UI is migrated

//namespace Machete.Domain
//{
//    public class SkillRule :Record
//    {
//        [StringLength(30), Required]
//        public string key { get; set; }
//        [StringLength(50)]
//        public string subcategory { get; set; } // category is skill, the object type
//        public int? level { get; set; }
//        public double wage { get; set; }
//        public int? minHour { get; set; }
//        public int? maxHour { get; set; }
//        public double? minimumCost { get; set; }
//        public Boolean? fixedJob { get; set; }
//        public Boolean speciality { get; set; }
//        [StringLength(3)]
//        public string ltrCode { get; set; }
//        [StringLength(300)]
//        public string descriptionEn { get; set; }
//        [StringLength(300)]
//        public string descriptionEs { get; set; }
//        public bool active { get; set; }
//    }
//}
