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
    public interface IPersonService
    {
        IEnumerable<Person> GetPersons(bool inactive);
        Person GetPerson(int id);
        Person CreatePerson(Person person, string user);
        void DeletePerson(int id, string user);
        void SavePerson(Person person, string user);
        ServiceIndexView<Person> GetIndexView(
                CultureInfo CI,
                string search,
                int? parentID,
                int? status,
                bool orderDescending,
                int displayStart,
                int displayLength,
                string sortColName
    );
    }

    // Business logic for Person record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository pRepo;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "PersonService", "");
        private Person _person;
        //
        public PersonService(IPersonRepository personRepository, IUnitOfWork unitOfWork)
        {
            this.pRepo = personRepository;
            this.unitOfWork = unitOfWork;
        }  

        public IEnumerable<Person> GetPersons(bool showInactive)
        {
            IEnumerable<Person> persons;
            //TODO Unit test this
            if (showInactive == false)
            {
                persons = pRepo.GetAll().Where(w => w.active == true);
            }
            else
            {
                persons = pRepo.GetAll();
            }
            return persons;
        }

        public Person GetPerson(int id)
        {
            var person = pRepo.GetById(id);
            return person;
        }

        public Person CreatePerson(Person person, string user)
        {
            person.createdby(user);
            _person = pRepo.Add(person);
            unitOfWork.Commit();
            _log(person.ID, user, "Person created");
            return _person;
        }

        public void DeletePerson(int id, string user)
        {
            var person = pRepo.GetById(id);
            pRepo.Delete(person);
            _log(id, user, "Person deleted");
            unitOfWork.Commit();
        }

        public void SavePerson(Person person, string user)
        {
            person.updatedby(user);
            _log(person.ID, user, "Person edited");
            unitOfWork.Commit();
        }

        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info; 
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
        public ServiceIndexView<Person> GetIndexView(
                        CultureInfo CI,
                        string search,
                        int? parentID,
                        int? status,
                        bool orderDescending,
                        int displayStart,
                        int displayLength,
                        string sortColName
            )
        {
            //Get all the records
            IQueryable<Person> filteredP = pRepo.GetAllQ();
            IQueryable<Person> orderedP;
            bool isDateTime = false;
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(search))
            {
                filteredP = filteredP
                    .Where(p => p.firstname1.Contains(search) ||
                                p.firstname2.Contains(search) ||
                                p.lastname1.Contains(search) ||
                                p.lastname2.Contains(search) ||
                                p.phone.Contains(search) ||
                                p.Updatedby.Contains(search));
            }

            //
            //Sort the Persons based on column selection
            //var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            switch (sortColName)
            {
                case "active": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.active) : filteredP.OrderBy(p => p.active); break;
                case "firstname1": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.firstname1) : filteredP.OrderBy(p => p.firstname1); break;
                case "firstname2": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.firstname2) : filteredP.OrderBy(p => p.firstname2); break;
                case "lastname1": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.lastname1) : filteredP.OrderBy(p => p.lastname1); break;
                case "lastname2": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.lastname2) : filteredP.OrderBy(p => p.lastname2); break;
                case "phone": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.phone) : filteredP.OrderBy(p => p.phone); break;
                case "dateupdated": orderedP = orderDescending ? filteredP.OrderByDescending(p => p.dateupdated) : filteredP.OrderBy(p => p.dateupdated); break;
                default: orderedP = orderDescending ? filteredP.OrderByDescending(p => p.dateupdated) : filteredP.OrderBy(p => p.dateupdated); break;
            }
            //
            //SKIP & TAKE for display
            orderedP = orderedP.Skip<Person>((int)displayStart).Take((int)displayLength);
            var filtered = filteredP.Count();
            var total = pRepo.GetAllQ().Count();
            //return what's left to datatables
            return new ServiceIndexView<Person>
            {
                query = orderedP,
                filteredCount = filtered,
                totalCount = total
            };
        }
    }
}