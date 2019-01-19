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
            var mapperConfig = new MvcMapperConfiguration().Config;
            var mapper = mapperConfig.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
