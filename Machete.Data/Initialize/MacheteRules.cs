using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Data.Initialize
{
    public static class MacheteRules
    {
        private static List<TransportRule> transportList = new List<TransportRule>
        {
            new TransportRule {
                ID=1,
                key = "bus_inside_zone",
                lookupKey =  "transport_bus",
                zoneLabel = "inside",
                zipcodes = "",
                costRules = new List<TransportCostRule>
                {
                    new TransportCostRule {ID=1, minWorker=0, maxWorker=100, cost=5 }
                }
            },
            new TransportRule {
                ID=2,
                key = "bus_outside_zone",
                lookupKey =  "transport_bus",
                zoneLabel = "outside",
                zipcodes = "",
                costRules = new List<TransportCostRule>
                {
                    new TransportCostRule {ID=2, minWorker=0, maxWorker=100, cost=10 }
                }
            },
            new TransportRule {
                ID=3,
                key = "van_inside_zone",
                lookupKey =  "transport_van",
                zoneLabel = "inside",
                zipcodes = "",
                costRules = new List<TransportCostRule>
                {
                    new TransportCostRule {ID=3, minWorker=0, maxWorker=1, cost=15 },
                    new TransportCostRule {ID=4, minWorker=1, maxWorker=2, cost=5 },
                    new TransportCostRule {ID=5, minWorker=2, maxWorker=10, cost=0 },
                }
            },
            new TransportRule {
                ID=4,
                key = "van_outside_zone",
                lookupKey =  "transport_van",
                zoneLabel = "outside",
                zipcodes = "",
                costRules = new List<TransportCostRule>
                {
                    new TransportCostRule {ID=6, minWorker=0, maxWorker=1, cost=25 },
                    new TransportCostRule {ID=7, minWorker=1, maxWorker=10, cost=0 },
                }
            },
            new TransportRule {
                ID=5,
                key = "pickup",
                lookupKey =  "transport_pickup",
                zoneLabel = "all",
                zipcodes = "",
                costRules = new List<TransportCostRule>
                {
                    new TransportCostRule {ID=8, minWorker=0, maxWorker=100, cost=0 },
                }
            },
        };
    }
}
