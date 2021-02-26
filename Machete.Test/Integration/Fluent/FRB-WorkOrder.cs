using System;
using System.Collections.Generic;
using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;
using ViewModel = Machete.Web.ViewModel;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
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
            _servWO = container.GetRequiredService<IWorkOrderService>();

            //
            // ARRANGE
            _wo = (WorkOrder)Records.order.Clone();
            _wo.Employer = _emp;
            _wo.workAssignments = new List<WorkAssignment>();
            if (datecreated != null) _wo.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wo.dateupdated = (DateTime)dateupdated;
            if (paperordernum == null) _wo.paperOrderNum = new Random().Next(10000, 99999);
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

        public Web.ViewModel.WorkOrderMVC CloneWorkOrder()
        {
            ToWebMapper();
            var wo = _webMap.Map<WorkOrder, ViewModel.WorkOrderMVC>((WorkOrder)Records.order.Clone());
            wo.contactName = RandomString(10);
            return wo;
        }

        public WorkOrder CloneDomainWorkOrder()
        {
            var wo = (WorkOrder)Records.order.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }
    }
}
