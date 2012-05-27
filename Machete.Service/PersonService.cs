using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;
using System.Globalization;

namespace Machete.Service
{
    public interface IPersonService : IService<Person>
    {
        IEnumerable<Person> GetIndexView(viewOptions o);
    }

    // Business logic for Person record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class PersonService : ServiceBase<Person>, IPersonService
    {
        public PersonService(IPersonRepository pRepo, 
                             IUnitOfWork unitOfWork) : base(pRepo, unitOfWork) 
        {
            this.logPrefix = "Person";
        }  

        public IEnumerable<Person> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Person> q = repo.GetAllQ();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.search)) IndexViewBase.search(o, ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);

            q = q.Skip<Person>(o.displayStart).Take(o.displayLength);
            return q;
        }
    }
}