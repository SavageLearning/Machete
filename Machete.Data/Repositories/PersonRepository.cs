using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
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
