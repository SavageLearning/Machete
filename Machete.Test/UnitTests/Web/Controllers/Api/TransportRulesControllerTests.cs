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
    public class TransportRulesControllerTests
    {
        private Mock<ITransportRuleService> _transportRuleServ;
        private Mock<ITransportCostRuleService> _transportCostServ;
        private IMapper _mapper;
        private List<TransportRule> _fakeTransportRules;
        private TransportRule _fakeTransportRule;
        private TransportRule _savedTransportRule;
        private TransportRulesController _controller;
        private TransportCostRule _fakeTransportCostRule;
        private List<TransportCostRule> _fakeTransportCostRules;

        [TestInitialize]
        public void TestInitialize()
        {
            _savedTransportRule = new TransportRule();
            _fakeTransportRule = new TransportRule()
            {
                ID = 1,
                key = "sdf",
                zoneLabel = "adsfwer",
                lookupKey = "fakeLookupKey"
            };
            _fakeTransportRules = new List<TransportRule>
            {
                _fakeTransportRule,
                new TransportRule() {ID = 2, key = "ghj", zoneLabel = "fghjrtyu", lookupKey = "fakeLookupKey2"}
            };

            _transportRuleServ = new Mock<ITransportRuleService>();
            _transportRuleServ.Setup(s => s.GetAll())
                .Returns(_fakeTransportRules.AsQueryable);
            _transportRuleServ.Setup(s => s.Get(1))
                .Returns(_fakeTransportRule);
            _transportRuleServ.Setup(s => s.Get(1000))
                .Returns((TransportRule)null);
            _transportRuleServ.Setup(s => s.Create(It.IsAny<TransportRule>(), It.IsAny<string>()))
                .Returns(_fakeTransportRule);
            _transportRuleServ.Setup(s => s.Save(It.Is<TransportRule>(r => r.ID == 1), It.IsAny<string>()))
                .Callback((TransportRule tr, string user) => _savedTransportRule = tr);
            _transportRuleServ.Setup(s => s.Delete(1, It.IsAny<string>()))
                .Verifiable();

            _fakeTransportCostRule = new TransportCostRule()
            {
                ID = 1,
                cost = 10,
                maxWorker = 10,
                minWorker = 1,
                transportRuleID = 1
            };
            _fakeTransportCostRules = new List<TransportCostRule>
            {
                _fakeTransportCostRule,
                new TransportCostRule
                {
                    ID = 2,
                    cost = 10,
                    maxWorker = 10,
                    minWorker = 1,
                    transportRuleID = 2
                }
            };

            _transportCostServ = new Mock<ITransportCostRuleService>();
            _transportCostServ.Setup(s => s.GetAll())
                .Returns(_fakeTransportCostRules.AsQueryable);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new TransportRulesController(_transportRuleServ.Object, _transportCostServ.Object, _mapper);
        }

        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void GetMany_with_existing_returns_all_records_of_type_in_data_object()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<TransportRuleVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeTransportRules.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<TransportRuleVM>));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void GetMany_service_exception_returns_server_error()
        {
            // arrange
            _transportRuleServ.Setup(s => s.GetAll()).Throws(new Exception());
            // Act
            var result = _controller.Get().Result as ObjectResult;
            // Assert
            Assert.IsTrue(result?.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Get_by_existing_Id_returns_correct_item_in_data_obj()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<TransportRuleVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(data, typeof(TransportRuleVM));
            Assert.AreEqual(_fakeTransportRule.ID, data.id);
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new TransportRuleVM();
            _controller.ModelState.AddModelError("key", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new TransportRuleVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Post_valid_data_returns_new_record_in_data_object()
        {
            // arrange
            var validViewModel = new TransportRuleVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<TransportRuleVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(TransportRuleVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion Post
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new TransportRuleVM();
            _controller.ModelState.AddModelError("key", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<TransportRuleVM>(_fakeTransportRule);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<TransportRuleVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeTransportRule, _savedTransportRule);
            Assert.AreEqual(_fakeTransportRule.ID, _savedTransportRule.ID);
            Assert.AreEqual(_fakeTransportRule.key, _savedTransportRule.key);
            Assert.AreEqual(_fakeTransportRule.lookupKey, _savedTransportRule.lookupKey);
            Assert.IsInstanceOfType(returnedViewModel, typeof(TransportRuleVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Put_invalid_id_returns_not_found()
        {
            // act
            var result = _controller.Put(1000, new TransportRuleVM());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.TransportRules)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _transportRuleServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete
    }
}
