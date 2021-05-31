using System;
using System.Collections.Generic;
using Machete.Data.Dynamic;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class ExportsControllerTests
    {
        private Mock<IReportsV2Service> _reportsServ;
        private Mock<ITenantService> _tenantServ;
        private ExportsController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _reportsServ = new Mock<IReportsV2Service>();
            _tenantServ = new Mock<ITenantService>();

            _tenantServ.Setup(s => s.GetCurrentTenant())
                .Returns(UnitTestExtensions.TestingTenant);

            _controller = new ExportsController(_reportsServ.Object, _tenantServ.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [TestMethod]
        public void Reports_controller_get_tables_returns_OkResult()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        
        [TestMethod]
        public void Reports_controller_get_tables_returns_records_in_data_obj()
        {
            // act
            var result = _controller.Get();
            var typedResult = (result as ObjectResult).Value;
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;
            // assert
            Assert.IsTrue(resultHasDataProp);
        }

        // Necessary for mocking methods with a ref
        // https://github.com/moq/moq4/issues/105#issuecomment-334575881
        private delegate void getXlsxFile(Service.DTO.SearchOptions o, ref byte[] bytes);
        
        [TestMethod]
        public void Reports_controller_table_execute_returns_FileContentResult()
        {
            // arrange
            var queryString = "?beginDate=03%2F15%2F2021&endDate=03%2F21%2F2021&filterField=dateEnd&ID=true&name=true&type=true&dateStart=true&dateEnd=true&teacher=true&notes=true&datecreated=true&dateupdated=true&Createdby=true&Updatedby=true&recurring=true&firstID=true&nameEN=true&nameES=true&typeEN=true&typeES=true";
            _reportsServ.Setup(s => s.GetXlsxFile(It.IsAny<Service.DTO.SearchOptions>(), ref It.Ref<byte[]>.IsAny))
                .Callback(new getXlsxFile((Service.DTO.SearchOptions so, ref byte[] b) => b = new byte[20]))
                .Verifiable();
            _controller.Request.QueryString = new QueryString(queryString);
            // act
            var result = _controller.Execute(ValidTableNames.Activities.ToString(), "dateEnd", new DateTime(2021,3,15),new DateTime(2021,3,21));
            // assert
            Assert.IsInstanceOfType(result, typeof(FileContentResult));
        }

        [TestMethod]
        public void ReportsController_Get_table_metadata_returns_Ok()
        {
            // arrange
            _reportsServ.Setup(s => s.GetColumns(ValidTableNames.Activities.ToString()))
                .Returns(new List<QueryMetadata>());
            // act
            var result = _controller.Get(ValidTableNames.Activities.ToString());
            var typedResult = (result as ObjectResult).Value;
            var data = UnitTestExtensions.ExtractFromDataObject<List<QueryMetadata>>(typedResult);
            var resultHasDataProp = data != null;
            // assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(data, typeof(List<QueryMetadata>));
            Assert.IsTrue(resultHasDataProp);
        }
    }
}
