using Machete.Data;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase : IDisposable
    {
        private IScheduleRuleService _servSR;
        private ScheduleRule _sr;



        public FluentRecordBase AddScheduleRule(
    )
        {
            //
            // DEPENDENCIES
            _servSR = container.Resolve<IScheduleRuleService>();

            //
            // ARRANGE
            _sr = (ScheduleRule)Records.scheduleRule.Clone();

            //
            // ACT
            _servSR.Create(_sr, _user);
            return this;
        }

        public ScheduleRule ToScheduleRule()
        {
            if (_sr == null) AddScheduleRule();
            return _sr;
        }
    }
}
