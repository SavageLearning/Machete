using System;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class TransportProviderAvailabilityOverride : Record
    {
        public virtual TransportProvider Provider { get; set; }
        public int transportProviderID { get; set; }
        [StringLength(50)]
        public string key { get; set; } // TODO: delete this attrib
        [StringLength(50)]
        public string lookupKey { get; set; } // TODO: delete this attrib
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool available { get; set; }
    }
}
