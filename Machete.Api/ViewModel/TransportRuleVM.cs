using System.Collections.Generic;

namespace Machete.Api.ViewModel
{
    public class TransportRuleVM : RecordVM
    {
        public string key { get; set; }
        public string lookupKey { get; set; }
        public string zoneLabel { get; set; }
        public string zipcodes { get; set; }
        public List<TransportCostRuleVM> costRules { get; set; }
    }

    public class TransportRuleListVM : ListVM
    {

    }

    public class TransportCostRuleVM : RecordVM
    {
        public int transportRuleId { get; set; }
        public int minWorker { get; set; }
        public int maxWorker { get; set; }
        public double cost { get; set; }
    }

    public class TransportCostRuleListVM : ListVM
    {

    }
}