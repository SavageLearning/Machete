#region COPYRIGHT
// File:     EmailServiceTests.cs
// Author:   Savage Learning, LLC.
// Created:  2013/05/04
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
namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class EmailSTests
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
                sortColName = "",
                displayStart = 0,
                displayLength = 20
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb = null;
        }
        [Ignore]
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void Create_and_validate_key()
        {
            //Arrange
            //Act
            var result = frb.ToEmail();
            //Assert
            Assert.IsNotNull(result.ID, "Email.ID is null");
        }
        [Ignore]
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void Create_with_WorkOrderID_joins_successfully()
        {
            var wo = frb.ToWorkOrder();
            var email = frb.ToEmail();
            var serv = frb.ToServEmail();
            //ACT
            var result = serv.Create(email, "integration test", wo.ID);
            // ASSERT
            Assert.IsNotNull(result.ID, "Email.ID is null");
            Assert.IsNotNull(result.WorkOrders);
            Assert.AreEqual(wo.ID, result.WorkOrders.SingleOrDefault().ID);
        }
        [Ignore]
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void GetIndex_filterOn_recordid()
        {
            var serv = frb.ToServEmail();
            var _em = frb.ToEmail();
            dOptions.sortColName = "RelatedTo";
            dOptions.emailID = _em.ID;
            // ACT
            var result = serv.GetIndexView(dOptions);
            // ASSERT
            Assert.AreEqual(1, result.filteredCount);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void getIndex_filterOn_woid()
        {
            var wo = frb.ToWorkOrder();
            var email = frb.ToEmail();
            var serv = frb.ToServEmail();
            var joinedEmail = serv.Create(email, "integration test", wo.ID);
            dOptions.woid = wo.ID;
            // ACT
            var result = serv.GetIndexView(dOptions);
            // ASSERT
            Assert.AreEqual(1, result.filteredCount);
        }
        [Ignore]
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void SendEmailSimple()
        {
            var email = frb.ToEmail();
            var cfg = frb.ToEmailConfig();
            email.emailFrom = cfg.fromAddress;
            email.emailTo = cfg.fromAddress;
            var serv = frb.ToServEmail();
            email.statusID = Email.iReadyToSend;
            var emailsent = serv.Create(email, "integration test");
            Assert.AreEqual(Email.iSent, emailsent.statusID);
        }
    }
}
