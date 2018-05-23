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
        private OnlineOrdersService _servOO;

        public FluentRecordBase AddServOnlineOrders()
        {
            //
            // DEPENDENCIES
            if (_servWO == null) AddServWorkOrder();
            if (_servWA == null) AddServWorkAssignment();
            if (_servTR == null) AddServTransportRule();
            if (_servTP == null) AddServTransportProvider();
            if (_servL == null) AddServLookup();
            if (_apiMap == null) AddMapper();
            _servOO = new OnlineOrdersService(_servWO, _servWA, _servTR, _servTP, _servL, _apiMap);
            return this;
        }

        public OnlineOrdersService ToServOnlineOrders()
        {
            if (_servOO == null) AddServOnlineOrders();
            return _servOO;
        }

        public Api.ViewModel.WorkOrder CloneOnlineOrder()
        {
            var wo = (Api.ViewModel.WorkOrder)Records.onlineOrder.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }
    }
}
