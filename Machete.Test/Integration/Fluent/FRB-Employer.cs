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
        private IEmployerService _servE;
        private Employer _emp;

        public FluentRecordBase AddEmployer(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servE = container.Resolve<IEmployerService>();
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
            AddMapper();
            var e = _webMap.Map<Machete.Domain.Employer, Web.ViewModel.Employer>((Employer)Records.employer.Clone());
            e.name = RandomString(10);
            e.email = "changeme@gmail.com";
            return e;
        }

    }
}
