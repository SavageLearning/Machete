using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Data
{
    public static class MacheteTransportVehicles
    {
        public static void Initialize(MacheteContext c)
        {
            var van1 = "Van 1";
            var van2 = "Van 2";
            if (c.TransportVehicles.Count() == 0)
            {
                c.TransportVehicles.Add(
                    new Domain.TransportVehicle {
                        Name = van1,
                        Capacity = 10,
                        Active = true,
                        TransportProviderID = c.TransportProviders.Single(p => p.key == "transport_van").ID,
                        datecreated = DateTime.Now,
                        dateupdated = DateTime.Now,
                        createdby = "Init T. Script",
                        updatedby = "Init T. Script"
                    });
                c.TransportVehicles.Add(
                    new Domain.TransportVehicle
                    {
                        Name = van2,
                        Capacity = 8,
                        Active = true,
                        TransportProviderID = c.TransportProviders.Single(p => p.key == "transport_van").ID,
                        datecreated = DateTime.Now,
                        dateupdated = DateTime.Now,
                        createdby = "Init T. Script",
                        updatedby = "Init T. Script"
                    });
                c.SaveChanges();
            }
            var v1 = c.TransportVehicles.Single(v => v.Name == van1);
            var v2 = c.TransportVehicles.Single(v => v.Name == van2);

            if (c.TransportVehicleAvailabilities.Count() == 0)
            {
                for (var i =0; i < 7; i++)
                {
                    c.TransportVehicleAvailabilities.Add(new Domain.TransportVehicleAvailability
                    {
                        Day = i, 
                        TransportVehicle = v1,
                        TransportVehicleID = v1.ID,
                        datecreated = DateTime.Now,
                        dateupdated = DateTime.Now,
                        createdby = "Init T. Script",
                        updatedby = "Init T. Script"
                    });
                    c.TransportVehicleAvailabilities.Add(new Domain.TransportVehicleAvailability
                    {
                        Day = i,
                        TransportVehicle = v2,
                        TransportVehicleID = v2.ID,
                        datecreated = DateTime.Now,
                        dateupdated = DateTime.Now,
                        createdby = "Init T. Script",
                        updatedby = "Init T. Script"
                    });
                }
            }
            c.SaveChanges();

            if (c.TransportVehicleAvailabilityTimeBlocks.Count() == 0)
            {
                var tva_array = c.TransportVehicleAvailabilities.Where(a => a.TransportVehicleID == v1.ID);
                foreach (var tva in tva_array)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        c.TransportVehicleAvailabilityTimeBlocks.Add(
                            new Domain.TransportVehicleAvailabilityTimeBlock
                            {
                                TransportVehicleAvailability = tva,
                                TransportVehicleAvailabilityID = tva.ID,
                                StartTime = String.Format("",)DateTime.MinValue.AddHours(9+i),
                                EndTime = DateTime.MinValue.AddHours(9+i).AddMinutes(45),
                                datecreated = DateTime.Now,
                                dateupdated = DateTime.Now,
                                createdby = "Init T. Script",
                                updatedby = "Init T. Script"
                            }
                        );
                    }
                }
                c.SaveChanges();
            }
        }
    }
}
