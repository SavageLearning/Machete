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
using Machete.Test.IntegrationTests.Services;

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

            ui.activityMenuLink(); //Find Activity menu link and click
            var activityRecord = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            var activityRecordID = Convert.ToInt32(activityRecord.GetAttribute("recordid"));
            //Act
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            //Assert            
            // look for activityTab to open up
            var activityNewTab = ui.WaitForElement(By.CssSelector("#activity" + activityRecordID + "-EditTab"));
            var newTabAnchor = ui.WaitForElement(By.CssSelector("#activity" + activityRecordID + "-EditTab a"));
            int anchorID = Convert.ToInt32(newTabAnchor.GetAttribute("recordid"));
            Assert.IsNotNull(activityNewTab, "Failed to find #activityList");
            Assert.AreEqual(activityRecordID, anchorID, "activityRecordID from datatables does not match ID in tab for record");
        }

        [TestMethod]
        public void SeActivity_unauth_maxtabs()
        {
            // Verify maxTabs
            // Open activity table, click List Activities, open another activity, make sure there is only one activity tab open

            //Arrange

            //Act
            ui.activityMenuLink(); //Find Activity menu link and click
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            
            //get current activity tab ID
            var activityTabSelected = ui.WaitForElement(By.CssSelector(".ui-tabs-selected"));
            var activityTabSelectedID = activityTabSelected.GetAttribute("id");

            //go back to List Activities
            //driver.FindElement(By.LinkText("List Activities")).Click();
            ui.WaitThenClickElement(By.Id("activityListTab"));

            // open a different record
            var activityNewRecord = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[2]"));
            int activityNewID = Convert.ToInt32(activityNewRecord.GetAttribute("recordid"));
            var activityNewIDString = "activity" + activityNewID +"-EditTab";
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[2]"));

            //get NEW current activity tab ID
            var activityNewTabSelected = ui.WaitForElement(By.CssSelector(".ui-tabs-selected"));
            var activityNewTabSelectedID = activityNewTabSelected.GetAttribute("id");
            
            //Assert
            // old tab doesn't exist
            Assert.IsNull(ui.WaitForElement(By.Id(activityTabSelectedID)));

            // new tab does exist
            // activityNewIDString;
            Assert.IsNotNull(ui.WaitForElement(By.Id(activityNewIDString)));

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
            var activityRecord = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            var activityRecordID = Convert.ToInt32(activityRecord.GetAttribute("recordid"));
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            ui.activitySignIn("asi" + activityRecordID + "-dwccardnum", firstCardNum);

            // Assert
            // borrowed from sharedUI.cs -- activitySignInValidate()
            Assert.IsNotNull(ui.WaitForElementValue(By.XPath("//table[@id='asi" + activityRecordID + "-asiTable']/tbody/tr[" + rowcount + "]/td[2]"), firstCardNum.ToString()), "Did not find added record number in signin table");

        }

        [TestMethod]
        public void SeActivity_unauth_exposed_actions()
        {
            //Arrange
            ui.activityMenuLink(); //Find Activity menu link and click
            //TODO: Create at least one Activity record that has a start time wwithin 1 hour of the current clock
            //
            //The line below fails because the unauthenticated login now does not have records to show
            // because of the time.
            ActivityServiceTests test = new ActivityServiceTests();
            test.TestInitialize();
            test.Integration_Activity_service_CreateClass_within_hour();
            var activityRecord = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));
            var activityRecordID = activityRecord.GetAttribute("recordid");

            //Act
            // Open activity tab
            ui.WaitAndDoubleClick(By.XPath("//table[@id='activityTable']/tbody/tr[1]"));

            // Look for edit and delete features on the page
            var activityEditForm = ui.WaitForElement(By.CssSelector("#ActivityTab-" + activityRecordID));
            var activityDeleteLink = ui.WaitForElement(By.CssSelector(".confirm_delete"));

            //Assert
            Assert.IsNull(activityEditForm, "Activity Edit form is displaying for unauthorized users");
            Assert.IsNull(activityDeleteLink, "Activity registration table is showing registration delete option to unauthorized users");

        }

    }
}
