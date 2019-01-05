using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Machete.Data
{
    public static class MacheteTransports
    {
        public static void Initialize(MacheteContext c)
        {
            if (c.TransportProviders.Count() == 0)
            {
                if (c.Database.GetDbConnection().GetType().Name == "SqlServerConnection")
                {
                    c.Database.ExecuteSqlCommand(@"insert into dbo.TransportProviders
                        ( [key], text_EN, text_ES, defaultAttribute, sortorder, active, datecreated, dateupdated, Createdby, Updatedby )
                            select [key], text_EN, text_ES, selected, sortorder, active, datecreated, dateupdated, Createdby, Updatedby
                            from dbo.Lookups
                            where category = 'transportmethod'");
                    c.Commit();
                } else {
                    c.Database.ExecuteSqlCommand(@"insert into TransportProviders
                        ( [key], text_EN, text_ES, defaultAttribute, sortorder, active, datecreated, dateupdated, Createdby, Updatedby )
                            select [key], text_EN, text_ES, selected, sortorder, active, datecreated, dateupdated, Createdby, Updatedby
                            from Lookups
                            where category = 'transportmethod'");
                    c.Commit();
                }
            }

            if (c.TransportProvidersAvailability.Count() == 0)
            {
                var providers = c.TransportProviders.ToList();
                foreach (var p in providers)
                {
                    p.AvailabilityRules = new List<TransportProviderAvailability>();
                    for (var i = 0; i < 7; i++)
                    {
                        p.AvailabilityRules.Add(new Domain.TransportProviderAvailability
                        {
                            transportProviderID = p.ID,
                            day = i,
                            available = i == 0 ? p.key == "transport_pickup" ? true : false : true,
                            datecreated = DateTime.Now,
                            dateupdated = DateTime.Now,
                            createdby = "Init T. Script",
                            updatedby = "Init T. Script"
                        });
                    }
                }
                c.Commit();
            }

        }
    }
}
