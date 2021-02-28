using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface ITransportCostRuleService : IService<TransportCostRule> {}
    public class TransportCostRuleService : ServiceBase2<TransportCostRule>, ITransportCostRuleService
    {
        public TransportCostRuleService(IDatabaseFactory db, IMapper map)  : base(db, map) {}
    }
}
