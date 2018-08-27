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
        private TransportProvidersRepository _repoTP;
        private TransportProvidersService _servTP;
        private TransportProvider _tp;

        public FluentRecordBase AddRepoTransportProvider()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoTP = new TransportProvidersRepository(_dbFactory);
            return this;
        }

        public TransportProvidersRepository ToRepoTransportProvider()
        {
            if (_repoTP == null) AddRepoTransportProvider();
            return _repoTP;
        }

        public FluentRecordBase AddServTransportProvider()
        {
            //
            // Dependencies
            if (_repoTP == null) AddRepoTransportProvider();
            if (_uow == null) AddUOW();
            if (_apiMap == null) AddMapper();
            if (_servTPA == null) AddServTransportProviderAvailability();

            _servTP = new TransportProvidersService(_repoTP, _servTPA, _uow, _apiMap );
            return this;
        }

        public TransportProvidersService ToServTransportProvider()
        {
            if (_servTP == null) AddServTransportProvider();
            return _servTP;
        }

        public FluentRecordBase AddTransportProvider(
            )
        {
            //
            // DEPENDENCIES
            if (_servTP == null) AddServTransportProvider();

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
