using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Domain;

namespace Machete.Test.IntegrationTests.Services
{

    
    /// <summary>
    /// Summary description for WorkerSigninServiceUnitTest
    /// </summary>
    [TestClass]
    public class WorkerSigninServiceUnitTest
    {
        Mock<IWorkerSigninRepository> _srepo;
        Mock<IWorkerRepository> _wrepo;
        Mock<IPersonRepository> _prepo;
        Mock<IUnitOfWork> _uow;
        Mock<IImageRepository> _irepo;
        List<WorkerSignin> _signins;
        List<Worker> _workers;
        List<Person> _persons;

        public WorkerSigninServiceUnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
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
        [TestInitialize()]
        public void TestInitialize() 
        {
            _signins = new List<WorkerSignin>();
            _signins.Add(new WorkerSignin() { ID = 111, dwccardnum = 12345, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 112, dwccardnum = 12346, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 113, dwccardnum = 12347, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 114, dwccardnum = 12348, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 115, dwccardnum = 12349, dateforsignin = DateTime.Today });

            _workers = new List<Worker>();
            _workers.Add(new Worker() { ID = 1, dwccardnum = 12345 });
            _workers.Add(new Worker() { ID = 2, dwccardnum = 12347 });
            _workers.Add(new Worker() { ID = 3, dwccardnum = 12349 });
            _workers.Add(new Worker() { ID = 3, dwccardnum = 66666 });

            _persons = new List<Person>();
            _persons.Add(new Person() { ID = 1, firstname1 = "UnitTest" });
            _persons.Add(new Person() { ID = 2, firstname1 = "UnitTest" });

            //
            // Arrange WorkerSignin
            _srepo = new Mock<IWorkerSigninRepository>();
            _srepo.Setup(s => s.GetAll()).Returns(_signins);
            // Arrange Worker
            _wrepo = new Mock<IWorkerRepository>();
            _wrepo.Setup(w => w.GetAll()).Returns(_workers);
            // Arrange Person
            _prepo = new Mock<IPersonRepository>();
            _prepo.Setup(s => s.GetAll()).Returns(_persons);

            _irepo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void WorkerSigninService_getView_finds_joined_records()
        {
            //
            //Arrange
            var _serv = new WorkerSigninService(_srepo.Object, _wrepo.Object, _prepo.Object, _irepo.Object, _uow.Object);
            //
            //Act
            var result = _serv.getView(DateTime.Today);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkerSigninView>));
            Assert.IsTrue(result.Count() == 5);
            Assert.IsTrue((from rec in result
                           where rec.person.ID != 0
                           select rec).Count() == 2);
            Assert.IsTrue((from rec in result
                           where rec.person.firstname1 == "UnitTest"
                           select rec).Count() == 2);
        }

        [TestMethod]
        public void WorkerSigninService_Create_without_worker_match_succeeds()
        {
            //
            //Arrange
            var _serv = new WorkerSigninService(_srepo.Object, _wrepo.Object, _prepo.Object, _irepo.Object, _uow.Object);
            var _signin = new WorkerSignin() { dwccardnum = 66666, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = new WorkerSignin();
            _srepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
            //
            //Act
            _serv.CreateWorkerSignin(_signin, "UnitTest");
            //
            //Assert
            Assert.AreEqual(_signin, _cbsignin);
        }

        [TestMethod]
        public void WorkerSigninService_Create_with_worker_match_succeeds()
        {
            //
            //Arrange
            int fakeid = 66666;
            var _serv = new WorkerSigninService(_srepo.Object, _wrepo.Object, _prepo.Object, _irepo.Object, _uow.Object);
            var _signin = new WorkerSignin() { dwccardnum = fakeid, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = new WorkerSignin();
            _srepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
            //
            //Act
            _serv.CreateWorkerSignin(_signin, "UnitTest");
            //
            //Assert
            Assert.AreEqual(_signin, _cbsignin);
            Assert.IsNotNull(_signin.dwccardnum);
            Assert.AreEqual(_signin.dwccardnum, fakeid);
        }

        [TestMethod]
        public void WorkerSigninService_Create_deduplicate_succeeds()
        {
            //
            //Arrange
            int fakeid = 12345;
            var _serv = new WorkerSigninService(_srepo.Object, _wrepo.Object, _prepo.Object, _irepo.Object, _uow.Object);
            var _signin = new WorkerSignin() { dwccardnum = fakeid, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = null;
            _srepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
            //
            //Act
            _serv.CreateWorkerSignin(_signin, "UnitTest");
            //
            //Assert
            Assert.IsNull(_cbsignin);
            Assert.IsNull(_signin.Createdby);
        }
    }
}
