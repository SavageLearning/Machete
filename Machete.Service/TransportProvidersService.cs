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
    public class TransportProvidersService : ServiceBase<TransportProvider>, ITransportProvidersService
    {
        private readonly IMapper map;
        private readonly ITransportProvidersAvailabilityService tpaServ;

        public TransportProvidersService(
            ITransportProvidersRepository repo, 
            ITransportProvidersAvailabilityService tpaServ,
            IUnitOfWork uow, 
            IMapper map) : base(repo, uow)
        {
            this.tpaServ = tpaServ;
            this.map = map;
            this.logPrefix = "TransportRule";
        }

        public TransportProviderAvailability CreateAvailability(int id, TransportProviderAvailability tpa, string user)
        {
            TransportProviderAvailability entity;
            var provider = Get(id);
            if (provider.AvailabilityRules.SingleOrDefault(a => a.day == tpa.day) == null)
            {
                tpa.Provider = provider;
                tpa.transportProviderID = provider.ID;
                entity = tpaServ.Create(tpa, user);
                return entity;
            }
            throw new MacheteValidationException("Availability record already exists");
        }
}
}
