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
using Machete.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Machete.Data;
using Machete.Data.Initialize;
using Machete.Test.Integration.Fluent;
using Microsoft.Extensions.DependencyInjection;
using DbFunctions = Machete.Service.DbFunctions;

namespace Machete.Test.Integration.Data
{
    [TestClass]
    public class DatabaseTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void Initialize()
        {
            frb = new FluentRecordBase();
            //frb._dbContext = frb.container.GetRequiredService<MacheteContext>();
        }
        /// <summary>
        /// Used with SQL Profiler to see what SQL is produced
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data)]
        public void Integration_Queryable_test()
        {
            // Arrange - load test records
            var worker = frb.AddWorkerSignin().ToWorker();
            var signin = frb.ToWorkerSignin();
            // Act
            var q = frb.ToFactory().WorkerSignins.AsQueryable();
            q = q.Where(r => r.dwccardnum == signin.dwccardnum
                             && DbFunctions.DiffDays(r.dateforsignin, signin.dateforsignin) == 0);           
            WorkerSignin result = q.FirstOrDefault();
            // Assert
            Assert.IsNotNull(result.ID);
            Assert.AreEqual(result.WorkerID, worker.ID);
            Assert.AreEqual(result.dwccardnum, worker.dwccardnum);
        }
        /// <summary>
        /// Truncate ReportDefinitions and recreate
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data)]
        public void MacheteReportDefinitions_Initialize_counts_match()
        {
            // Arrange - load test records
            var context = frb.ToFactory();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE ReportDefinitions");
            var cache = MacheteReportDefinitions.cache;
            var count = cache.Count;
            
            // Act
            MacheteReportDefinitions.Initialize(context);
            var result = frb.ToFactory().ReportDefinitions.Count();
            
            // Assert
            Assert.AreEqual(count, result, "static cache and DB report definitions' count not equal");
        }
    }
}
