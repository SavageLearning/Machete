using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Machete.Domain;
using System.Reflection;
using Machete.Data;
using System.Data.Entity;
using Machete.Service;

namespace Machete.Test
{
    [TestClass]
    public class PersonTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private sharedUI ui;
        private static string testdir;
        private static string testimagefile;
        private MacheteContext DB;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            string solutionDirectory = sharedUI.SolutionDirectory();
            testdir = solutionDirectory + "\\Machete.test\\";
            testimagefile = testdir + "jimmy_machete.jpg";
        }

        [TestInitialize]
        public void SetupTest()
        {
            Database.SetInitializer<MacheteContext>(new MacheteInitializer());
            DB = new MacheteContext();
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            driver = new FirefoxDriver();
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL);
            verificationErrors = new StringBuilder();
            ui.login();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            ////
            //// Loggoff
            ui.WaitForElement(By.LinkText("Logoff"));
            driver.FindElement(By.LinkText("Logoff")).Click();
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser/.ujhkmn,.
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        [ClassCleanup]
        public static void ClassCleanup() { }

        [TestMethod]
        public void SePerson_create_person()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            //Act
            ui.personCreate(_per);
            //Assert
            ui.personValidate(_per);
        }

        [TestMethod]
        public void SePerson_create_worker()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Worker _wkr = (Worker)Records.worker.Clone();

            //Act
            ui.personCreate(_per);
            _wkr.ID = _per.ID;
            ui.workerCreate(_wkr, testimagefile);

            //Assert
            ui.workerValidate(_wkr);
        }

        [TestMethod]
        public void SePerson_create_event()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Event _ev = (Event)Records.event1.Clone();
            _ev.Person = _per;

            //Act
            ui.personCreate(_per);
            _ev.PersonID = _per.ID;
            ui.eventCreate(_ev);

            //Assert
            ui.eventValidate(_ev);
        }

        [TestMethod]
        public void SePerson_signin_sanctioned_worker_fails()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Worker _wkr = (Worker)Records.worker.Clone();
            _wkr.dwccardnum = sharedUI.nextAvailableDwccardnum(DB);
            Event _san = (Event)Records.event1.Clone();
            _san.Person = _per;
            _san.eventType = MacheteLookup.cache.First(x => x.category == "eventtype" && x.text_EN == "Sanction").ID;
            Activity _act = (Activity)Records.activity.Clone();

            //Act
            ui.personCreate(_per);
            _wkr.ID = _per.ID;
            ui.workerCreate(_wkr, testimagefile);
            //
            ui.workerSanction(_wkr);
            //
            _san.PersonID = _per.ID;
            ui.eventCreate(_san);
            ui.activityCreate(_act);
            // refactored other test to identify idPrefix-dwccardnum
            ui.activitySignIn(_act.idChild,_wkr.dwccardnum);
            //Assert
            Assert.IsTrue(ui.activitySignInIsSanctioned());
        }
    }
}
