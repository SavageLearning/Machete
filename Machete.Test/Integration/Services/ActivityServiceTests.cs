#region COPYRIGHT
// File:     ActivityServiceTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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

//using HibernatingRhinos.Profiler.Appender.EntityFramework;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class ActivityTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext c)
        {
            //EntityFrameworkProfiler.Initialize();

        }

        [TestInitialize]
        public void TestInitialize()
        {
            frb = FluentRecordBaseFactory.Get();
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
        public void Create_random_class()
        {
            //Used once to create dummy data to support report creation
            // requires change in app.config to point test database to production
            IEnumerable<int> cardlist = frb.ToFactory().Workers.Select(q => q.dwccardnum).Distinct().ToList();
            IEnumerable<int> classlist = frb.ToFactory().Lookups.Where(l => l.category == "activityName").Select(q => q.ID).ToList();
            Activity a = new Activity();
            //random date, within last 30 days
            Random rand = new Random();
            DateTime today = DateTime.Today.AddDays(-rand.Next(40));
            a.dateStart = today.AddHours(7 + rand.Next(5));
            a.dateEnd = a.dateStart.AddHours(1.5);
            a.nameID = classlist.ElementAt(rand.Next(classlist.Count()));
            a.typeID = 101; //type==class
            a.teacher = "UnitTest script";
            a.notes = "From Integration_Activity_Service";
            frb.ToServ<IActivityService>().Create(a, "TestScript");
            int rAttendance = rand.Next(cardlist.Count() / 10);
            for (var i = 0; i < rAttendance; i++)
            {
                ActivitySignin asi = (ActivitySignin)Records.activitysignin.Clone();
                asi.dateforsignin = today;
                asi.activityID = a.ID;
                asi.dwccardnum = cardlist.ElementAt(rand.Next(cardlist.Count()));
                frb.ToServ<IActivitySigninService>().CreateSignin(asi, "TestScript");
            }
            //a.
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Activities)]
        public void CreateClass_within_hour()
        {
            IEnumerable<int> classlist = frb.ToFactory().Lookups.Where(l => l.category == "activityName").Select(q => q.ID).ToList();
            Activity a = new Activity();
            //random date, within last 30 days
            Random rand = new Random();
            DateTime today = DateTime.Now;
            a.dateStart = today;
            a.dateEnd = a.dateStart.AddHours(1.5);
            a.nameID = classlist.ElementAt(rand.Next(classlist.Count()));
            a.typeID = 101; //type==class
            a.teacher = "UnitTest script";
            a.notes = "From Integration_Activity_Service";
            frb.ToServ<IActivityService>().Create(a, "TestScript");
            
            Assert.IsTrue(1 == frb.ToServ<IActivityService>().GetMany(aa => aa.ID == a.ID).Count());
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Activities)]
        public void GetIndexView_authenticated()
        {
            //
            //Arrange
            var maxDate = frb.ToFactory().Activities.Select(a => a.dateStart).Max().AddDays(1);
            var teacher = "teacher_" + frb.RandomString(4);
            frb.AddActivity(startTime: maxDate, endTime: maxDate.AddHours(1), teacher: teacher);
            frb.AddActivity(startTime: maxDate.AddHours(-4), endTime: maxDate.AddHours(-3));
            dOptions.date = maxDate;
            dOptions.sSearch = teacher;
            //
            //Act
            dataTableResult<DTO.ActivityList> result = frb.ToServ<IActivityService>().GetIndexView(dOptions);
            //
            //Assert
            IEnumerable<DTO.ActivityList> query = result.query.ToList();
            Assert.IsNotNull(result, "IEnumerable is Null");
            Assert.AreEqual(1, query.Count());
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Activities)]
        public void GetIndexView_persons_attended_signins()
        {
            //
            //Arrange
            var maxDate = frb.ToFactory().Activities.Select(a => a.dateStart).Max().AddDays(1);
            var teacher = "teacher_" + frb.RandomString(4);

            frb.AddActivity(startTime: maxDate, endTime: maxDate.AddHours(1), teacher: teacher);
            frb.AddActivity(startTime: maxDate.AddHours(-4), endTime: maxDate.AddHours(-3));
            //dOptions.date = maxDate;
            dOptions.attendedActivities = true;
            dOptions.sSearch = teacher;

            //
            //Act
            dataTableResult<DTO.ActivityList> result = frb.ToServ<IActivityService>().GetIndexView(dOptions);
            //
            //Assert
            IEnumerable<DTO.ActivityList> query = result.query.ToList();
            Assert.IsNotNull(result, "IEnumerable is Null");
            Assert.AreEqual(1, query.Count());
        }
    }
}
