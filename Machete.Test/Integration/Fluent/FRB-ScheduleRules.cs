using Machete.Data;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase : IDisposable
    {
        private ScheduleRuleRepository _repoSR;
        private ScheduleRuleService _servSR;
        private ScheduleRule _sr;

        public FluentRecordBase AddRepoScheduleRule()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoSR = new ScheduleRuleRepository(_dbFactory);
            return this;
        }

        public ScheduleRuleRepository ToRepoScheduleRule()
        {
            if (_repoSR == null) AddRepoScheduleRule();
            return _repoSR;
        }

        public FluentRecordBase AddServScheduleRule()
        {
            //
            // Dependencies
            if (_repoSR == null) AddRepoScheduleRule();
            if (_uow == null) AddUOW();
            if (_apiMap == null) AddMapper();

            _servSR = new ScheduleRuleService(_repoSR, _uow, _apiMap);
            return this;
        }

        public ScheduleRuleService ToServScheduleRule()
        {
            if (_servSR == null) AddServScheduleRule();
            return _servSR;
        }

        public FluentRecordBase AddScheduleRule(
    )
        {
            //
            // DEPENDENCIES
            if (_servTR == null) AddServScheduleRule();

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
