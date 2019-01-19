#region COPYRIGHT
// File:     WorkOrderServiceTest.cs
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
    public class WorkOrderTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext c)
        {
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

        }
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
        [Ignore] // TODO: Fix after dotnet core
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void GetSummary()
        {
            //
            //Arrange
            var Date = frb.ToServ<IWorkOrderService>().GetSummary().OrderByDescending(o => o.date).First().date.Value.AddDays(1);
            frb.AddWorkOrder(status: WorkOrder.iCancelled, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iPending, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iCompleted, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iCompleted, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iActive, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iActive, dateTimeOfWork: Date).AddWorkAssignment();
            //
            //Act
            IEnumerable<WorkOrderSummary> result = frb.ToServ<IWorkOrderService>().GetSummary(Date.ToShortDateString()).ToList();
            //
            //Assert
            Assert.IsNotNull(result, "GetSummary result is Null");
            Assert.AreEqual(2, result.Where(r => r.status == WorkOrder.iActive).First().count, "GetSummary returned incorrect number of Active records");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iPending).First().count == 1, "GetSummary returned incorrect number of Pending records");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iCompleted).First().count == 2, "GetSummary returned incorrect number of Completed records");
            Assert.IsTrue(result.Where(r => r.status == WorkOrder.iCancelled).First().count == 1, "GetSummary returned incorrect number of Cancelled records");
        }
        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void CombinedSummary()
        {
            //
            // Arrange
            string search = "";
            bool orderdescending = true;
            int displayStart = 0;
            int displayLength = 50;
            var Date = frb.ToServ<IWorkOrderService>().GetSummary().OrderByDescending(o => o.date).First().date.Value.AddDays(1);
            frb.AddWorkOrder(status: WorkOrder.iCancelled, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iPending, dateTimeOfWork: Date).AddWorkAssignment().AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iCompleted, dateTimeOfWork: Date).AddWorkAssignment().AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iCompleted, dateTimeOfWork: Date).AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iActive, dateTimeOfWork: Date).AddWorkAssignment().AddWorkAssignment()
               .AddWorkOrder(status: WorkOrder.iActive, dateTimeOfWork: Date).AddWorkAssignment().AddWorkAssignment();
            //
            //Act
            dataTableResult<WOWASummary> result = frb.ToServ<IWorkOrderService>().CombinedSummary(search, orderdescending, displayStart, displayLength);
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
        public void get_GroupView()
        {
            // Arrange            
            frb.AddWorkOrder(status: WorkOrder.iActive, dateTimeOfWork: DateTime.Now).AddWorkAssignment();
            //
            //Act
            var result = frb.ToServ<IWorkOrderService>().GetActiveOrders(DateTime.Now, assignedOnly:false);
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void GetIndexView()
        {
            //
            //Arrange
            // get find latest workorder, get date from it, and add a day (make sure we're only records for this test)
            var Date = frb.ToServ<IWorkOrderService>().GetSummary().OrderByDescending(a => a.date).First().date.Value.AddDays(1);

            frb.AddWorkOrder(dateTimeOfWork: Date).AddWorkOrder(dateTimeOfWork: Date).AddWorkOrder(dateTimeOfWork: Date);
            frb.AddWorkOrder(dateTimeOfWork: Date).AddWorkOrder(dateTimeOfWork: Date).AddWorkOrder(dateTimeOfWork: Date);
            viewOptions o = new viewOptions();
            o.CI = new CultureInfo("en-US", false);
            o.sSearch = Date.ToShortDateString();
            o.EmployerID = null;
            o.status = null;
            o.orderDescending = true;
            o.displayStart = 0;
            o.displayLength = 20;
            o.sortColName = "WOID";
            //
            //Act
            dataTableResult<DTO.WorkOrdersList> result = frb.ToServ<IWorkOrderService>().GetIndexView(o);
            //
            //Assert
            IEnumerable<DTO.WorkOrdersList> query = result.query.ToList();
            Assert.IsNotNull(result, "IEnumerable is Null");
            Assert.IsNotNull(query, "IEnumerable.query is null");
            Assert.AreEqual(6, query.Count(), "Expected 6, but GetIndexView returned {0} records", query.Count());

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void AutoMapper_WorkOrder()
        {
            //
            //Arrange
            var wo = frb.ToWorkOrder();
            var map = frb.ToWebMapper();
            //
            //Act
            var dto_wolist = map.Map<Machete.Domain.WorkOrder, Machete.Service.DTO.WorkOrdersList>(wo);
            //
            //Assert
            Assert.IsNotNull(dto_wolist, "DTO.WorkOrderList is Null");
            Assert.AreEqual(dto_wolist.workers.Count(), 0, "Found assigned workers when not expecting them");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void AutoMapper_WorkOrder_WorkAssignment_Unassigned()
        {
            //
            //Arrange
            var wo = frb.AddWorkAssignment(assignWorker: false).ToWorkOrder();
            var map = frb.ToWebMapper();
            //
            //Act
            var dto_wolist = map.Map<Machete.Domain.WorkOrder, Machete.Service.DTO.WorkOrdersList>(wo);
            //
            //Assert
            Assert.IsNotNull(dto_wolist, "DTO.WorkOrderList is Null");
            Assert.AreEqual(dto_wolist.workers.Count(), 0, "Found assigned workers when not expecting them");
        }

        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WorkOrders)]
        public void AutoMapper_WorkOrder_WorkAssignment_Assigned()
        {
            //
            //Arrange
            var wo = frb.AddWorkAssignment(assignWorker: true).ToWorkOrder();
            var map = frb.ToWebMapper();
            //
            //Act
            var dto_wolist = map.Map<Machete.Domain.WorkOrder, Machete.Service.DTO.WorkOrdersList>(wo);
            //
            //Assert
            Assert.IsNotNull(dto_wolist, "DTO.WorkOrderList is Null");
            Assert.AreEqual(dto_wolist.workers.Count(), 1, "Unexpected number of assigned workers");
        }
    }
}
