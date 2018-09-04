using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test
{
    [TestClass]
    public class MapperTests
    {
        [Ignore, TestMethod]
        public void TestMethod1()
        {
            var mapper = new Machete.Web.MapperConfig().getMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

        }
    }
}
