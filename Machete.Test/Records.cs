#region COPYRIGHT
// File:     Records.cs
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
using Machete.Domain;
using Machete.Data;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace Machete.Test 
{
    public static class Records
    {
        public static int GetNextMemberID(DbSet<Worker> db)
        {
            //debug
            //return 10000;
            var next = 10000;
            var last = (int?)db.OrderByDescending(x => x.dwccardnum).Select(x => x.dwccardnum).FirstOrDefault() ?? next;
            if (last == next) return last + 1;
            else if (last == 99999) throw new ArgumentOutOfRangeException("Sorry, I'm having trouble finding a card number.");
            else return last + 1;
        }

        public static Image image = new Image
        {
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static Lookup lookup = new Lookup
        {
            category = "test category",
            text_EN = "Test EN",
            text_ES = "Test ES",
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static Worker worker = new Worker
        {
            typeOfWorkID = 20,
            RaceID = MacheteLookup.cache.First(x => x.category == "race" && x.text_EN =="Latino").ID,                     //byte
            raceother = "Records._worker1", //string
            height = "too tall",            //string
            weight = "too big",             //string
            englishlevelID = 1,             //byte
            recentarrival = true,           //bool
            dateinUSA = DateTime.Now,       //datetime
            dateinseattle = DateTime.Now,   //datetime
            disabled = true,                //bool
            disabilitydesc = "foo",         //string
            maritalstatus = MacheteLookup.cache.First(x => x.category == "maritalstatus" && x.text_EN == "Single").ID,            //string
            livewithchildren = true,        //bool
            numofchildren = 0,              //byte
            incomeID = MacheteLookup.cache.First(x => x.category == "income" && x.text_EN == "Poor (Less than $15,000)").ID,                   //byte
            livealone = true,               //bool
            emcontUSAname = "Bill Clinton", //string
            emcontUSAphone = "1234567890",  //string
            emcontUSArelation = "idol",     //string
            dwccardnum = 0,             //int
            neighborhoodID = MacheteLookup.cache.First(x => x.category == "neighborhood" && x.text_EN == "Primary City").ID,             //byte
            immigrantrefugee = true,        //bool
            countryoforiginID = MacheteLookup.cache.First(x => x.category == "countryoforigin" && x.text_EN == "Mexico").ID,        //string
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
            Createdby = "initialization script",
            Updatedby = "initialization script",
            dateOfBirth = DateTime.Now,
            dateOfMembership = DateTime.Now,
            memberStatus = MacheteLookup.cache.First(x => x.category == "memberstatus" && x.text_EN == "Active").ID
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
            gender = MacheteLookup.cache.First(x => x.category == "gender" && x.text_EN == "Male").ID,
            active = true, //true
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static Event event1 = new Event
        {
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer",
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            dateFrom = DateTime.Now,
            dateTo = DateTime.Now + TimeSpan.FromDays(30),
            eventType = MacheteLookup.cache.First(x => x.category == "eventtype" && x.text_EN == "Complaint").ID,
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
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"//,
//            attachment = @"<!DOCTYPE html>
//<html>
//<body>
//
//<h1>My First Heading</h1>
//
//<p>My first paragraph.</p>
//
//</body>
//</html>",
////            attachmentContentType = System.Net.Mime.MediaTypeNames.Text.Html
        };
//
        public static Employer employer = new Employer
        {
            name = "Willy Wonka",
            active = false,
            address1 = "Mayor's Office",
            address2 = "P.O. Box 94749",
            city = "Seattle",
            state = "WA",
            zipcode = "98124-4749",
            phone = "206-684-4000",
            cellphone = "123-456-7890",
            referredby = MacheteLookup.cache.First(c => c.category == "emplrreference" && c.text_EN == "Facebook").ID,
            email = "willy@wonka.com",
            notes = "A note!",
            referredbyOther = "another reference",
            blogparticipate = true,
            business = true,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static WorkOrder order = new WorkOrder
        {
            contactName = "Oompa Loompa",
            workSiteAddress1 = "2400 Main Ave E",
            workSiteAddress2 = "Apt 207",
            status = 42,
            city = "Seattle",
            state = "WA",
            zipcode = "12345",
            phone = "123-456-7890",
            typeOfWorkID = 20,
            timeFlexible = true,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            englishRequiredNote = "",
            description = "description string",
            dateTimeofWork = DateTime.Today,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkAssignment assignment = new WorkAssignment
        {
            //ID = 1,
            active = true,
            days = 1,
            hours = 5,
            //chambita = false,
            hourlyWage = 20.50,
            skillID = 69,
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
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static Activity activity = new Activity
        {
            name = 98,
            type = 101,
            teacher = "jadmin",
            notes = "foo too",
            dateStart = DateTime.Now,
            dateEnd = DateTime.Now,
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static ActivitySignin activitysignin = new ActivitySignin
        {
            dwccardnum = 12345,
            dateforsignin = DateTime.Now,
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static WorkerSignin signin = new WorkerSignin
        {
            dwccardnum = 30040,
            dateforsignin = DateTime.Today,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static WorkerRequest request = new WorkerRequest
        {
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };
    }
}
