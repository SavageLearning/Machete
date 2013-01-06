#region COPYRIGHT
// File:     PersonServiceTest.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Machete.Test
{
    [TestClass]
    public class PersonServiceTest : ServiceTest
    {
        [TestInitialize]
        public void TestInitialize()
        {

            Database.SetInitializer<MacheteContext>(new TestInitializer());
            base.Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Integration_Person_Service_CreatePerson()
        {
            //Arrange
            Person _person = (Person)Records.person.Clone();
            _person.firstname2 = "PersonService_Intergation_CreatePerson";
            //Act
            _pServ.Create(_person, "UnitTest");
            //Assert
            Assert.IsNotNull(_person.ID, "Person.ID is Null");
            Assert.IsTrue(_person.ID == 4);
        }
        /// <summary>
        /// CreatePerson calls DbSet.Add() and  Context.SaveChanges() This leads to duplication
        /// </summary>
        [TestMethod]
        public void Integration_Person_Service_CreatePersons_TestDuplicateBehavior()
        {
            int reccount = 0;
            //
            //Arrange
            DB.Database.Delete();
            DB.Database.Initialize(true);
            Person _p = (Person)Records.person.Clone();
            _p.firstname2 = "PersonService_Int_CrePer_NoDuplicate";
            //
            //Act
            try
            {
                _pServ.Create(_p, "UnitTest");
                _pServ.Create(_p, "UnitTest");
                _pServ.Create(_p, "UnitTest");
                reccount = DB.Persons.Count(n => n.firstname1 == _p.firstname1);
            }
            catch (DbEntityValidationException ex)
            {
                Assert.Fail(string.Format("Validation exception for field {0} caught: {1}",
                    ex.EntityValidationErrors.First().ValidationErrors.First().PropertyName,
                    ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage));
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                ex.GetType(), ex.Message));
            }
            //
            //Assert
            Assert.IsNotNull(_p.ID);
            Assert.IsTrue(reccount == 3, "Expected record count of 3, received {0}", reccount);

        }
    }
}
