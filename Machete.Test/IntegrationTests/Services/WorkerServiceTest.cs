#region COPYRIGHT
// File:     WorkerServiceTest.cs
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
using System.Globalization;

namespace Machete.Test
{
    [TestClass]
    public class WorkerServiceTest
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
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
        public void Integration_Worker_Service_CreateWorker()
        {
            //
            //Arrange
            //
            //Act
            var _w = frb.ToWorker();
            //Assert
            Assert.IsNotNull(_w.ID, "Worker.ID is Null");
            Assert.IsTrue(_w.ID == _w.Person.ID, "Worker.ID doesn't match Person.ID");
        }
        /// <summary>
        /// Create, Edit, and Save a worker record from the Worker Service
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers), TestCategory(TC.Fluent)]
        public void Integration_Worker_Service_EditWorker()
        {
            //
            //Arrange
            Worker _w = (Worker)Records.worker.Clone();
            Person _p = (Person)Records.person.Clone();
            _p.firstname2 = "WorkerService_Intergation_CreateWorker";
            _w.height = "tall";
            _w.Person = _p;
            _w.dwccardnum = frb.GetNextMemberID();
            //
            //Act
            Worker result = frb.ToServWorker().Create(_w, "UnitTest");
            result.height = "short"; //EF should keep _w and result the same
            frb.ToServWorker().Save(result, "UnitTest");
            //
            //Assert
            Assert.IsNotNull(_w.ID, "Worker.ID is Null");
            Assert.IsNotNull(result.ID, "(worker) result.ID is Null");
            Assert.IsTrue(_w.ID == _p.ID, "Worker.ID doesn't match Person.ID");
            Assert.IsTrue(_w.height == "short", "SaveWorker failed to save property change");
            Assert.AreSame(_w, result, "CreateWorker did not return the expected object");
        }
    }
}
