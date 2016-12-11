#region COPYRIGHT
// File:     WorkAssignmentControllerTests.cs
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
using Moq;
using Machete.Data;
using Machete.Service;
using Machete.Data.Infrastructure;
using Machete.Web.Controllers;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Test;
using Machete.Web.ViewModel;
using Machete.Web.Helpers;
using AutoMapper;

namespace Machete.Test.Unit.Controller
{
    /// <summary>
    /// Summary description for WorkAssignmentControllerUnitTests
    /// </summary>

    [TestClass]
    public class WorkAssignmentTests
    {
        Mock<IWorkAssignmentService> waServ;
        Mock<IWorkerService> wkrServ;
        Mock<IWorkOrderService> woServ;
        Mock<IWorkerSigninService> wsiServ;
        Mock<IDefaults> def;
        Mock<IMapper> map;
        WorkAssignmentController _ctrlr;
        WorkAssignmentIndex _view;
        FormCollection fakeform;
        Mock<ILookupCache> lcache;
        Mock<IDatabaseFactory> dbfactory;

        [TestInitialize]
        public void TestInitialize()
        {
            waServ = new Mock<IWorkAssignmentService>();
            wkrServ = new Mock<IWorkerService>();
            woServ = new Mock<IWorkOrderService>();
            wsiServ = new Mock<IWorkerSigninService>();
            def = new Mock<IDefaults>();
            map = new Mock<IMapper>();
            lcache = new Mock<ILookupCache>();
            dbfactory = new Mock<IDatabaseFactory>();
            _ctrlr = new WorkAssignmentController(waServ.Object, wkrServ.Object, woServ.Object, wsiServ.Object, lcache.Object, def.Object, map.Object);
            _view = new WorkAssignmentIndex();
            _ctrlr.SetFakeControllerContext();
            fakeform = new FormCollection();
            fakeform.Add("ID", "12345");
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Unit_WA_Controller_index_get_returns_WorkAssignmentIndexViewModel()
        {
            //Arrange

            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignmentIndex));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void create_get_returns_workAssignment()
        {
            // Arrange
            var vmwo = new Machete.Web.ViewModel.WorkAssignment();
            map.Setup(x => x.Map<Domain.WorkAssignment, Machete.Web.ViewModel.WorkAssignment>(It.IsAny<Domain.WorkAssignment>()))
                .Returns(vmwo);
            var lc = new List<Lookup>();
            lcache.Setup(p => p.getCache()).Returns(() => lc);
            //Act
            var result = (PartialViewResult)_ctrlr.Create(0, "Unit WA Cntroller desc");
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.WorkAssignment));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void create_valid_post_returns_json()
        {
            //Arrange            
            var vmwo = new Machete.Web.ViewModel.WorkAssignment();
            map.Setup(x => x.Map<Domain.WorkAssignment, Machete.Web.ViewModel.WorkAssignment>(It.IsAny<Domain.WorkAssignment>()))
                .Returns(vmwo);
            Domain.WorkAssignment _asmt = new Domain.WorkAssignment();
            fakeform.Add("ID", "11");
            fakeform.Add("englishlevelID", "0");
            fakeform.Add("skillID", "60");
            fakeform.Add("hours", "5");
            fakeform.Add("hourlyWage", "12");
            fakeform.Add("days", "1");
            Domain.WorkOrder _wo = new Domain.WorkOrder();
            _wo.paperOrderNum = 12345;
            _wo.ID = 123;
            int _num = 0;

            string username = "UnitTest";
            woServ.Setup(p => p.Get(_num)).Returns(() => _wo);
            waServ.Setup(p => p.Create(_asmt, username)).Returns(() => _asmt);
            
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            JsonResult result = (JsonResult)_ctrlr.Create(_asmt, username);
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            //Assert.AreEqual("{ sNewRef = /WorkAssignment/Edit/12345, sNewLabel = Assignment #: 12345-01, iNewID = 12345 }", 
            //                result.Data.ToString());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void create_post_invalid_throws_exception()
        {
            //Arrange
            Domain.WorkAssignment _asmt = new Domain.WorkAssignment();
            fakeform.Add("hours", "invalid data type");
            waServ.Setup(p => p.Create(_asmt, "UnitTest")).Returns(_asmt);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            _ctrlr.Create(_asmt, "UnitTest");
            //Assert
        }
        #endregion
        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Unit_WA_Controller_edit_get_returns_workAssignment()
        {
            //Arrange
            var vmwo = new Machete.Web.ViewModel.WorkAssignment();
            map.Setup(x => x.Map<Domain.WorkAssignment, Machete.Web.ViewModel.WorkAssignment>(It.IsAny<Domain.WorkAssignment>()))
                .Returns(vmwo);
            int testid = 4242;
            var fakeworkAssignment = new Domain.WorkAssignment();
            fakeworkAssignment.ID = 4243;
            waServ.Setup(p => p.Get(testid)).Returns(() => fakeworkAssignment);
            //Act
            PartialViewResult result = (PartialViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Domain.WorkAssignment));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Unit_WA_Controller_edit_post_valid_updates_model_returns_json()
        {
            //Arrange
            var vmwo = new Machete.Web.ViewModel.WorkAssignment();
            map.Setup(x => x.Map<Domain.WorkAssignment, Machete.Web.ViewModel.WorkAssignment>(It.IsAny<Domain.WorkAssignment>()))
                .Returns(vmwo);
            int testid = 4242;
            Worker wkr = new Worker();
            wkr.ID = 424;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("description", "blah");     //Every required field must be populated,
            fakeform.Add("comments", "UnitTest");  //or result will be null.            
            Domain.WorkAssignment asmt = new Domain.WorkAssignment();
            Domain.WorkAssignment savedAsmt = null;
            asmt.workerAssignedID = wkr.ID;
            asmt.ID = testid;
            string user = "";
            waServ.Setup(p => p.Get(testid)).Returns(asmt);
            waServ.Setup(x => x.Save(It.IsAny<Domain.WorkAssignment>(),
                                          It.IsAny<string>())
                                         ).Callback((Domain.WorkAssignment p, string str) =>
                                         {
                                             savedAsmt = p;
                                             user = str;
                                         });
            wkrServ.Setup(p => p.Get((int)asmt.workerAssignedID)).Returns(wkr);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, null, "UnitTest") as JsonResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ jobSuccess = True }",
                            result.Data.ToString());
            Assert.AreEqual(asmt, savedAsmt);
            Assert.AreEqual(savedAsmt.description, "blah");
            Assert.AreEqual(savedAsmt.comments, "UnitTest");
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void Unit_WA_Controller_edit_post_invalid_throws_exception()
        {
            //Arrange
            var asmt = new Web.ViewModel.WorkAssignment();
            Worker wkr = new Worker();
            wkr.ID = 424;
            int testid = 4243;
            asmt.ID = testid;
            asmt.workerAssignedID = wkr.ID;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("hours", "blah");
            fakeform.Add("comments", "UnitTest");
            //
            // Mock service and setup SaveWorkAssignment mock
            waServ.Setup(p => p.Save(asmt, "UnitTest"));
            waServ.Setup(p => p.Get(testid)).Returns(asmt);
            wkrServ.Setup(p => p.Get((int)asmt.workerAssignedID)).Returns(wkr);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            //_ctrlr.ModelState.AddModelError("TestError", "foo");
            _ctrlr.Edit(testid, null, "UnitTest");
            //Assert
        }
        #endregion

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void delete_post_returns_json()
        {
            //Arrange
            int testid = 4242;
            FormCollection fakeform = new FormCollection();

            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();

            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as JsonResult;
            //Assert
            Assert.AreEqual("{ status = OK, jobSuccess = True, deletedID = 4242 }", 
                            result.Data.ToString());
        }
    }
}
