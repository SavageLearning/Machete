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
        Mock<IWorkAssignmentRepository> _waRepo;
        Mock<IUnitOfWork> _uow;
        Mock<ILookupRepository> _lRepo;
        Mock<IWorkerRepository> _wRepo;
        Mock<IWorkerSigninRepository> _wsiRepo;
        WorkAssignmentService _waServ;

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
        [TestInitialize]
        public void TestInitialize()
        {
            _waRepo = new Mock<IWorkAssignmentRepository>();
            _uow = new Mock<IUnitOfWork>();
            _wRepo = new Mock<IWorkerRepository>();
            _lRepo = new Mock<ILookupRepository>();
            _wsiRepo = new Mock<IWorkerSigninRepository>();
            _waServ = new WorkAssignmentService(_waRepo.Object, _wRepo.Object, _lRepo.Object, _wsiRepo.Object, _uow.Object);
        }
        [TestMethod]
        public void WorkAssignmentService_GetWorkAssignments_returns_Enumerable()
        {
            //
            //Arrange

            //Act
            var result = _waServ.GetMany();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkAssignment>));
        }
        //TODO Test filter for workAssignments
        [TestMethod]
        public void WorkAssignmentService_GetWorkAssignment_returns_workAssignment()
        {
            //
            //Arrange
            int id = 1; //This matches Records._workAssignment3 ID value
            _waRepo.Setup(r => r.GetById(id)).Returns(Records._workAssignment1);
            //Act
            var result = _waServ.Get(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod]
        public void WorkAssignmentService_CreateWorkAssignment_returns_workAssignment()
        {
            //
            //Arrange
            string user = "UnitTest";
            Records._workAssignment1.datecreated = DateTime.MinValue;
            Records._workAssignment1.dateupdated = DateTime.MinValue;
            _waRepo.Setup(r => r.Add(Records._workAssignment1)).Returns(Records._workAssignment1);
            //
            //Act
            var result = _waServ.Create(Records._workAssignment1, user);
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
            string user = "UnitTest";
            int id = 1;
            WorkAssignment dp = new WorkAssignment();
            _waRepo.Setup(r => r.Delete(It.IsAny<WorkAssignment>())).Callback((WorkAssignment p) => { dp = p; });
            _waRepo.Setup(r => r.GetById(id)).Returns(Records._workAssignment1);
            //
            //Act
            _waServ.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records._workAssignment1);
        }
        [TestMethod]
        public void WorkAssignmentService_SaveWorkAssignment_updates_timestamp()
        {
            //
            //Arrange
            string user = "UnitTest";
            Records._workAssignment1.datecreated = DateTime.MinValue;
            Records._workAssignment1.dateupdated = DateTime.MinValue;
            //
            //Act
            _waServ.Save(Records._workAssignment1, user);
            //
            //Assert
            Assert.IsTrue(Records._workAssignment1.Updatedby == user);
            Assert.IsTrue(Records._workAssignment1.dateupdated > DateTime.MinValue);
        }
    }
}
