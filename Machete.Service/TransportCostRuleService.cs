using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service
{
    public interface ITransportCostRuleService : IService<TransportCostRule>
    {

    }
    public class TransportCostRuleService : ServiceBase<TransportCostRule>, ITransportCostRuleService
    {
        private readonly IMapper map;

        public TransportCostRuleService(ITransportCostRuleRepository repo, IUnitOfWork uow, IMapper map) : base(repo, uow)
        {
            this.map = map;
            this.logPrefix = "TransportCostRule";
        }
    }
}
