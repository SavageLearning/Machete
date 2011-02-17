using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;

namespace Machete.Service
{
    public interface IPersonService
    {
        IEnumerable<Person> GetPersons();
        Person GetPerson(int id);
        void CreatePerson(Person person);
        void DeletePerson(int id);
        void SavePerson();
    }
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IUnitOfWork unitOfWork;
        public PersonService(IPersonRepository personRepository, IUnitOfWork unitOfWork)
        {
            this.personRepository = personRepository;
            this.unitOfWork = unitOfWork;
        }  
        #region IPersonService Members

        public IEnumerable<Person> GetPersons()
        {
            var categories = personRepository.GetAll();
            return categories;
        }

        public Person GetPerson(int id)
        {
            var person = personRepository.GetById(id);
            return person;
        }

        public void CreatePerson(Person person)
        {
            personRepository.Add(person);
            unitOfWork.Commit();
        }

        public void DeletePerson(int id)
        {
            var person = personRepository.GetById(id);
            personRepository.Delete(person);
            unitOfWork.Commit();
        }

        public void SavePerson()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
