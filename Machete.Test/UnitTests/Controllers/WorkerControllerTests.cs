#region COPYRIGHT
// File:     WorkerControllerTests.cs
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
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewModel = Machete.Web.ViewModel;

namespace Machete.Test.UnitTests.Controllers
{
    [TestClass]
    public class WorkerTests
    {
        Mock<IWorkerService> _wserv;
        Mock<IPersonService> _pserv;
        Mock<IImageService> _iserv;
        Mock<IDefaults> def;
        Mock<IMapper> map;
        WorkerController _controller;
        private Mock<IModelBindingAdaptor> _adaptor;

        [TestInitialize]
        public void TestInitialize()
        {
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            _iserv = new Mock<IImageService>();
            def = new Mock<IDefaults>();
            map = new Mock<IMapper>();
            new Mock<IDatabaseFactory>();
            
            _adaptor = new Mock<IModelBindingAdaptor>();
            
            _adaptor.Setup(dependency => dependency.TryUpdateModelAsync(It.IsAny<Controller>(), It.IsAny<Record>()))
                .Returns(Task.FromResult(true));
            
            _controller = new WorkerController(_wserv.Object, _iserv.Object, def.Object, map.Object, _adaptor.Object);

            var fakeFormValues = new Worker {
                ID = 12345,
                typeOfWorkID = 1,
                RaceID = 1,
                height = "too tall",
                weight = "too big",
                englishlevelID = 1,
                recentarrival = true,
                dateinUSA = new DateTime(2000, 1, 1),
                dateinseattle = new DateTime(2000, 1, 1),
                disabled = true,
                maritalstatus = 1,
                livewithchildren = true,
                numofchildren = 1,
                incomeID = 1,
                livealone = true,
                emcontUSAname = "",
                emcontUSAphone = "",
                emcontUSArelation = "",
                dwccardnum = 12345,
                neighborhoodID = 1,
                immigrantrefugee = false,
                countryoforiginID = 1,
                emcontoriginname = "",
                emcontoriginphone = "",
                emcontoriginrelation = "",
                memberexpirationdate = new DateTime(2000, 1, 1),
                driverslicense = false,
                licenseexpirationdate = new DateTime(2000, 1, 1),
                carinsurance = false,
                insuranceexpiration = new DateTime(2000, 1, 1),
                dateOfBirth = new DateTime(2000, 1, 1),
                dateOfMembership = new DateTime(2000, 1, 1)
            };
        }

        //
        //   Testing /Index functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Workers)]
        public void index_get_WorkIndexViewModel()
        {
            //Arrange
            
            //Act
            var result = _controller.Index() as ViewResult;
            
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Workers)]
        public void create_get_returns_worker()
        {
            //Arrange
            var p = new Machete.Web.ViewModel.Worker();
            map.Setup(x => x.Map<Worker, Machete.Web.ViewModel.Worker>(It.IsAny<Worker>()))
                .Returns(p);
            //Act
            var result = (PartialViewResult)_controller.Create(0);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Worker));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Workers)]
        public async Task WorkerController_create_post_valid_returns_json()
        {
            //Arrange
            var w = new Machete.Web.ViewModel.Worker {
                ID = 12345
            };
            map.Setup(x => x.Map<Worker, Web.ViewModel.Worker>(It.IsAny<Worker>()))
                .Returns(w);
            var worker = new Worker();
            var person = new Person();
            //
            _wserv.Setup(p => p.Create(worker, "UnitTest")).Returns(worker);
            _pserv.Setup(p => p.Create(person, "UnitTest")).Returns(person);

            //Act
            var result = await _controller.Create(worker, "UnitTest", null) as JsonResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ sNewRef = , sNewLabel = , iNewID = 12345, jobSuccess = True }",
                            result.Value.ToString());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Workers)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public async Task create_post_invalid_throws_exception()
        {
            //Arrange
            var worker = new Worker();

            //Act
            _controller.ModelState.AddModelError("hell no", "not happening");
            await _controller.Create(worker, "UnitTest", null);

            //Assert
        }

        //
        //   Testing /Edit functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Workers)]
        public void edit_get_returns_worker()
        {
            //Arrange
            var ww = new ViewModel.Worker();
            map.Setup(x => x.Map<Worker, ViewModel.Worker>(It.IsAny<Worker>()))
                .Returns(ww);
            var worker = new Worker();
            var _person = new Person();
            int testid = 4242;
            Person fakeperson = new Person();
            _wserv.Setup(p => p.Get(testid)).Returns(worker);
            //Act
            var result = (PartialViewResult)_controller.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Worker));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Workers)]
        public async Task edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            int testid = 4242;
            
            Worker fakeworker = new Worker();
            fakeworker.height = "UnitTest";
            fakeworker.weight = "UnitTest";
            
            Worker savedworker = new Worker();
            Person fakeperson = new Person();
            fakeworker.Person = fakeperson;

            string user = "TestUser";
            _wserv.Setup(p => p.Get(testid)).Returns(fakeworker);
            _pserv.Setup(p => p.Get(testid)).Returns(fakeperson);
            _wserv.Setup(x => x.Save(It.IsAny<Worker>(),
                                          It.IsAny<string>())
                                         ).Callback((Worker worker, string str) =>
                                         {
                                             savedworker = worker;
                                             user = str;
                                         });

            //Act
            var unused = await _controller.Edit(testid, "UnitTest", null) as PartialViewResult;
            
            //Assert
            Assert.AreEqual(fakeworker, savedworker);
            Assert.AreEqual("UnitTest", savedworker.height);
            Assert.AreEqual("UnitTest", savedworker.weight);
        }
    }
}
