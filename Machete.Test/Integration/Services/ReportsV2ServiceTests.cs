using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;
using Machete.Service.DTO.Reports;
using System.Collections.Generic;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class ReportsV2ServiceTests
    {
        DTO.SearchOptions o;
        FluentRecordBase frb;

        [ClassInitialize]
        public static void ClassInitialize(TestContext c)
        {

        }

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Get_JobsDispatchedCount_IsNotNull()
        {
            // Arrange
            var skill = frb.ToLookup();
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
               .AddWorkAssignment(skill: 61);
            o = new DTO.SearchOptions
            {
                beginDate = DateTime.Parse("1/1/2013"),
                endDate = DateTime.Parse("1/1/2014")
            };
            // Act
            List<SimpleDataRow> result = frb.ToServReportsV2().getJobsDispatchedCount(o);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

        }
    }
}
