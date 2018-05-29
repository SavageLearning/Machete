using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
{
    public class TransportProvider : BaseModel
    {
        public string key { get; set; }
        public string lookupKey { get; set; }
        public int day { get; set; }
        public bool active { get; set; }
        public string text { get; set; }
        public List<TransportProviderAvailability> availabilityRules { get; set; }
    }
}