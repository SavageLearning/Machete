#region COPYRIGHT
// File:     WorkOrderControllerTests.cs
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
using System.Threading.Tasks;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Machete.Web.Maps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers
{
    /// <summary>
    /// Summary description for WorkOrderControllerUnitTests
    /// </summary>

    [TestClass]
    public class WorkOrderControllerTests
    {
        Mock<IWorkOrderService> _serv;
        Mock<IDefaults> def;
        IMapper map;
        List<WorkerRequest> _workerRequest;
        WorkOrderController _controller;
        private int _testid = 4242;

        private WorkOrder _fakeWorkOrder;
        private Mock<IModelBindingAdaptor> _adaptor;
        private WorkOrder _savedWorkOrder;
        private Mock<ITenantService> _tenantService;
        private readonly Tenant _tenant = UnitTestExtensions.TestingTenant;

        //
        [TestInitialize]
        public void TestInitialize()
        {
            _fakeWorkOrder = new WorkOrder {
                ID = _testid,
                workSiteAddress1 = "blah",
                city = "UnitTest",
                state = "WA",
                phone = "123-456-7890",
                zipcode = "12345-6789",
                typeOfWorkID = 1,
                dateTimeofWork = new DateTime(2019, 01, 01),
                transportMethodID = 1,
                transportFee = 20.00,
                transportFeeExtra = 8.00,
                statusID = 43, // active work order
                contactName = "test script contact name",
                paperOrderNum = 42424
            };

            _savedWorkOrder = new WorkOrder();

            _serv = new Mock<IWorkOrderService>();
            _serv.Setup(p => p.Create(_fakeWorkOrder, It.IsAny<List<WorkerRequest>>(), "UnitTest", null)).Returns(() => _fakeWorkOrder);
            _serv.Setup(p => p.Get(_testid)).Returns(_fakeWorkOrder);
            _serv.Setup(x => x.Save(It.IsAny<WorkOrder>(), It.IsAny<List<WorkerRequest>>(), It.IsAny<string>()))
                .Callback((WorkOrder p, List<WorkerRequest> wr, string str) => {
                    _savedWorkOrder = p;
                });

            def = new Mock<IDefaults>();

            var mapperConfig = new MapperConfiguration(config => { config.ConfigureMvc(); });
            map = mapperConfig.CreateMapper();

            _workerRequest = new List<WorkerRequest>();
            
            _adaptor = new Mock<IModelBindingAdaptor>();
            _adaptor.Setup(dependency => dependency.TryUpdateModelAsync(It.IsAny<Controller>(), It.IsAny<WorkOrder>()))
                .Returns(Task.FromResult(true));

            _tenantService = new Mock<ITenantService>();
            _tenantService.Setup(service => service.GetCurrentTenant()).Returns(_tenant);
            _tenantService.Setup(service => service.GetAllTenants()).Returns(new List<Tenant> {_tenant});

            Mock<IWorkerRequestService> req = new Mock<IWorkerRequestService>();
            req.Setup(service => service.GetByID(It.IsAny<int>(), It.IsAny<int>())).Returns(new WorkerRequest());
            req.Setup(service => service.GetAllByWorkOrderID(It.IsAny<int>())).Returns(new List<WorkerRequest>());
            
            _controller = new WorkOrderController(_serv.Object, req.Object, def.Object, map, _adaptor.Object, _tenantService.Object);
        }

        //   Testing /Index functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void index_get_returns_enumerable_list()
        {
            var result = (ViewResult)_controller.Index();
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        //   Testing /Create functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void create_get_returns_workOrder()
        {
            // Arrange

            // Act
            var result = (PartialViewResult)_controller.Create(0);

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Machete.Web.ViewModel.WorkOrderMVC));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public async Task create_valid_post_returns_JSON()
        {
            //Arrange

            //Act
            var result = await _controller.Create(_fakeWorkOrder, "UnitTest", new List<int>()) as JsonResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual(result.Value.ToString(), "{ sNewRef = /WorkOrder/Edit/4242, sNewLabel = Order #: 42424 @ blah, iNewID = 4242 }");
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task create_post_invalid_throws_exception()
        {
            //Arrange
            var workOrder = new WorkOrder();
            _serv.Setup(p => p.Create(workOrder, "UnitTest", null)).Returns(workOrder);

            //Act
            _controller.ModelState.AddModelError("this is supposed to um...", "throw");
            await _controller.Create(workOrder, "UnitTest", new List<int>());
            
            //Assert
        }

        //   Testing /Edit functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void edit_get_returns_workOrder()
        {
            //Arrange
            _serv.Setup(p => p.Get(_testid)).Returns(_fakeWorkOrder);
            
            //Act
            var result = _controller.Edit(_testid) as PartialViewResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Machete.Web.ViewModel.WorkOrderMVC));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public async Task edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            List<WorkerRequest> list = new List<WorkerRequest> {
                new WorkerRequest {WorkerID = 12345},
                new WorkerRequest {WorkerID = 30002},
                new WorkerRequest {WorkerID = 30311},
                new WorkerRequest {WorkerID = 30420},
                new WorkerRequest {WorkerID = 30421}
            };
            
            //Act
            var result = await _controller.Edit(_testid, "UnitTest", list.Select(x => x.ID).ToList());
            
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime("1/1/2019 12:00:00 AM"),
                              TimeZoneInfo.FindSystemTimeZoneById(_tenant.Timezone));
            //Assert
            Assert.AreEqual(_fakeWorkOrder, _savedWorkOrder);
            Assert.AreEqual("blah", _savedWorkOrder.workSiteAddress1);
            Assert.AreEqual("UnitTest", _savedWorkOrder.city);
            Assert.AreEqual("WA", _savedWorkOrder.state);
            Assert.AreEqual("123-456-7890", _savedWorkOrder.phone);
            Assert.AreEqual("12345-6789", _savedWorkOrder.zipcode);
            Assert.AreEqual(1, _savedWorkOrder.typeOfWorkID);
            Assert.AreEqual(utcTime, _savedWorkOrder.dateTimeofWork);
            Assert.AreEqual(1, _savedWorkOrder.transportMethodID);
            Assert.AreEqual(20.00, _savedWorkOrder.transportFee);
            //Assert.AreEqual(5, savedworkOrder.workerRequests.Count()); // TODO investigate wr broken?
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task edit_post_invalid_throws_exception()
        {
            //Arrange
            var workOrder = new WorkOrder { workerRequestsDDD = _workerRequest };
            _testid = 4242;
            _serv.Setup(p => p.Get(_testid)).Returns(workOrder);
            var list = new List<WorkerRequest>();

            //Act    
            _controller.ModelState.AddModelError("hell to the", "NO");
            await _controller.Edit(_testid, "UnitTest", list.Select(x => x.ID).ToList());

            //Assert
        }

        #region delete tests
        /// <summary>
        /// delete GET returns workOrder
        /// </summary>
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void delete_get_returns_JSON()
        {
            //Arrange
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            _serv.Setup(p => p.Get(testid)).Returns(fakeworkOrder);
            //Act
            JsonResult result = (JsonResult)_controller.Delete(testid, "test user");
            //Assert
            IDictionary<string,object> data = new RouteValueDictionary(result.Value);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(4242, data["deletedID"]);
            
        }
        /// <summary>
        /// delete POST redirects to index
        /// </summary>
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void delete_post_returns_json()
        {
            //Arrange
            _testid = 4242;

            //Act
            var result = _controller.Delete(_testid, "UnitTest") as JsonResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value.ToString(), "{ status = OK, deletedID = 4242 }");
        }
        #endregion
    }
}
