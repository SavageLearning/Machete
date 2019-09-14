using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private ITransportProvidersAvailabilityService _servTPA;
        private TransportProviderAvailabilities _tpa;

        public FluentRecordBase AddTransportProviderAvailability(
    )
        {
            //
            // DEPENDENCIES
            _servTPA = container.GetRequiredService<ITransportProvidersAvailabilityService>();

            //
            // ARRANGE
            _tpa = (TransportProviderAvailabilities)Records.transportProviderAvailabilities.Clone();

            //
            // ACT
            _servTPA.Create(_tpa, _user);
            return this;
        }

        public TransportProviderAvailabilities ToTransportProviderAvailability()
        {
            if (_tpa == null) AddTransportProviderAvailability();
            return _tpa;
        }
    }
}
