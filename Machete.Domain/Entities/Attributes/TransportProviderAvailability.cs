using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class TransportProviderAvailability : Record
    {
        public virtual TransportProvider Provider { get; set; }
        public int transportProviderID { get; set; }
        [StringLength(50)]
        public string key { get; set; } // TODO: delete this attrib
        [StringLength(50)]
        public string lookupKey { get; set; } // TODO: delete this attrib
        public int day { get; set; }
        public bool available { get; set; }
    }
}
