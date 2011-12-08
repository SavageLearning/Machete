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
    /// Summary description for WorkOrderServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkOrderServiceUnitTests
    {
        Mock<IWorkOrderRepository> _repo;
        Mock<IWorkAssignmentService> _waServ;
        Mock<IUnitOfWork> _uow;
        WorkOrderService _serv;

        public WorkOrderServiceUnitTests()
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
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            _waServ = new Mock<IWorkAssignmentService>();
            _serv = new WorkOrderService(_repo.Object, _waServ.Object, _uow.Object);
        }
        [TestMethod]
        public void WorkOrderService_GetWorkOrders_returns_Enumerable()
        {
            //
            //Arrange
            //Act
            var result = _serv.GetWorkOrders();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkOrder>));
        }
        //TODO Test filter for workOrders
        [TestMethod]
        public void WorkOrderService_GetWorkOrder_returns_workOrder()
        {
            //
            //Arrange
            WorkOrder order = (WorkOrder)Records.order.Clone();
            order.ID = 3; //This matches Records._workOrder3 ID value
            _repo.Setup(r => r.GetById(3)).Returns(order);
            //Act
            var result = _serv.GetWorkOrder(3);
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkOrder));
            Assert.IsTrue(result.ID == 3);
        }

        [TestMethod]
        public void WorkOrderService_CreateWorkOrder_returns_workOrder()
        {
            //
            //Arrange
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records._workOrder1.datecreated = DateTime.MinValue;
            Records._workOrder1.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(Records._workOrder1)).Returns(Records._workOrder1);
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _uow.Object);
            //
            //Act
            var result = _serv.CreateWorkOrder(Records._workOrder1, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkOrder));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod]
        public void WorkOrderService_DeleteWorkOrder()
        {
            //
            //Arrange
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            int id = 1;
            WorkOrder dp = new WorkOrder();
            _repo.Setup(r => r.Delete(It.IsAny<WorkOrder>())).Callback((WorkOrder p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(Records._workOrder1);
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _uow.Object);
            //
            //Act
            _serv.DeleteWorkOrder(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records._workOrder1);
        }

        [TestMethod]
        public void WorkOrderService_SaveWorkOrder_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records._workOrder1.datecreated = DateTime.MinValue;
            Records._workOrder1.dateupdated = DateTime.MinValue;
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _uow.Object);
            //
            //Act
            _serv.SaveWorkOrder(Records._workOrder1, user);
            //
            //Assert
            Assert.IsTrue(Records._workOrder1.Updatedby == user);
            Assert.IsTrue(Records._workOrder1.dateupdated > DateTime.MinValue);
        }
    }
}
