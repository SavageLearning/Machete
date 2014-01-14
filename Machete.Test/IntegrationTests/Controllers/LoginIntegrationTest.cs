using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Web.Security;
using Machete.Data.Infrastructure;
using Machete.Data;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Machete.Test.IntegrationTests.Controllers
{
    [TestClass]
    public class LoginIntegrationTest
    {
        IDatabaseFactory idb;
        string conString;
        MyUserManager userManager;

        [TestInitialize]
        public void Initialize()
        {
            conString = "macheteDevTest";
            idb = new DatabaseFactory(conString);
            userManager = new MyUserManager(idb);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            idb = null;
        }

        // This test inexplicably returns null.
        [TestMethod]
        public async Task FindIdentitySignin()
        {
            var task = await userManager.FindAsync("", "");
            //var user = task.Wait(20000);
            Assert.IsNotNull(task);
        }
    }
}
