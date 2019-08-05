using System;
using Machete.Domain;
using Machete.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase
    {
        private IPersonService _servP;

        public Person AddPerson(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            string testID = null
        )
        {
            // DEPENDENCIES
            _servP = container.GetRequiredService<IPersonService>();

            // ARRANGE
            var _p = (Person)Records.person.Clone();
            if (datecreated != null) _p.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _p.dateupdated = (DateTime)dateupdated;
            if (testID != null) _p.firstname2 = testID;
            
            // ACT
            _servP.Create(_p, _user);
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
