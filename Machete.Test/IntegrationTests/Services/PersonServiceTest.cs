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
    public class PersonServiceTest
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb = null;
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void Integration_Person_Service_CreatePerson()
        {
            //Arrange
            //Act
            var result = frb.ToPerson();
            //Assert
            Assert.IsNotNull(result.ID, "Person.ID is Null");
        }
        /// <summary>
        /// CreatePerson calls DbSet.Add() and  Context.SaveChanges() This leads to duplication
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void Integration_Person_Service_CreatePersons_TestDuplicateBehavior()
        {
            int reccount = 0;
            //
            //Arrange
            Person _p = frb.ClonePerson();
            //
            //Act
            try
            {
                // Using Service Create to test behavior when same object is created
                // 3 times. Expecting 3 different records. 
                frb.ToServPerson().Create(_p, "UnitTest");
                frb.ToServPerson().Create(_p, "UnitTest");
                frb.ToServPerson().Create(_p, "UnitTest");
                reccount = frb.ToServPerson().GetAll().Count(n => n.firstname1 == _p.firstname1);
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
