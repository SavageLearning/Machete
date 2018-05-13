using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportProvidersService : IService<TransportProvider>
    {

    }
    public class TransportProvidersService : ServiceBase<TransportProvider>, ITransportProvidersService
    {
        private readonly IMapper map;

        public TransportProvidersService(ITransportProvidersRepository repo, IUnitOfWork uow, IMapper map) : base(repo, uow)
        {
            this.map = map;
            this.logPrefix = "TransportRule";
        }
    }
}
