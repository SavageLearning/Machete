#region COPYRIGHT
// File:     WorkerServiceUnitTests.cs
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

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for WorkerServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkerServiceUnitTests
    {
        Mock<IWorkerRepository> _repo;
        Mock<IUnitOfWork> _uow;
        Mock<IWorkerCache> _wcache;
        WorkerService _serv;
        public WorkerServiceUnitTests()
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
            _wcache = new Mock<IWorkerCache>();
            _serv = new WorkerService(_repo.Object, _wcache.Object, _uow.Object);
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void WorkerService_GetWorkers_returns_Enumerable()
        {
            //
            //Arrange
            //Act
            var result = _serv.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Worker>));
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void WorkerService_GetWorker_returns_worker()
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
        public void WorkerService_CreateWorker_returns_worker()
        {
            //
            //Arrange
            string user = "UnitTest";
            Worker _w = (Worker)Records.worker.Clone();
            _w.Person = (Person)Records.person.Clone();
            _w.Person.datecreated = DateTime.MinValue;
            _w.Person.dateupdated = DateTime.MinValue;
            //Records._worker1.datecreated = DateTime.MinValue;
            //Records._worker1.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_w)).Returns(_w);
            //
            //Act
            var result = _serv.Create(_w, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Worker));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
            Assert.IsTrue(result.Person.datecreated == DateTime.MinValue);
            Assert.IsTrue(result.Person.dateupdated == DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void WorkerService_DeleteWorker()
        {
            //
            //Arrange
            _repo = new Mock<IWorkerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _wcache = new Mock<IWorkerCache>();
            string user = "UnitTest";
            int id = 1;
            Worker dp = new Worker();
            _repo.Setup(r => r.Delete(It.IsAny<Worker>())).Callback((Worker p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(Records.worker);
            var _serv = new WorkerService(_repo.Object, _wcache.Object, _uow.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records.worker);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void WorkerService_SaveWorker_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IWorkerRepository>();
            _wcache = new Mock<IWorkerCache>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Records.worker.datecreated = DateTime.MinValue;
            Records.worker.dateupdated = DateTime.MinValue;
            var _serv = new WorkerService(_repo.Object, _wcache.Object, _uow.Object);
            //
            //Act
            _serv.Save(Records.worker, user);
            //
            //Assert
            Assert.IsTrue(Records.worker.Updatedby == user);
            Assert.IsTrue(Records.worker.dateupdated > DateTime.MinValue);
        }
    }
}
