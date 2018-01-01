using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportRule : Record
    {
        [StringLength(50)]
        public string key { get; set; }
        [StringLength(50)]
        public string lookupKey { get; set; }
        [StringLength(50)]
        public string zoneLabel { get; set; }
        [StringLength(1000)]
        public string zipcodes { get; set; }
        [StringLength(50)]
        public virtual ICollection<TransportCostRule> costRules { get; set; }
    }
}
