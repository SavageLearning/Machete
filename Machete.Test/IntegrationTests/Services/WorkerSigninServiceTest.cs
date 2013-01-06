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
using System.Globalization;
using Machete.Web.Helpers;

namespace Machete.Test
{
    [TestClass]
    public class WorkerSigninServiceTest : ServiceTest
    {
        viewOptions _dOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            base.Initialize();
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
        [TestMethod]
        public void Integration_WorkerSigin_LotterySignin()
        {
            var result = _wsiServ.GetSignin(30040, DateTime.Today);
            Assert.IsNotNull(result);
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
        [TestMethod]
        public void Integration_WorkerSigninService_Integration_GetIndexView_record_unknown_worker()
        {
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
        [TestMethod]
        public void Integration_WorkerSigninService_Intergation_GetIndexView_check_search_dwccardnum()
        {
            //
            //Act
            _dOptions.dwccardnum = 30040;
            dataTableResult<wsiView> result = _wsiServ.GetIndexView(_dOptions);
            //
            //Assert
            List<wsiView> tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<wsiView>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(3, result.query.Count());
            Assert.AreEqual(5, _wsiServ.TotalCount());
        }
        /// <summary>
        /// Filter on requested grouping
        /// </summary>
        [TestMethod]
        public void Integration_WorkerSigninService_Intergation_GetIndexView_workerRequested()
        {
            //
            //Act
            _dOptions.dwccardnum = 30040;
            _dOptions.wa_grouping = "requested";
            dataTableResult<wsiView> result = _wsiServ.GetIndexView(_dOptions);
            //
            //Assert
            List<wsiView> tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<wsiView>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(5, _wsiServ.TotalCount());
        }
        //[TestMethod]
        //public void Integration_TestMethod5()
        //{

        //    IEnumerable<WorkerSignin> testing = _wsiServ.GetSigninsForAssignment(DateTime.Today,
        //                                                "Jose",
        //                                                "asc",
        //                                                null,
        //                                                null);
        //    Assert.IsNotNull(testing, "null");
        //}
    }
}