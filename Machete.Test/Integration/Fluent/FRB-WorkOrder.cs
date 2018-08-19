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
        private WorkOrderRepository _repoWO;
        private WorkOrderService _servWO;
        private WorkOrder _wo;

        public FluentRecordBase AddRepoWorkOrder()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWO = new WorkOrderRepository(_dbFactory);
            return this;
        }

        public WorkOrderRepository ToRepoWorkOrder()
        {
            if (_repoWO == null) AddRepoWorkOrder();
            return _repoWO;
        }

        public FluentRecordBase AddServWorkOrder()
        {
            //
            // DEPENDENCIES
            if (_repoWO == null) AddRepoWorkOrder();
            if (_servWA == null) AddServWorkAssignment();
            if (_repoL == null) AddRepoLookup();
            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            if (_servC == null) AddServConfig();
            if (_servTP == null) AddServTransportProvider();
            _servWO = new WorkOrderService(_repoWO, _servWA, _servTP, _repoL, _uow, _webMap, _servC);
            return this;
        }

        public WorkOrderService ToServWorkOrder()
        {
            if (_servWO == null) AddServWorkOrder();
            return _servWO;
        }

        public FluentRecordBase AddWorkOrder(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            DateTime? dateTimeOfWork = null,
            int? paperordernum = null,
            int? status = null
        )
        {
            //
            // DEPENDENCIES
            if (_emp == null) AddEmployer();
            if (_servWO == null) AddServWorkOrder();

            //
            // ARRANGE
            _wo = (WorkOrder)Records.order.Clone();
            _wo.Employer = _emp;
            _wo.workAssignments = new List<WorkAssignment>();
            if (datecreated != null) _wo.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wo.dateupdated = (DateTime)dateupdated;
            if (paperordernum != null) _wo.paperOrderNum = paperordernum;
            if (dateTimeOfWork != null) _wo.dateTimeofWork = (DateTime)dateTimeOfWork;
            if (status != null) _wo.statusID = (int)status;
            //
            // ACT
            _servWO.Create(_wo, _user);
            return this;
        }

        public WorkOrder ToWorkOrder()
        {
            if (_wo == null) AddWorkOrder();
            return _wo;
        }

        public Web.ViewModel.WorkOrder CloneWorkOrder()
        {
            var wo = (Web.ViewModel.WorkOrder)Records.order.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }

        public Machete.Domain.WorkOrder CloneDomainWorkOrder()
        {
            var wo = (Machete.Domain.WorkOrder)Records.order.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }
    }
}
