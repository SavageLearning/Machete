using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace Machete.Test.IntegrationTests.Views
{
    [TestClass]
    public class SeFunctionality
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        [TestMethod]
        public void maximize_screen()
        {
            driver = new InternetExplorerDriver();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            baseURL = "http://localhost:4213/";
            //driver.Navigate().GoToUrl(baseURL);
            var result = js.ExecuteScript("if (window.screen){window.moveTo(0, 0);window.resizeTo(window.screen.availWidth,window.screen.availHeight);};");
            //var result = js.ExecuteScript("alert('well shoot me');");
            Assert.IsTrue(false);
        }
    }
}
