// #region COPYRIGHT
// // File:     EmployerServiceUnitTests.cs
// // Author:   Savage Learning, LLC.
// // Created:  2012/06/17 
// // License:  GPL v3
// // Project:  Machete.Test.Old
// // Contact:  savagelearning
// // 
// // Copyright 2011 Savage Learning, LLC., all rights reserved.
// // 
// // This source file is free software, under either the GPL v3 license or a
// // BSD style license, as supplied with this software.
// // 
// // This source file is distributed in the hope that it will be useful, but 
// // WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// // or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
// //  
// // For details please refer to: 
// // http://www.savagelearning.com/ 
// //    or
// // http://www.github.com/jcii/machete/
// // 
// #endregion

// using System;
// using System.Collections.Generic;
// using AutoMapper;
// using Machete.Data;
// using Machete.Data.Infrastructure;
// using Machete.Domain;
// using Machete.Service;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Moq;

// namespace Machete.Test.UnitTests.Services
// {
//     /// <summary>
//     /// Summary description for EmployerServiceUnitTests
//     /// </summary>
//     [TestClass]
//     public class EmployerTests
//     {
//         Mock<IDatabaseFactory> _dbf;
//         Mock<MacheteContext> _db;
//         Mock<DbSet<Employer>> _dbset;
//         Mock<IWorkOrderService> _woServ;
//         Mock<IMapper> _map;
//         EmployerService _serv;

//         public EmployerTests()
//         {
//             _dbf = new Mock<IDatabaseFactory>();
//             _db = new Mock<MacheteContext>();
//             _woServ = new Mock<IWorkOrderService>();
//             _map = new Mock<IMapper>();
//             _serv = new EmployerService(_dbf.Object, _woServ.Object, _map.Object);

//         }

//         private TestContext testContextInstance;

//         /// <summary>
//         ///Gets or sets the test context which provides
//         ///information about and functionality for the current test run.
//         ///</summary>
//         public TestContext TestContext
//         {
//             get
//             {
//                 return testContextInstance;
//             }
//             set
//             {
//                 testContextInstance = value;
//             }
//         }


//         [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
//         public void GetEmployers_returns_Enumerable()
//         {
//             //
//             //Arrange
//             //Act
//             var result = _serv.GetAll();
//             //Assert
//             Assert.IsInstanceOfType(result, typeof(IEnumerable<Employer>));
//         }
//         [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
//         public void GetEmployer_returns_employer()
//         {
//             //
//             //Arrange
//             var employer = (Employer)Records.employer.Clone();
//             employer.ID = 3;
            
//             _dbf.Setup(r => r.Get()).Returns(_db);
//             _db.Setup(r => r.Find(3)).Returns(employer);           
//             //Act
//             var result = _serv.Get(3);
//             //Assert
//             Assert.IsInstanceOfType(result, typeof(Employer));
//             Assert.IsTrue(result.ID == 3);
//         }

//         [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
//         public void CreateEmployer_returns_employer()
//         {
//             //
//             //Arrange
//             _dbf = new Mock<IDatabaseFactory>();
//             _woServ = new Mock<IWorkOrderService>();
//             _map = new Mock<IMapper>();
//             string user = "UnitTest";
//             var _e = (Employer)Records.employer.Clone();
//             _e.datecreated = DateTime.MinValue;
//             _e.dateupdated = DateTime.MinValue;
//             _dbf.Setup(r => r.(_e)).Returns(_e);
//             var _serv = new EmployerService(_dbf.Object, _woServ.Object, _map.Object);
//             //
//             //Act
//             var result = _serv.Create(_e, user);
//             //
//             //Assert
//             Assert.IsInstanceOfType(result, typeof(Employer));
//             Assert.IsTrue(result.createdby == user);
//             Assert.IsTrue(result.updatedby == user);
//             Assert.IsTrue(result.datecreated > DateTime.MinValue);
//             Assert.IsTrue(result.dateupdated > DateTime.MinValue);
//         }

//         [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
//         public void DeleteEmployer()
//         {
//             //
//             //Arrange
//             _dbf = new Mock<IEmployerRepository>();
//             _uow = new Mock<IUnitOfWork>();
//             _woServ = new Mock<IWorkOrderService>();
//             _map = new Mock<IMapper>();

//             var _e = (Employer)Records.employer.Clone();

//             string user = "UnitTest";
//             int id = 1;
//             Employer dp = new Employer();
//             _dbf.Setup(r => r.Delete(It.IsAny<Employer>())).Callback((Employer p) => { dp = p; });
//             _dbf.Setup(r => r.GetById(id)).Returns(_e);
//             var _serv = new EmployerService(_dbf.Object, _woServ.Object, _uow.Object, _map.Object);
//             //
//             //Act
//             _serv.Delete(id, user);
//             //
//             //Assert
//             Assert.AreEqual(dp, _e);
//         }

//         [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Employers)]
//         public void SaveEmployer_updates_timestamp()
//         {
//             //
//             //Arrange
//             _dbf = new Mock<IEmployerRepository>();
//             _uow = new Mock<IUnitOfWork>(); 
//             _woServ = new Mock<IWorkOrderService>();
//             _map = new Mock<IMapper>();

//             var _e = (Employer)Records.employer.Clone();

//             string user = "UnitTest";
//             _e.datecreated = DateTime.MinValue;
//             _e.dateupdated = DateTime.MinValue;
//             var _serv = new EmployerService(_dbf.Object, _woServ.Object, _uow.Object, _map.Object);
//             //
//             //Act
//             _serv.Save(_e, user);
//             //
//             //Assert
//             Assert.IsTrue(_e.updatedby == user);
//             Assert.IsTrue(_e.dateupdated > DateTime.MinValue);
//         }
//     }
// }
