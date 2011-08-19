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
    /// Summary description for WorkAssignmentServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkAssignmentServiceUnitTests
    {
        Mock<IWorkAssignmentRepository> _repo;
        Mock<IUnitOfWork> _uow;
        Mock<IWorkerRepository> _wRepo;

        public WorkAssignmentServiceUnitTests()
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
        public void WorkAssignmentService_GetWorkAssignments_returns_Enumerable()
        {
            //
            //Arrange
            _repo = new Mock<IWorkAssignmentRepository>();
            _uow = new Mock<IUnitOfWork>();
            _wRepo = new Mock<IWorkerRepository>();
            var _serv = new WorkAssignmentService(_repo.Object, _wRepo.Object, _uow.Object);
            //Act
            var result = _serv.GetMany();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkAssignment>));
        }
        //TODO Test filter for workAssignments
        [TestMethod]
        public void WorkAssignmentService_GetWorkAssignment_returns_workAssignment()
        {
            //
            //Arrange
            _repo = new Mock<IWorkAssignmentRepository>();
            _uow = new Mock<IUnitOfWork>();
            int id = 1; //This matches Records._workAssignment3 ID value
            _repo.Setup(r => r.GetById(id)).Returns(Records._workAssignment1);
            _wRepo = new Mock<IWorkerRepository>();
            var _serv = new WorkAssignmentService(_repo.Object, _wRepo.Object, _uow.Object);
            //Act
            var result = _serv.Get(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod]
        public void WorkAssignmentService_CreateWorkAssignment_returns_workAssignment()
        {
            //
            //Arrange
            _repo = new Mock<IWorkAssignmentRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records._workAssignment1.datecreated = DateTime.MinValue;
            Records._workAssignment1.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(Records._workAssignment1)).Returns(Records._workAssignment1);
            _wRepo = new Mock<IWorkerRepository>();
            var _serv = new WorkAssignmentService(_repo.Object, _wRepo.Object, _uow.Object);
            //
            //Act
            var result = _serv.Create(Records._workAssignment1, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod]
        public void WorkAssignmentService_DeleteWorkAssignment()
        {
            //
            //Arrange
            _repo = new Mock<IWorkAssignmentRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            int id = 1;
            WorkAssignment dp = new WorkAssignment();
            _repo.Setup(r => r.Delete(It.IsAny<WorkAssignment>())).Callback((WorkAssignment p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(Records._workAssignment1);
            _wRepo = new Mock<IWorkerRepository>();
            var _serv = new WorkAssignmentService(_repo.Object, _wRepo.Object, _uow.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records._workAssignment1);
        }

        [TestMethod]
        public void WorkAssignmentService_SaveWorkAssignment_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IWorkAssignmentRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records._workAssignment1.datecreated = DateTime.MinValue;
            Records._workAssignment1.dateupdated = DateTime.MinValue;
            _wRepo = new Mock<IWorkerRepository>();
            var _serv = new WorkAssignmentService(_repo.Object, _wRepo.Object, _uow.Object);
            //
            //Act
            _serv.Save(Records._workAssignment1, user);
            //
            //Assert
            Assert.IsTrue(Records._workAssignment1.Updatedby == user);
            Assert.IsTrue(Records._workAssignment1.dateupdated > DateTime.MinValue);
        }
    }
}
