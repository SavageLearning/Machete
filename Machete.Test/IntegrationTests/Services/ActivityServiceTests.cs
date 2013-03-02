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
        protected WorkerSigninRepository _wsiRepo;
        protected WorkerRepository _wRepo;
        protected PersonRepository _pRepo;
        protected WorkOrderRepository _woRepo;
        protected WorkAssignmentRepository _waRepo;
        protected WorkerRequestRepository _wrRepo;
        protected ILookupRepository _lRepo;
        protected ImageRepository _iRepo;
        protected DatabaseFactory _dbFactory;
        protected WorkerSigninService _wsiServ;
        protected WorkerService _wServ;
        protected PersonService _pServ;
        protected ImageService _iServ;
        protected WorkerRequestService _wrServ;
        protected WorkOrderService _woServ;
        protected WorkAssignmentService _waServ;
        protected ActivityRepository _aRepo;
        protected ActivitySigninRepository _asRepo;
        protected ActivityService _aServ;
        protected ActivitySigninService _asServ;
        protected IUnitOfWork _unitofwork;
        protected MacheteContext DB;
        [TestInitialize]
        public void TestInitialize()
        {
            //Doesn't blast the database
            Database.SetInitializer<MacheteContext>(new MacheteInitializer());
            DB = new MacheteContext("macheteConnection"); //name of DB in sql server
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            _dbFactory = new DatabaseFactory();
            _dbFactory.Set(DB); // overriding default context to use non-test DB
            _iRepo = new ImageRepository(_dbFactory);
            _wRepo = new WorkerRepository(_dbFactory);
            _woRepo = new WorkOrderRepository(_dbFactory);
            _wrRepo = new WorkerRequestRepository(_dbFactory);
            _waRepo = new WorkAssignmentRepository(_dbFactory);
            _wsiRepo = new WorkerSigninRepository(_dbFactory);
            _lRepo = new LookupRepository(_dbFactory);
            _pRepo = new PersonRepository(_dbFactory);
            _aRepo = new ActivityRepository(_dbFactory);
            _asRepo = new ActivitySigninRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _pServ = new PersonService(_pRepo, _unitofwork);
            _iServ = new ImageService(_iRepo, _unitofwork);            
            _asServ = new ActivitySigninService(_asRepo, _wRepo, _pRepo, _iRepo, _wrRepo, _unitofwork);
            _aServ = new ActivityService(_aRepo, _asServ, _unitofwork);
            _wrServ = new WorkerRequestService(_wrRepo, _unitofwork);
            _waServ = new WorkAssignmentService(_waRepo, _wRepo, _lRepo, _wsiRepo, _unitofwork);
            _wServ = new WorkerService(_wRepo, _unitofwork);
            _woServ = new WorkOrderService(_woRepo, _waServ, _unitofwork);
            _wsiServ = new WorkerSigninService(_wsiRepo, _wRepo, _iRepo, _wrRepo, _unitofwork);
        }

        [TestMethod]
        public void onehundred()
        {
            //    Integration_Activity_Service_CreateRandomClass();
        }
        [TestMethod]
        public void Integration_Activity_Service_CreateRandomClass()
        {
            //Used once to create dummy data to support report creation
            // requires change in app.config to point test database to production
            IEnumerable<int> cardlist = DB.Workers.Select(q => q.dwccardnum).Distinct();
            IEnumerable<int> classlist = DB.Lookups.Where(l => l.category == "activityName").Select(q => q.ID);
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
            _aServ.Create(a, "TestScript");
            int rAttendance = rand.Next(cardlist.Count() / 5);
            for (var i = 0; i < rAttendance; i++)
            {
                ActivitySignin asi = (ActivitySignin)Records.activitysignin.Clone();
                asi.dateforsignin = today;
                asi.activityID = a.ID;
                asi.dwccardnum = cardlist.ElementAt(rand.Next(cardlist.Count()));
                _asServ.CreateSignin(asi, "TestScript");
            }
            //a.
        }
        [TestMethod]
        public void Integration_Activity_service_CreateClass_within_hour()
        {
            IEnumerable<int> cardlist = DB.Workers.Select(q => q.dwccardnum).Distinct();
            IEnumerable<int> classlist = DB.Lookups.Where(l => l.category == "activityName").Select(q => q.ID);
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
            _aServ.Create(a, "TestScript");
            
            Assert.IsTrue(1 == DB.Activities.Where(aa => aa.ID == a.ID).Count());
        }

        [TestMethod]
        public void Integration_Activity_Service_GetIndexView_authenticated()
        {
            //
            //Arrange
            viewOptions o = new viewOptions();
            o.CI = new CultureInfo("en-US", false);
            //o.sSearch = DateTime.Today.ToShortDateString();
            o.EmployerID = null;
            o.status = null;
            o.displayStart = 0;
            o.displayLength = 20;
            o.authenticated = false;
            //
            //Act
            Assert.Inconclusive("Need to add several records and count what's visible");
            dataTableResult<Activity> result = _aServ.GetIndexView(o);
            //
            //Assert
            IEnumerable<Activity> query = result.query.ToList();
            Assert.IsNotNull(result, "IEnumerable is Null");
            Assert.IsNotNull(query, "IEnumerable.query is null");
        }
    }
}