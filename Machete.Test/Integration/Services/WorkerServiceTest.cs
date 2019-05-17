#region COPYRIGHT
// File:     WorkerServiceTest.cs
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
using System.Globalization;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class WorkerTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = FluentRecordBaseFactory.Get();
            dOptions = new viewOptions
            {
                CI = new CultureInfo("en-US", false),
                sSearch = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = false,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
        }
        /// <summary>
        /// Create a worker record from the Worker Service
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers), TestCategory(TC.Fluent)]
        public void CreateWorker()
        {
            //
            //Arrange
            //
            //Act
            var worker = frb.AddWorker();
            //Assert
            Assert.IsNotNull(worker.ID, "Worker.ID is Null");
            Assert.IsTrue(worker.ID == worker.Person.ID, "Worker.ID doesn't match Person.ID");
        }
        /// <summary>
        /// Create, Edit, and Save a worker record from the Worker Service
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers), TestCategory(TC.Fluent)]
        public void EditWorker()
        {
            //
            //Arrange
            var worker = frb.AddWorker();
            var person = worker.Person;
            person.firstname2 = "WorkerService_Integration_CreateWorker";
            worker.height = "tall";
            //_w.Person = _p;
            worker.dwccardnum = frb.GetNextMemberID();
            worker.height = "short"; //EF should keep _w and result the same
            //
            //Act
            frb.ToServ<IWorkerService>().Save(worker, "UnitTest");
            //
            //Assert
            Assert.IsNotNull(worker.ID, "Worker.ID is Null");
            Assert.IsTrue(worker.ID == person.ID, "Worker.ID doesn't match Person.ID");
            Assert.IsTrue(worker.height == "short", "SaveWorker failed to save property change");
        }
    }
}
