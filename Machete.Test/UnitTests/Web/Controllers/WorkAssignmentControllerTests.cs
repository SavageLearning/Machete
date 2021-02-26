#region COPYRIGHT
// File:     WorkAssignmentControllerTests.cs
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
using System.Threading.Tasks;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Machete.Web.Maps;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Extensions = Machete.Web.Helpers.Extensions;
using ViewModel = Machete.Web.ViewModel;

namespace Machete.Test.UnitTests.Controllers
{
    /// <summary>
    /// Summary description for WorkAssignmentControllerUnitTests
    /// </summary>

    [TestClass]
    public class WorkAssignmentTests
    {
        Mock<IWorkAssignmentService> _waServ;
        Mock<IWorkerService> _wkrServ;
        Mock<IWorkOrderService> _woServ;
        Mock<IWorkerSigninService> _wsiServ;
        Mock<IDefaults> _def;
        IMapper _map;
        WorkAssignmentController _controller;
        private int _fakeId;
        private Mock<IModelBindingAdaptor> _adaptor;
        private Mock<ITenantService> _tenantService;
        private Tenant _tenant = UnitTestExtensions.TestingTenant;

        [TestInitialize]
        public void TestInitialize()
        {
            _waServ = new Mock<IWorkAssignmentService>();
            _wkrServ = new Mock<IWorkerService>();
            _woServ = new Mock<IWorkOrderService>();
            _wsiServ = new Mock<IWorkerSigninService>();
            _def = new Mock<IDefaults>();
            var mapperConfig = new MapperConfiguration(config => { config.ConfigureMvc(); });
            _map = mapperConfig.CreateMapper();
            
            _adaptor = new Mock<IModelBindingAdaptor>();
            _adaptor.Setup(dependency => 
                    dependency.TryUpdateModelAsync(It.IsAny<MacheteController>(), It.IsAny<object>()))
                .Returns(Task.FromResult(true));
            
            _tenantService = new Mock<ITenantService>();
            _tenantService.Setup(service => service.GetCurrentTenant()).Returns(_tenant);
            _tenantService.Setup(service => service.GetAllTenants()).Returns(new List<Tenant> {_tenant});
            
            _controller = new WorkAssignmentController(_waServ.Object, _woServ.Object, _wsiServ.Object, _def.Object,
                _map, _adaptor.Object, _tenantService.Object);

            _fakeId = 12345;
        }

        //   Testing /Index functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Unit_WA_Controller_index_get_returns_WorkAssignmentIndexViewModel()
        {
            //Arrange

            //Act
            var result = (ViewResult)_controller.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignmentIndex));
        }

        //   Testing /Create functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void create_get_returns_workAssignment()
        {
            // Arrange
            var vmwo = new ViewModel.WorkAssignmentMVC();
//            _map.Setup(x => x.Map<WorkAssignment, ViewModel.WorkAssignment>(It.IsAny<WorkAssignment>()))
//                .Returns(vmwo);
            var lc = new List<Domain.Lookup>();
            //Act
            var result = (PartialViewResult)_controller.Create(0, "Unit WA Controller desc");
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(ViewModel.WorkAssignmentMVC));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public async Task create_valid_post_returns_json()
        {
            //Arrange            
            var vmwo = new ViewModel.WorkAssignmentMVC();
            Domain.WorkAssignment asmt = new Domain.WorkAssignment();
            asmt.ID = 4242;
            asmt.pseudoID = 01;

            Domain.WorkOrder _wo = new Domain.WorkOrder();
            _wo.paperOrderNum = _fakeId;
            _wo.ID = 123;
            int _num = 0;

            string username = "UnitTest";
            _woServ.Setup(p => p.Get(_num)).Returns(() => _wo);
            _waServ.Setup(p => p.Create(asmt, username)).Returns(() => asmt);
            
            //Act
            var result = await _controller.Create(asmt, username) as JsonResult;

            //Assert
            Assert.IsNotNull(result);            
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ sNewRef = /WorkAssignment/Edit/4242, sNewLabel = Assignment #: 12345-01, iNewID = 4242 }", 
                            result.Value.ToString());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task create_post_invalid_throws_exception()
        {
            //Arrange
            Domain.WorkAssignment _asmt = new Domain.WorkAssignment();
            _waServ.Setup(p => p.Create(_asmt, "UnitTest")).Returns(_asmt);
            _controller.ModelState.AddModelError("WorkAssignment", "null");
            
            //Act
            await _controller.Create(_asmt, "UnitTest");
            //Assert
        }

        //
        //   Testing /Edit functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Unit_WA_Controller_edit_get_returns_workAssignment()
        {
            //Arrange
            var vmwo = new Machete.Web.ViewModel.WorkAssignmentMVC();
//            _map.Setup(x => x.Map<Domain.WorkAssignment, Machete.Web.ViewModel.WorkAssignment>(It.IsAny<Domain.WorkAssignment>()))
//                .Returns(vmwo);
            int testid = 4242;
            var fakeworkAssignment = new Domain.WorkAssignment();
            fakeworkAssignment.ID = 4243;
            _waServ.Setup(p => p.Get(testid)).Returns(() => fakeworkAssignment);
            //Act
            PartialViewResult result = (PartialViewResult)_controller.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Machete.Web.ViewModel.WorkAssignmentMVC));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task Unit_WA_Controller_edit_post_invalid_throws_exception()
        {
            //Arrange
            var asmt = new Domain.WorkAssignment();
            Domain.Worker wkr = new Domain.Worker();
            wkr.ID = 424;
            int testid = 4243;
            asmt.ID = testid;
            asmt.workerAssignedID = wkr.ID;
            var values = new Dictionary<string, StringValues>();
            values.Add("ID", testid.ToString());
            values.Add("hours", "blah");
            values.Add("comments", "UnitTest");
            _waServ.Setup(p => p.Save(asmt, "UnitTest"));
            _waServ.Setup(p => p.Get(testid)).Returns(asmt);
            _wkrServ.Setup(p => p.Get((int)asmt.workerAssignedID)).Returns(wkr);
            
            //Act
            _controller.ModelState.AddModelError("TestError", "foo");
            await _controller.Edit(testid, null, "UnitTest");
            //Assert
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void delete_post_returns_json()
        {
            //Arrange
            int testid = 4242;
            var values = new Dictionary<string, StringValues>();
            var fakeform = new FormCollection(values);

            //_ctrlr.SetFakeControllerContext();
            //_ctrlr.ValueProvider = fakeform.ToValueProvider();

            //Act
            var result = _controller.Delete(testid, fakeform, "UnitTest") as JsonResult;
            //Assert
            Assert.AreEqual("{ status = OK, jobSuccess = True, deletedID = 4242 }", 
                            result.Value.ToString());
        }
    }
}
