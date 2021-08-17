using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Machete.Service.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers.Api;
using Machete.Web.Maps.Api;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class ReportsControllerTests
    {
        private Mock<IReportsV2Service> _reportsServ;
        private Mock<ITenantService> _tenantServ;
        private IMapper _mapper;
        private ReportsController _controller;
        private List<ReportDefinition> _fakeReports;
        private ReportDefinition _fakeGetReport;
        private List<dynamic> _fakeDynamicReportResult;
        private ReportDefinition _fakeReport;
        private const string FAKETIMEZONE = "America/Los_Angeles";

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeReport = new ReportDefinition()
            {
                name = "fakeReportName",
                commonName = "Fake Report Common Name",
                category = "fakeCategory",
                description = "Fake Description",
                sqlquery =
                    @"fake sql query"
            };
            _fakeReports = new List<ReportDefinition>();
            _fakeGetReport = new ReportDefinition
            {
                name = "fakeReportName",
                commonName = "Fake Report Name",
                sqlquery = "SELECT * FROM FOO"
            };
            _fakeDynamicReportResult = new List<dynamic>();
            _fakeDynamicReportResult.Add(new {fakeData = "fake AF"});
            _fakeReports.Add(_fakeReport);
            _fakeReports.Add(new ReportDefinition()
            {
                name = "allActive",
                commonName = "All Active",
                category = "activeRecords",
                description = "All Active Records",
                sqlquery =
                    @"all active"
            });
            
            _tenantServ = new Mock<ITenantService>();
            _reportsServ = new Mock<IReportsV2Service>();
            
            _tenantServ.Setup(s => s.GetCurrentTenant())
                .Returns(new Tenant() {Timezone = FAKETIMEZONE});
            _reportsServ.Setup(s => s.GetList())
                .Returns(_fakeReports);
            _reportsServ.Setup(r => r.Get("fakeReportName"))
                .Returns(new ReportDefinition());
            _reportsServ.Setup(s => s.GetQuery(It.Is<SearchOptions>(so=> so.idOrName == "fakeReportName")))
                .Returns(_fakeDynamicReportResult);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new ReportsController(_reportsServ.Object, _tenantServ.Object, _mapper);
             var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "jciispam@gmail.com"),
                new Claim(ClaimTypes.NameIdentifier, (new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482")).ToString()),
            }, "mock"));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        private DateTime DateTimeLocal(string tz)
        {
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(tz);
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, tzInfo);
        }
        
        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void GetMany_with_existing_returns_all_records_of_type()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<ReportDefinitionVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeReports.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<ReportDefinitionVM>));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void GetMany_with_existing_returns_records_in_data_prop()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            // Assert
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get("fakeReportName", DateTimeLocal(FAKETIMEZONE), DateTimeLocal(FAKETIMEZONE), 0);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_by_existing_Id_returns_correct_item_in_data_obj()
        {
            // Act
            var result = _controller.Get("fakeReportName", DateTimeLocal(FAKETIMEZONE), DateTimeLocal(FAKETIMEZONE), 0).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<List<dynamic>>(result?.Value);
            var singleRecord = data.FirstOrDefault();
            // assert
            Assert.IsInstanceOfType(data, typeof(List<dynamic>));
            Assert.AreEqual(singleRecord?.GetType().GetProperty("fakeData").GetValue(singleRecord, null), "fake AF");
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        #endregion GetOne

        #region Put

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Put_Returns_Not_Found_when_invalid_id_passed()
        {
            // Arrange
            _reportsServ.Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(false);
            // act
            var result =_controller.Put("placeholder", new ReportDefinitionVM()).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Put_Returns_BadRequest_when_invalid_query_passed()
        {
            // Arrange
            List<string> fakeValidationErrors = new List<string>() {"blah"};
            _reportsServ.Setup(s => s.ValidateQuery(It.IsAny<string>()))
                .Returns(fakeValidationErrors);
            _reportsServ.Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);
            // act
            var result =_controller.Put("placeholder", new ReportDefinitionVM()).Result as ObjectResult;
            var resPayload = UnitTestExtensions.ExtractFromDataObject<List<string>>(result.Value);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.IsTrue(fakeValidationErrors.FirstOrDefault() == resPayload.FirstOrDefault());
            Assert.IsTrue(fakeValidationErrors.Count() == resPayload.Count());
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Put_validation_fails_when_empty_query_passed()
        {
            // Arrange
            _reportsServ.Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);
            _controller.ModelState.AddModelError("sqlquery", "Required");
            // act
            var result =_controller.Put("placeholder", new ReportDefinitionVM());
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Put_Returns_OKResult_when_valid_query_passed()
        {
            // Arrange
            _reportsServ.Setup(s => s.ValidateQuery(It.IsAny<string>()))
                .Returns(new List<string>());
            _reportsServ.Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);
            _reportsServ.Setup(s => s.Save(It.IsAny<ReportDefinition>(), It.IsAny<string>()))
                .Verifiable();
            // act
            var result =_controller.Put("placeholder", new ReportDefinitionVM()).Result;
            var resultStatusCode = UnitTestExtensions.ExtractFromUntypedProp<int>(result, "StatusCode");
            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.IsTrue((int)resultStatusCode == (int)HttpStatusCode.OK);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Put_Returns_500_when_serv_throws()
        {
            // Arrange
            _reportsServ.Setup(s => s.ValidateQuery(It.IsAny<string>()))
                .Returns(new List<string>());
            _reportsServ.Setup(s => s.Exists(It.IsAny<string>()))
                .Returns(true);
            _reportsServ.Setup(s => s.Save(It.IsAny<ReportDefinition>(), It.IsAny<string>()))
                .Callback(() => throw new Exception());
            // act
            var result =_controller.Put("placeholder", new ReportDefinitionVM()).Result;
            var resultStatusCode = UnitTestExtensions.ExtractFromUntypedProp<int>(result, "StatusCode");
            var message = UnitTestExtensions.ExtractFromUntypedProp<string>(result, "Value");
            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.IsTrue((int)resultStatusCode == (int)HttpStatusCode.InternalServerError);
            Assert.IsTrue(message == "Exception of type 'System.Exception' was thrown.");
        }

        #endregion Put

        #region GETReportDefinition

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_Definition_by_existing_Id_returns_OkReponse()
        {
            // Arrange
            _reportsServ.Setup(r => r.Exists("fakeReportName"))
                .Returns(true);
            // Act
            var result = _controller.Get("fakeReportName").Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<ReportDefinitionVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(data, typeof(ReportDefinitionVM));
            Assert.AreEqual(data.name, data.name);
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Reports)]
        public void Get_invalid_Definition_returns_NotFound()
        {
            // Arrange
            _reportsServ.Setup(r => r.Exists("fakeReportName"))
                .Returns(false);
            // Act
            var result = _controller.Get("fakeReportName").Result;
            // assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        
        #endregion

    }
}
