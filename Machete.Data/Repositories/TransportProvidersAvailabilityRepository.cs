using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Data
{
    public interface ITransportProvidersAvailabilityRepository : IRepository<TransportProviderAvailabilities>
    {
    }
    public class TransportProvidersAvailabilityRepository : RepositoryBase<TransportProviderAvailabilities>, ITransportProvidersAvailabilityRepository
    {

        public TransportProvidersAvailabilityRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        { }

    }
}