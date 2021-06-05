using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportVehiclesAvailabilityService : IService<TransportVehicleAvailability>
    {

    }
    public class TransportVehiclesAvailabilityService : ServiceBase2<TransportVehicleAvailability>, ITransportVehiclesAvailabilityService
    {
        private readonly IMapper map;

        public TransportVehiclesAvailabilityService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TVA";
        }
    }
}
