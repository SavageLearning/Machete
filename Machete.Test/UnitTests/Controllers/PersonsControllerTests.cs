#region COPYRIGHT
// File:     PersonsControllerTests.cs
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
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Machete.Web.Maps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewModel = Machete.Web.ViewModel;

namespace Machete.Test.UnitTests.Controllers
{
    /// <summary>
    /// Summary description for PersonControllerUnitTests
    /// </summary>

    [TestClass]
    public class PersonTests
    {
        public PersonTests() { }
        
        Mock<IPersonService> _service;
        Mock<IDefaults> _defaults;
        IMapper _mapper;
        PersonController _controller;
        private ViewModel.Person _personView;
        private Person _person;
        private Mock<IModelBindingAdaptor> _adaptor;
        private int _testid;
        private Person _fakeperson;
        private Person _savedperson;
        public PersonTests(Person savedperson)
        {
            _savedperson = savedperson;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _service = new Mock<IPersonService>();
            _defaults = new Mock<IDefaults>();
            _adaptor = new Mock<IModelBindingAdaptor>();
            
            _person = new Person {
                firstname1 = "Ronald",
                lastname1 = "Reagan",
                ID = 12345
            };
            _personView = new ViewModel.Person {
                firstname1 = "Ronald",
                lastname1 = "Reagan"
            };
            
            _testid = 4242;
            _fakeperson = new Person {
                ID = _testid,
                firstname1 = "blah",
                lastname1 = "UnitTest",
                gender = 47
            };
            _savedperson = _fakeperson;

            _service.Setup(service => service.Create(_person, "UnitTest")).Returns(_person);
            _service.Setup(service => service.Get(_testid)).Returns(_fakeperson);
            _service.Setup(service => service.Save(It.IsAny<Person>(), It.IsAny<string>()))
                 .Callback((Person p, string str) => { });
            _service.Setup(service => service.Save(_person, "UnitTest"));
            _service.Setup(service => service.Get(12345)).Returns(_person);
            _service.Setup(service => service.Save(_fakeperson, "UnitTest"))
                .Callback((Person person, string str) => { person = _savedperson; });
            
            var mapperConfig = new MvcMapperConfiguration().Config;
            _mapper = mapperConfig.CreateMapper();
            
            _adaptor.Setup(dependency => 
                    dependency.TryUpdateModelAsync(It.IsAny<MacheteController>(), It.IsAny<Person>()))
                .Returns(Task.FromResult(true));
            
            _controller = new PersonController(_service.Object, _defaults.Object, _mapper, _adaptor.Object);
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public async Task index_get_returns_ActionResult()
        {
            //Arrange
            
            //Act
            var result = await _controller.Index() as ViewResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        //   Testing /Create functionality
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public async Task create_get_returns_person()
        {
            //Arrange
            
            //Act
            var result = await _controller.Create() as PartialViewResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(ViewModel.Person));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public async Task create_valid_post_returns_json()
        {
            //Arrange

            //Act
            var result = await _controller.Create(_person, "UnitTest") as JsonResult;
            
            //Assert
            Assert.IsNotNull(result);
            IDictionary<string, object> data = new RouteValueDictionary(result.Value);
            Assert.AreEqual(12345, data["iNewID"]);
            Assert.AreEqual("Ronald Reagan", data["sNewLabel"]);
            Assert.AreEqual("/Person/Edit/12345", data["sNewRef"]);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task create_post_invalid_throws_exception()
        {
            //Arrange
            var person = new Person { firstname1 = null };
            _controller.ModelState.AddModelError("firstname1", "Required");

            //Act
            await _controller.Create(person, "UnitTest");
            
            //Assert
        }

        //   Testing /Edit functionality
        #region edittests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public async Task edit_get_returns_person()
        {
            //Arrange
            _service.Setup(p => p.Get(_testid)).Returns(_person);
            
            //Act
            var result = await _controller.Edit(_testid) as PartialViewResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Person));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public async Task edit_post_valid_updates_model_returns_JSON()
        {
            //Arrange
            
            //Act
            var result = await _controller.Edit(_testid, "UnitTest") as JsonResult;
            
            //Assert
            Assert.IsNotNull(result);
            IDictionary<string, object> data = new RouteValueDictionary(result.Value);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(_fakeperson, _savedperson);
            Assert.AreEqual("blah", _savedperson.firstname1);
            Assert.AreEqual("UnitTest", _savedperson.lastname1);
            Assert.AreEqual(47, _savedperson.gender);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        [ExpectedException(typeof(InvalidOperationException), "An invalid UpdateModel was inappropriately allowed.")]
        public async Task edit_post_invalid_throws_exception()
        {
            //Arrange
            var person = new Person();
            _service.Setup(service => service.Save(person, "UnitTest"));
            _service.Setup(service => service.Get(12345)).Returns(person);
            _controller.ModelState.AddModelError("TestError", "foo");

            //Act
            await _controller.Edit(12345, "UnitTest");
            
            //Assert
        }
        #endregion

        //
        // Testing /Delete functionality
        //

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public async Task delete_post_returns_JSON()
        {
            //Arrange
                        
            //Act
            var result = await _controller.Delete(_testid, "UnitTest") as JsonResult;
 
            //Assert
            Assert.IsNotNull(result);
            IDictionary<string, object> data = new RouteValueDictionary(result.Value);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(4242, data["deletedID"]);            
        }
    }
}
