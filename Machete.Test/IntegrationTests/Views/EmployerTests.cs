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

namespace Machete.Test
{
    [TestClass]
    public class EmployerTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private sharedUI ui;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) { }

        [TestInitialize]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "http://localhost/";
            ui = new sharedUI(driver, baseURL);
            verificationErrors = new StringBuilder();
            ui.login();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            ////
            //// Loggoff
            //ui.WaitForElement(By.LinkText("Logoff"));
            //driver.FindElement(By.LinkText("Logoff")).Click();
            //try
            //{
            //    driver.Quit();
            //}
            //catch (Exception)
            //{
            //    // Ignore errors if unable to close the browser
            //}
            Assert.AreEqual("", verificationErrors.ToString());
        }
        [ClassCleanup]
        public static void ClassCleanup() { }

        [TestMethod]
        public void SeEmployer_Create_employer()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            ui.employerCreate(_emp);
            ui.employerValidate(_emp);

        }
        [TestMethod]
        public void SeEmployer_Create_workorder()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            WorkOrder _wo = (WorkOrder)Records.order.Clone();
            
            ui.employerCreate(_emp);
            ui.workOrderCreate(_emp, _wo);
            ui.workOrderValidate(_emp, _wo);
        }
    }
}

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion