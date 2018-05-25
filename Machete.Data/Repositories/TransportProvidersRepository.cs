using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

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

        override public IQueryable<TransportProvider> GetManyQ(Func<TransportProvider, bool> where)
        {
            return dbset.Include(a => a.AvailabilityRules).AsNoTracking().Where(where).AsQueryable();
        }
    }
}