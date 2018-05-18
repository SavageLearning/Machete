using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.ViewModels
{
    public class TransportProvider : BaseModel
    {
        public string key { get; set; }
        public string lookupKey { get; set; }
        public int day { get; set; }
        public bool available { get; set; }
        public string text { get; set; }

    }
}