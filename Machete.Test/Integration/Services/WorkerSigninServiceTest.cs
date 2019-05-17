#region COPYRIGHT
// File:     WorkerSigninServiceTest.cs
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
using System.Globalization;
using System.Linq;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class WorkerSigninTests
    {
        FluentRecordBase frb;
        viewOptions _dOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = FluentRecordBaseFactory.Get();
            _dOptions = new viewOptions
            {
                CI = new CultureInfo("en-US", false),
                sSearch = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = true,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void LotterySignin()
        {
            // Arrange
            var w = frb.AddWorker();
            frb.AddWorkerSignin(w);
            
            var wsi = frb.ToWorkerSignin();
            // Act
            var result = frb.ToServ<IWorkerSigninService>().GetSignin(w.dwccardnum, wsi.dateforsignin);
            // Assert
            Assert.AreEqual(w.dwccardnum, result.dwccardnum);
            Assert.AreEqual(wsi.dateforsignin, result.dateforsignin);
        }
        /// <summary>
        /// Filters WSI IndexView based on dwccardnum option. should return all records.
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void GetIndexView_check_search_dwccardnum()
        {
            // Arrange
            var w = frb.AddWorker(skill1:63);
            frb.AddWorkerSignin(w);
            var wsi = frb.ToWorkerSignin();
            //
            //Act
            _dOptions.dwccardnum = w.dwccardnum;
            dataTableResult<DTO.WorkerSigninList> result = frb.ToServ<IWorkerSigninService>().GetIndexView(_dOptions);
            //
            //Assert
            List<DTO.WorkerSigninList> tolist = result.query.ToList();
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(63, tolist[0].skill1);
        }
        /// <summary>
        /// Filter on requested grouping
        /// </summary>
        [Ignore] // TODO: Fix this after dotnet core
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void GetIndexView_workerRequested()
        {
            // Arrange
            frb.AddWorker(skill1: 63);
            frb.AddWorkerSignin();
            frb.AddWorkerRequest();
            var w = frb.AddWorker();
            //
            //Act
            _dOptions.dwccardnum = w.dwccardnum;
            _dOptions.wa_grouping = "requested";
            var wsi = frb.ToServ<IWorkerSigninService>();
            dataTableResult<DTO.WorkerSigninList> result = wsi.GetIndexView(_dOptions);
            //
            //Assert
            List<DTO.WorkerSigninList> tolist = result.query.ToList();
            Assert.AreEqual(63, tolist[0].skill1);
            Assert.AreEqual(1, result.query.Count());
        }    
    }
}