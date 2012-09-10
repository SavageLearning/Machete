using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using Machete.Domain;
using Machete.Data;
using System.Linq;
using System.Collections.Generic;
using Machete.Service;
using Machete.Data.Infrastructure;
using System.Data.Entity;

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
            ui.logout();
            ui.activityMenuLink(); //Find Activity menu link and click

            //Assert
            
            // look for activityList
            var activityList = WaitForElement(By.CssSelector("#activityList"));
            Assert.IsNotNull(activityList, "Failed to find #activityList");

            // look for activityTable
            var activityTable = WaitForElement(By.CssSelector("#activityTable"));
            Assert.IsNotNull(activityTable, "Failed to find #activityTable");

        }

        [TestMethod]
        public void SeActivity_unauth_open_record()
        {
            //Arrange

            //Act
            ui.activityMenuLink(); //Find Activity menu link and click

            //Assert
            
            // look for activityList
            var activityList = WaitForElement(By.CssSelector("#activityList"));
            Assert.IsNotNull(activityList, "Failed to find #activityList");

            // look for activityTable
            var activityTable = WaitForElement(By.CssSelector("#activityTable"));
            Assert.IsNotNull(activityTable, "Failed to find #activityTable");

        }

        [TestMethod]
        public void SeActivity_unauth_worker_signin()
        {
            //Arrange
            //Act
            //Assert
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
