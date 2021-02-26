#region COPYRIGHT
// File:     WorkAassignmentServiceUnitTests.cs
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
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for WorkAssignmentServiceUnitTests
    /// </summary>
    [TestClass]
    public class WorkAssignmentTests
    {
        private Mock<IWorkAssignmentRepository> waRepo;
        private Mock<IUnitOfWork> uow;
        private Mock<ILookupRepository> lRepo;
        private Mock<IWorkerRepository> wRepo;
        private Mock<IWorkerSigninRepository> wsiRepo;
        private WorkAssignmentService waServ;
        private Mock<IMapper> _map;
        private Mock<IWorkOrderRepository> woRepo;
        private Mock<ITenantService> _tenantService;

        public WorkAssignmentTests()
        {
        }


        [TestInitialize]
        public void TestInitialize()
        {
            waRepo = new Mock<IWorkAssignmentRepository>();
            uow = new Mock<IUnitOfWork>();
            wRepo = new Mock<IWorkerRepository>();
            lRepo = new Mock<ILookupRepository>();
            wsiRepo = new Mock<IWorkerSigninRepository>();
            _map = new Mock<IMapper>();
            woRepo = new Mock<IWorkOrderRepository>();
            _tenantService = new Mock<ITenantService>();
            _tenantService.Setup(service => service.GetCurrentTenant()).Returns(UnitTestExtensions.TestingTenant);            
            
            waServ = new WorkAssignmentService(waRepo.Object, wRepo.Object, woRepo.Object, lRepo.Object, wsiRepo.Object, uow.Object, _map.Object, _tenantService.Object);
            
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
            var _l = (Lookup)Records.lookup.Clone();

            _wa.datecreated = DateTime.MinValue;
            _wa.dateupdated = DateTime.MinValue;
            _wa.workOrder = (WorkOrder)Records.order.Clone();

            woRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_wa.workOrder);
            
            _wa.workOrder.paperOrderNum = _wa.workOrder.ID;
            waRepo.Setup(r => r.Add(_wa)).Returns(_wa);
            lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);
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
            var _l = (Lookup)Records.lookup.Clone();
            lRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(_l);

            string user = "UnitTest";
            _wa.datecreated = DateTime.MinValue;
            _wa.dateupdated = DateTime.MinValue;
            _wa.workOrder = (WorkOrder)Records.order.Clone();

            woRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_wa.workOrder);
            
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
