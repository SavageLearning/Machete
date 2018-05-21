using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
{
    public class TransportRule : BaseModel
    {
        public string key { get; set; }
        public string lookupKey { get; set; }
        public string zoneLabel { get; set; }
        public string zipcodes { get; set; }
        public List<TransportCostRule> costRules { get; set; }
    }

    public class TransportCostRule : BaseModel
    {
        public int transportRuleId { get; set; }
        public int minWorker { get; set; }
        public int maxWorker { get; set; }
        public double cost { get; set; }
    }
}