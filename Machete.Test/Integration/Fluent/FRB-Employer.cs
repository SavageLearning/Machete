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
        private EmployerRepository _repoE;
        private EmployerService _servE;
        private Employer _emp;

        public FluentRecordBase AddRepoEmployer()
        {
            if (_dbFactory == null) AddDBFactory();
            _repoE = new EmployerRepository(_dbFactory);
            return this;
        }

        public EmployerRepository ToRepoEmployer()
        {
            if (_repoE == null) AddRepoEmployer();
            return _repoE;
        }

        public FluentRecordBase AddServEmployer()
        {
            //
            // DEPENDENCIES
            if (_repoE == null) AddRepoEmployer();
            if (_servWO == null) AddServWorkOrder();
            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            _servE = new EmployerService(_repoE, _servWO, _uow, _webMap);
            return this;
        }

        public EmployerService ToServEmployer()
        {
            if (_servE == null) AddServEmployer();
            return _servE;
        }

        public FluentRecordBase AddEmployer(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servE == null) AddServEmployer();
            //
            // ARRANGE
            _emp = (Employer)Records.employer.Clone();
            if (datecreated != null) _emp.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _emp.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servE.Create(_emp, _user);
            return this;
        }

        public Employer ToEmployer()
        {
            if (_emp == null) AddEmployer();
            return _emp;
        }

        public Web.ViewModel.Employer CloneEmployer()
        {
            var e = (Web.ViewModel.Employer)Records.employer.Clone();
            e.name = RandomString(10);
            e.email = "changeme@gmail.com";
            return e;
        }

    }
}
