using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Machete.Service
{
    public interface ITransportVehiclesScheduleService : IService<TransportVehicleSchedule>
    {
        void populateScheduleFor(DateTime date, string user);
    }
    public class TransportVehiclesScheduleService : ServiceBase2<TransportVehicleSchedule>, ITransportVehiclesScheduleService
    {
        private readonly IMapper map;
        private readonly ITransportVehiclesScheduleLoadHistoryService lh;

        public TransportVehiclesScheduleService(IDatabaseFactory db, ITransportVehiclesScheduleLoadHistoryService loadHistory, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TVS";
            this.lh = loadHistory;
        }

        public override IEnumerable<TransportVehicleSchedule> GetAll()
        {
            return dbset.Include(api_refactor => api_refactor.TransportVehicle).AsNoTracking().AsQueryable();
        }
        // This method should be idempotent'ish. If a schedule entry exists, this
        // function shouldn't overwrite it or create a conflicting entry. 
        public void populateScheduleFor(DateTime date, string user)
        {
            var today = date.DayOfWeek;
            var vehicles = db.TransportVehicles.Where(tv => tv.Active == true).ToList();
            bool dirty;
            //
            // cycle through vehicles
            foreach (var v in vehicles)
            {
                // If there's been a schedule-load for this day & vehicle, skip
                var result = db.TransportVehicleScheduleLoads.Where(h =>
                    DbFunctions.DiffDays(h.ExecutionTime, date) == 0 ? true : false &&
                    v.ID == h.ID
                ).Count(); 
                if (result > 0) { continue; }
                dirty = false;
                // if there's more than 1 availability record for a cvehicle
                // on a given day, then throw exception
                var tva = v.Availability.Single(a => a.Day == (int)today);
                var timeblocks = tva.TimeBlocks.ToList();
                var scheds = db.TransportVehicleSchedules
                                .Where(s =>
                                    DbFunctions.DiffDays(s.StartTime, date) == 0 &&
                                    s.TransportVehicleId == v.ID
                                ).ToList();

                foreach (var tb in timeblocks)
                {
                    var exists = false;
                    foreach (var s in scheds)
                    {
                        // Comparing time boundaries to see if a schedule already exists that 
                        // overlaps with tva-tb. 
                        if (
                             (s.StartTime <= tb.StartTime && s.EndTime > tb.EndTime) ||
                             (s.StartTime > tb.StartTime && s.StartTime < tb.EndTime) ||
                             (s.EndTime > tb.StartTime && s.EndTime < tb.EndTime)
                           ) {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        dirty = true;
                        Create(new TransportVehicleSchedule() {
                            StartTime = tb.StartTime,
                            EndTime = tb.EndTime,
                            TransportVehicle = v                           
                        }, user);
                    }
                }
                // 
                if (dirty)
                {
                    lh.Create(new TransportVehicleScheduleLoadHistory() {
                        TransportVehicle = v,
                        ExecutionTime = DateTime.Now
                    }, user);
                }
            }
            db.SaveChanges();
        }
        
    }
}
