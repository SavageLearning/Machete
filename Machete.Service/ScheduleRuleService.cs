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
    public interface IScheduleRuleService : IService<ScheduleRule>
    {

    }

    public class ScheduleRuleService : ServiceBase<ScheduleRule>, IScheduleRuleService
    {
        private readonly IMapper map;

        public ScheduleRuleService(IScheduleRuleRepository repo, IUnitOfWork uow, IMapper map) : base(repo, uow)
        {
            this.map = map;
            this.logPrefix = "ScheduleRule";
        }
    }
}
