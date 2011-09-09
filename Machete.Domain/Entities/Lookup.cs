using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Lookup
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string category { get; set; } //Race, Language, M-Status
        [StringLength(50)]
        public string text_EN { get; set; }
        [StringLength(50)]
        public string text_ES { get; set; }    
        public bool selected { get; set; }
        //
        // Skill specific fields
        //
        [StringLength(50)]
        public string subcategory { get; set; } // used in skills; allows hierarchy for skill match
        public int? level { get; set; }      //progression, 0 if unused
        public double? wage { get; set; }
        public int? minHour { get; set; }
        public bool? fixedJob { get; set; }

        public int sortorder { get; set; }
        public int typeOfWorkID { get; set; } // 1 DWC, 2 HHH
        public bool speciality { get; set; }
        [StringLength(1)]
        public string ltrCode { get; set; }
    }
}
