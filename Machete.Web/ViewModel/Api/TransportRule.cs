using System.Collections.Generic;

namespace Machete.Web.ViewModel.Api
{
    public class TransportRuleVM : RecordVM
    {
        public string key { get; set; }
        public string lookupKey { get; set; }
        public string zoneLabel { get; set; }
        public string zipcodes { get; set; }
        public List<TransportCostRuleVM> costRules { get; set; }
    }

    public class TransportCostRuleVM : RecordVM
    {
        public int transportRuleId { get; set; }
        public int minWorker { get; set; }
        public int maxWorker { get; set; }
        public double cost { get; set; }
    }
}