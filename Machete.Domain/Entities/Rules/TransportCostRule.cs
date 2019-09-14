using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportCostRule : Record
    {
        public virtual TransportRule transportRule { get; set; }
        public int transportRuleID { get; set; }
        public int minWorker { get; set; }
        public int maxWorker { get; set; }
        public double cost { get; set; }
    }
}
