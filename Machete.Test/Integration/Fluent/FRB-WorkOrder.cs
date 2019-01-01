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
        private IWorkOrderService _servWO;
        private WorkOrder _wo;

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
            _servWO = container.Resolve<IWorkOrderService>();

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
            AddMapper();
            var wo = _webMap.Map<Machete.Domain.WorkOrder, Web.ViewModel.WorkOrder>((WorkOrder)Records.order.Clone());
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
