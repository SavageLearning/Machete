#region COPYRIGHT
// File:     ActivityServiceTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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
using System.Data.Entity;
using Machete.Service;
using Machete.Data.Infrastructure;
using System.Globalization;

namespace Machete.Test.IntegrationTests.Services
{
    [TestClass]
    public class ActivityServiceTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
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
                sortColName = "",
                displayStart = 0,
                displayLength = 20
            };
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Activities)]
        public void Integration_Activity_Service_CreateRandomClass()
        {
            //Used once to create dummy data to support report creation
            // requires change in app.config to point test database to production
            IEnumerable<int> cardlist = frb.ToRepoWorker().GetAllQ().Select(q => q.dwccardnum).Distinct();
            IEnumerable<int> classlist = frb.ToRepoLookup().GetAllQ().Where(l => l.category == "activityName").Select(q => q.ID);
            Activity a = new Activity();
            //random date, within last 30 days
            Random rand = new Random();
            DateTime today = DateTime.Today.AddDays(-rand.Next(40));
            a.dateStart = today.AddHours(7 + rand.Next(5));
            a.dateEnd = a.dateStart.AddHours(1.5);
            a.name = classlist.ElementAt(rand.Next(classlist.Count()));
            a.type = 101; //type==class
            a.teacher = "UnitTest script";
            a.notes = "From Integration_Activity_Service";
            frb.ToServActivity().Create(a, "TestScript");
            int rAttendance = rand.Next(cardlist.Count() / 10);
            for (var i = 0; i < rAttendance; i++)
            {
                ActivitySignin asi = (ActivitySignin)Records.activitysignin.Clone();
                asi.dateforsignin = today;
                asi.activityID = a.ID;
                asi.dwccardnum = cardlist.ElementAt(rand.Next(cardlist.Count()));
                frb.ToServActivitySignin().CreateSignin(asi, "TestScript");
            }
            //a.
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Activities)]
        public void Integration_Activity_service_CreateClass_within_hour()
        {
            IEnumerable<int> cardlist = frb.ToRepoWorker().GetAllQ().Select(q => q.dwccardnum).Distinct();
            IEnumerable<int> classlist = frb.ToRepoLookup().GetAllQ().Where(l => l.category == "activityName").Select(q => q.ID);
            Activity a = new Activity();
            //random date, within last 30 days
            Random rand = new Random();
            DateTime today = DateTime.Now;
            a.dateStart = today;
            a.dateEnd = a.dateStart.AddHours(1.5);
            a.name = classlist.ElementAt(rand.Next(classlist.Count()));
            a.type = 101; //type==class
            a.teacher = "UnitTest script";
            a.notes = "From Integration_Activity_Service";
            frb.ToServActivity().Create(a, "TestScript");
            
            Assert.IsTrue(1 == frb.ToRepoActivity().GetAllQ().Where(aa => aa.ID == a.ID).Count());
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Activities)]
        public void Integration_Activity_Service_GetIndexView_authenticated()
        {
            //
            //Arrange
            var maxDate = frb.ToRepoActivity().GetAllQ().Select(a => a.dateStart).Max().AddDays(1);
            frb.AddActivity(startTime: maxDate, endTime: maxDate.AddHours(1));
            frb.AddActivity(startTime: maxDate.AddHours(-4), endTime: maxDate.AddHours(-3));
            dOptions.authenticated = false;
            dOptions.date = maxDate;
            //
            //Act
            dataTableResult<Activity> result = frb.ToServActivity().GetIndexView(dOptions);
            //
            //Assert
            IEnumerable<Activity> query = result.query.ToList();
            Assert.IsNotNull(result, "IEnumerable is Null");
            Assert.AreEqual(1, query.Count());
        }
    }
}