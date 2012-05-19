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
        dTableList<Person> GetIndexView(viewOptions o);
    }

    // Business logic for Person record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class PersonService : ServiceBase<Person>, IPersonService
    {

        public PersonService(IPersonRepository pRepo, 
                             IUnitOfWork unitOfWork) : base(pRepo, unitOfWork) {}  

        public dTableList<Person> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Person> filteredP = repo.GetAllQ();
            IQueryable<Person> orderedP;
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.search))
            {
                filteredP = filteredP
                    .Where(p => p.firstname1.Contains(o.search) ||
                                p.firstname2.Contains(o.search) ||
                                p.lastname1.Contains(o.search) ||
                                p.lastname2.Contains(o.search) ||
                                p.phone.Contains(o.search));
            }

            //
            //Sort the Persons based on column selection
            //var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            switch (o.sortColName)
            {
                case "active": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.active) : filteredP.OrderBy(p => p.active); break;
                case "firstname1": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.firstname1) : filteredP.OrderBy(p => p.firstname1); break;
                case "firstname2": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.firstname2) : filteredP.OrderBy(p => p.firstname2); break;
                case "lastname1": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.lastname1) : filteredP.OrderBy(p => p.lastname1); break;
                case "lastname2": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.lastname2) : filteredP.OrderBy(p => p.lastname2); break;
                case "phone": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.phone) : filteredP.OrderBy(p => p.phone); break;
                case "dateupdated": orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.dateupdated) : filteredP.OrderBy(p => p.dateupdated); break;
                default: orderedP = o.orderDescending ? filteredP.OrderByDescending(p => p.dateupdated) : filteredP.OrderBy(p => p.dateupdated); break;
            }
            orderedP = orderedP.Skip<Person>(o.displayStart).Take(o.displayLength);
            return new dTableList<Person>
            {
                query = orderedP,
                filteredCount = filteredP.Count(),
                totalCount = repo.GetAllQ().Count()
            };
        }
    }
}