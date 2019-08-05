using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private ITransportProvidersService _servTP;
        private TransportProvider _tp;

        public FluentRecordBase AddTransportProvider(
            )
        {
            //
            // DEPENDENCIES
            _servTP = container.GetRequiredService<ITransportProvidersService>();

            //
            // ARRANGE
            _tp = (TransportProvider)Records.transportProvider.Clone();

            //
            // ACT
            _servTP.Create(_tp, _user);
            return this;
        }

        public TransportProvider ToTransportProvider()
        {
            if (_tp == null) AddTransportProvider();
            return _tp;
        }
    }
}
