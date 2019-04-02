#region COPYRIGHT
// File:     WorkerServiceUnitTests.cs
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
using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for WorkerServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkerTests
    {
        Mock<IWorkerRepository> _repo;
        Mock<IUnitOfWork> _uow;
        Mock<ILookupRepository> _lRepo;
        WorkerService _serv;
        Mock<IWorkAssignmentRepository> _waRepo;
        Mock<IWorkOrderRepository> _woRepo;
        Mock<IPersonRepository> _pRepo;
        Mock<IMapper> _map;
        public WorkerTests()
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
            _repo = new Mock<IWorkerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _waRepo = new Mock<IWorkAssignmentRepository>();
            _woRepo = new Mock<IWorkOrderRepository>();
            _pRepo = new Mock<IPersonRepository>();
            _map = new Mock<IMapper>();
            _lRepo = new Mock<ILookupRepository>();
            _serv = new WorkerService(_repo.Object, _lRepo.Object, _uow.Object, _waRepo.Object, _woRepo.Object, _pRepo.Object, _map.Object);
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void GetWorkers_returns_Enumerable()
        {
            //
            //Arrange
            //Act
            var result = _serv.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Worker>));
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void GetWorker_returns_worker()
        {
            //
            //Arrange
            Worker worker = (Worker)Records.worker.Clone();
            worker.ID = 3;
            int id = 3; //This matches Records._worker3 ID value
            _repo.Setup(r => r.GetById(id)).Returns(worker);
            //Act
            var result = _serv.Get(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(Worker));
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void CreateWorker_returns_worker()
        {
            //
            //Arrange
            string user = "UnitTest";
            Worker _w = (Worker)Records.worker.Clone();
            Lookup _l = (Lookup)Records.lookup.Clone();
            Person _p = (Person)Records.person.Clone();
            _w.Person = _p;
            _w.Person.datecreated = DateTime.MinValue;
            _w.Person.dateupdated = DateTime.MinValue;
            //Records._worker1.datecreated = DateTime.MinValue;
            //Records._worker1.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_w)).Returns(_w);
            _pRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_p);
            _lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
            //
            //Act
            var result = _serv.Create(_w, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Worker));
            Assert.IsTrue(result.createdby == user);
            Assert.IsTrue(result.updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
            Assert.IsTrue(result.Person.datecreated == DateTime.MinValue);
            Assert.IsTrue(result.Person.dateupdated == DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void DeleteWorker()
        {
            //
            //Arrange
            _repo = new Mock<IWorkerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _map = new Mock<IMapper>();
            _lRepo = new Mock<ILookupRepository>();
            string user = "UnitTest";
            int id = 1;
            Worker dp = new Worker();
            _repo.Setup(r => r.Delete(It.IsAny<Worker>())).Callback((Worker p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns((Domain.Worker)Records.worker);
            var _serv = new WorkerService(_repo.Object, _lRepo.Object, _uow.Object, _waRepo.Object, _woRepo.Object, _pRepo.Object, _map.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records.worker);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void SaveWorker_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IWorkerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _map = new Mock<IMapper>();
            _lRepo = new Mock<ILookupRepository>();
            Worker _w = (Worker)Records.worker.Clone();
            Lookup _l = (Lookup)Records.lookup.Clone();
            _w.Person = (Person)Records.person.Clone();
            _w.Person.datecreated = DateTime.MinValue;
            _w.Person.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_w)).Returns(_w);
            _lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
            string user = "UnitTest";
            var _serv = new WorkerService(_repo.Object, _lRepo.Object, _uow.Object, _waRepo.Object, _woRepo.Object, _pRepo.Object, _map.Object);
            //
            //Act
            _serv.Save(_w, user);
            //
            //Assert
            Assert.IsTrue(_w.updatedby == user);
            Assert.IsTrue(_w.dateupdated > DateTime.MinValue);
        }
    }
}
