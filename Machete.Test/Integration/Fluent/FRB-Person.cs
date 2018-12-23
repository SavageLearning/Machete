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
        private IPersonService _servP;
        private Person _p;

        public FluentRecordBase AddPerson(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            string testID = null
        )
        {
            //
            // DEPENDENCIES
            _servP = container.Resolve<IPersonService>();
            //
            // ARRANGE
            _p = (Person)Records.person.Clone();
            if (datecreated != null) _p.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _p.dateupdated = (DateTime)dateupdated;
            if (testID != null) _p.firstname2 = testID;
            //
            // ACT
            var result = _servP.Create(_p, _user);
            return this;
        }

        public Person ToPerson()
        {
            if (_p == null) AddPerson();
            return _p;
        }

        public Person ClonePerson()
        {
            var p = (Person)Records.person.Clone();
            p.firstname1 = RandomString(5);
            p.lastname1 = RandomString(8);
            return p;
        }


    }
}
