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
    public class LookupsControllerTests
    {
        private Mock<ILookupService> _lookupServ;
        private IMapper _mapper;
        private LookupsController _controller;
        private List<Lookup> _fakeLookups;
        private Lookup _fakeLookup;
        private Lookup _savedLookup;
        [TestInitialize]
        public void TestInitialize()
        {
            _savedLookup = new Lookup();
            _fakeLookup = new Lookup
            {
                category = "test category",
                text_EN = "Test EN",
                text_ES = "Test ES",
                createdby = "initialization script",
                updatedby = "initialization script",
                key = "blah"
            };
            _fakeLookups = new List<Lookup>();
            _fakeLookups.Add(_fakeLookup);
            _fakeLookups.Add(new Lookup()
            {
                active = true,
                category = LCategory.orderstatus,
                ID = 2,
                text_EN = "pendiente",
                text_ES = "pending"
            });
            
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _lookupServ = new Mock<ILookupService>();
            _lookupServ.Setup(s => s.GetMany(It.IsAny<Func<Lookup, bool>>()))
                .Returns(_fakeLookups);
            _lookupServ.Setup(s => s.Get(1))
                .Returns(_fakeLookup);
            _lookupServ.Setup(s => s.Get(1000))
                .Returns((Lookup) null);
            _lookupServ.Setup(s => s.Create(It.IsAny<Lookup>(), It.IsAny<string>()))
                .Returns(_fakeLookup);
            _lookupServ.Setup(s => s.Save(It.Is<Lookup>(l => l.key == "blah"), It.IsAny<string>()))
                .Callback((Lookup l, string s) => _savedLookup = l);

            _controller = new LookupsController(_lookupServ.Object, _mapper);
        }
        
        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void GetMany_with_existing_returns_all_records_of_type()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<LookupVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeLookups.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<LookupVM>));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void GetMany_with_existing_returns_records_in_data_prop()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var resultHasDataProp = result?.Value.GetType().GetProperty("data") != null;
            // Assert
            Assert.IsTrue(resultHasDataProp);
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void GetMany_with_service_throw_returns_server_error()
        {
            // arrange
            _lookupServ.Setup(s => s.GetMany(It.IsAny<Func<Lookup, bool>>()))
                .Throws(new Exception());
            // Act
            var result = _controller.Get().Result as ObjectResult;
            // Assert
            Assert.IsTrue(result?.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Get_by_existing_Id_returns_correct_item()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<LookupVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(data, typeof(LookupVM));
            Assert.AreEqual(_fakeLookup.ID, data.id);
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Get_by_Id_pass_existing_id_returns_record_in_data_object()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var resultHasDataProp = result?.Value.GetType().GetProperty("data") != null;
            // Assert
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new LookupVM();
            _controller.ModelState.AddModelError("value", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new LookupVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Post_valid_data_returns_new_record_in_data_oject()
        {
            // arrange
            var validViewModel = new LookupVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<LookupVM>(result?.Value);
            var resultHasDataProp = result?.Value.GetType().GetProperty("data") != null;
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(LookupVM));
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion Post 
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new LookupVM();
            _controller.ModelState.AddModelError("value", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<LookupVM>(_fakeLookup);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel);
            var parsedObject = (result.Result as ObjectResult)?.Value;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<LookupVM>(parsedObject);
            var resultHasDataProp = parsedObject?.GetType().GetProperty("data") != null;
            //assert
            Assert.IsInstanceOfType(result?.Result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeLookup, _savedLookup);
            Assert.AreEqual("blah", _savedLookup.key);
            Assert.IsInstanceOfType(returnedViewModel, typeof(LookupVM));
            Assert.IsTrue(resultHasDataProp);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Put_invalid_id_returns_not_found()
        {
            // act
            var result = _controller.Put(1000,  new LookupVM());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // arrange
            _lookupServ.Setup(s => s.Delete(1000, It.IsAny<string>()))
                .Verifiable();
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Lookups)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _lookupServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete

    }
}
