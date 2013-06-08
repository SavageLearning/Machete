#region COPYRIGHT
// File:     EmployerControllerTests.cs
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Machete.Service;
using Machete.Web.Controllers;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Web.ViewModel;
using Machete.Web.Helpers;
using Machete.Data;

namespace Machete.Test.UnitTests.Controllers
{
    /// <summary>
    /// Summary description for EmployerControllerUnitTests
    /// </summary>

    [TestClass]
    public class EmployersControllerTests
    {
        Mock<IEmployerService> serv;
        Mock<IWorkOrderService> woServ;
        Mock<ILookupCache> lcache;
        EmployerController ctrlr;
        FormCollection form;
        const int Testid = 4242;

        [TestInitialize]
        public void TestInitialize()
        {
            WorkOrder.iPending = 123;
            serv = new Mock<IEmployerService>();
            woServ = new Mock<IWorkOrderService>();
            lcache = new Mock<ILookupCache>();

            ctrlr = new EmployerController(serv.Object, woServ.Object);
            ctrlr.SetFakeControllerContext();
            form = new FormCollection
                       {
                           {"ID", "12345"},
                           {"name", "blah"},
                           {"address1", "UnitTest"},
                           {"city", "footown"},
                           {"state", "WA"},
                           {"phone", "123-456-7890"},
                           {"zipcode", "1234567890"}
                       };
            Lookups.Initialize(lcache.Object);
            MacheteMapper.Initialize();
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_index_get_returns_enumerable_list()
        {
            //Arrange
            //Act
            var result = (ViewResult)ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_create_get_returns_employer()
        {
            //Arrange
            //Act
            var result = (PartialViewResult)ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_create_valid_post_returns_json()
        {
            //Arrange
            var employer = new Employer {ID = 4242, name = "unit test"};
            serv.Setup(p => p.Create(employer, "UnitTest")).Returns(employer);
            ctrlr.ValueProvider = form.ToValueProvider();
            //Act
            var result = ctrlr.Create(employer, "UnitTest");
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ sNewRef = /Employer/Edit/12345, sNewLabel = blah, iNewID = 12345, jobSuccess = True }", 
                            result.Data.ToString());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_createcombined_valid_post_returns_json()
        {
            //Arrange
            var combined = new EmployerWoCombined { name = "unit test" };
            var employer = new Employer { ID = 4242 };
            var wo = new WorkOrder { ID= 4243, EmployerID = 4242 };
            serv.Setup(p => p.Create(It.IsAny<Employer>(), "UnitTest")).Returns(employer);
            woServ.Setup(p => p.Create(It.IsAny<WorkOrder>(), "UnitTest")).Returns(wo);
            ctrlr.ValueProvider = form.ToValueProvider();
            //Act
            var result = ctrlr.CreateCombined(combined, "UnitTest");
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            //Assert.IsInstanceOfType(result.Data["EmployerWoConbined"], typeof(EmployerWoCombined));
            Assert.AreEqual("{ iEmployerID = 4242, iWorkOrderID = 4243, jobSuccess = True }", result.Data.ToString());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void EmployerController_create_post_invalid_throws_exception()
        {
            //Arrange
            var employer = new Employer();
            form.Remove("name");

            serv = new Mock<IEmployerService>();
            serv.Setup(p => p.Create(employer, "UnitTest")).Returns(employer);
            woServ = new Mock<IWorkOrderService>();
            ctrlr = new EmployerController(serv.Object, woServ.Object);
            ctrlr.SetFakeControllerContext();
            ctrlr.ValueProvider = form.ToValueProvider();
            JsonResult result = ctrlr.Create(employer, "UnitTest");
            Assert.IsNotNull(result);

        }
        #endregion
        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_edit_get_returns_employer()
        {
            //Arrange
            serv = new Mock<IEmployerService>();
            var fakeemployer = new Employer();
            serv.Setup(p => p.Get(Testid)).Returns(fakeemployer);
            woServ = new Mock<IWorkOrderService>();
            ctrlr = new EmployerController(serv.Object, woServ.Object);
            //Act
            var result = ctrlr.Edit(Testid) as PartialViewResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_edit_post_valid_updates_model_returns_json()
        {
            //Arrange
            const int testid = 4242;
            var fakeemployer = new Employer();
            var savedemployer = new Employer();
            serv.Setup(p => p.Get(testid)).Returns(fakeemployer);
            serv.Setup(x => x.Save(It.IsAny<Employer>(),
                                          It.IsAny<string>())
                                         ).Callback((Employer p, string str) =>
                                         {
                                             savedemployer = p;
                                         });
            ctrlr.ValueProvider = form.ToValueProvider();
            //Act
            var result = ctrlr.Edit(testid, form, "UnitTest");
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ jobSuccess = True }", result.Data.ToString());
            Assert.AreEqual(fakeemployer, savedemployer);
            Assert.AreEqual(savedemployer.name, "blah");
            Assert.AreEqual(savedemployer.address1, "UnitTest");
            Assert.AreEqual(savedemployer.city, "footown");
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void EmployerController_edit_post_invalid_throws_exception()
        {
            //Arrange
            var employer = new Employer();
            //
            // Mock service and setup SaveEmployer mock
            serv.Setup(p => p.Save(employer, "UnitTest"));
            serv.Setup(p => p.Get(Testid)).Returns(employer);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            form.Remove("phone");
            ctrlr.ValueProvider = form.ToValueProvider();
            //
            //Act
            //_ctrlr.ModelState.AddModelError("TestError", "foo");
            ctrlr.Edit(Testid, form, "UnitTest");
            //Assert
        }
        #endregion

        //
        // Testing /Delete functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void EmployerController_delete_post_returns_json()
        {
            //Arrange
            serv = new Mock<IEmployerService>();
            var fakeform = new FormCollection();
            woServ = new Mock<IWorkOrderService>();
            ctrlr = new EmployerController(serv.Object, woServ.Object);
            ctrlr.SetFakeControllerContext();
            ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = ctrlr.Delete(Testid, "UnitTest");
            //Assert
            Assert.AreEqual("{ status = OK, jobSuccess = True, deletedID = 4242 }", 
                            result.Data.ToString());
        }
    }
}
