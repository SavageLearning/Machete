using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Domain
{
    [Table("TransportProviderAvailabilities")]
    public class TransportProviderAvailability : Record
    {
        public int transportProviderID { get; set; }
        [StringLength(50)]
        public string key { get; set; } // TODO: delete this attrib
        [StringLength(50)]
        public string lookupKey { get; set; } // TODO: delete this attrib
        public int day { get; set; }
        public bool available { get; set; }
        
        public virtual TransportProvider TransportProvider { get; set; }
    }
}
