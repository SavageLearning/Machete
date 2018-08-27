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

        private PersonRepository _repoP;
        private PersonService _servP;
        private Person _p;

        public FluentRecordBase AddRepoPerson()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoP = new PersonRepository(_dbFactory);
            return this;
        }

        public PersonRepository ToRepoPerson()
        {
            if (_repoP == null) AddRepoPerson();
            return _repoP;
        }

        public FluentRecordBase AddServPerson()
        {
            //
            // DEPENDENCIES
            if (_repoP == null) AddRepoPerson();
            if (_uow == null) AddUOW();
            if (_repoL == null) AddRepoLookup();
            if (_webMap == null) AddMapper();

            _servP = new PersonService(_repoP, _uow, _repoL, _webMap);
            return this;
        }

        public PersonService ToServPerson()
        {
            if (_servP == null) AddServPerson();
            return _servP;
        }

        public FluentRecordBase AddPerson(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            string testID = null
        )
        {
            //
            // DEPENDENCIES
            if (_servP == null) AddServPerson();
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
