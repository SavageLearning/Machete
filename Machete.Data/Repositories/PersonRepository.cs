using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
    // : RepositoryBase<Person> -- [ class-base ]
    //              the direct base class of the class being defined
    //                  if there is base class, then object is the base class
    // , IPersonRepository -- [ interface-type ] 
    //              defined a (weak) contract 
    //                  - which methods are available
    //                  - what their names are
    //                  - what types they take
    //                  - what types they return
    public class PersonRepository: RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IPersonRepository : IRepository<Person>
    {
    }
}
