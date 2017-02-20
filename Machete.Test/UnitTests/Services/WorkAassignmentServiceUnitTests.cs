#region COPYRIGHT
// File:     WorkAassignmentServiceUnitTests.cs
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
    /// Summary description for WorkAssignmentServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkAssignmentTests
    {
        Mock<IWorkAssignmentRepository> waRepo;
        Mock<IUnitOfWork> uow;
        Mock<ILookupRepository> lRepo;
        Mock<IWorkerRepository> wRepo;
        Mock<IWorkerSigninRepository> wsiRepo;
        WorkAssignmentService waServ;
        Mock<IWorkerRequestRepository> wrRepo;
        Mock<ILookupCache> lcache;
        Mock<IMapper> _map;

        public WorkAssignmentTests()
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
            waRepo = new Mock<IWorkAssignmentRepository>();
            uow = new Mock<IUnitOfWork>();
            wRepo = new Mock<IWorkerRepository>();
            lRepo = new Mock<ILookupRepository>();
            wsiRepo = new Mock<IWorkerSigninRepository>();
            wrRepo = new Mock<IWorkerRequestRepository>();
            lcache = new Mock<ILookupCache>();
            _map = new Mock<IMapper>();
            waServ = new WorkAssignmentService(waRepo.Object, wRepo.Object, lRepo.Object, wsiRepo.Object, lcache.Object, uow.Object, _map.Object);
            
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WAs)]
        public void GetWorkAssignments_returns_Enumerable()
        {
            //
            //Arrange

            //Act
            var result = waServ.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkAssignment>));
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WAs)]
        public void GetWorkAssignment_returns_workAssignment()
        {
            //
            //Arrange
            WorkAssignment assignment = (WorkAssignment)Records.assignment.Clone();
            assignment.ID = 1; //This matches Records._workAssignment3 ID value
            waRepo.Setup(r => r.GetById(1)).Returns(assignment);
            //Act
            var result = waServ.Get(1);
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
            Assert.IsTrue(result.ID == 1);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WAs)]
        public void CreateWorkAssignment_returns_workAssignment()
        {
            //
            //Arrange
            string user = "UnitTest";
            var _wa = (WorkAssignment)Records.assignment.Clone();
            _wa.datecreated = DateTime.MinValue;
            _wa.dateupdated = DateTime.MinValue;
            _wa.workOrder = (WorkOrder)Records.order.Clone();
            _wa.workOrder.paperOrderNum = _wa.workOrder.ID;
            waRepo.Setup(r => r.Add(_wa)).Returns(_wa);
            //
            //Act
            var result = waServ.Create(_wa, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
            Assert.IsTrue(result.createdby == user);
            Assert.IsTrue(result.updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WAs)]
        public void DeleteWorkAssignment()
        {
            //
            //Arrange
            string user = "UnitTest";
            int id = 1;
            var _wa = (WorkAssignment)Records.assignment.Clone();

            WorkAssignment dp = new WorkAssignment();
           waRepo.Setup(r => r.Delete(It.IsAny<WorkAssignment>())).Callback((WorkAssignment p) => { dp = p; });
            waRepo.Setup(r => r.GetById(id)).Returns(_wa);
            //
            //Act
            waServ.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, _wa);
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WAs)]
        public void SaveWorkAssignment_updates_timestamp()
        {
            //
            //Arrange
            var _wa = (WorkAssignment)Records.assignment.Clone();

            string user = "UnitTest";
            _wa.datecreated = DateTime.MinValue;
            _wa.dateupdated = DateTime.MinValue;
            _wa.workOrder = (WorkOrder)Records.order.Clone();
            _wa.workOrder.paperOrderNum = _wa.workOrder.ID;
            _wa.pseudoID = 1;
            //
            //Act
            waServ.Save(_wa, user);
            //
            //Assert
            Assert.IsTrue(_wa.updatedby == user);
            Assert.IsTrue(_wa.dateupdated > DateTime.MinValue);
        }
    }
}
