#region COPYRIGHT
// File:     EmailServiceTests.cs
// Author:   Savage Learning, LLC.
// Created:  2013/05/04
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
using System.Globalization;
using System.Linq;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class EmailSTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

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
            var serv = frb.ToServ<IEmailService>();
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
            var serv = frb.ToServ<IEmailService>();
            var _em = frb.ToEmail();
            dOptions.sortColName = "RelatedTo";
            dOptions.emailID = _em.ID;
            // ACT
            var result = serv.GetIndexView(dOptions);
            // ASSERT
            Assert.AreEqual(1, result.filteredCount);
        }

        // TODO figure out why this is creating the email twice, it causes the IDENTITY_INSERT exception of EF Core fame
        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void getIndex_filterOn_woid()
        {
            var wo = frb.ToWorkOrder();
            var email = frb.ToEmail();
            var serv = frb.ToServ<IEmailService>();
            serv.Create(email, "integration test", wo.ID);
            dOptions.woid = wo.ID;
            // ACT
            var result = serv.GetIndexView(dOptions);
            // ASSERT
            Assert.AreEqual(1, result.filteredCount);
        }
    }
}
