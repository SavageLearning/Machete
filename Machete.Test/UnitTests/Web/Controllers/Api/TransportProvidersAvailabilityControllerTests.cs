using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Api.Controllers;
using Machete.Api.Maps;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class TransportProvidersAvailabilityControllerTests
    {
        private Mock<ITransportProvidersAvailabilityService> _transportAvailabilityServ;
        private IMapper _mapper;
        private List<TransportProviderAvailability> _fakeTransportAvailabilities;
        private TransportProviderAvailability _fakeTransportAvailability;
        private TransportProviderAvailability _savedTransportAvailability;
        private TransportProvidersAvailabilityController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _savedTransportAvailability = new TransportProviderAvailability();
            _fakeTransportAvailability = new TransportProviderAvailability()
            {
                available = true,
                day = 1,
                ID = 1,
                key = "fakeTAKey",
                transportProviderID = 1,
                lookupKey = "fakeLUKey"
            };
            _fakeTransportAvailabilities = new List<TransportProviderAvailability>();
            _fakeTransportAvailabilities.Add(_fakeTransportAvailability);
            _fakeTransportAvailabilities.Add(new TransportProviderAvailability()
            {
                available = true,
                day = 4,
                ID = 2,
                key = "fakeTAKey2",
                transportProviderID = 2,
                lookupKey = "fakeLUKey2"
            });

            _transportAvailabilityServ = new Mock<ITransportProvidersAvailabilityService>();
            _transportAvailabilityServ.Setup(s => s.GetAll())
                .Returns(_fakeTransportAvailabilities.AsQueryable);
            _transportAvailabilityServ.Setup(s => s.Get(1))
                .Returns(_fakeTransportAvailability);
            _transportAvailabilityServ.Setup(s => s.Get(1000))
                .Returns((TransportProviderAvailability)null);
            _transportAvailabilityServ.Setup(s => s.Create(It.IsAny<TransportProviderAvailability>(), It.IsAny<string>()))
                .Returns(_fakeTransportAvailability);
            _transportAvailabilityServ.Setup(s => s.Save(It.Is<TransportProviderAvailability>(r => r.ID == 1), It.IsAny<string>()))
                .Callback((TransportProviderAvailability tpa, string user) => _savedTransportAvailability = tpa);
            _transportAvailabilityServ.Setup(s => s.Delete(1, It.IsAny<string>()))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new TransportProvidersAvailabilityController(_transportAvailabilityServ.Object, _mapper);
        }
        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void GetMany_with_existing_returns_all_records_of_type_in_data_object()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<TransportProviderAvailabilityVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeTransportAvailabilities.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<TransportProviderAvailabilityVM>));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void GetMany_service_exception_returns_server_error()
        {
            // arrange
            _transportAvailabilityServ.Setup(s => s.GetAll()).Throws(new Exception());
            // Act
            var result = _controller.Get().Result as ObjectResult;
            // Assert
            Assert.IsTrue(result?.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Get_by_existing_Id_returns_correct_item_in_data_obj()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<TransportProviderAvailabilityVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(data, typeof(TransportProviderAvailabilityVM));
            Assert.AreEqual(_fakeTransportAvailability.ID, data.id);
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new TransportProviderAvailabilityVM();
            _controller.ModelState.AddModelError("day", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new TransportProviderAvailabilityVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Post_valid_data_returns_new_record_in_data_object()
        {
            // arrange
            var validViewModel = new TransportProviderAvailabilityVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<TransportProviderAvailabilityVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(TransportProviderAvailabilityVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion Post
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new TransportProviderAvailabilityVM();
            _controller.ModelState.AddModelError("day", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<TransportProviderAvailabilityVM>(_fakeTransportAvailability);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<TransportProviderAvailabilityVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeTransportAvailability, _savedTransportAvailability);
            Assert.AreEqual(_fakeTransportAvailability.day, _savedTransportAvailability.day);
            Assert.IsInstanceOfType(returnedViewModel, typeof(TransportProviderAvailabilityVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Put_invalid_id_returns_not_found()
        {
            // act
            var result = _controller.Put(1000, new TransportProviderAvailabilityVM());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProvidersAvailabilities)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _transportAvailabilityServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete
    }
}
