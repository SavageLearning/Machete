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
    public class PersonTtests
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
        public void SePerson_create_person()
        {
            Person _per = (Person)Records.person.Clone();
            ui.personCreate(_per);
        }

        [TestMethod]
        public void SePerson_create_worker()
        {
            Person _per = (Person)Records.person.Clone();
            ui.personCreate(_per);
            ui.workerCreate(_per);
        }

        [TestMethod]
        public void Se_Person_Change_Zip()
        {
            //driver.Navigate().GoToUrl(baseURL);

            ui.WaitThenClickElement(By.Id("menulinkperson"));
            //driver.FindElement(By.Id("menulinkperson")).Click();
            //
            // Persons Index page
            ui.WaitForElement(By.Id("personSearchBox"));
            driver.FindElement(By.Id("personSearchBox")).Clear();
            driver.FindElement(By.Id("personSearchBox")).SendKeys("carter");
            //
            // Datatables javascript update
            ui.WaitForElementValue(By.XPath("//table[@id='personTable']/tbody/tr/td[2]"), "Jimmy");
            IWebElement rowrecord = driver.FindElement(By.XPath("//table[@id='personTable']/tbody/tr/td[2]"));
            Actions actionProvider = new Actions(driver);
            IAction doubleClick = actionProvider.DoubleClick(rowrecord).Build();
            doubleClick.Perform();
            //
            // Persons edit record
            ui.WaitForElement(By.Id("zipcode"));
            driver.FindElement(By.Id("zipcode")).Clear();
            driver.FindElement(By.Id("zipcode")).SendKeys("23456");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            //Assert.IsTrue(false);

        }
        //
        //

    }
}
