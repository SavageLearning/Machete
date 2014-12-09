﻿#region COPYRIGHT
// File:     WorkerCacheTests.cs
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
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;

namespace Machete.Test.IntegrationTests.System
{
    [TestClass]
    public class WorkerCacheTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

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

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void Integration_WorkerCache_ExpireMembers_expires_1_active()
        {

            //Arrange
            frb.AddWorker(status: Worker.iActive, skill1: 62, memberexpirationdate: DateTime.Now.AddDays(-1));
            var _w = frb.ToWorker();
            //Act
            frb.ToWorkerCache().ExpireMembers();
            IEnumerable<Worker> result = frb.ToRepoWorker().GetAll().AsEnumerable()
                .Where(p => p.memberStatus == Worker.iExpired && p.dwccardnum == _w.dwccardnum);
            //Assert
            Assert.AreEqual(1, result.Count(), "Failed to expire members");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void Integration_WorkerCache_ExpireMembers_doesnt_expire_1_inactive()
        {

            //Arrange
            frb.AddWorker(status: Worker.iInactive, skill1: 62, memberexpirationdate: DateTime.Now.AddDays(-1));
            var _w = frb.ToWorker();
            //Act
            frb.ToWorkerCache().ExpireMembers();
            IEnumerable<Worker> result = frb.ToRepoWorker().GetAll().AsEnumerable()
                .Where(p => p.memberStatus == Worker.iExpired && p.dwccardnum == _w.dwccardnum);
            //Assert
            Assert.AreEqual(0, result.Count(), "Failed to expire members");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void Integration_WorkerCache_ReactivateMembers_activates_1_sanctioned()
        {

            //Arrange
            frb.AddWorker(status: Worker.iSanctioned, memberReactivateDate: DateTime.Now.AddDays(-1),
                memberexpirationdate: DateTime.Now.AddDays(1));
            var _w = frb.ToWorker();
            //Act
            frb.ToWorkerCache().ReactivateMembers();
            IEnumerable<Worker> result = frb.ToRepoWorker().GetAll().AsEnumerable()
                .Where(p => p.memberStatus == Worker.iActive && p.dwccardnum == _w.dwccardnum);
            //Assert
            Assert.AreEqual(1, result.Count(), "Failed to reactivate members");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void Integration_WorkerCache_ExpireMembers_doesnt_activate_1_current_sanction()
        {

            //Arrange
            frb.AddWorker(status: Worker.iSanctioned, memberReactivateDate: DateTime.Now.AddMonths(1),
                memberexpirationdate: DateTime.Now.AddDays(1));
            var _w = frb.ToWorker();
            //Act
            frb.ToWorkerCache().ReactivateMembers();
            IEnumerable<Worker> result = frb.ToRepoWorker().GetAll().AsEnumerable()
                .Where(p => p.memberStatus == Worker.iSanctioned && p.dwccardnum == _w.dwccardnum);
            //Assert
            Assert.AreEqual(1, result.Count(), "Failed to reactivate members");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public void Integration_WorkerCache_ExpireMembers_doesnt_activate_1_null_reactivation_date()
        {

            //Arrange
            frb.AddWorker(status: Worker.iSanctioned, 
                    memberexpirationdate: DateTime.Now.AddDays(1));
            var _w = frb.ToWorker();
            //Act
            frb.ToWorkerCache().ReactivateMembers();
            IEnumerable<Worker> result = frb.ToRepoWorker().GetAll().AsEnumerable()
                .Where(p => p.memberStatus == Worker.iSanctioned && p.dwccardnum == _w.dwccardnum);
            //Assert
            Assert.AreEqual(1, result.Count(), "Failed to reactivate members");
        }

        private static void recInitialize(MacheteContext DB)
        {
            Person p1 = (Person)Records.person.Clone(); 
            p1.Worker = (Worker)Records.worker.Clone(); 
            p1.Worker.dwccardnum = 30040; p1.Worker.skill1 = 62;
            p1.Worker.memberStatus = Worker.iActive;
            p1.Worker.memberexpirationdate = DateTime.Now.AddDays(-1);
            p1.Worker.Person = p1;
            DB.Persons.Add(p1);
            DB.Workers.Add(p1.Worker);
            Person p2 = (Person)Records.person.Clone(); 
            DB.Persons.Add(p2); p2.Worker = (Worker)Records.worker.Clone(); 
            p2.Worker.dwccardnum = 30041;
            p2.Worker.memberStatus = Worker.iActive;
            p2.Worker.memberexpirationdate = DateTime.Now.AddDays(1);
            Person p3 = (Person)Records.person.Clone(); 
            DB.Persons.Add(p3); p3.Worker = (Worker)Records.worker.Clone(); 
            p3.Worker.dwccardnum = 30042;
            p3.Worker.memberReactivateDate = DateTime.Now.AddDays(-1);
            p3.Worker.memberStatus = Worker.iSanctioned;
            p3.Worker.memberexpirationdate = DateTime.Now.AddDays(1);
            DB.SaveChanges();
        }
    }
}
