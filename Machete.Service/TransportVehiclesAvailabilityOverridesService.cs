using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportVehiclesAvailabilityOverridesService : IService<TransportVehicleAvailabilityOverride>
    {

    }
    public class TransportVehiclesAvailabilityOverridesService : ServiceBase2<TransportVehicleAvailabilityOverride>, ITransportVehiclesAvailabilityOverridesService
    {
        private readonly IMapper map;

        public TransportVehiclesAvailabilityOverridesService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TVA";
        }
    }
}
