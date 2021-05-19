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
    public class ScheduleRulesControllerTests
    {
        private Mock<IScheduleRuleService> _scheduleRuleServ;
        private IMapper _mapper;
        private List<ScheduleRule> _fakeScheduleRules;
        private ScheduleRule _fakeScheduleRule;
        private ScheduleRule _savedScheduleRule;
        private ScheduleRulesController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _savedScheduleRule = new ScheduleRule();
            _fakeScheduleRule = new ScheduleRule()
            {
                day = 1,
                leadHours = 48,
                maxEndMin = 30,
                minStartMin = 1,
                ID = 1
            };
            _fakeScheduleRules = new List<ScheduleRule>();
            _fakeScheduleRules.Add(_fakeScheduleRule);
            _fakeScheduleRules.Add(new ScheduleRule()
            {
                day = 1,
                leadHours = 45,
                maxEndMin = 20,
                minStartMin = 2,
                ID = 2
            });
            
            _scheduleRuleServ = new Mock<IScheduleRuleService>();
            _scheduleRuleServ.Setup(s => s.GetAll())
                .Returns(_fakeScheduleRules);
            _scheduleRuleServ.Setup(s => s.Get(1000))
                .Returns((ScheduleRule) null);
            _scheduleRuleServ.Setup(s => s.Get(1))
                .Returns(_fakeScheduleRule);
            _scheduleRuleServ.Setup(s => s.Create(It.IsAny<ScheduleRule>(), It.IsAny<string>()))
                .Returns(_fakeScheduleRule);
            _scheduleRuleServ.Setup(s => s.Save(It.Is<ScheduleRule>(r => r.ID == 1), It.IsAny<string>()))
                .Callback((ScheduleRule sr, string user) => _savedScheduleRule = sr);
            _scheduleRuleServ.Setup(s => s.Delete(1, It.IsAny<string>()))
                .Verifiable();
            _scheduleRuleServ.Setup(s => s.Delete(1000, It.IsAny<string>()))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new ScheduleRulesController(_scheduleRuleServ.Object, _mapper);
        }   
        
        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void GetMany_with_existing_returns_all_records_of_type_in_data_object()
        {
            // Act
            var result = _controller.Get().Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<ScheduleRuleVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeScheduleRules.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<ScheduleRuleVM>));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void GetMany_service_exception_returns_server_error()
        {
            // arrange
            _scheduleRuleServ.Setup(s => s.GetAll()).Throws(new Exception());
            // Act
            var result = _controller.Get().Result as ObjectResult;
            // Assert
            Assert.IsTrue(result?.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Get_by_existing_Id_returns_correct_item_in_data_obj()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<ScheduleRuleVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(data, typeof(ScheduleRuleVM));
            Assert.AreEqual(_fakeScheduleRule.ID, data.id);
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new ScheduleRuleVM();
            _controller.ModelState.AddModelError("day", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new ScheduleRuleVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Post_valid_data_returns_new_record_in_data_object()
        {
            // arrange
            var validViewModel = new ScheduleRuleVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<ScheduleRuleVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(ScheduleRuleVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion Post 
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new ScheduleRuleVM();
            _controller.ModelState.AddModelError("day", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<ScheduleRuleVM>(_fakeScheduleRule);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<ScheduleRuleVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeScheduleRule, _savedScheduleRule);
            Assert.AreEqual(_fakeScheduleRule.day, _savedScheduleRule.day);
            Assert.IsInstanceOfType(returnedViewModel, typeof(ScheduleRuleVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Put_invalid_id_returns_not_found()
        {
            // act
            var result = _controller.Put(1000,  new ScheduleRuleVM());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.ScheduleRules)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _scheduleRuleServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete
    }
}
