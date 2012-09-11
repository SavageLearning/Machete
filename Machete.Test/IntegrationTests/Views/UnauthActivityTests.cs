using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Machete.Test
{
    [TestClass]
    public class UnauthActivityTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private sharedUI ui;
        private DatabaseFactory _dbfactory;
        private WorkerService _wserv;
        private WorkerRepository _wRepo;
        private IUnitOfWork _unitofwork;
        private MacheteContext DB;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) { }

        [TestInitialize]
        public void SetupTest()
        {
            Database.SetInitializer<MacheteContext>(new MacheteInitializer());
            DB = new MacheteContext("machete");
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            _dbfactory = new DatabaseFactory();
            _wRepo = new WorkerRepository(_dbfactory);
            _unitofwork = new UnitOfWork(_dbfactory);
            _wserv = new WorkerService(_wRepo, _unitofwork);
            driver = new FirefoxDriver();
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL);
            verificationErrors = new StringBuilder();
            //ui.login();
            ui.gotoMachete();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                //ignoring errors if we can't close the browser.
            }
        }

        [ClassCleanup]
        public static void ClassCleanup() { }

        // BRIAN TODO
        // -------------------------------------------------------------------------logout
        // -------------------------------------------------------------------------attempt opening Activities page
        // -------------------------------------------------------------------------   assert that Activities option exists in the menu
        // -------------------------------------------------------------------------   assert that #activityList exists
        // -------------------------------------------------------------------------   assert that #activityTable exists
        // attempt double clicking on activity record
        // attempt signing in with wrong ID
        //    assert: it shows error
        // attempt signing in with legit ID
        //    assert: new record appended
        //    assert: new image pops up
        // check exposed options
        //    assert that .EditPost form doesn't exist on the page
        //    assert that .saveBtn doesn't exist on the page
        //    assert that .deleteButton doesn't exist on the page
        //    assert that .managerAccordion doesn't exist on the page
        //    assert that .confirm_delete in class table doesn't exist

        [TestMethod]
        public void SeActivity_unauth_page_load()
        {
            //Arrange

            //Act
            ui.activityMenuLink(); //Find Activity menu link and click
            //Assert
            //  look for activityList
            var activityList = ui.WaitForElement(By.CssSelector("#activityList"));
            Assert.IsNotNull(activityList, "Failed to find #activityList");

            // look for activityTable
            var activityTable = ui.WaitForElement(By.CssSelector("#activityTable"));
            Assert.IsNotNull(activityTable, "Failed to find #activityTable");

        }

        [TestMethod]
        public void SeActivity_unauth_open_record()
        {
            //Arrange
            // example from EmployerTests.cs -- SeEmployer_Create_and_move_Workorder()
            // var selectedTab = ui.WaitForElement(By.CssSelector("li.employer.ui-tabs-selected a"));
            // var recID = Convert.ToInt32(selectedTab.GetAttribute("recordid"));

            var activityRecord = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            var activityRecordID = Convert.ToInt32(activityRecord.GetAttribute("recordid"));

            //Act
            ui.activityMenuLink(); //Find Activity menu link and click
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));

            //Assert
            
            // look for activityTab to open up
            var activityNewTab = ui.WaitForElement(By.CssSelector("#activity" + activityRecordID + "-EditTab"));
            Assert.IsNotNull(activityNewTab, "Failed to find #activityList");
        }

        [TestMethod]
        public void SeActivity_unauth_worker_signin()
        {
            // example from ActivityTests.cs -- SeActivity_Create_signin_simple()

            // Arrange
            Random rand = new Random();
            int rowcount = 1;
            MacheteContext DB = new MacheteContext("machete");
            var workers = DB.Workers;
            Activity _act = (Activity)Records.activity.Clone();
            ActivitySignin _asi = (ActivitySignin)Records.activitysignin.Clone();
            IEnumerable<int> cardlist = DB.Workers.Select(q => q.dwccardnum).Distinct();
            int firstCardNum = workers.First().dwccardnum;
            
            // Act
            //ui.activityCreate(_act);
            ui.activityMenuLink(); //Find Activity menu link and click
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            ui.activitySignIn(firstCardNum);

            // Assert
            Assert.IsTrue(ui.activitySignInValidate(firstCardNum, rowcount));
        }

        [TestMethod]
        public void SeActivity_unauth_exposed_actions()
        {
            //Arrange
            //Act
            //Assert
        }

    }
}
