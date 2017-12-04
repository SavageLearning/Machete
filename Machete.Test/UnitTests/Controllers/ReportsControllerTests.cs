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
using System.Linq;
using Newtonsoft.Json;
using FluentAssertions;
using Moq.Language.Flow;
using System;

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
            serv.SetupGetList();
            serv.SetupGet();

            map = new MapperConfig().getMapper();

            controller = new ReportsController(serv.Object, map);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_WithNoArgs_ReturnsJsonListOfAllReports()
        {
            // Arrange
            // we expect the returned object to have fields added by the mapper
            var expectedResult = ReportsControllerTestHelpers.FakeReportList().Select(report => map.Map<Domain.ReportDefinition, ReportDefinition>(report));

            // Act
            var result = controller.Get().AsCSharpObject<List<ReportDefinition>>();          

            // Assert
            result.Should().HaveCount(2, because: "that's what the test object had");
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_WithId_ReturnsSingleReport_WithSameId()
        {
            // Arrange: This only works because the IDs equal the index of the list members for the test class.
            var expectedResult = ReportsControllerTestHelpers.FakeReportList()[1];

            // Act
            var result = controller.Get("1").AsCSharpObject<ReportDefinition>();

            // Assert: It's unclear to me how we wind up with id, columns, and inputs, because we're not mapping to the view model this time.
            result.ShouldBeEquivalentTo(expectedResult, x => x
              .Excluding(field => field.id)
              .Excluding(field => field.columns)
              .Excluding(field => field.inputs)
            );
        }
    }
    public static class ReportsControllerTestHelpers
    {
        // my extension methods
        public static T AsCSharpObject<T>(this IHttpActionResult result)
        {
            var response = result.ExecuteAsync(CancellationToken.None).Result;
            var sResponse = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(sResponse);
            return JsonConvert.DeserializeObject<T>(jsonResponse["data"].ToString());
        }

        public static IReturnsResult<IReportsV2Service> SetupGetList(this Mock<IReportsV2Service> service)
        {
            return service.Setup(rs => rs.getList()).Returns(FakeReportList);
        }

        public static IReturnsResult<IReportsV2Service> SetupGet(this Mock<IReportsV2Service> service)
        {
            return service.Setup(rs => rs.Get(It.IsAny<string>())).Returns((string i) => FakeReportList().Single(report => report.ID == Int32.Parse(i)));
        }

        public static List<Domain.ReportDefinition> FakeReportList()
        {
            return new List<Domain.ReportDefinition> {
                new Domain.ReportDefinition
                {
                    ID = 0,
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
                    ID = 1,
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
