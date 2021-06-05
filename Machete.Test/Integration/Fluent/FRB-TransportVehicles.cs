using Machete.Domain;
using Machete.Service;
using Microsoft.Practices.Unity;
using System;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase : IDisposable
    {
        private ITransportVehiclesScheduleService _servTVS;
        private TransportVehicleSchedule _tvs;

        private ITransportVehiclesService _servTV;
        private TransportVehicle _tv;


        public FluentRecordBase AddTransportVehicleSchedule(
            )
        {
            // DEPENDENCIES
            if (_tv == null) AddTransportVehicle();
            _servTVS = container.Resolve<ITransportVehiclesScheduleService>();
            // ARRANGE
            _tvs = (TransportVehicleSchedule)Records.transportVehicleSchedule.Clone();
            _tvs.StartTime = DateTime.Now;
            _tvs.EndTime = DateTime.Now;
            _tvs.TransportVehicle = _tv;
            // ACT
            _servTVS.Create(_tvs, _user);
            return this;
        }

        public TransportVehicleSchedule ToTransportVehicleSchedule()
        {
            if (_tvs == null) AddTransportVehicleSchedule();
            return _tvs;
        }

        public FluentRecordBase AddTransportVehicle(
    )
        {
            // DEPENDENCIES
            if (_tp == null) AddTransportProvider();
            _servTV = container.Resolve<ITransportVehiclesService>();
            // ARRANGE
            _tv = (TransportVehicle)Records.transportVehicle.Clone();
            _tv.TransportProvider = _tp;
            // ACT
            _servTV.Create(_tv, _user);
            return this;
        }

        public TransportVehicle ToTransportVehicle()
        {
            if (_tv == null) AddTransportVehicle();
            return _tv;
        }

    }
}
