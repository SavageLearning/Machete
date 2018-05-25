using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportProvider : Attribute
    {
        public virtual ICollection<TransportProviderAvailability> AvailabilityRules { get; set; }

    }
}
