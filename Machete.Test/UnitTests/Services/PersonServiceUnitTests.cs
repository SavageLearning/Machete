using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data;
using Moq;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Domain;
using Machete.Test;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for PersonServiceUnitTests
    /// </summary>
    [TestClass]
    public class PersonServiceUnitTests
    {
        Mock<IPersonRepository> _repo;
        Mock<IUnitOfWork> _uow;
        
        public PersonServiceUnitTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PersonService_GetPersons_returns_Enumerable()
        {
            //
            //Arrange
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            var _serv = new PersonService(_repo.Object, _uow.Object);
            //Act
            var result = _serv.GetPersons(false);
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Person>));
        }
        //TODO Test filter for persons
        [TestMethod]
        public void PersonService_GetPerson_returns_person()
        {
            //
            //Arrange
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            int id = 3; //This matches Records._person3 ID value
            _repo.Setup(r => r.GetById(id)).Returns(Records._person3);
            var _serv = new PersonService(_repo.Object, _uow.Object);
            //Act
            var result = _serv.GetPerson(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(Person));
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod]
        public void PersonService_CreatePerson_returns_person()
        {
            //
            //Arrange
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records._person1.datecreated = DateTime.MinValue;
            Records._person1.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(Records._person1)).Returns(Records._person1);
            var _serv = new PersonService(_repo.Object, _uow.Object);
            //
            //Act
            var result = _serv.CreatePerson(Records._person1, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Person));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod]
        public void PersonService_DeletePerson()
        {
            //
            //Arrange
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            int id = 1;
            Person dp = new Person();
            _repo.Setup(r => r.Delete(It.IsAny<Person>())).Callback((Person p) => { dp = p;  });
            _repo.Setup(r => r.GetById(id)).Returns(Records._person1);
            var _serv = new PersonService(_repo.Object, _uow.Object);
            //
            //Act
            _serv.DeletePerson(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records._person1);
        }

        [TestMethod]
        public void PersonService_SavePerson_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records._person1.datecreated = DateTime.MinValue;
            Records._person1.dateupdated = DateTime.MinValue;
            var _serv = new PersonService(_repo.Object, _uow.Object);
            //
            //Act
            _serv.SavePerson(Records._person1, user);
            //
            //Assert
            Assert.IsTrue(Records._person1.Updatedby == user);
            Assert.IsTrue(Records._person1.dateupdated > DateTime.MinValue);
        }
    }
}
