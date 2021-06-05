using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportVehiclesScheduleLoadHistoryService : IService<TransportVehicleScheduleLoadHistory>
    {

    }
    public class TransportVehiclesScheduleLoadHistoryService : ServiceBase2<TransportVehicleScheduleLoadHistory>, ITransportVehiclesScheduleLoadHistoryService
    {
        private readonly IMapper map;

        public TransportVehiclesScheduleLoadHistoryService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TVSLH";
        }
    }
}
