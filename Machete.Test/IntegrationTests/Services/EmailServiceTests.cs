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
namespace Machete.Test.IntegrationTests.Services
{
    [TestClass]
    public class EmailServiceTests
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

        [TestCleanup]
        public void TestCleanup()
        {
            frb.Dispose();
            frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void Integration_Email_Service_CreateAndValidateKey()
        {
            //Arrange
            //Act
            var result = frb.ToEmail();
            //Assert
            Assert.IsNotNull(result.ID, "Email.ID is null");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void Integration_Email_Service_CreateWithWorkOrderID_joins_successfully()
        {
            var wo = frb.ToWorkOrder();
            var email = frb.ToEmail();
            var serv = frb.ToServEmail();
            //ACT
            var result = serv.CreateWithWorkorder(email, wo.ID, "interation test");
            // ASSERT
            Assert.IsNotNull(result.ID, "Email.ID is null");
            Assert.IsNotNull(result.WorkOrders);
            Assert.AreEqual(wo.ID, result.WorkOrders.SingleOrDefault().ID);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Emails)]
        public void Integration_Email_Service_GetIndex_filterOn_recordid()
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
        public void Integration_Email_Service_getIndex_filterOn_woid()
        {
            var wo = frb.ToWorkOrder();
            var email = frb.ToEmail();
            var serv = frb.ToServEmail();
            var joinedEmail = serv.CreateWithWorkorder(email, wo.ID, "interation test");
            dOptions.woid = wo.ID;
            // ACT
            var result = serv.GetIndexView(dOptions);
            // ASSERT
            Assert.AreEqual(1, result.filteredCount);
        }
    }
}
