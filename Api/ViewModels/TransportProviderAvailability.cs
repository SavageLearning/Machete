using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.ViewModels
{
    public class TransportProviderAvailability : BaseModel
    {
        public int day { get; set; }
        public bool available { get; set; }
    }
}