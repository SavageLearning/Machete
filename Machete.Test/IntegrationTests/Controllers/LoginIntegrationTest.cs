using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Web.Security;
using Machete.Data.Infrastructure;
using Machete.Data;
using Microsoft.AspNet.Identity;

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
        public void NewMethod()
        {
            var task = userManager.FindAsync("", "");
            var user = task.Wait(20000);
            Assert.IsNotNull(task.Result);
        }

        // This test can't find its SQL Membership stored procedure.
        [TestMethod]
        public void OldMethod()
        {
            var provider = Membership.Provider;
            var result = provider.ValidateUser("", "");
            Assert.IsNotNull(result);
        }
    }
}
