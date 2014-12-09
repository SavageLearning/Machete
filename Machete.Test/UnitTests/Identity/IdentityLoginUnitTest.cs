using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Web.Security;

namespace Machete.Test.UnitTests.Identity
{
    [TestClass]
    public class IdentityLoginUnitTest
    {
        [TestMethod]
        public void NewMethod()
        {
            var userManager = new MyUserManager();
            var task = userManager.FindAsync("jadmin", "");
            var user = task.Wait(20000);
            Assert.IsNotNull(task.Result);
        }

        [TestMethod]
        public void OldMethod()
        {
            var provider = Membership.Provider;
            var result = provider.ValidateUser("jadmin", "");
            Assert.IsNotNull(result);
        }
    }
}
