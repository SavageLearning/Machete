#region COPYRIGHT
// File:     PersonServiceTest.cs
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

using System.Linq;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class PersonTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = FluentRecordBaseFactory.Get();
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
        public void CreatePerson()
        {
            //Arrange
            //Act
            var result = frb.AddPerson();
            //Assert
            Assert.IsNotNull(result.ID, "Person.ID is Null");
        }

        /// <summary>
        /// CreatePerson calls DbSet.Add() and  Context.SaveChanges() This leads to duplication
        /// </summary>
        /// IGNORED because this is entirely contradictory to the way SaveChanges is setup;
        /// you can't expect to save a person, get back an ID, and then save the same person
        /// with an explicit ID and have the framework create an entirely new record with a
        /// new ID. It errors out as it should. If we rewrite this test, it should expect error:
        ///
        /// Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while updating the entries. See the inner exception for details. ---> System.Data.SqlClient.SqlException: Cannot insert explicit value for identity column in table 'Persons' when IDENTITY_INSERT is set to OFF.
        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void CreatePersons_TestDuplicateBehavior()
        {
            int reccount = 0;
            //
            //Arrange
            Person person = frb.ClonePerson();
            //
            //Act
            // Using Service Create to test behavior when same object is created
            // 3 times. Expecting 3 different records. 
            frb.ToServ<IPersonService>().Create(person, "UnitTest");
            frb.ToServ<IPersonService>().Create(person, "UnitTest");
            frb.ToServ<IPersonService>().Create(person, "UnitTest");
            reccount = frb.ToServ<IPersonService>().GetAll().Count(n => n.firstname1 == person.firstname1);

            //Assert
            Assert.IsNotNull(person.ID);
            Assert.IsTrue(reccount == 3, "Expected record count of 3, received {0}", reccount);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Persons)]
        public void GetIndexView_Filters_Workes_When_Skill_Codes_when_keyword_passed()
        {
            // Arrange
            viewOptions vo = new viewOptions
            {
                sSearch = "skill: a, b ,c", // skill codes
                sortColName = "text_EN",
            };

            var skill1 = frb.ToServ<ILookupService>().Create(new Lookup
                {
                    text_EN = "special skill 1",
                    text_ES = "special skill 1",
                    speciality = true,
                    ltrCode = "a",
                    level = 1,
                    category = "skill",
                    subcategory = "general",
                    wage = 23,
                    minHour = 3,
                    typeOfWorkID = 20
                }, "fake");
            var skill2 = frb.ToServ<ILookupService>().Create(new Lookup
                {
                    text_EN = "special skill 2",
                    text_ES = "special skill 2",
                    ltrCode = "b",
                    speciality = true,
                    level = 1,
                    category = "skill",
                    subcategory = "general",
                    wage = 23,
                    minHour = 3,
                    typeOfWorkID = 20
                }, "fake");
            var skill3 = frb.ToServ<ILookupService>().Create(new Lookup
                {
                    text_EN = "special skill 3",
                    text_ES = "special skill 3",
                    ltrCode = "c",
                    speciality = true,
                    level = 1,
                    category = "skill",
                    subcategory = "general",
                    wage = 23,
                    minHour = 3,
                    typeOfWorkID = 20 
                }, "fake");

            var w = frb.AddWorker(skill1.ID, skill2.ID, skill3.ID);

            // Act
            dataTableResult<Service.DTO.PersonList> result = frb.ToServ<IPersonService>().GetIndexView(vo);

            // Assert
            Assert.IsTrue(result.filteredCount == 1);

            // Clean up
            frb.ToServ<ILookupService>().Delete(skill1.ID, "fake");
            frb.ToServ<ILookupService>().Delete(skill2.ID, "fake");
            frb.ToServ<ILookupService>().Delete(skill3.ID, "fake");
            frb.ToServ<IWorkerService>().Delete(w.ID, "test");
        }
    }
}
