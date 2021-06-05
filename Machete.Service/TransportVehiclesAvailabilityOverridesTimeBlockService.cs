using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportVehiclesAvailabilityOverrideTimeBlocksService : IService<TransportVehicleAvailabilityOverrideTimeBlock>
    {

    }
    public class TransportVehiclesAvailabilityOverridesTimeBlockService : ServiceBase2<TransportVehicleAvailabilityOverrideTimeBlock>, ITransportVehiclesAvailabilityOverrideTimeBlocksService
    {
        private readonly IMapper map;

        public TransportVehiclesAvailabilityOverridesTimeBlockService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TVA-TB";
        }
    }
}
