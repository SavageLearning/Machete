using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Database;
using System.Data.Entity.Validation;

namespace Machete.Test
{
    [TestClass]
    public class PersonServiceTest
    {
        PersonRepository _personRepo;
        DatabaseFactory _dbFactory;
        PersonService _service;
        IUnitOfWork _unitofwork;
        MacheteContext MacheteDB;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //DbDatabase.SetInitializer<MacheteContext>(new MacheteInitializer());
        }


        [TestInitialize]
        public void TestInitialize()
        {

            DbDatabase.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();
            _dbFactory = new DatabaseFactory();
            _personRepo = new PersonRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _service = new PersonService(_personRepo, _unitofwork);
        }

        [TestMethod]
        public void PersonService_Intergation_CreatePerson()
        {
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person = Records._person4;
            _person.firstname2 = "PersonService_Intergation_CreatePerson";

            //
            //Act
            _service.CreatePerson(_person, "UnitTest");

            //
            //Assert
            Assert.IsNotNull(_person.ID, "Person.ID is Null");
            Assert.IsTrue(_person.ID == 1);
        }
        [TestMethod]
        public void PersonService_Intergation_CreatePersons_NoDuplicate()
        {
            int reccount = 0;
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person4 = Records._person4;
            _person4.firstname2 = "PersonService_Int_CrePer_NoDuplicate";
            //
            //Act
            try
            {
                _service.CreatePerson(_person4, "UnitTest");
                _service.CreatePerson(_person4, "UnitTest");
                _service.CreatePerson(_person4, "UnitTest");
                reccount = MacheteDB.Persons.Count(n => n.firstname1 == _person4.firstname1);
            }
            catch (DbEntityValidationException ex)
            {
                Assert.Fail(string.Format("Validation exception for field {0} caught: {1}",
                    ex.EntityValidationErrors.First().ValidationErrors.First().PropertyName,
                    ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage));
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                ex.GetType(), ex.Message));
            }
            //
            //Assert
            //TODO: figure out why de-dup isn't working
            Assert.IsNotNull(_person4.ID);
            Assert.IsTrue(reccount == 1, "Expected record count of 1, received {0}", reccount);
      
        }
    }
}
