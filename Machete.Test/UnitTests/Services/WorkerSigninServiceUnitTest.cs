#region COPYRIGHT
// File:     WorkerSigninServiceUnitTest.cs
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
    /// Summary description for WorkerSigninServiceUnitTest
    /// </summary>
    [TestClass]
    public class WorkerSigninTests
    {
        Mock<IWorkerSigninRepository> _wsiRepo;
        Mock<IWorkerService> _wServ;
        Mock<IPersonService> _pServ;
        Mock<IWorkerRequestService> _wrServ;
        Mock<IUnitOfWork> _uow;
        Mock<IImageService> _iServ;
        Mock<IConfigService> _cServ = null;
        Mock<IMapper> _map;
        List<WorkerSignin> _signins;
        List<Worker> _workers;
        List<Person> _persons;
        List<WorkerRequest> _requests;

        public WorkerSigninTests()
        {
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            _signins = new List<WorkerSignin>();
            _signins.Add(new WorkerSignin() { ID = 111, dwccardnum = 12345, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 112, dwccardnum = 12346, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 113, dwccardnum = 12347, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 114, dwccardnum = 12348, dateforsignin = DateTime.Today });
            _signins.Add(new WorkerSignin() { ID = 115, dwccardnum = 12349, dateforsignin = DateTime.Today });

            _workers = new List<Worker>();
            _workers.Add(new Worker() { ID = 1, dwccardnum = 12345 });
            _workers.Add(new Worker() { ID = 2, dwccardnum = 12347 });
            _workers.Add(new Worker() { ID = 3, dwccardnum = 12349 });
            _workers.Add(new Worker() { ID = 3, dwccardnum = 66666 });

            _persons = new List<Person>();
            _persons.Add(new Person() { ID = 1, firstname1 = "UnitTest" });
            _persons.Add(new Person() { ID = 3, firstname1 = "UnitTest" });

            _requests = new List<WorkerRequest>();
            //_requests.Add(new WorkerRequest() {ID = 1, WorkOrderID = }
            //
            // Arrange WorkerSignin
            _wsiRepo = new Mock<IWorkerSigninRepository>();
            _wsiRepo.Setup(s => s.GetAllQ()).Returns(_signins.AsQueryable());
            // Arrange Worker
            _wServ = new Mock<IWorkerService>();
            _wServ.Setup(w => w.GetAll()).Returns(_workers);
            //
            _wrServ = new Mock<IWorkerRequestService>();
            _wrServ.Setup(w => w.GetAll()).Returns(_requests);
            // Arrange Person
            _pServ = new Mock<IPersonService>();
            _pServ.Setup(s => s.GetAll()).Returns(_persons);

            _iServ = new Mock<IImageService>();
            _uow = new Mock<IUnitOfWork>();
            _map = new Mock<IMapper>();
            
            _cServ = new Mock<IConfigService>();
        }

        // 2019 TODO not sure what we are testing here; a WSI without a worker match literally should not succeed
        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void Create_WSI_without_worker_match_succeeds()
        {
            //
            //Arrange
            var tenantService = new Mock<ITenantService>();
            tenantService.Setup(service => service.GetCurrentTenant()).Returns(UnitTestExtensions.TestingTenant);

            var _serv = new WorkerSigninService(_wsiRepo.Object, _wServ.Object, _iServ.Object, _wrServ.Object, _uow.Object, _map.Object, _cServ.Object, tenantService.Object);
            var _signin = new WorkerSignin { dwccardnum = 66666, dateforsignin = DateTime.Today };
            WorkerSignin _cbsignin = new WorkerSignin();
            _wsiRepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
            //
            //Act
            _serv.CreateSignin(66666, DateTime.Today, "UnitTest");
            //
            //Assert
            Assert.AreEqual(_signin, _cbsignin);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void Create_WSI_with_worker_match_succeeds()
        {
            //
            //Arrange
            const int fakeid = 66666;
            var tenantService = new Mock<ITenantService>();
            tenantService.Setup(service => service.GetCurrentTenant()).Returns(UnitTestExtensions.TestingTenant);
            var _signin = new WorkerSignin { dwccardnum = fakeid, dateforsignin = DateTime.Today };
            
            var _serv = new WorkerSigninService(_wsiRepo.Object, _wServ.Object, _iServ.Object, _wrServ.Object, _uow.Object, _map.Object, _cServ.Object, tenantService.Object);
            
            var _cbsignin = new WorkerSignin();
            _wsiRepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
            //
            //Act
            _serv.CreateSignin(fakeid, DateTime.Today, "UnitTest");
            //
            //Assert
            Assert.AreEqual(_signin.dwccardnum, _cbsignin.dwccardnum);
            Assert.AreEqual(_signin.dateforsignin, _cbsignin.dateforsignin);
        }

        // TODO2017: Doesn't need to be a unit test; testing use of LINQ mostly
        //[TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        //public void Create_WSI_deduplicate_succeeds()
        //{
        //    //
        //    //Arrange
        //    int fakeid = 12345;
        //    IQueryable<WorkerSignin> wsiList = new WorkerSignin[] { 
        //        new WorkerSignin() {dwccardnum = 12345, dateforsignin = DateTime.Today} 
        //    }.AsQueryable();
        //    var _serv = new WorkerSigninService(_wsiRepo.Object, _wRepo.Object, _iRepo.Object, _wrRepo.Object, _uow.Object, _map.Object);
        //    var _signin = new WorkerSignin() { dwccardnum = fakeid, dateforsignin = DateTime.Today };
        //    WorkerSignin _cbsignin = null;
        //    _wsiRepo.Setup(s => s.Add(It.IsAny<WorkerSignin>())).Callback((WorkerSignin s) => { _cbsignin = s; });
        //    _wsiRepo.Setup(s => s.GetAllQ()).Returns(wsiList);
        //    //
        //    //Act
        //    try
        //    {
        //        _serv.CreateSignin(_signin, "UnitTest");
        //    }
        //    catch(InvalidOperationException ex)
        //    {
        //        Console.WriteLine(fakeid.ToString() + ex.Message);
        //    }
        //    //
        //    //Assert
        //    Assert.IsNull(_cbsignin);
        //    Assert.IsNull(_signin.createdby);
        //}
    }
}
