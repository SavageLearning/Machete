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
        private ITransportProvidersAvailabilityService _servTPA;
        private TransportProviderAvailability _tpa;

        public FluentRecordBase AddTransportProviderAvailability(
    )
        {
            //
            // DEPENDENCIES
            _servTPA = container.Resolve<ITransportProvidersAvailabilityService>();

            //
            // ARRANGE
            _tpa = (TransportProviderAvailability)Records.transportProviderAvailability.Clone();

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
