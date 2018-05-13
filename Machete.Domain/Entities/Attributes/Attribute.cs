using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class Attribute : Record
    {
        [StringLength(50)]
        public string key { get; set; }
        [StringLength(50)]
        public string text_EN { get; set; }
        [StringLength(50)]
        public string text_ES { get; set; }
        public bool defaultAttribute { get; set; } // used to be called selected
        public int? sortorder { get; set; }
        public bool active { get; set; }
    }
}
