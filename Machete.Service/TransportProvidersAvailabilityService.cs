using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportProvidersAvailabilityService : IService<TransportProviderAvailabilities>
    {

    }
    public class TransportProvidersAvailabilityService : ServiceBase<TransportProviderAvailabilities>, ITransportProvidersAvailabilityService
    {
        private readonly IMapper map;

        public TransportProvidersAvailabilityService(ITransportProvidersAvailabilityRepository repo, IUnitOfWork uow, IMapper map) : base(repo, uow)
        {
            this.map = map;
            this.logPrefix = "TransportRule";
        }
    }
}
