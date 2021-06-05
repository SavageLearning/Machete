using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportProvidersAvailabilityOverrideService : IService<TransportProviderAvailabilityOverride>
    {

    }
    public class TransportProvidersAvailabilityOverrideService : ServiceBase2<TransportProviderAvailabilityOverride>, ITransportProvidersAvailabilityOverrideService
    {
        private readonly IMapper map;

        public TransportProvidersAvailabilityOverrideService(IDatabaseFactory db, IMapper map) : base(db)
        {
            this.map = map;
            this.logPrefix = "TPA-Override";
        }
    }
}
