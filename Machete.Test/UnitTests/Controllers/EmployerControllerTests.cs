#region COPYRIGHT
// File:     EmployerControllerTests.cs
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
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Machete.Web.Maps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers
{
    [TestClass]
    public class EmployerControllerTests
    {
        Mock<IEmployerService> _serv;
        IMapper map;
        Mock<IDefaults> _defaults;
        EmployerController _controller;
        private Mock<IModelBindingAdaptor> _adaptor;
        private Employer _savedemployer;
        private Employer _fakeemployer;
        const int testId = 4242;

        [TestInitialize]
        public void TestInitialize()
        {
            _serv = new Mock<IEmployerService>();
            _defaults = new Mock<IDefaults>();
            _adaptor = new Mock<IModelBindingAdaptor>();

            var mapperConfig = new MvcMapperConfiguration().Config;
            map = mapperConfig.CreateMapper();

            _adaptor.Setup(dependency => 
                    dependency.TryUpdateModelAsync(It.IsAny<MacheteController>(), It.IsAny<Employer>()))
                .Returns(Task.FromResult(true));
            
            _fakeemployer = new Employer {
                ID = 12345,
                name = "blah",
                address1 = "UnitTest",
                city = "footown",
                state = "WA",
                phone = "123-456-7890",
                zipcode = "1234567890"
            };
            _savedemployer = new Employer();
            _serv.Setup(p => p.Get(testId)).Returns(_fakeemployer);
            _serv.Setup(x => x.Save(It.Is<Employer>(employer => employer.name == "blah"), It.IsAny<string>()))
                 .Callback((Employer p, string str) => { _savedemployer = p; });
            
            _controller = new EmployerController(_serv.Object, _defaults.Object, map, _adaptor.Object);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public async Task index_get_returns_enumerable_list()
        {
            //Arrange
            
            //Act
            var result = await _controller.Index() as ViewResult;
            
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public async Task create_get_returns_employer()
        {
            //Arrange
            
            //Act
            var result = await _controller.Create() as PartialViewResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Employer));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public async Task create_valid_post_returns_json()
        {
            //Arrange
            var employer = new Employer { ID = 12345, name = "blah" };
            _serv.Setup(p => p.Create(employer, "UnitTest")).Returns(employer);
            
            //Act
            var result = await _controller.Create(employer, "UnitTest");

            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ sNewRef = /Employer/Edit/12345, sNewLabel = blah, iNewID = 12345, jobSuccess = True }", 
                            result.Value.ToString());
        }

        // Passing an invalid model isn't a valid approach, since model binding isn't running
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-2.2
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task create_post_invalid_throws_exception()
        {
            //Arrange
            var employer = new Employer { ID = testId, name = string.Empty };
            _serv.Setup(p => p.Create(employer, "UnitTest")).Returns(employer);
            _controller = new EmployerController(_serv.Object, _defaults.Object, map, _adaptor.Object);
            _controller.ModelState.AddModelError("name", "Required");
            
            //Act
            var result = await _controller.Create(employer, "UnitTest");
            
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public async Task EmployerController_edit_get_returns_employer()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            _serv.Setup(p => p.Get(testId)).Returns(new Employer());
                        
            var mapperConfig = new MvcMapperConfiguration().Config;
            var mapper = mapperConfig.CreateMapper();
            
            _controller = new EmployerController(_serv.Object, _defaults.Object, mapper, _adaptor.Object);
            //Act
            var result = await _controller.Edit(testId) as PartialViewResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Employer));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public async Task EmployerController_edit_post_valid_updates_model_returns_json()
        {
            //Arrange
            
            //Act
            var result = await _controller.Edit(testId, "UnitTest");
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ jobSuccess = True }", result.Value.ToString());
            Assert.AreEqual(_fakeemployer, _savedemployer);
            Assert.AreEqual("blah", _savedemployer.name);
            Assert.AreEqual("UnitTest", _savedemployer.address1);
            Assert.AreEqual("footown", _savedemployer.city);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task EmployerController_edit_post_invalid_throws_exception()
        {
            //Arrange
            var employer = new Employer();
            _serv.Setup(p => p.Save(employer, "UnitTest"));
            _serv.Setup(p => p.Get(testId)).Returns(employer);
            _controller.ModelState.AddModelError("TestError", "foo");

            //Act
            await _controller.Edit(testId, "UnitTest");
            
            //Assert
        }

        //
        // Testing /Delete functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void delete_post_returns_json()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            _controller = new EmployerController(_serv.Object, _defaults.Object, map, _adaptor.Object);
            
            //Act
            var result = _controller.Delete(testId, "UnitTest");
            //Assert
            Assert.AreEqual("{ status = OK, jobSuccess = True, deletedID = 4242 }", 
                            result.Value.ToString());
        }
    }
}
