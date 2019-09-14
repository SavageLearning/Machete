#region COPYRIGHT
// File:     WorkOrderServiceTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test.Old
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
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for WorkOrderServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkOrderServiceTests
    {
        Mock<IWorkOrderRepository> _repo;
        Mock<IWorkAssignmentService> _waServ;
        Mock<IWorkerRequestService> _wrServ;
        Mock<IWorkerService> _wServ;
        Mock<ILookupRepository> _lRepo;
        Mock<IUnitOfWork> _uow;
        Mock<IMapper> _map;
        Mock<IConfigService> _cfg;
        Mock<ITransportProvidersService> _tpServ;
        WorkOrderService _serv;
        string user;
        Mock<ITenantService> _tenantService;

        public WorkOrderServiceTests()
        {
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
            _waServ = new Mock<IWorkAssignmentService>();
            _wrServ = new Mock<IWorkerRequestService>();
            _wServ = new Mock<IWorkerService>();
            _uow = new Mock<IUnitOfWork>();
            _map = new Mock<IMapper>();
            _lRepo = new Mock<ILookupRepository>();
            _cfg = new Mock<IConfigService>();
            _tpServ = new Mock<ITransportProvidersService>();
            _tenantService = new Mock<ITenantService>();
            
            _tenantService.Setup(service => service.GetCurrentTenant()).Returns(UnitTestExtensions.TestingTenant);
            
            _serv = new WorkOrderService(_repo.Object, _waServ.Object, _tpServ.Object, _wrServ.Object, _wServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object, _tenantService.Object);
            user = "UnitTest";
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
            _tpServ = new Mock<ITransportProvidersService>();

            var _wo = (WorkOrder)Records.order.Clone();
            var _l = (Lookup)Records.lookup.Clone();
            var _tp = (TransportProvider)Records.transportProvider.Clone();
            string user = "UnitTest";
            _wo.datecreated = DateTime.MinValue;
            _wo.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_wo)).Returns(_wo);
            _lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
            _tpServ.Setup(r => r.Get(It.IsAny<int>())).Returns(_tp);

            var _serv = new WorkOrderService(
                _repo.Object, 
                _waServ.Object, 
                _tpServ.Object, 
                _wrServ.Object, 
                _wServ.Object, 
                _lRepo.Object, 
                _uow.Object, 
                _map.Object, 
                _cfg.Object, 
                _tenantService.Object
            );
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
            int id = 1;
            WorkOrder dp = new WorkOrder();
            _repo.Setup(r => r.Delete(It.IsAny<WorkOrder>())).Callback((WorkOrder p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(_wo);
            _waServ = new Mock<IWorkAssignmentService>();
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _tpServ.Object, _wrServ.Object, _wServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object, _tenantService.Object);
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
            _tpServ = new Mock<ITransportProvidersService>();
            var _tp = (TransportProvider)Records.transportProvider.Clone();
            _tpServ.Setup(r => r.Get(It.IsAny<int>())).Returns(_tp);
            var _serv = new WorkOrderService(_repo.Object, _waServ.Object, _tpServ.Object, _wrServ.Object, _wServ.Object, _lRepo.Object, _uow.Object, _map.Object, _cfg.Object, _tenantService.Object);
            //
            //Act
            _serv.Save(_wo, user);
            //
            //Assert
            Assert.IsTrue(_wo.updatedby == user);
            Assert.IsTrue(_wo.dateupdated > DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void SaveWorkOrder_finds_duplicate_workrequests()
        {
            //Arrange

            // Lookups are called for updateComputedValues
            Lookup _l = (Lookup)Records.lookup.Clone();
            TransportProvider _tp = (TransportProvider)Records.transportProvider.Clone();
            _lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            var workerRequest = new List<WorkerRequest>();
            fakeworkOrder.workerRequestsDDD = workerRequest;
            fakeworkOrder.ID = testid;
            WorkerRequest wr1 = new WorkerRequest
            {
                ID = 111,
                WorkerID = 1,
                WorkOrderID = testid,
                workerRequested = new Worker { ID = 1, dwccardnum = 12345 }

            };

            WorkerRequest wr2 = new WorkerRequest
            {
                ID = 222,
                WorkerID = 2,
                WorkOrderID = testid,
                workerRequested = new Worker { ID = 2, dwccardnum = 12346 }
            };
            workerRequest.Add(wr1);
            workerRequest.Add(wr2);

            // receives WO passed to repository
            
            List<WorkerRequest> list = new List<WorkerRequest>();
            list.Add(new WorkerRequest { WorkerID = 12345 });
            list.Add(new WorkerRequest { WorkerID = 30002 });
            list.Add(new WorkerRequest { WorkerID = 30311 });
            list.Add(new WorkerRequest { WorkerID = 30420 });
            list.Add(new WorkerRequest { WorkerID = 30421 });

            string user = "";
            _repo.Setup(r => r.GetById(testid)).Returns(fakeworkOrder);

            _wrServ.Setup(x => x.GetByID(testid, 1)).Returns(wr1);
            _wrServ.Setup(x => x.GetByID(testid, 2)).Returns(wr2);
            _wrServ.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<string>()));
            _tpServ.Setup(x => x.Get(It.IsAny<int>())).Returns(_tp);
            _tpServ.Setup(x => x.Get(It.IsAny<int>())).Returns(_tp);
            
            //Act
            _serv.Save(fakeworkOrder, list, user);
            
            //Assert
            //Assert.AreEqual(fakeworkOrder, savedworkOrder);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(), 5);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(a => a.WorkerID == 12345), 1);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(a => a.WorkerID == 30002), 1);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(a => a.WorkerID == 30311), 1);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(a => a.WorkerID == 30420), 1);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(a => a.WorkerID == 30421), 1);
            Assert.AreEqual(fakeworkOrder.workerRequestsDDD.Count(a => a.WorkerID == 12346), 0);
        }

    }
}
