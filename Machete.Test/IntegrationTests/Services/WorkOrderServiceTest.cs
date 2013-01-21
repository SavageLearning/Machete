#region COPYRIGHT
// File:     WorkOrderServiceTest.cs
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
    public class WorkOrderServiceTest : ServiceTest
    {

        [TestInitialize]
        public void TestInitialize()
        {
            base.Initialize();
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void Integration_WO_Service_GetSummary()
        {
            //
            //Arrange
            //
            //Act
            IEnumerable<WorkOrderSummary> result = _woServ.GetSummary("").ToList();
            //
            //Assert
            Assert.IsNotNull(result, "GetSummary result is Null");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iActive).First().count == 2, "GetSummary returned incorrect number of Active records");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iPending).First().count == 1, "GetSummary returned incorrect number of Pending records");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iCompleted).First().count == 2, "GetSummary returned incorrect number of Completed records");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iCancelled).First().count == 1, "GetSummary returned incorrect number of Cancelled records");
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void Integration_WO_Service_CombinedSummary()
        {
            //
            // Arrange
            string search = "";
            bool orderdescending = true;
            int displayStart = 0;
            int displayLength = 50;
            //
            //Act
            dataTableResult<WOWASummary> result = _woServ.CombinedSummary(search, orderdescending, displayStart, displayLength);
            WOWASummary wowa = result.query.First();
            //
            //Assert
            Assert.IsNotNull(result, "CombinedSummary.IEnumerable is Null");
            Assert.IsNotNull(wowa, "CombinedSummary.IEnumerable.query is null");
            Assert.AreEqual(4, wowa.active_wa, "CombinedSummary returned unexpected active_wa value");
            Assert.AreEqual(2, wowa.active_wo, "CombinedSummary returned unexpected active_wo value");
            Assert.AreEqual(1, wowa.cancelled_wa, "CombinedSummary returned unexpected cancelled_wa value");
            Assert.AreEqual(1, wowa.cancelled_wo, "CombinedSummary returned unexpected cancelled_wo value");
            Assert.AreEqual(3, wowa.completed_wa, "CombinedSummary returned unexpected completed_wa value");
            Assert.AreEqual(2, wowa.completed_wo, "CombinedSummary returned unexpected completed_wo value");
            Assert.AreEqual(0, wowa.expired_wa, "CombinedSummary returned unexpected expired_wa value");
            Assert.AreEqual(0, wowa.expired_wo, "CombinedSummary returned unexpected expired_wo value");
            Assert.AreEqual(2, wowa.pending_wa, "CombinedSummary returned unexpected pending_wa value");
            Assert.AreEqual(1, wowa.pending_wo, "CombinedSummary returned unexpected pending_wo value");

        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void Integration_WO_Service_get_GroupView()
        {
            //
            //Act
            var result = _woServ.GetActiveOrders(DateTime.Now, false);
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void Integration_WO_Service_GetIndexView()
        {
            //
            //Arrange
            viewOptions o = new viewOptions();
            o.CI = new CultureInfo("en-US", false);
            o.sSearch = DateTime.Today.ToShortDateString();
            o.EmployerID = null;
            o.status = null;
            o.orderDescending = true;
            o.displayStart = 0;
            o.displayLength = 20;
            o.sortColName = "WOID";
            //
            //Act
            dataTableResult<WorkOrder> result = _woServ.GetIndexView(o);
            //
            //Assert
            IEnumerable<WorkOrder> query = result.query.ToList();
            Assert.IsNotNull(result, "IEnumerable is Null");
            Assert.IsNotNull(query, "IEnumerable.query is null");
            Assert.IsTrue(query.Count() == 6, "Expected 6, but GetIndexView returned {0} records", query.Count());

        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void Integration_WO_Service_GetWorkOrders_returns_all()
        {
            // Arrange
            IEnumerable<WorkOrder> result = _woServ.GetAll().ToList();
            //
            int count = DB.WorkOrders.Count();
            //
            Assert.IsTrue(result.Count() == 6, "Expected record count of 6, received {0}", result.Count());
            Assert.AreEqual(result.Count(), count, "GetWorkOrders() doesn't return all orders");            
        }
    }
}
