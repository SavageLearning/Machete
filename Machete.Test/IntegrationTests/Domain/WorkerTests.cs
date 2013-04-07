#region COPYRIGHT
// File:     WorkerTests.cs
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
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Machete.Service;
using System.Data.Entity.Validation;
using System.Globalization;
namespace Machete.Test
{
    [TestClass]
    public class WorkerTests
    {

        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void Initialize()
        {
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteConnection");
            dOptions = new viewOptions
            {
                CI = new CultureInfo("en-US", false),
                sSearch = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = false,
                sortColName = null,
                displayStart = 0,
                displayLength = 20
            };
        }
        /// <summary>
        /// Inspecting how/when EntityFramework makes the link between parent/child records
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers), TestCategory(TC.Fluent)]
        public void Integration_Worker_add_worker_check_person_link() 
        {
            //Arrange
            frb.AddWorker();
            Person _p = frb.ToPerson();
            Worker _w = frb.ToWorker();
            //Assert
            Assert.IsNotNull(_p.Worker);
            Assert.IsNotNull(_w.Person);
        }
    }
}
