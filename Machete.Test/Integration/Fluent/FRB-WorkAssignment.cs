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
        private WorkAssignmentRepository _repoWA;
        private WorkAssignmentService _servWA;
        private WorkAssignment _wa;


        public FluentRecordBase AddRepoWorkAssignment()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWA = new WorkAssignmentRepository(_dbFactory);
            return this;
        }

        public WorkAssignmentRepository ToRepoWorkAssignment()
        {
            if (_repoWA == null) AddRepoWorkAssignment();
            return _repoWA;
        }

        public FluentRecordBase AddServWorkAssignment()
        {
            //
            // DEPENDENCIES
            if (_repoWA == null) AddRepoWorkAssignment();
            if (_repoW == null) AddRepoWorker();
            if (_repoL == null) AddRepoLookup();
            if (_repoWSI == null) AddRepoWorkerSignin();
            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            _servWA = new WorkAssignmentService(_repoWA, _repoW, _repoL, _repoWSI, _uow, _webMap);
            return this;
        }

        public WorkAssignmentService ToServWorkAssignment()
        {
            if (_servWA == null) AddServWorkAssignment();
            return _servWA;
        }

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

        public WorkAssignment CloneWorkAssignment()
        {
            var wa = (WorkAssignment)Records.assignment.Clone();
            wa.description = RandomString(10);
            return wa;
        }

    }
}
