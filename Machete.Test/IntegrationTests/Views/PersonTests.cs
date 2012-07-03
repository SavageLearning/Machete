using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Machete.Domain;
using System.Reflection;

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

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            string solutionDirectory = sharedUI.SolutionDirectory();
            testdir = solutionDirectory + "\\Machete.test\\";
            testimagefile = testdir + "jimmy_machete.jpg";
        }

        [TestInitialize]
        public void SetupTest()
        {
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
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        [ClassCleanup]
        public static void ClassCleanup() { }

        [TestMethod]
        public void SePerson_create_person()
        {
            Person _per = (Person)Records.person.Clone();
            ui.personCreate(_per);
        }

        [TestMethod]
        public void SePerson_create_worker()
        {
            Person _per = (Person)Records.person.Clone();
            Worker _wkr = (Worker)Records.worker.Clone();
            ui.personCreate(_per);
            _wkr.ID = _per.ID;
            ui.workerCreate(_wkr, testimagefile);
        }

    }
}
