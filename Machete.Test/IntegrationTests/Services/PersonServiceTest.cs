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
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Machete.Test
{
    [TestClass]
    public class PersonServiceTest
    {
        PersonRepository _pRepo;
        DatabaseFactory _dbFactory;
        PersonService _service;
        IUnitOfWork _unitofwork;
        MacheteContext MacheteDB;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //Database.SetInitializer<MacheteContext>(new MacheteInitializer());
        }


        [TestInitialize]
        public void TestInitialize()
        {

            Database.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();
            _dbFactory = new DatabaseFactory();
            _pRepo = new PersonRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _service = new PersonService(_pRepo, _unitofwork);
        }

        [TestMethod]
        public void DbSet_PersonService_Intergation_CreatePerson()
        {
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person = (Person)Records.person.Clone();
            _person.firstname2 = "PersonService_Intergation_CreatePerson";

            //
            //Act
            _service.CreatePerson(_person, "UnitTest");

            //
            //Assert
            Assert.IsNotNull(_person.ID, "Person.ID is Null");
            Assert.IsTrue(_person.ID == 1);
        }
        //
        // CreatePerson calls DbSet.Add() and  Context.SaveChanges()
        //    This leads to duplication
        //
        [TestMethod]
        public void DbSet_PersonService_Intergation_CreatePersons_TestDuplicateBehavior()
        {
            int reccount = 0;
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _p = (Person)Records.person.Clone();
            _p.firstname2 = "PersonService_Int_CrePer_NoDuplicate";
            //
            //Act
            try
            {
                _service.CreatePerson(_p, "UnitTest");
                _service.CreatePerson(_p, "UnitTest");
                _service.CreatePerson(_p, "UnitTest");
                reccount = MacheteDB.Persons.Count(n => n.firstname1 == _p.firstname1);
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
            Assert.IsNotNull(_p.ID);
            Assert.IsTrue(reccount == 3, "Expected record count of 3, received {0}", reccount);

        }
    }
}
