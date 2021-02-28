using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportRuleService : IService<TransportRule> {}
    public class TransportRuleService : ServiceBase2<TransportRule>, ITransportRuleService
    {
        public TransportRuleService(IDatabaseFactory db, IMapper map) : base(db, map) {}
    }
}
