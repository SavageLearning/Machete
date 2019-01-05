using System;
using System.Collections.Generic;
using Machete.Domain;
using Microsoft.EntityFrameworkCore;

namespace Machete.Data.Initialize
{
    public static class MacheteRules
    {
        public static void Initialize(MacheteContext context)
        {
            var transportRules = new List<TransportRule> {
                new TransportRule
                {
                    ID = 1,
                    key = "bus_inside_zone",
                    lookupKey = "transport_bus",
                    zoneLabel = "inside",
                    zipcodes = "98101,98102,98103,98104,98105,98106,98107,98108,98109,98112,98115,98116,98117,98118,98119,98121,98122,98125,98126,98133,98134,98136,98144,98146,98154,98161,98164,98168,98174,98178,98195,98199",
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script",
                },
                new TransportRule
                {
                    ID = 2,
                    key = "bus_outside_zone",
                    lookupKey = "transport_bus",
                    zoneLabel = "outside",
                    zipcodes = "98001,98003,98004,98005,98006,98007,98008,98020,98026,98027,98028,98029,98030,98031,98032,98033,98034,98037,98039,98040,98043,98052,98055,98056,98057,98059,98074,98075,98155,98158,98166,98168,98188",
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script",
                },
                new TransportRule
                {
                    ID = 3,
                    key = "van_inside_zone",
                    lookupKey = "transport_van",
                    zoneLabel = "inside",
                    zipcodes = "98101,98102,98103,98104,98105,98106,98107,98108,98109,98112,98115,98116,98117,98118,98119,98121,98122,98125,98126,98133,98134,98136,98144,98146,98154,98161,98164,98168,98174,98177,98178,98195,98199",
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script",
                },
                new TransportRule
                {
                    ID = 4,
                    key = "van_outside_zone",
                    lookupKey = "transport_van",
                    zoneLabel = "outside",
                    zipcodes = "98001,98003,98004,98005,98006,98007,98008,9801198020,98021,98026,98027,98028,98029,98030,98031,98032,98033,98034,98037,98039,98040,98043,98052,98055,98056,98057,98059,98074,98075,98155,98158,98166,98168",
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script",
                },
                new TransportRule
                {
                    ID = 5,
                    key = "pickup",
                    lookupKey = "transport_pickup",
                    zoneLabel = "all",
                    zipcodes = "*",
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script",
                },
            };
            
            transportRules.ForEach(rule =>
                context.TransportRules.Add(rule)
            );
            context.Database.OpenConnection();
            if (context.Database.GetDbConnection().GetType().Name == "SqlConnection")
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.TransportRules ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.TransportRules OFF");
            } else {
                context.SaveChanges();
            }
            
            transportRules.Find(rule => rule.ID == 1).costRules = new List<TransportCostRule> {
                new TransportCostRule { ID = 1, minWorker = 0, maxWorker = 100, cost = 5, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" }
            };
            transportRules.Find(rule => rule.ID == 2).costRules = new List<TransportCostRule> {
                new TransportCostRule { ID = 2, minWorker = 0, maxWorker = 100, cost = 10, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" }
            };
            transportRules.Find(rule => rule.ID == 3).costRules = new List<TransportCostRule> {
                new TransportCostRule { ID = 3, minWorker = 0, maxWorker = 1, cost = 15, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" },
                new TransportCostRule { ID = 4, minWorker = 1, maxWorker = 2, cost = 5, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" },
                new TransportCostRule { ID = 5, minWorker = 2, maxWorker = 10, cost = 0, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" }
            };
            transportRules.Find(rule => rule.ID == 4).costRules = new List<TransportCostRule> {
                new TransportCostRule { ID = 6, minWorker = 0, maxWorker = 1, cost = 25, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" },
                new TransportCostRule { ID = 7, minWorker = 1, maxWorker = 10, cost = 0, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" }
            };      
            transportRules.Find(rule => rule.ID == 5).costRules = new List<TransportCostRule> {
                new TransportCostRule { ID = 8, minWorker = 0, maxWorker = 100, cost = 0, datecreated = DateTime.Now, dateupdated = DateTime.Now, createdby = "Init T. Script", updatedby = "Init T. Script" }
            };
            
            context.Database.OpenConnection();
            if (context.Database.GetDbConnection().GetType().Name == "SqlConnection")
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.TransportCostRules ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.TransportCostRules OFF");
            } else {
                context.SaveChanges();
            }
            // Schedule Rules
            for (var i = 0; i < 7; i++)
            {
                context.ScheduleRules.Add(new ScheduleRule {
                    day = i,
                    leadHours = 48,
                    minStartMin = 420,
                    maxEndMin = 1020,
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    createdby = "Init T. Script",
                    updatedby = "Init T. Script"
                });
                context.Commit();
            }
        }
    }
}
