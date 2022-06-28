using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Machete.Service.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Api.Controllers;
using Machete.Api.Maps;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class OnlineOrdersControllerTests
    {
        private Mock<IOnlineOrdersService> _onlineOrdersServ;
        private Mock<ITenantService> _tenantServ;
        private Mock<IConfigService> _configServ;
        private IMapper _mapper;
        private OnlineOrdersController _controller;
        private List<WorkOrdersList> _fakeWorOrderDTOList;
        private WorkOrdersList _fakeWorkOrderDTO;
        private WorkOrder _fakeWorkOrder;
        private Employer _fakeEmployer;
        [TestInitialize]
        public void TestInitialize()
        {
            _fakeWorkOrder = new WorkOrder()
            {
                EmployerID = 1
            };
            _fakeEmployer = new Employer()
            {
                ID = 1,
                name = "Jimmy Hendrix",
                address1 = "123 Yellow Sub",
                phone = "123-123-1234",
                city = "seattle",
                state = "wa",
                email = "jciispam@gmail.com"
            };

            // Only used in get many
            _fakeWorkOrderDTO = new Service.DTO.WorkOrdersList()
            {
                ID = 1,
                dateTimeofWork = DateTime.Now.Add(TimeSpan.FromDays(3)),
            };
            _fakeWorOrderDTOList = new List<Service.DTO.WorkOrdersList>();
            _fakeWorOrderDTOList.Add(_fakeWorkOrderDTO);
            _fakeWorOrderDTOList.Add(new Service.DTO.WorkOrdersList
            {
                ID = 2,
                dateTimeofWork = DateTime.Now.Add(TimeSpan.FromDays(2))
            });

            _onlineOrdersServ = new Mock<IOnlineOrdersService>();
            _tenantServ = new Mock<ITenantService>();
            _configServ = new Mock<IConfigService>();

            _tenantServ.Setup(s => s.GetCurrentTenant())
                .Returns(new Tenant() { Timezone = "America/Los_Angeles" });
            _configServ.Setup(s => s.getConfig(Cfg.PaypalId))
                .Returns("adsfadfadf145345");
            _configServ.Setup(s => s.getConfig(Cfg.PaypalSecret))
                .Returns("asdf235@#%");
            _configServ.Setup(s => s.getConfig(Cfg.PaypalUrl))
                .Returns("https://test.com/test?test=test");
            _onlineOrdersServ.Setup(s => s.GetIndexView(It.IsAny<viewOptions>()))
                .Returns(new dataTableResult<WorkOrdersList>() { query = _fakeWorOrderDTOList.AsEnumerable() });
            _onlineOrdersServ.Setup(s => s.Get(1))
                .Returns(_fakeWorkOrder);
            _onlineOrdersServ.Setup(s => s.Get(1000))
                .Returns((WorkOrder)null);
            _onlineOrdersServ.Setup(s => s.Create(It.IsAny<WorkOrder>(), It.IsAny<string>()))
                .Returns(_fakeWorkOrder);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new OnlineOrdersController(
                _onlineOrdersServ.Object,
                _tenantServ.Object,
                _mapper,
                _configServ.Object);
        }
        private void ArrangeClaimsPrincipalAndExistingEmployer()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "jciispam@gmail.com"),
                new Claim(ClaimTypes.NameIdentifier, (new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482")).ToString()),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _onlineOrdersServ.Setup(s => s.GetEmployer("9245fe4a-d402-451c-b9ed-9c1a04247482"))
                .Returns(_fakeEmployer);
        }

        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.OnlineOrders)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.OnlineOrders)]
        public void GetMany_with_existing_returns_all_records_of_type()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var resultHasDataProp = result?.Value.GetType().GetProperty("data") != null;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<WorkOrderVM>>(result?.Value);
            // Assert
            Assert.IsTrue(resultHasDataProp);
            Assert.IsTrue(viewModelList.Count() == _fakeWorOrderDTOList.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<WorkOrderVM>));
        }

        #endregion GetMany
        #region GetOne
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.OnlineOrders)]
        [ExpectedException(typeof(Exception))]
        public void Get_by_unknown_ID_throws_exception()
        {
            // arrange
            ArrangeClaimsPrincipalAndExistingEmployer();
            // Act
            var result = _controller.Get(1000);
            // assert
            // Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.OnlineOrders)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // arrange
            ArrangeClaimsPrincipalAndExistingEmployer();
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.OnlineOrders)]
        public void Get_by_existing_Id_returns_correct_item()
        {
            // Arrange
            ArrangeClaimsPrincipalAndExistingEmployer();
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<WorkOrderVM>(result?.Value);
            var resultHasDataProp = result?.Value.GetType().GetProperty("data") != null;
            // assert
            Assert.IsTrue(resultHasDataProp);
            Assert.IsInstanceOfType(data, typeof(WorkOrderVM));
            Assert.AreEqual(_fakeWorkOrder.ID, data.id);
        }

        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            ArrangeClaimsPrincipalAndExistingEmployer();
            var invalidRecord = new WorkOrderVM();
            _controller.ModelState.AddModelError("name", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            ArrangeClaimsPrincipalAndExistingEmployer();
            var validViewModel = new WorkOrderVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Post_valid_data_returns_new_record_in_data_oject()
        {
            // arrange
            ArrangeClaimsPrincipalAndExistingEmployer();
            var validViewModel = new WorkOrderVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<WorkOrderVM>(result?.Value);
            var resultHasDataProp = result?.Value.GetType().GetProperty("data") != null;
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(WorkOrderVM));
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion Post
    }
}
