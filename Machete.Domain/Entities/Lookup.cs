using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Domain
{
    public class Lookup
    {
        public int ID { get; set; }
        public string category { get; set; } //Race, Language, M-Status
        public int? level { get; set; }      //progression, 0 if unused
        public double? wage { get; set; }
        public int? minHour { get; set; }
        public bool? fixedJob { get; set; }
        public string text_EN { get; set; }
        public string text_ES { get; set; }    
        public bool selected { get; set; }
    }
}
