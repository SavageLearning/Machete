using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers.Api;
using Machete.Web.Maps.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class ConfigsControllerTests
    {
        private ConfigsController _controller;
        private Mock<IConfigService> _serv;
        private Mock<IConfiguration> _configuration;
        private Mock<IConfigurationSection> _configurationSection;
        private IMapper _map;
        private List<Config> _fakeConfigs = new List<Config>();
        private Config _fakeConfig;
        private Config _savedConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _savedConfig = new Config();
            _fakeConfig = new Config
            {
                ID = 1,
                key = "fakeOrgName",
                value = "Machete"
            };
            _fakeConfigs.Add(_fakeConfig);
            _fakeConfigs.Add(new Config
                {ID = 2, key = "fakeOrgName2", value = "Machete"});

            var mapperConfig = new MapperConfiguration(config => config.ConfigureApi());
            _map = mapperConfig.CreateMapper();
            _serv = new Mock<IConfigService>();
            _configuration = new Mock<IConfiguration>();

            _serv.Setup(s => s.GetMany(It.IsAny<Func<Config, bool>>()))
                .Returns(_fakeConfigs.AsEnumerable);
            _serv.Setup(s => s.Get(1000))
                .Returns((Config) null);
            _serv.Setup(s => s.Get(1))
                .Returns(_fakeConfig);
            _serv.Setup(s => s.Create(It.IsAny<Config>(), It.IsAny<string>()))
                .Returns(_fakeConfig);
            _serv.Setup(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()))
                .Verifiable();
            _serv.Setup(s => s.Save(It.Is<Config>(c => c.key == "fakeOrgName"), It.IsAny<string>()))
                .Callback((Config c, string s) => _savedConfig = c);

            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Authentication:Facebook:AppId")])
                .Returns("mock FacebookAppId");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Authentication:Google:ClientId")])
                .Returns("mock GoogleClientId");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Authentication:State")])
                .Returns("mock AuthState");

            _controller = new ConfigsController(_serv.Object, _configuration.Object, _map);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void GetMany_with_existing_configs_returns_all_records_of_type()
        {
            // Act
            var result = _controller.Get();
            var typedResult = (result.Result as ObjectResult).Value;
            var configVMList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<ConfigVM>>(typedResult);

            // Assert
            Assert.IsTrue(configVMList.Count() == 5);
            Assert.IsInstanceOfType(configVMList, typeof(IEnumerable<ConfigVM>));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void GetMany_with_existing_configs_returns_records_in_data_prop()
        {
            // Act
            var result = _controller.Get();
            var typedResult = (result.Result as ObjectResult).Value;
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;

            // Assert
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion GetMany

        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Get_by_ID_unknown_config_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Get_by_Id_existing_returns_correct_item()
        {
            // Act
            var result = _controller.Get(1);
            var typedResult = (result.Result as ObjectResult).Value;
            var configVM = UnitTestExtensions.ExtractFromDataObject<ConfigVM>(typedResult);
            // assert
            Assert.IsInstanceOfType(configVM, typeof(ConfigVM));
            Assert.AreEqual(_fakeConfig.ID, configVM.id);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Get_by_Id_pass_existing_id_returns_record_in_data_object()
        {
            // Act
            var result = _controller.Get(1);
            var typedResult = (result.Result as ObjectResult).Value;
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;

            // Assert
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion

        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidConfig = new ConfigVM() {key = "fake key"};
            _controller.ModelState.AddModelError("value", "Required");
            // Act
            var result = _controller.Post(invalidConfig);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validConfig = new ConfigVM() {key = "fakeOrgName", value = "Machete"};
            // act
            var result = _controller.Post(validConfig);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Post_valid_data_returns_new_record_in_data_oject()
        {
            // arrange
            var validConfig = new ConfigVM() {key = "fakeOrgName", value = "Machete"};
            // act
            var result = _controller.Post(validConfig);
            var typedResult = (result.Result as ObjectResult).Value;
            var configVM = UnitTestExtensions.ExtractFromDataObject<ConfigVM>(typedResult);
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;
            //assert
            Assert.IsInstanceOfType(configVM, typeof(ConfigVM));
            Assert.IsTrue(resultHasDataProp);
            Assert.AreEqual("fakeOrgName", configVM.key);
        }

        #endregion Post        
        
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidConfig = new ConfigVM() {id = 3, key = "fake key"};
            _controller.ModelState.AddModelError("value", "Required");
            // Act
            var result = _controller.Put(invalidConfig.id, invalidConfig);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validConfig = _map.Map<ConfigVM>(_fakeConfig);
            validConfig.description = "fake desc.";
            // act
            var result = _controller.Put(1, validConfig);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeConfig, _savedConfig);
            Assert.AreEqual("fakeOrgName", _savedConfig.key);
            Assert.AreEqual("Machete", _savedConfig.value);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Put_invalid_id_returns_not_found()
        {
            // arrange
            // act
            var deleteResult = _controller.Put(1000,  _map.Map<ConfigVM>(_fakeConfig));
            // assert
            Assert.IsInstanceOfType(deleteResult.Result, typeof(NotFoundResult));
        }

        #endregion PUT

        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // arrange
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Configs)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _serv.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete
    }
}
