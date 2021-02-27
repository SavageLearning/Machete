﻿#region COPYRIGHT
// File:     Records.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Selenium.Old
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
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Machete.Data.Initialize;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Machete.Test.Integration.HttpClientUtil;
using Activity = Machete.Web.ViewModel.Activity;
using ActivitySignin = Machete.Web.ViewModel.ActivitySignin;
using Email = Machete.Web.ViewModel.Email;
using Employer = Machete.Web.ViewModel.Employer;
using Event = Machete.Web.ViewModel.Event;
using Lookup = Machete.Web.ViewModel.Lookup;
using Person = Machete.Web.ViewModel.Person;
using WorkAssignment = Machete.Web.ViewModel.WorkAssignment;
using Worker = Machete.Web.ViewModel.Worker;
using WorkerRequest = Machete.Web.ViewModel.WorkerRequest;
using WorkerSignin = Machete.Web.ViewModel.WorkerSignin;
using WorkOrder = Machete.Web.ViewModel.WorkOrder;

namespace Machete.Test 
{
    public static class ViewModelRecords
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static int GetNextMemberID(DbSet<Worker> db)
        {
            //debug
            //return 10000;
            var first = 10000;
            var last = db.OrderByDescending(x => x.dwccardnum).Select(x => x.dwccardnum).FirstOrDefault();
            var next = last == 0 ? first + 1 : last + 1;
            if (first > next || last == 99999) throw new ArgumentOutOfRangeException("Sorry, I'm having trouble finding a card number.");
            else return next;
        }


        public static Lookup lookup = new Lookup
        {
            category = "test category",
            text_EN = "Test EN",
            text_ES = "Test ES",
            createdby = "initialization script",
            updatedby = "initialization script"
        };

        public static Worker worker = new Worker
        {
            typeOfWorkID = 20,
            RaceID = HttpClientUtil.GetFirstLookupInCategory(LCategory.race),                     //byte
            raceother = "Records._worker1", //string
            height = "too tall",            //string
            weight = "too big",             //string
            englishlevelID = 1,             //byte
            recentarrival = true,           //bool
            dateinUSA = DateTime.Now,       //datetime
            dateinseattle = DateTime.Now,   //datetime
            disabled = true,                //bool
            disabilitydesc = "foo",         //string
            maritalstatus = HttpClientUtil.GetFirstLookupInCategory(LCategory.maritalstatus), //string
            livewithchildren = true,        //bool
            numofchildren = 0,              //byte
            incomeID =  HttpClientUtil.GetFirstLookupInCategory(LCategory.income), //byte
            livealone = true,               //bool
            emcontUSAname = "Bill Clinton", //string
            emcontUSAphone = "1234567890",  //string
            emcontUSArelation = "idol",     //string
            dwccardnum = 0,             //int
            neighborhoodID =  HttpClientUtil.GetFirstLookupInCategory(LCategory.neighborhood), //byte
            immigrantrefugee = true,        //bool
            countryoforiginID = HttpClientUtil.GetFirstLookupInCategory(LCategory.countryoforigin),        //string
            emcontoriginname = "Barak Obama",   //string
            emcontoriginphone = "1234567890",   //string
            emcontoriginrelation = "friend",    //string
            memberexpirationdate = DateTime.Now,    //datetime
            driverslicense = true,                  //bool
            licenseexpirationdate = DateTime.Now,   //datetime
            carinsurance = true,                    //bool
            insuranceexpiration = DateTime.Now,     //datetime
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "initialization script",
            updatedby = "initialization script",
            dateOfBirth = DateTime.Now,
            dateOfMembership = DateTime.Now,
            memberStatusID = HttpClientUtil.GetLookup(LCategory.memberstatus, "Active")
        };

        public static Person person = new Person
        {
            firstname1 = "barack",
            lastname1 = "obama",
            address1 = "12345 6th Ave NE",
            address2 = "Apt 789",
            city = "Gotham",
            phone = "206-555-3825",
            state = "CO",
            zipcode = "67123",
            gender = HttpClientUtil.GetFirstLookupInCategory(LCategory.gender),
            active = true, //true
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };

        public static Event event1 = new Event
        {
            createdby = "TestInitializer",
            updatedby = "TestInitializer",
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            dateFrom = DateTime.Now,
            dateTo = DateTime.Now + TimeSpan.FromDays(30),
            eventTypeID = HttpClientUtil.GetLookup(LCategory.eventtype, "Complaint"),
            notes = "Event note"
        };

        public static Email email = new Email
        {
            emailTo = "changeme@gmail.com",
            emailFrom = "changeme@gmail.com",
            subject = "machete email queue test",
            body = "foo",
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };
//
        public static Employer employer = new Employer
        {
            name = "Willy Wonka",
            businessname = "Chocolate Factory",
            active = false,
            address1 = "Mayor's Office",
            address2 = "P.O. Box 94749",
            city = "Seattle",
            state = "WA",
            zipcode = "98124-4749",
            phone = "206-684-4000",
            cellphone = "123-456-7890",
            referredby = HttpClientUtil.GetFirstLookupInCategory(LCategory.emplrreference),
            email = "willy@wonka.com",
            driverslicense = "wonkawi028f5",
            licenseplate = "123-CDY",
            notes = "A note!",
            referredbyOther = "another reference",
            blogparticipate = true,
            business = true,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };

        public static WorkOrder order = new WorkOrder
        {
            contactName = "Oompa Loompa",
            workSiteAddress1 = "2400 Main Ave E",
            workSiteAddress2 = "Apt 207",
            statusID = HttpClientUtil.GetLookup(LCategory.orderstatus, LOrderStatus.Active),
            city = "Seattle",
            state = "WA",
            zipcode = "12345",
            phone = "123-456-7890",
            typeOfWorkID = HttpClientUtil.GetFirstLookupInCategory(LCategory.worktype),
            timeFlexible = true,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportProviderID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            englishRequiredNote = "",
            description = "description string",
            dateTimeofWork = DateTime.Today,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "initialization script",
            updatedby = "initialization script",
            transportTransactType = 256,
            transportTransactID = "#6169"
        };

//        public static Machete.Api.ViewModel.WorkOrder onlineOrder = new Api.ViewModel.WorkOrder
//        {
//            workSiteAddress1 = "2400 Main Ave E",
//            workSiteAddress2 = "Apt 207",
//            statusID = 42,
//            city = "Seattle",
//            state = "WA",
//            zipcode = "12345",
//            phone = "123-456-7890",
//            //typeOfWorkID = 20,
//            timeFlexible = true,
//            englishRequired = false,
//           // lunchSupplied = false,
//           // permanentPlacement = false,
//            transportProviderID = 1,
//            transportFee = 20.75,
//            //transportFeeExtra = 0,
//            englishRequiredNote = "",
//            description = "description string",
//            dateTimeofWork = DateTime.Today.ToString("o", CultureInfo.InvariantCulture),
//            datecreated = DateTime.Now.ToString("o", CultureInfo.InvariantCulture),
//            dateupdated = DateTime.Now.ToString("o", CultureInfo.InvariantCulture),       
//            createdby = "initialization script",
//            updatedby = "initialization script",
//            //transportTransactType = 256,
//           // transportTransactID = "#6169"
//        };

        public static WorkAssignment assignment = new WorkAssignment
        {
            //ID = 1,
            active = true,
            days = 1,
            hours = 5,
            //chambita = false,
            hourlyWage = 20.50,
            skillID = HttpClientUtil.GetFirstLookupInCategory(LCategory.skill),
            description = "init script",
            englishLevelID = 1,
            //evaluation
            qualityOfWork = 5,
            followDirections = 4,
            attitude = 4,
            reliability = 5,
            transportProgram = 3,
            comments = "I really like to make comments.",
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "initialization script",
            updatedby = "initialization script"
        };

        public static Activity activity = new Activity
        {
            // typeID = HttpClientUtil.TenantLookupsCache.First(x => x.category == Domain.LCategory.activityType && x.text_EN == Domain.LActType.Class).ID,
            typeID = HttpClientUtil.GetFirstLookupInCategory(LCategory.activityType),
            // nameID = HttpClientUtil.TenantLookupsCache.First(x => x.category == Domain.LCategory.activityName && x.text_EN == "Basic English").ID,
            nameID = HttpClientUtil.GetFirstLookupInCategory(LCategory.activityName),
            teacher = "selenium.uitest",
            notes = "foo too",
            dateStart = DateTime.Now,
            dateEnd = DateTime.Now,
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };

        public static ActivitySignin activitysignin = new ActivitySignin
        {
            dwccardnum = 12345,
            dateforsignin = DateTime.Now,
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };

        public static WorkerSignin signin = new WorkerSignin
        {
            dwccardnum = 30040,
            dateforsignin = DateTime.Today,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };

        public static WorkerRequest request = new WorkerRequest
        {
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            createdby = "TestInitializer",
            updatedby = "TestInitializer"
        };       
    }
}
