#region COPYRIGHT
// File:     EmployerServiceUnitTests.cs
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

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for EmployerServiceUnitTests
    /// </summary>
    [TestClass]
    public class EmployerServiceUnitTests
    {
        Mock<IEmployerRepository> _repo;
        Mock<IUnitOfWork> _uow;
        Mock<IWorkOrderService> _woServ;
        EmployerService _serv;

        public EmployerServiceUnitTests()
        {
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);

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


        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
        public void EmployerService_GetEmployers_returns_Enumerable()
        {
            //
            //Arrange
            //Act
            var result = _serv.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Employer>));
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
        public void EmployerService_GetEmployer_returns_employer()
        {
            //
            //Arrange
            var employer = (Employer)Records.employer.Clone();
            employer.ID = 3;
            
            _repo.Setup(r => r.GetById(3)).Returns(employer);           
            //Act
            var result = _serv.Get(3);
            //Assert
            Assert.IsInstanceOfType(result, typeof(Employer));
            Assert.IsTrue(result.ID == 3);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
        public void EmployerService_CreateEmployer_returns_employer()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            string user = "UnitTest";
            var _e = (Employer)Records.employer.Clone();
            _e.datecreated = DateTime.MinValue;
            _e.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_e)).Returns(_e);
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //
            //Act
            var result = _serv.Create(_e, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Employer));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
        public void EmployerService_DeleteEmployer()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            var _e = (Employer)Records.employer.Clone();

            string user = "UnitTest";
            int id = 1;
            Employer dp = new Employer();
            _repo.Setup(r => r.Delete(It.IsAny<Employer>())).Callback((Employer p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(_e);
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, _e);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
        public void EmployerService_SaveEmployer_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>(); 
            _woServ = new Mock<IWorkOrderService>();
            var _e = (Employer)Records.employer.Clone();

            string user = "UnitTest";
            _e.datecreated = DateTime.MinValue;
            _e.dateupdated = DateTime.MinValue;
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //
            //Act
            _serv.Save(_e, user);
            //
            //Assert
            Assert.IsTrue(_e.Updatedby == user);
            Assert.IsTrue(_e.dateupdated > DateTime.MinValue);
        }
    }
}
