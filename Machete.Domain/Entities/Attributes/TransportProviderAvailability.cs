using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class TransportProviderAvailability : Record
    {
        [StringLength(50)]
        public string key { get; set; }
        [StringLength(50)]
        public string lookupKey { get; set; }
        public int day { get; set; }
        public bool available { get; set; }
    }
}
