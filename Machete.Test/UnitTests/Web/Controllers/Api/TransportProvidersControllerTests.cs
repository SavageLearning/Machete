using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
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
    public class TransportProvidersControllerTests
    {
        private Mock<ITransportProvidersService> _transportProvidersServ ;
        private IMapper _mapper;
        private List<TransportProvider> _fakeTransportProviders;
        private TransportProvider _fakeTransportProvider;
        private TransportProvider _savedTransportProvider;
        private TransportProvidersController _controller;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _savedTransportProvider = new TransportProvider();
            _fakeTransportProvider = new TransportProvider()
            {
                active = true,
                defaultAttribute = true,
                ID = 1,
                key = "asdf",
                text_EN = "asdf"
            };
            _fakeTransportProviders = new List<TransportProvider>();
            _fakeTransportProviders.Add(new TransportProvider()            {
                active = true,
                defaultAttribute = true,
                ID = 2,
                key = "dth",
                text_EN = "wert"
            });

            _transportProvidersServ  = new Mock<ITransportProvidersService>();
            _transportProvidersServ .Setup(s => s.GetMany(It.IsAny<Func<TransportProvider,bool>>()))
                .Returns(_fakeTransportProviders);
            _transportProvidersServ .Setup(s => s.Get(1))
                .Returns(_fakeTransportProvider);            
            _transportProvidersServ .Setup(s => s.Get(1000))
                .Returns((TransportProvider)null);
            _transportProvidersServ .Setup(s => s.Create(It.IsAny<TransportProvider>(), It.IsAny<string>()))
                .Returns(_fakeTransportProvider);
            _transportProvidersServ .Setup(s => s.Save(It.Is<TransportProvider>(r => r.ID == 1), It.IsAny<string>()))
                .Callback((TransportProvider tp, string user) => _savedTransportProvider = tp);
            _transportProvidersServ .Setup(s => s.Delete(1, It.IsAny<string>()))
                .Verifiable();
            
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new TransportProvidersController(_transportProvidersServ.Object, _mapper);
        }
        
        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void GetMany_with_existing_returns_all_records_of_type_in_data_object()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<TransportProviderVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeTransportProviders.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<TransportProviderVM>));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void GetMany_service_exception_returns_server_error()
        {
            // arrange
            _transportProvidersServ.Setup(s => s.GetMany(It.IsAny<Func<TransportProvider,bool>>())).Throws(new Exception());
            // Act
            var result = _controller.Get().Result as ObjectResult;
            // Assert
            Assert.IsTrue(result?.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Get_by_existing_Id_returns_correct_item_in_data_obj()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<TransportProviderVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(data, typeof(TransportProviderVM));
            Assert.AreEqual(_fakeTransportProvider.ID, data.id);
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new TransportProviderVM();
            _controller.ModelState.AddModelError("day", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new TransportProviderVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Post_valid_data_returns_new_record_in_data_object()
        {
            // arrange
            var validViewModel = new TransportProviderVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<TransportProviderVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(TransportProviderVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion Post 
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new TransportProviderVM();
            _controller.ModelState.AddModelError("day", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<TransportProviderVM>(_fakeTransportProvider);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<TransportProviderVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeTransportProvider, _savedTransportProvider);
            Assert.AreEqual(_fakeTransportProvider.ID, _savedTransportProvider.ID);
            Assert.AreEqual(_fakeTransportProvider.key, _savedTransportProvider.key);
            Assert.AreEqual(_fakeTransportProvider.text_EN, _savedTransportProvider.text_EN);
            Assert.IsInstanceOfType(returnedViewModel, typeof(TransportProviderVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Put_invalid_id_returns_not_found()
        {
            // act
            var result = _controller.Put(1000,  new TransportProviderVM());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportProviders)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _transportProvidersServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete
    }
}
