using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.ViewModels
{
    public class TransportRule
    {
        public int id { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }

        public string key { get; set; }
        public string lookupKey { get; set; }
        public string zoneLabel { get; set; }
        public string zipcodes { get; set; }
        public List<TransportCostRule> costRules { get; set; }
    }

    public class TransportCostRule
    {
        public int id { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }

        public int transportRuleId { get; set; }
        public int minWorker { get; set; }
        public int maxWorker { get; set; }
        public double cost { get; set; }
    }
}