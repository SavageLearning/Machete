using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Linq;

namespace Machete.Service
{
    public interface ITransportProvidersService : IService<TransportProvider>
    {
        TransportProviderAvailability CreateAvailability(int id, TransportProviderAvailability tpa, string user);
    }
    public class TransportProvidersService : ServiceBase2<TransportProvider>, ITransportProvidersService
    {
        private readonly ITransportProvidersAvailabilityService tpaServ;

        public TransportProvidersService(
            IDatabaseFactory db,
            ITransportProvidersAvailabilityService tpaServ,
            IMapper map) : base(db, map)
        {
            this.tpaServ = tpaServ;
        }

        public TransportProviderAvailability CreateAvailability(int id, TransportProviderAvailability tpa, string user)
        {
            TransportProviderAvailability entity;
            var provider = Get(id);
            if (provider.AvailabilityRules.SingleOrDefault(a => a.day == tpa.day) == null)
            {
                tpa.TransportProvider = provider;
                tpa.transportProviderID = provider.ID;
                entity = tpaServ.Create(tpa, user);
                return entity;
            }
            throw new MacheteValidationException("Availability record already exists");
        }
}
}
