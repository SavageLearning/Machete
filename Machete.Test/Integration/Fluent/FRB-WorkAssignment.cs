using System;
using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;
using ViewModel = Machete.Web.ViewModel;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private IWorkAssignmentService _servWA;
        private WorkAssignment _wa;

        public FluentRecordBase AddWorkAssignment(
            string desc = null,
            int? skill = null,
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            string updatedby = null,
            bool assignWorker = false
        )
        {
            //
            // DEPENDENCIES
            if (_wo == null) AddWorkOrder();
            _servWA = container.GetRequiredService<IWorkAssignmentService>();
            //
            // ARRANGE
            _wa = (WorkAssignment)Records.assignment.Clone();
            _wa.workOrder = _wo;
            _wa.workOrderID = _wo.ID;

            if (assignWorker) _wa.workerAssignedDDD = AddWorker();
            if (datecreated != null) _wa.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wa.dateupdated = (DateTime)dateupdated;
            if (desc != null) _wa.description = desc;
            if (updatedby != null) _user = updatedby;
            if (skill != null) _wa.skillID = (int)skill;
            //
            // ACT
            _servWA.Create(_wa, _user);
            return this;
        }

        public WorkAssignment ToWorkAssignment()
        {
            if (_wa == null) AddWorkAssignment();
            return _wa;
        }

        public ViewModel.WorkAssignmentMVC CloneWorkAssignment()
        {
            ToWebMapper();
            var wa = _webMap.Map<WorkAssignment, ViewModel.WorkAssignmentMVC>
                ((WorkAssignment)Records.assignment.Clone());
            wa.description = RandomString(10);
            return wa;
        }

        public WorkAssignment CloneDomainWorkAssignment()
        {
            var wa = (WorkAssignment)Records.assignment.Clone();
            wa.description = RandomString(10);
            return wa;
        }
    }
}
