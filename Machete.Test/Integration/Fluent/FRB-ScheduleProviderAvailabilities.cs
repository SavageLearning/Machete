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
        private TransportProvidersAvailabilityRepository _repoTPA;
        private TransportProvidersAvailabilityService _servTPA;
        private TransportProviderAvailability _tpa;

        public FluentRecordBase AddRepoTransportProviderAvailability()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoTPA = new TransportProvidersAvailabilityRepository(_dbFactory);
            return this;
        }

        public TransportProvidersAvailabilityRepository ToRepoTransportProviderAvailability()
        {
            if (_repoTPA == null) AddRepoTransportProviderAvailability();
            return _repoTPA;
        }

        public FluentRecordBase AddServTransportProviderAvailability()
        {
            //
            // Dependencies
            if (_repoTPA == null) AddRepoTransportProviderAvailability();
            if (_uow == null) AddUOW();
            if (_apiMap == null) AddMapper();

            _servTPA = new TransportProvidersAvailabilityService(_repoTPA, _uow, _apiMap);
            return this;
        }

        public TransportProvidersAvailabilityService ToServTransportProviderAvailability()
        {
            if (_servTPA == null) AddServTransportProviderAvailability();
            return _servTPA;
        }

        public FluentRecordBase AddTransportProviderAvailability(
    )
        {
            //
            // DEPENDENCIES
            if (_servTR == null) AddServTransportProviderAvailability();

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
