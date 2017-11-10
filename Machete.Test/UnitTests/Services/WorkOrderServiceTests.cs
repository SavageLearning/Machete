#region COPYRIGHT
// File:     WorkOrderServiceTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
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
using AutoMapper;

namespace Machete.Test.Unit.Service
{
    /// <summary>
    /// Summary description for WorkOrderServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkOrderTests
    {
        Mock<IWorkOrderRepository> _repo;
        Mock<IWorkAssignmentService> _waServ;
        Mock<ILookupRepository> _lRepo;
        Mock<IUnitOfWork> _uow;
        Mock<IMapper> _map;
        Mock<IConfigService> _cfg;
        WorkOrderService _serv;

        public WorkOrderTests()
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
            _map = new Mock<IMapper>();
            _lRepo = new Mock<ILookupRepository>();
            _cfg = new Mock<IConfigService>();
            _serv = new WorkOrderService(_repo.Object, _waServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object );
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void GetWorkOrders_returns_Enumerable()
        {
            //
            //Arrange
            //Act
            var result = _serv.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkOrder>));
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void GetWorkOrder_returns_workOrder()
        {
            //
            //Arrange
            WorkOrder order = (WorkOrder)Records.order.Clone();
            order.ID = 3; //This matches Records._workOrder3 ID value
            _repo.Setup(r => r.GetById(3)).Returns(order);
            //Act
            var result = _serv.Get(3);
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkOrder));
            Assert.IsTrue(result.ID == 3);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void CreateWorkOrder_returns_workOrder()
        {
            //
            //Arrange
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            _lRepo = new Mock<ILookupRepository>();
            _cfg = new Mock<IConfigService>();

            var _wo = (WorkOrder)Records.order.Clone();
            var _l = (Lookup)Records.lookup.Clone();
            string user = "UnitTest";
            _wo.datecreated = DateTime.MinValue;
            _wo.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_wo)).Returns(_wo);
            _lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object);
            //
            //Act
            var result = _serv.Create(_wo, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkOrder));
            Assert.IsTrue(result.createdby == user);
            Assert.IsTrue(result.updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void DeleteWorkOrder()
        {
            //
            //Arrange
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            _cfg = new Mock<IConfigService>();
            _lRepo = new Mock<ILookupRepository>();
            var _wo = (WorkOrder)Records.order.Clone();
            string user = "UnitTest";
            int id = 1;
            WorkOrder dp = new WorkOrder();
            _repo.Setup(r => r.Delete(It.IsAny<WorkOrder>())).Callback((WorkOrder p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(_wo);
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, _wo);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void SaveWorkOrder_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IWorkOrderRepository>();
            _uow = new Mock<IUnitOfWork>();
            _lRepo = new Mock<ILookupRepository>();
            _cfg = new Mock<IConfigService>();
            Lookup _l = (Lookup)Records.lookup.Clone();
            _lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
            string user = "UnitTest";
            var _wo = (WorkOrder)Records.order.Clone();
            _wo.datecreated = DateTime.MinValue;
            _wo.dateupdated = DateTime.MinValue;
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object);
            //
            //Act
            _serv.Save(_wo, user);
            //
            //Assert
            Assert.IsTrue(_wo.updatedby == user);
            Assert.IsTrue(_wo.dateupdated > DateTime.MinValue);
        }
    }
}
