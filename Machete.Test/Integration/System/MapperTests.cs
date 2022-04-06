﻿using AutoMapper;
using Machete.Web.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.Integration.System
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mapperConfig = new MapperConfiguration(config => config.ConfigureMvc());
            var mapper = mapperConfig.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Ignore, TestMethod]
        public void AutoMapper_ApiValidation()
        {
            var mapper = new Api.MapperConfig().getMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

        }
    }
}
