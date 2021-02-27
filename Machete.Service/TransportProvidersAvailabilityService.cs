using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportProvidersAvailabilityService : IService<TransportProviderAvailability> {}
    public class TransportProvidersAvailabilityService : ServiceBase2<TransportProviderAvailability>, ITransportProvidersAvailabilityService
    {
        public TransportProvidersAvailabilityService(IDatabaseFactory db, IMapper map) : base(db, map) {}
    }
}
