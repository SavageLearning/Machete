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
        private ITransportProvidersService _servTP;
        private TransportProvider _tp;

        public FluentRecordBase AddTransportProvider(
            )
        {
            //
            // DEPENDENCIES
            _servTP = container.Resolve<ITransportProvidersService>();

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
