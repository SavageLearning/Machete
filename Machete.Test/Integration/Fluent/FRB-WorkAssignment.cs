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
            if (assignWorker == true && _w == null) AddWorker();
            _servWA = container.Resolve<IWorkAssignmentService>();
            //
            // ARRANGE
            _wa = (WorkAssignment)Records.assignment.Clone();
            _wa.workOrder = _wo;
            if (assignWorker) _wa.workerAssigned = _w;
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

        public Web.ViewModel.WorkAssignment CloneWorkAssignment()
        {
            AddMapper();
            var wa = _webMap.Map<Machete.Domain.WorkAssignment, Web.ViewModel.WorkAssignment>
                ((WorkAssignment)Records.assignment.Clone());
            wa.description = RandomString(10);
            return wa;
        }

        public Machete.Domain.WorkAssignment CloneDomainWorkAssignment()
        {
            var wa = (Machete.Domain.WorkAssignment)Records.assignment.Clone();
            wa.description = RandomString(10);
            return wa;
        }
    }
}
