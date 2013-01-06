#region COPYRIGHT
// File:     WorkAssignmentServiceTest.cs
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
    public class WorkAssignmentServiceTest : ServiceTest
    {
        viewOptions dOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            base.Initialize();
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
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_basic()
        {       
            //
            //Act
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(result, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual(8, result.query.Count()); //pending excluded
            Assert.AreEqual(10, _waServ.TotalCount());            
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_workerjoin_blank_worker_ok()
        {
            dOptions.sortColName = "assignedWorker";
            var result = _waServ.GetIndexView(dOptions);
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual(8, result.query.Count()); //pending excluded
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_checkwoidfilter()
        {
            //Act
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual(3, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_paperordernum()
        {            
            //
            //Act
            dOptions.sSearch = "12420";
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual(12420, tolist[0].workOrder.paperOrderNum);
            Assert.AreEqual(3, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_description()
        {
            //
            //Act
            dOptions.sSearch = "foostring1";
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual("foostring1", tolist[0].description);
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_Updatedby()
        {
            dOptions.sSearch = "foostring1";
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual("foostring2", tolist[0].Updatedby);
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_skill()
        {
            dOptions.sSearch = "Digging";
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual(70, tolist[0].skillID); //ID=70 is Digging
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_searchdateTimeofWork()
        {
            //
            //Act
            dOptions.sSearch = DateTime.Today.AddHours(9).ToString();
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            Assert.AreEqual(70, tolist[0].skillID);
            Assert.AreEqual(3, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        //
        // Simulates doubleclicking on a worker in the workerSignin list
        // 
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_searchdwccardnum()
        {
            //
            //Act
            dOptions.dwccardnum = 30040;
            dOptions.orderDescending = true;
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(7, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_requested_filter()
        {
            //
            //Act
            dOptions.orderDescending = true;
            dOptions.wa_grouping = "requested";
            var result = _waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<WorkAssignment>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(1, result.query.Count());
            Assert.AreEqual(10, _waServ.TotalCount());
        }
        [TestMethod]
        public void Integration_WA_Service_Assign_updates_WSI_and_WA()
        {
            WorkerSignin wsi1 = _wsiServ.Get(1);
            WorkAssignment wa1 = _waServ.Get(1);
            var result = _waServ.Assign(wa1, wsi1, "test script");
            WorkerSignin wsi2 = _wsiServ.Get(1);
            WorkAssignment wa2 = _waServ.Get(1);
            Assert.IsNotNull(result);
            Assert.IsNotNull(wa2.workerAssignedID);
            Assert.IsNotNull(wa2.workerSigninID);
            Assert.IsNotNull(wsi2.WorkAssignmentID);
            Assert.IsNotNull(wsi2.WorkerID);
        }
        [TestMethod]
        public void Integration_WA_Service_GetSummary()
        {
            var result = _waServ.GetSummary("");
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod]
        public void Integration_WA_Service_Delete_removes_record()

        {
            var before = _waServ.GetAll();
            Assert.IsTrue(before.Count() == 10, "Unanticipated list count from Assignment.GetMany()");
            _waServ.Delete(1, "Intg Test");
            var after = _waServ.GetAll();
            Assert.IsTrue(after.Count() == 9, "Unanticipated list count from Assignment.GetMany()");
            Assert.AreNotSame(before.Count(), after.Count());
        }
    }
}