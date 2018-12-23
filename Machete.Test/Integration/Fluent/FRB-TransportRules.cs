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
        private ITransportRuleService _servTR;
        private TransportRule _tr;

        public FluentRecordBase AddTransportRule(
            )
        {
            //
            // DEPENDENCIES
            _servTR = container.Resolve<ITransportRuleService>();

            //
            // ARRANGE
            _tr = (TransportRule)Records.transportRule.Clone();

            //
            // ACT
            _servTR.Create(_tr, _user);
            return this;
        }

        public TransportRule ToTransportRule()
        {
            if (_tr == null) AddTransportRule();
            return _tr;
        }
    }
}
