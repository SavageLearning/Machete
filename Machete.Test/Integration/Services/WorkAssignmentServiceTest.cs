﻿#region COPYRIGHT
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
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class WorkAssignmentTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            frb.ToFactory();
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
        [TestCleanup]
        public void TestCleanup()
        {
            //frb.DB.Database.Delete();
            frb = null;
        }


        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_basic()
        {   
            //
            // Arrange
            var desc = "DESCRIPTION " + frb.RandomString(20);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            dOptions.sSearch = desc;
            //
            //Act
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(result, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(8, result.query.Count()); //pending excluded
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_workerjoin_blank_worker_ok()
        {
            frb.AddWorkOrder(status: WorkOrder.iActive).AddWorkAssignment(assignWorker: true);
            dOptions.sortColName = "assignedWorker";
            dOptions.woid = frb.ToWorkOrder().ID;
            dOptions.wa_grouping = "assigned";
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(1, result.query.Count()); //pending excluded
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_checkwoidfilter()
        {
            //Arrange
            frb.AddWorkOrder()
               .AddWorkAssignment()
               .AddWorkAssignment()
               .AddWorkAssignment();
            //Act
            dOptions.woid = frb.ToWorkOrder().ID;
            dOptions.orderDescending = true;
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(3, result.query.Count());
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_search_paperordernum()
        { 
            //
            // arrange
            frb.AddWorkOrder().AddWorkAssignment().AddWorkAssignment();
            var ordernum = frb.ToWorkOrder().ID;
            //
            //Act
            dOptions.sSearch = ordernum.ToString();
            dOptions.orderDescending = true;
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(ordernum, tolist[0].paperOrderNum);
            Assert.AreEqual(2, result.query.Count());
        }
        [TestMethod, TestCategory(TC.Fluent)]
        public void GetIndexView_check_search_description()
        {
            //
            // Arrange
            var description = frb.RandomString(30);
            var wo = frb.AddWorkAssignment(desc: description).ToWorkOrder();
            //
            //Act
            dOptions.sSearch = description;
            dOptions.woid = wo.ID;
            dOptions.orderDescending = true;
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(description, tolist[0].description);
            Assert.AreEqual(1, result.query.Count());
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_search_Updatedby()
        {
            var updatedby = frb.RandomString(10);
            frb.AddWorkAssignment();
            frb.AddWorkAssignment(updatedby: updatedby);
            dOptions.sSearch = updatedby;
            dOptions.orderDescending = true;
            dOptions.woid = frb.ToWorkOrder().ID;
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(updatedby, tolist[0].updatedby);
            Assert.AreEqual(1, result.query.Count());
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_search_skill()
        {
            // arrange
            frb.AddWorkAssignment(skill: 71);
            dOptions.sSearch = "masonry";
            dOptions.orderDescending = true;
            dOptions.woid = frb.ToWorkOrder().ID;
            // Act
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(71, tolist[0].skillID); //ID=71 is masonry
            Assert.AreEqual(1, result.query.Count());
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_searchWODateTimeofWork()
        {
            //Arrange
            var time = DateTime.Today.AddHours(9);
            frb.AddWorkOrder(dateTimeOfWork: time)
                .AddWorkAssignment( );
            dOptions.sSearch = time.ToString();
            dOptions.orderDescending = true;
            dOptions.woid = frb.ToWorkOrder().ID;
            //Act
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(1, result.query.Count());
        }
        //
        // Simulates doubleclicking on a worker in the workerSignin list
        // 
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_searchdwccardnum()
        {
            //arrange
            var skill = frb.ToLookupCache().getByKeys(LCategory.skill,LKey.Default);
            var w = frb.AddWorker(skill1: skill).ToWorker();
            dOptions.dwccardnum = w.dwccardnum;
            dOptions.orderDescending = true;
            dOptions.woid = frb.ToWorkOrder().ID;
            frb.AddWorkAssignment(skill: skill);
            //Act
            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            Assert.AreEqual(1, result.query.Count());
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetIndexView_check_requested_filter()
        {
            //Arrange
            frb.AddWorkOrder(status: WorkOrder.iActive)
                .AddWorkerRequest()
                .AddWorkAssignment();
            dOptions.orderDescending = true;
            dOptions.wa_grouping = "requested";
            dOptions.woid = frb.ToWorkOrder().ID;
            //Act

            var result = frb.ToServWorkAssignment().GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(dataTableResult<Machete.Service.DTO.WorkAssignmentList>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(1, result.query.Count());
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void Assign_updates_WSI_and_WA()
        {
            var wsi1 = frb.ToWorkerSignin();
            var wa1 = frb.ToWorkAssignment();
            var result = frb.ToServWorkAssignment().Assign(wa1, wsi1, "test script");
            var wsi2 = frb.ToWorkerSignin();
            var wa2 = frb.ToWorkAssignment();
            Assert.IsNotNull(result);
            Assert.IsNotNull(wa2.workerAssignedID);
            Assert.IsNotNull(wa2.workerSigninID);
            Assert.IsNotNull(wsi2.WorkAssignmentID);
            Assert.IsNotNull(wsi2.WorkerID);
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void GetSummary()
        {
            var result = frb.ToServWorkAssignment().GetSummary("");
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.WAs), TestCategory(TC.Fluent)]
        public void Delete_removes_record()

        {
            frb.AddWorkAssignment().AddWorkAssignment().AddWorkAssignment().AddWorkAssignment().AddWorkAssignment();
            frb.AddWorkAssignment().AddWorkAssignment().AddWorkAssignment().AddWorkAssignment().AddWorkAssignment();
            dOptions.woid = frb.ToWorkOrder().ID;
            var before = frb.ToServWorkAssignment().GetIndexView(dOptions);
            Assert.AreEqual(10, before.query.Count());

            frb.ToServWorkAssignment().Delete(frb.ToWorkAssignment().ID, "Intg Test");
            var after = frb.ToServWorkAssignment().GetIndexView(dOptions);
            Assert.AreEqual(9, after.query.Count());
            Assert.AreNotSame(before.query.Count(), after.query.Count());
        }
    }
}