#region COPYRIGHT
// File:     WorkerSigninServiceTest.cs
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
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class WorkerSigninTests
    {
        FluentRecordBase frb;
        viewOptions _dOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
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
            frb.AddWorker().AddWorkerSignin();
            var w = frb.ToWorker();
            var wsi = frb.ToWorkerSignin();
            // Act
            var result = frb.ToServWorkerSignin().GetSignin(w.dwccardnum, wsi.dateforsignin);
            // Assert
            Assert.AreEqual(w.dwccardnum, result.dwccardnum);
            Assert.AreEqual(wsi.dateforsignin, result.dateforsignin);
        }
        /// <summary>
        /// 
        /// </summary>
        //[TestMethod]
        //public void Integration_WorkerSignin_GetView()
        //{
        //    DateTime date = DateTime.Today;
        //    IEnumerable<wsiView> filteredWSI = _wsiServ.getView(date);
        //    IEnumerable<wsiView> result = filteredWSI.ToList();
        //    Assert.IsNotNull(filteredWSI, "WorkerSignin getView return is Null");
        //    Assert.IsNotNull(result, "WorkerSignin getview.ToList() is Null");
        //}
        /// <summary>
        /// Submit an unknown dwccardnum, verify it is recorded and returned by GetIndexView
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void GetIndexView_record_unknown_worker()
        {
            //TODO Enable test
            ////            
            //WorkerSignin _signin = new WorkerSignin();
            //int cardnum = 30040;
            //Worker _w = _wServ.GetWorkerByNum(cardnum);
            //_signin.dwccardnum = cardnum;
            //_signin.dateforsignin = DateTime.Today;
            //_wsiServ.CreateWorkerSignin(_signin, "TestUser");            
            //_dOptions.search = _w.dwccardnum.ToString();
            ////            
            //IEnumerable<wsiView> result = _wsiServ.GetIndexView(_dOptions);
            //List<wsiView> tolist = result.query.ToList();
            ////
            //Assert.AreEqual(1, result.filteredCount);
        }
        /// <summary>
        /// Filters WSI IndexView based on dwccardnum option. should return all records.
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void GetIndexView_check_search_dwccardnum()
        {
            //
            // Arrange
            frb.AddWorker(skill1:61).AddWorkerSignin();
            var w = frb.ToWorker();
            var wsi = frb.ToWorkerSignin();
            frb.ToWorkerCache().Refresh(); // gets worker info from cache
            //
            //Act
            _dOptions.dwccardnum = w.dwccardnum;
            dataTableResult<wsiView> result = frb.ToServWorkerSignin().GetIndexView(_dOptions);
            //
            //Assert
            List<wsiView> tolist = result.query.ToList();
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(61, tolist[0].skill1);
        }
        /// <summary>
        /// Filter on requested grouping
        /// </summary>
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void GetIndexView_workerRequested()
        {
            //
            // Arrange
            frb.AddWorker(skill1: 61);
            frb.AddWorkerSignin();
            frb.AddWorkerRequest();
            var w = frb.ToWorker();
            //
            //Act
            _dOptions.dwccardnum = w.dwccardnum;
            _dOptions.wa_grouping = "requested";
            var wsi = frb.ToServWorkerSignin();
            dataTableResult<wsiView> result = wsi.GetIndexView(_dOptions);
            //
            //Assert
            List<wsiView> tolist = result.query.ToList();
            Assert.AreEqual(61, tolist[0].skill1);
            Assert.AreEqual(1, result.query.Count());
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void list_duplicates()
        {
            //
            // Arrange
            //
            //Act
            var result = frb.ToServWorkerSignin().listDuplicate(DateTime.Parse("10/4/2013"), "Chaim");
            //
            //Assert
            Assert.IsNotNull(result);
        }
    
    }
}