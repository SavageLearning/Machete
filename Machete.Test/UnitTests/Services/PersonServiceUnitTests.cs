#region COPYRIGHT
// File:     PersonServiceUnitTests.cs
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
using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for PersonServiceUnitTests
    /// </summary>
    [TestClass]
    public class PersonTests
    {
        Mock<IPersonRepository> _repo;
        Mock<IUnitOfWork> _uow;
        Mock<ILookupRepository> _lcache;
        Mock<IMapper> _map;
        PersonService _serv;
        public PersonTests()
        {
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            _lcache = new Mock<ILookupRepository>();
            _map = new Mock<IMapper>();
            _serv = new PersonService(_repo.Object, _uow.Object, _lcache.Object, _map.Object);
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void GetPersons_returns_Enumerable()
        {
            //
            //Arrange
            //Act
            var result = _serv.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Person>));
        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void GetPerson_returns_person()
        {
            //
            //Arrange
            Person _person = (Person)Records.person.Clone();
            _person.ID = 3; //This matches Records._person3 ID value
            _repo.Setup(r => r.GetById(3)).Returns(_person);
            //Act
            var result = _serv.Get(3);
            //Assert
            Assert.IsInstanceOfType(result, typeof(Person));
            Assert.IsTrue(result.ID == 3);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void CreatePerson_returns_person()
        {
            //
            //Arrange
            Person _person = (Person)Records.person.Clone(); 
            string user = "UnitTest";

            _repo.Setup(r => r.Add(_person)).Returns(_person);
            //
            //Act
            var result = _serv.Create(_person, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Person));
            Assert.IsTrue(result.createdby == user);
            Assert.IsTrue(result.updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated >  DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void DeletePerson()
        {
            //
            //Arrange
            Person _p = (Person)Records.person.Clone();
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            _lcache = new Mock<ILookupRepository>();
            string user = "UnitTest";
            int id = 1;
            Person dp = new Person();
            _repo.Setup(r => r.Delete(It.IsAny<Person>())).Callback((Person p) => { dp = p;  });
            _repo.Setup(r => r.GetById(id)).Returns(_p);
            var _serv = new PersonService(_repo.Object, _uow.Object, _lcache.Object, _map.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(dp, _p);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void SavePerson_updates_timestamp()
        {
            //
            //Arrange
            Person _p = (Person)Records.person.Clone();
            _repo = new Mock<IPersonRepository>();
            _uow = new Mock<IUnitOfWork>();
            _lcache = new Mock<ILookupRepository>();
            _map = new Mock<IMapper>();
            string user = "UnitTest";
            _p.datecreated = DateTime.MinValue;
            _p.dateupdated = DateTime.MinValue;
            var _serv = new PersonService(_repo.Object, _uow.Object, _lcache.Object, _map.Object);
            //
            //Act
            _serv.Save(_p, user);
            //
            //Assert
            Assert.IsTrue(_p.updatedby == user);
            Assert.IsTrue(_p.dateupdated > DateTime.MinValue);
        }
    }
}
