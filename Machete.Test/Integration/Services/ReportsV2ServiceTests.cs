using System;
using System.Collections.Generic;
using System.Linq;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;

namespace Machete.Test.Integration.Services
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
            frb = FluentRecordBaseFactory.Get();
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
            List<dynamic> result = frb.ToServ<IReportsV2Service>().getQuery(o);
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
            List<dynamic> result = frb.ToServ<IReportsV2Service>().getQuery(o);
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
            List<dynamic> result = frb.ToServ<IReportsV2Service>().getQuery(o);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("20130101-20140101-DispatchesByJob-63", result[0].id);
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
            List<dynamic> result = frb.ToServ<IReportsV2Service>().getQuery(o);
            // Assert

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void getDefaultURL()
        {
            // arrange
            // act
            var result = frb.ToServ<IReportsV2Service>()
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
            var result = frb.ToServ<IReportsV2Service>().Get(1);
            // Assert
            Assert.IsNotNull(result);
            // assumes data from ReportDefinitionsInitializer
            Assert.AreEqual("DispatchesByJob", result.name);
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void get_XlsxFile_noParams_succeeds()
        {
            // arrange
            var dict = new Dictionary<string, string>();
            dict.Add("ID", "true");
            dict.Add("dwccardnum", "true");
            dict.Add("datecreated", "true");
            byte[] result = null;
            var o = new DTO.SearchOptions {
                name = "Workers",
                beginDate = null,
                endDate = null,
                exportFilterField = null,
                exportIncludeOptions = dict
            };
            // act
            frb.ToServ<IReportsV2Service>()
                .getXlsxFile(o, ref result);
            // assert
            Assert.IsNotNull(result);
        }
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void get_XlsxFile_beginDate_succeeds()
        {
            // arrange
            var dict = new Dictionary<string, string>();
            dict.Add("ID", "true");
            dict.Add("dwccardnum", "true");
            dict.Add("datecreated", "true");
            byte[] result = null;
            var o = new DTO.SearchOptions
            {
                name = "Workers",
                beginDate = DateTime.Now,
                endDate = null,
                exportFilterField = "datecreated",
                exportIncludeOptions = dict
            };
            // act
            frb.ToServ<IReportsV2Service>()
                .getXlsxFile(o, ref result);
            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void get_XlsxFile_beginAndEndDate_succeeds()
        {
            // arrange
            var dict = new Dictionary<string, string>();
            dict.Add("ID", "true");
            dict.Add("dwccardnum", "true");
            dict.Add("datecreated", "true");
            byte[] result = null;
            var o = new DTO.SearchOptions
            {
                name = "Workers",
                beginDate = DateTime.Now,
                endDate = DateTime.Now.AddDays(1),
                exportFilterField = "datecreated",
                exportIncludeOptions = dict
            };
            // act
            frb.ToServ<IReportsV2Service>()
                .getXlsxFile(o, ref result);
            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void ValidateReturnsEmptyList_WhenNoErrorsAreFound() {
            var errorList = new List<string>();
            var query = "SELECT 1 from dbo.ReportDefinitions";
            errorList = frb.ToServ<IReportsV2Service>().validateQuery(query);
            Assert.AreEqual(0, errorList.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void ValidateReturnsListOfErrors_WhenTheyAreFound()
        {
            var errorList = new List<string>();
            var query = "DROP TABLE dbo.ReportDefinitions";
            errorList = frb.ToServ<IReportsV2Service>().validateQuery(query);
            Assert.AreEqual(1, errorList.Count);
            Assert.IsTrue(errorList[0].Equals("Cannot drop the table 'ReportDefinitions', because it does not exist or you do not have permission."));
        }
    }

}
