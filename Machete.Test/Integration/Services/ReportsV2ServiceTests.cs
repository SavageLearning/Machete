using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;
using Machete.Service.DTO.Reports;
using System.Collections.Generic;
using System.Linq;

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
        public void GetSimpleAggregate_IsNotNull()
        {
            // Arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
               .AddWorkAssignment(skill: 63); // known skill ID from machete lookup initializer
            o = new DTO.SearchOptions
            {
                idOrName = "DispatchesByJob",
                beginDate = DateTime.Parse("1/1/2013"),
                endDate = DateTime.Parse("1/1/2014")
            };
            // Act
            List<dynamic> result = frb.ToServReportsV2().getQuery(o);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void GetSimpleAggregate_IdGetSucceeds()
        {
            // Arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
               .AddWorkAssignment(skill: 63); // known skill ID from machete lookup initializer
            o = new DTO.SearchOptions
            {
                idOrName = "1",
                beginDate = DateTime.Parse("1/1/2013"),
                endDate = DateTime.Parse("1/1/2014")
            };
            // Act
            List<dynamic> result = frb.ToServReportsV2().getQuery(o);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void GetSimpleAggregate_IsCaseInsensitive()
        {
            // Arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
               .AddWorkAssignment(skill: 63); // known skill ID from machete lookup initializer
            o = new DTO.SearchOptions
            {
                idOrName = "dispatchesbyjob",
                beginDate = DateTime.Parse("1/1/2013"),
                endDate = DateTime.Parse("1/1/2014")
            };
            // Act
            List<dynamic> result = frb.ToServReportsV2().getQuery(o);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("2013-01-01T00:00:00-2014-01-01T00:00:00-63", result[0].id);
            Assert.AreEqual("general labor", result[0].label);
        }
        [ExpectedException(typeof(InvalidOperationException),  "Exception not thrown on bad query.")]
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void GetSimpleAggregate_MissingDefThrowsException()
        {
            // Arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
               .AddWorkAssignment(skill: 63); // known skill ID from machete lookup initializer
            o = new DTO.SearchOptions
            {
                idOrName = "blah",
                beginDate = DateTime.Parse("1/1/2013"),
                endDate = DateTime.Parse("1/1/2014")
            };
            // Act
            List<dynamic> result = frb.ToServReportsV2().getQuery(o);
            // Assert

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void getDefaultURL()
        {
            // arrange
            // act
            var result = frb.ToServReportsV2()
                .getList();
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Where(a => a.name == "DispatchesByJob").Count());

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Get_Int_succeeds()
        {
            // Arrange
            // Act
            var result = frb.ToServReportsV2().Get(1);
            // Assert
            Assert.IsNotNull(result);
            // assumes data from ReportDefinitionsInitializer
            Assert.AreEqual("DispatchesByJob", result.name);
        }
    }
}
