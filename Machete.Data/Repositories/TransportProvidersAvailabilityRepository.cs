using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Data
{
    public interface ITransportProvidersAvailabilityRepository : IRepository<TransportProviderAvailability>
    {
    }
    public class TransportProvidersAvailabilityRepository : RepositoryBase<TransportProviderAvailability>, ITransportProvidersAvailabilityRepository
    {

        public TransportProvidersAvailabilityRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        { }

    }
}