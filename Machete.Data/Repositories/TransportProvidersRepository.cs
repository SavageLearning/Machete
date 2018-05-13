using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Data
{
    public interface ITransportProvidersRepository : IRepository<TransportProvider>
    {
    }
    public class TransportProvidersRepository : RepositoryBase<TransportProvider>, ITransportProvidersRepository
    {

        public TransportProvidersRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        { }

    }
}