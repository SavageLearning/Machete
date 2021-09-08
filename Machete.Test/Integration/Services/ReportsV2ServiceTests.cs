using System;
using System.Collections.Generic;
using System.Linq;
using Machete.Domain;
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
                endDate = DateTime.Parse("1/1/2014"),
                dwccardnum = 0
            };
            // Act
            List<dynamic> result = frb.ToServ<IReportsV2Service>().GetQuery(o);
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
            List<dynamic> result = frb.ToServ<IReportsV2Service>().GetQuery(o);
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
                endDate = DateTime.Parse("1/1/2014"),
                dwccardnum = 0
            };
            // Act
            List<dynamic> result = frb.ToServ<IReportsV2Service>().GetQuery(o);
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
                endDate = DateTime.Parse("1/1/2014"),
                dwccardnum = 0
            };
            // Act
            List<dynamic> result = frb.ToServ<IReportsV2Service>().GetQuery(o);
            // Assert

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void getDefaultURL()
        {
            // arrange
            // act
            var result = frb.ToServ<IReportsV2Service>()
                .GetList();
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
                .GetXlsxFile(o, ref result);
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
                .GetXlsxFile(o, ref result);
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
                .GetXlsxFile(o, ref result);
            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void ValidateReturnsEmptyList_WhenNoErrorsAreFound() {
            var errorList = new List<string>();
            var query = "SELECT 1 from dbo.ReportDefinitions";
            errorList = frb.ToServ<IReportsV2Service>().ValidateQuery(query);
            Assert.AreEqual(0, errorList.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void ValidateReturnsListOfErrors_WhenTheyAreFound()
        {
            var errorList = new List<string>();
            var query = "DROP TABLE dbo.ReportDefinitions";
            errorList = frb.ToServ<IReportsV2Service>().ValidateQuery(query);
            Assert.AreEqual(1, errorList.Count);
            Assert.IsTrue(errorList[0].Equals("Cannot drop the table 'ReportDefinitions', because it does not exist or you do not have permission."));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Validate_Returns_Empty_List_of_Errors_When_Query_references_params()
        {
            // queries reference pre-defined params defined outside of query,
            // Method should account for them
            var errorList = new List<string>();
            var query = "SELECT @beginDate, @endDate, @dwccardnum";
            errorList = frb.ToServ<IReportsV2Service>().ValidateQuery(query);
            Assert.AreEqual(0, errorList.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Definition_Save_and_create_Updates_Columns()
        {
            // arrange
            var timeStamp = DateTime.UtcNow.ToString("MM-dd-yy-HH:mm tt");
            var fakeReport = new ReportDefinition()
            {
                name = $"fakeReportName-{timeStamp}",
                commonName = $"Fake Report Name {timeStamp}",
                sqlquery = "select * from ReportDefinitions",
                inputsJson = "{\"beginDate\":false,\"endDate\":false,\"memberNumber\":false}"
            };

            var expectedCreatedColumns = "[{\"field\":\"ID\",\"header\":\"ID\",\"visible\":true},{\"field\":\"name\",\"header\":\"name\",\"visible\":true},{\"field\":\"commonName\",\"header\":\"commonName\",\"visible\":true},{\"field\":\"title\",\"header\":\"title\",\"visible\":true},{\"field\":\"description\",\"header\":\"description\",\"visible\":true},{\"field\":\"sqlquery\",\"header\":\"sqlquery\",\"visible\":true},{\"field\":\"category\",\"header\":\"category\",\"visible\":true},{\"field\":\"subcategory\",\"header\":\"subcategory\",\"visible\":true},{\"field\":\"inputsJson\",\"header\":\"inputsJson\",\"visible\":true},{\"field\":\"columnsJson\",\"header\":\"columnsJson\",\"visible\":true},{\"field\":\"datecreated\",\"header\":\"datecreated\",\"visible\":true},{\"field\":\"dateupdated\",\"header\":\"dateupdated\",\"visible\":true},{\"field\":\"Createdby\",\"header\":\"Createdby\",\"visible\":true},{\"field\":\"Updatedby\",\"header\":\"Updatedby\",\"visible\":true}]";
            // act
            var createResult = frb.ToServ<IReportsV2Service>().Create(fakeReport, "test");

            // assert
            Assert.AreEqual(createResult.columnsJson, expectedCreatedColumns);
            createResult.sqlquery = "SELECT name, commonName, description FROM ReportDefinitions";
            frb.ToServ<IReportsV2Service>().Save(createResult, "testbot");
            var saveResult = frb.ToServ<IReportsV2Service>().Get($"fakeReportName-{timeStamp}");

            // assert
            Assert.AreEqual(saveResult.columnsJson, "[{\"field\":\"name\",\"header\":\"name\",\"visible\":true},{\"field\":\"commonName\",\"header\":\"commonName\",\"visible\":true},{\"field\":\"description\",\"header\":\"description\",\"visible\":true}]");
            Assert.AreNotEqual(saveResult.columnsJson, expectedCreatedColumns);

            // clean up
            frb.ToServ<IReportsV2Service>().Delete(createResult.ID, "test bot");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Delete_Removes_Record()
        {
            // setup
            var timeStamp = DateTime.UtcNow.ToString("MM-dd-yy-HH-mm-tt");
            var fakeReport = new ReportDefinition()
            {
                name = $"fakeReportName-{timeStamp}",
                commonName = $"Fake Report Name {timeStamp}",
                sqlquery = "select * from ReportDefinitions",
                inputsJson = "{\"beginDate\":false,\"endDate\":false,\"memberNumber\":false}"
            };
            var reportToDelete = frb.ToServ<IReportsV2Service>().Create(fakeReport, "test bot");
            // act
            frb.ToServ<IReportsV2Service>().Delete(reportToDelete.ID, "test bot");
            var exists = frb.ToServ<IReportsV2Service>().Get(reportToDelete.ID) != null;
            // assert
            Assert.IsFalse(exists);
        }

    }

}
