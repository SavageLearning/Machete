using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private ITransportRuleService _servTR;
        private TransportRule _tr;
        
        public void AddTransportRule()
        {
            // DEPENDENCIES
            _servTR = container.GetRequiredService<ITransportRuleService>();

            // ARRANGE
            _tr = (TransportRule)Records.transportRule.Clone();

            // ACT
            _servTR.Create(_tr, _user);
        }

        public TransportRule ToTransportRule()
        {
            if (_tr == null) AddTransportRule();
            return _tr;
        }
    }
}
