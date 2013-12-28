using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Security;

namespace Machete.Test.IntegrationTests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void OldMethod()
        {
            var provider = Membership.Provider;
            var result = provider.ValidateUser("jadmin", "");
            Assert.IsNotNull(result);
        }
    }
}
