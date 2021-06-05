using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportVehiclesAvailabilityTimeBlockService : IService<TransportVehicleAvailabilityTimeBlock>
    {

    }
    public class TransportVehiclesAvailabilityTimeBlockService : ServiceBase2<TransportVehicleAvailabilityTimeBlock>, ITransportVehiclesAvailabilityTimeBlockService
    {
        private readonly IMapper map;

        public TransportVehiclesAvailabilityTimeBlockService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TVA-TB";
        }
    }
}
