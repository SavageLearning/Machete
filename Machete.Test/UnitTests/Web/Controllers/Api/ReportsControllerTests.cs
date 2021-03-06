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
            _reportsServ.Setup(s => s.GetQuery(It.Is<SearchOptions>(so=> so.idOrName == "fakeReportName")))
                .Returns(_fakeDynamicReportResult);
            
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new ReportsController(_reportsServ.Object, _tenantServ.Object, _mapper);
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

    }
}
