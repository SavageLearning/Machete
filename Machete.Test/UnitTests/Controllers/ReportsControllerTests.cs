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
using Api.ViewModels;

namespace Machete.Test.UnitTests.Controllers
{
    [TestClass]
    public class ReportsControllerTests
    {
        public Mock<IReportsV2Service> serv;
        public IMapper map;
        public ReportsController controller;
        public DateTime testStartDate;
        public DateTime testEndDate;

        [TestInitialize]
        public void Initialize()
        {
            testStartDate = DateTime.Now.AddMonths(-6);
            testEndDate = DateTime.Now;

            serv = new Mock<IReportsV2Service>();
            serv.SetupGetList();
            serv.SetupGet();
            serv.SetupGetQuery(testStartDate, testEndDate);
            serv.SetupPost();

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

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_WithFourArgs_ReturnsSingleReport_MatchingThoseParameters()
        {
            // this test is quick-and-dirty and needs serious help.
            // all it shows currently is that this method can match on a name instead of an ID.

            // Arrange
            var expectedResult = new List<dynamic> {
                (dynamic)ReportsControllerTestHelpers.FakeReportList()[0]
            };

            // Act
            var result = controller.Get("FakeReport", testStartDate, testEndDate, null)
                                   .AsCSharpObject<List<ReportDefinition>>();

            // Assert
            result.ShouldBeEquivalentTo(expectedResult, x => x.ExcludingMissingMembers());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Post_WithGoodQuery_Returns304()
        {
            // Arrange
            // Act
            var result = controller.Post("goodQuery".AsReportQuery()).AsHttpResponseMessage();
            // Assert
            result.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.NotModified);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Post_WithBadQuery_ReturnsErrors_With200()
        {
            // Arrange
            var expectedResult = ReportsControllerTestHelpers.FakeSqlErrors();
            // Act
            var result = controller.Post("badQuery".AsReportQuery()).AsCSharpObject<List<string>>();
            // Assert
            result.ShouldBeEquivalentTo(expectedResult, x => x.ExcludingMissingMembers());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Post_Returns500WhenExceptionIsThrown() {
            // Arrange
            // Act
            var result = controller.Post("throwPlease".AsReportQuery()).AsHttpResponseMessage();
            // Assert
            result.StatusCode.ShouldBeEquivalentTo(System.Net.HttpStatusCode.InternalServerError);
        }
    }
    public static class ReportsControllerTestHelpers
    {
        public static ReportQuery AsReportQuery(this string operation)
        {
            return new ReportQuery { query = operation };
        }

        public static T AsCSharpObject<T>(this IHttpActionResult result)
        {
            var response = result.AsHttpResponseMessage();
            var sResponse = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(sResponse);
            return JsonConvert.DeserializeObject<T>(jsonResponse["data"].ToString());
        }

        public static HttpResponseMessage AsHttpResponseMessage(this IHttpActionResult result)
        {
            return result.ExecuteAsync(CancellationToken.None).Result;
        }

        public static IReturnsResult<IReportsV2Service> SetupGetList(this Mock<IReportsV2Service> service)
        {
            return service.Setup(rs => rs.getList()).Returns(FakeReportList);
        }

        public static IReturnsResult<IReportsV2Service> SetupGet(this Mock<IReportsV2Service> service)
        {
            return service.Setup(rs => rs.Get(It.IsAny<string>())).Returns((string i) => FakeReportList().Single(report => report.ID == Int32.Parse(i)));
        }

        public static IReturnsResult<IReportsV2Service> SetupGetQuery(this Mock<IReportsV2Service> service, DateTime beginDate, DateTime endDate)
        {
            return service.Setup(rs => rs.getQuery(It.IsAny<Service.DTO.SearchOptions>())).Returns((Service.DTO.SearchOptions o) => new List<dynamic> { FakeReportList().Single(report => System.String.Equals(report.name, o.idOrName))});
        }

        // not sure why I needed the return types on the other methods? I think I was trying to string them together...
        public static void SetupPost(this Mock<IReportsV2Service> service)
        {
            service.Setup(rs => rs.validateQuery(It.Is<string>(x => x == "goodQuery"))).Returns(new List<string>());
            service.Setup(rs => rs.validateQuery(It.Is<string>(x => x == "badQuery"))).Returns(FakeSqlErrors);
            service.Setup(rs => rs.validateQuery(It.Is<string>(x => x == "throwPlease"))).Throws(new Exception("You should log these!"));
        }

        public static List<string> FakeSqlErrors()
        {
            return new List<string> {
                "Error: The light shall burn you!",
                "Error: I will be your death!"
            };
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
