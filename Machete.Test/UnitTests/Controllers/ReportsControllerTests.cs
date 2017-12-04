using AutoMapper;
using Machete.Api;
using Machete.Api.Controllers;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;
using System;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using FluentAssertions;

namespace Machete.Test.UnitTests.Controllers
{
    [TestClass]
    public class ReportsControllerTests
    {
        public Mock<IReportsV2Service> serv;
        public IMapper map;
        public ReportsController controller;

        [TestInitialize]
        public void Initialize()
        {
            serv = new Mock<IReportsV2Service>();
            serv.ReturnReportList();

            map = new MapperConfig().getMapper();

            controller = new ReportsController(serv.Object, map);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Get_WithNoArgs_ReturnsJsonListOfAllReports()
        {
            // Arrange
            // we expect the returned object to have fields added by the mapper
            var expectedResult = ReportsControllerTestHelpers.FakeReportList().Select(report => map.Map<Domain.ReportDefinition, ReportDefinition>(report));

            // Act
            var result = controller.Get();
            var response = result.ExecuteAsync(CancellationToken.None).Result;
            var sResponse = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(sResponse);
            var reportList = JsonConvert.DeserializeObject<List<ReportDefinition>>(jsonResponse["data"].ToString());

            // Assert
            reportList.Should().HaveCount(2, because: "that's what the test object had");
            reportList.ShouldBeEquivalentTo(expectedResult);
        }
    }
    public static class ReportsControllerTestHelpers
    {
        // my extension methods
        public static Moq.Language.Flow.IReturnsResult<IReportsV2Service> ReturnReportList(this Mock<IReportsV2Service> service)
        {
            return service.Setup(rs => rs.getList()).Returns(FakeReportList);
        }

        public static List<Domain.ReportDefinition> FakeReportList()
        {
            return new List<Domain.ReportDefinition> {
                new Domain.ReportDefinition
                {
                    name = "FakeReport",
                    commonName = "Fake Report",
                    title = "A Report About Fakes",
                    description = "The fakest report you've ever seen",
                    sqlquery = "SELECT 1 FROM dbo.fake",
                    category = "Fake Reports",
                    subcategory = "Really Fake Reports",
                    inputsJson = "{ \"totally\": \"valid json\" }",
                    columnsJson = "{ \"columns\": [\"column A\", \"column B\"] }"
                },
                new Domain.ReportDefinition
                {
                    name = "FakeReport2",
                    commonName = "Fake Report 2",
                    title = "A Report About Fakes and the People Who Make Them",
                    description = "The second fakest report you've ever seen",
                    sqlquery = "SELECT 1 FROM dbo.fake JOIN dbo.People",
                    category = "Fake Reports",
                    subcategory = "Really Fake Reports",
                    inputsJson = "{ \"totally\": \"valid json\" }",
                    columnsJson = "{ \"columns\": [\"column A\", \"column B\"] }"
                }
            };
        }
    }
}
