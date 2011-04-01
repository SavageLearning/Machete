using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;

namespace Machete.Service
{
    public interface IPersonService
    {
        IEnumerable<Person> GetPersons(bool inactive);
        Person GetPerson(int id);
        Person CreatePerson(Person person, string user);
        void DeletePerson(int id, string user);
        void SavePerson(Person person, string user);
    }

    // Business logic for Person record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "PersonService", "");
        private Person _person;
        //
        public PersonService(IPersonRepository personRepository, IUnitOfWork unitOfWork)
        {
            this.personRepository = personRepository;
            this.unitOfWork = unitOfWork;
        }  

        public IEnumerable<Person> GetPersons(bool showInactive)
        {
            IEnumerable<Person> persons;
            //TODO Unit test this
            if (showInactive == false)
            {
                persons = personRepository.GetAll().Where(w => w.active == true);
            }
            else
            {
                persons = personRepository.GetAll();
            }
            return persons;
        }

        public Person GetPerson(int id)
        {
            var person = personRepository.GetById(id);
            return person;
        }

        public Person CreatePerson(Person person, string user)
        {
            person.createdby(user);
            _person = personRepository.Add(person);
            unitOfWork.Commit();
            _log(person.ID, user, "Person created");
            return _person;
        }

        public void DeletePerson(int id, string user)
        {
            var person = personRepository.GetById(id);
            personRepository.Delete(person);
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
    }
}