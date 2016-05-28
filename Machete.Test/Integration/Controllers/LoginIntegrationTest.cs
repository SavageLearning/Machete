using Machete.Data.Infrastructure;
using Machete.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Machete.Test.Integration.Controller
{
    [TestClass]
    public class LoginIntegrationTest
    {
        IDatabaseFactory idb;
        string conString;
        Machete.Web.Models.MyUserManager userManager;

        [TestInitialize]
        public void Initialize()
        {
            conString = "MacheteConnection";
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
            //passes with default values 4/3/2014 11:42 am
            var task = await userManager.FindAsync("jadmin", "ChangeMe");
            //var user = task.Wait(20000);
            Assert.IsNotNull(task);
        }
    }
}
