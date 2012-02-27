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
        Mock<IWorkerSigninRepository> _wsiRepo;
        Mock<IWorkerRepository> _wRepo;
        Mock<IPersonRepository> _pRepo;
        Mock<IWorkerRequestRepository> _wrRepo;
        Mock<IUnitOfWork> _uow;
        Mock<IImageRepository> _iRepo;
        List<WorkerSignin> _signins;
        List<Worker> _workers;
        List<Person> _persons;
        List<WorkerRequest> _requests;

        public WorkerSigninServiceUnitTest()
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
            _persons.Add(new Person() { ID = 3, firstname1 = "UnitTest" });

            _requests = new List<WorkerRequest>();
            //_requests.Add(new WorkerRequest() {ID = 1, WorkOrderID = }
            //
            // Arrange WorkerSignin
            _wsiRepo = new Mock<IWorkerSigninRepository>();
            _wsiRepo.Setup(s => s.GetAll()).Returns(_signins);
            // Arrange Worker
            _wRepo = new Mock<IWorkerRepository>();
            _wRepo.Setup(w => w.GetAll()).Returns(_workers);
            //
            _wrRepo = new Mock<IWorkerRequestRepository>();
            _wrRepo.Setup(w => w.GetAll()).Returns(_requests);
            // Arrange Person
            _pRepo = new Mock<IPersonRepository>();
            _pRepo.Setup(s => s.GetAll()).Returns(_persons);

            _iRepo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
        }

        [TestMethod]
        public void WorkerSigninService_Create_without_worker_match_succeeds()
        {
            //
            //Arrange
            var _serv = new WorkerSigninService(_wsiRepo.Object, _wRepo.Object, _pRepo.Object, _iRepo.Object, _wrRepo.Object, _uow.Object);
            var _signin = new WorkerSignin() { dwccardnum = 66666, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = new WorkerSignin();
            _wsiRepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
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
            var _serv = new WorkerSigninService(_wsiRepo.Object, _wRepo.Object, _pRepo.Object, _iRepo.Object, _wrRepo.Object, _uow.Object);
            var _signin = new WorkerSignin() { dwccardnum = fakeid, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = new WorkerSignin();
            _wsiRepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
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
            IQueryable<WorkerSignin> wsiList = new WorkerSignin[] { 
                new WorkerSignin() {dwccardnum = 12345, dateforsignin = DateTime.Today} 
            }.AsQueryable();
            var _serv = new WorkerSigninService(_wsiRepo.Object, _wRepo.Object, _pRepo.Object, _iRepo.Object, _wrRepo.Object, _uow.Object);
            var _signin = new WorkerSignin() { dwccardnum = fakeid, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = null;
            _wsiRepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
            _wsiRepo.Setup(s => s.GetAllQ()).Returns(wsiList);
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
