#region COPYRIGHT
// File:     DatabaseTests.cs
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
using System.Data.Entity;
using Machete.Domain;
using System.Data.Objects;

namespace Machete.Test.Data
{
    [TestClass]
    public class DatabaseTests
    {
        FluentRecordBase frb;


        [TestInitialize]
        public void Initialize()
        {
            frb = new FluentRecordBase();
            frb.AddDBFactory(connStringName: "macheteDevTest");
        }
        /// <summary>
        /// Tests permissions to drop and re-create database
        /// </summary>
        [TestMethod]
        public void Integration_Initializer_create_machete()
        {
            //Arrange
            frb.ToFactory().Get().Database.Delete();
            //Act
            try
            {
                frb.ToFactory().Get().Database.Initialize(true); // should be performed by TestInitializer
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
            
        }
        /// <summary>
        /// Used with SQL Profiler to see what SQL is produced
        /// </summary>
        [TestMethod, TestCategory(TC.Fluent)]
        public void Integration_Queryable_test()
        {
            // Arrange - load test records
            var worker = frb.AddWorkerSignin().ToWorker();
            var signin = frb.ToWorkerSignin();
            // Act
            var q = frb.ToRepoWorkerSignin().GetAllQ();
            q = q.Where(r => r.dwccardnum == signin.dwccardnum
                          && EntityFunctions.DiffDays(r.dateforsignin, signin.dateforsignin) == 0 ? true : false);           
            WorkerSignin result = q.FirstOrDefault();
            // Assert
            Assert.IsNotNull(result.ID);
            Assert.AreEqual(result.WorkerID, worker.ID);
            Assert.AreEqual(result.dwccardnum, worker.dwccardnum);
        }
        /// <summary>
        /// Used with SQL profiler to see what SQL is produced
        /// </summary>
        [TestMethod]
        public void Integration_Enumerable_test()
        {
            // Arrange - load test records
            var worker = frb.AddWorkerSignin().ToWorker();
            var signin = frb.ToWorkerSignin();
            IEnumerable<WorkerSignin> e = frb.ToRepoWorkerSignin().GetAll().AsEnumerable();
            // Act
            e = e.Where(r => r.dwccardnum == signin.dwccardnum 
                          && DateTime.Compare(r.dateforsignin.Date, signin.dateforsignin) == 0 ? true : false);
            WorkerSignin result = e.FirstOrDefault();
            // Assert
            Assert.IsNotNull(result.ID);
            Assert.AreEqual(result.WorkerID, worker.ID);
            Assert.AreEqual(result.dwccardnum, worker.dwccardnum);
        }
    }
}
