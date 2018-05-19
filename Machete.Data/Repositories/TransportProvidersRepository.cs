using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Data.Entity;
using System.Linq;

namespace Machete.Data
{
    public interface ITransportProvidersRepository : IRepository<TransportProvider>
    {
    }
    public class TransportProvidersRepository : RepositoryBase<TransportProvider>, ITransportProvidersRepository
    {

        public TransportProvidersRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        { }

        override public IQueryable<TransportProvider> GetAllQ()
        {
            return dbset.Include(a => a.AvailabilityRules).AsNoTracking().AsQueryable();
        }
    

    }
}