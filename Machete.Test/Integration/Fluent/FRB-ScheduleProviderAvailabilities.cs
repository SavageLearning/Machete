using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private ITransportProvidersAvailabilityService _servTPA;
        private TransportProviderAvailability _tpa;

        public FluentRecordBase AddTransportProviderAvailability(
    )
        {
            //
            // DEPENDENCIES
            _servTPA = container.GetRequiredService<ITransportProvidersAvailabilityService>();

            //
            // ARRANGE
            _tpa = (TransportProviderAvailability)Records.transportProviderAvailabilities.Clone();

            //
            // ACT
            _servTPA.Create(_tpa, _user);
            return this;
        }

        public TransportProviderAvailability ToTransportProviderAvailability()
        {
            if (_tpa == null) AddTransportProviderAvailability();
            return _tpa;
        }
    }
}
