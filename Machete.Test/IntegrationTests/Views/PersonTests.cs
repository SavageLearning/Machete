using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace Machete.Test
{
    [TestClass]
    public class PersonTtests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        public WebServer webServer;

        [TestInitialize]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "http://localhost/";
            verificationErrors = new StringBuilder();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            try
            {
                webServer.Stop();
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestMethod]
        public void TheWebdriverTest()
        {
            driver.Navigate().GoToUrl("http://localhost/");
            //
            // Front page, logon
            driver.FindElement(By.LinkText("Logon")).Click();
            WaitForText("Account Information", driver, 60);
            driver.FindElement(By.Id("UserName")).Clear();
            driver.FindElement(By.Id("UserName")).SendKeys("jadmin");
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("machete");
            driver.FindElement(By.Name("logonB")).Click();
            //
            // Front page, logged in
            WaitForText("Welcome", driver, 60);
            WaitForElement(By.Id("menulinkperson"));
            driver.FindElement(By.Id("menulinkperson")).Click();
            //
            // Persons Index page
            WaitForElement(By.Id("personSearchBox"));
            driver.FindElement(By.Id("personSearchBox")).Clear();
            driver.FindElement(By.Id("personSearchBox")).SendKeys("carter");
            //
            // Datatables javascript update
            WaitForElementValue(By.XPath("//table[@id='personTable']/tbody/tr/td[2]"), "Jimmy");
            IWebElement rowrecord = driver.FindElement(By.XPath("//table[@id='personTable']/tbody/tr/td[2]"));
            Actions actionProvider = new Actions(driver);
            IAction doubleClick = actionProvider.DoubleClick(rowrecord).Build();
            doubleClick.Perform();
            //
            // Persons edit record
            WaitForElement(By.Id("zipcode"));
            driver.FindElement(By.Id("zipcode")).Clear();
            driver.FindElement(By.Id("zipcode")).SendKeys("23456");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            //
            // Loggoff
            WaitForElement(By.LinkText("Logoff"));
            driver.FindElement(By.LinkText("Logoff")).Click();
        }
        //
        //
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        //
        //
        private bool IsElementValuePresent(By by, string value)
        {
            try
            {
                IWebElement elem = driver.FindElement(by);
                if (elem.Text == value) return true;
                else return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        //
        //
        private bool WaitForElement(By by)
        {
            for (int second = 0; second < 60; second++)
            {
                try
                {
                    if (IsElementPresent(by)) return true;
                }
                catch (Exception)
                { return false; }
                Thread.Sleep(1000);
            }
            return false;
        }
        //
        //
        private bool WaitForElementValue(By by, string value)
        {
            for (int second = 0; second < 60; second++)
            {
                try
                {
                    if (IsElementValuePresent(by, value))
                    {
                        return true;
                    }

                }
                catch (Exception)
                {
                    return false;
                }
                Thread.Sleep(1000);
            }
            return false;
        }

        private static bool isTextPresent(String what, IWebDriver driver)
        {
            try
            {
                driver.FindElement(By.XPath("//*[contains(.,'" + what + "')]"));
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }
        private static bool WaitForText(String what, IWebDriver driver, int waitfor)
        {
            for (int second = 0; second < waitfor; second++)
            {
                try
                {
                    if (isTextPresent(what, driver)) return true;
                }
                catch (Exception)
                { return false; }
                Thread.Sleep(1000);
            }
            return false;
        }
    }
}
