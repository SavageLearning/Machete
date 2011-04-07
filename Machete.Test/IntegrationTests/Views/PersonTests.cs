//using System;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Selenium;

//namespace Machete.Test
//{
//    [TestClass]
//    public class MacheteSignin
//        {
//        private ISelenium selenium;
//        private StringBuilder verificationErrors;
//        public WebServer webServer;

//        [TestInitialize]
//        public void SetupTest()
//        {
//            //var absolutePathToWebsite = ConfigurationManager.AppSettings["absolutePathToWebsite"];
//            var absolutePathToWebsite = @"H:\jimmy\Documents\VS2010\Projects\Machete\Machete.Web";
//            webServer = new WebServer(absolutePathToWebsite, 42420);
//            try
//            {
//                webServer.Start();
//            }
//            catch (Exception e)
//            {
//            }
//            selenium = new DefaultSelenium("localhost", 4444, "*firefox", "http://www.google.com/");
//            selenium.Start();
//            verificationErrors = new StringBuilder();
//        }

//        [TestCleanup]
//        public void TeardownTest()
//        {
//        try
//        {
//        webServer.Stop();
//        selenium.Stop();
//        }
//        catch (Exception e)
//        {
//        // Ignore errors if unable to close the browser
//        }
//        Assert.AreEqual("", verificationErrors.ToString());
//        }

//        [TestMethod]
//        public void Se_LogonLogoff()
//        {
//                selenium.Open("/");
//                    selenium.Click("link=Logon");
//                    selenium.WaitForPageToLoad("30000");
//                    selenium.Type("UserName", "jadmin");
//                    selenium.Type("Password", "1bud4me");
//                    selenium.Click("logonB");
//                    selenium.WaitForPageToLoad("30000");
//                    selenium.Click("link=Worker Signin");
//                    selenium.WaitForPageToLoad("30000");
//                    selenium.TypeKeys("dwccardentry", "12345");
//                    selenium.Click("//input[@value='Signin?!']");
//                    selenium.WaitForPageToLoad("30000");
//                    try
//                    {
//                        Assert.IsTrue(selenium.IsTextPresent("12345"));
//                    }
//                    catch (Exception e)
//                    {
//                        verificationErrors.Append(e.Message);
//                    }
//                    Assert.IsTrue(selenium.IsTextPresent("Jimmy"));
//                    selenium.TypeKeys("dwccardentry", "12345");
//                    selenium.Click("//input[@value='Signin?!']");
//                    selenium.WaitForPageToLoad("30000");
//                    selenium.Click("link=Delete");
//                    selenium.WaitForPageToLoad("30000");
//                    Assert.IsTrue(selenium.IsTextPresent("No data available in table"));
//                    selenium.Click("link=Logoff");
//                    selenium.WaitForPageToLoad("30000");
//                    Assert.IsTrue(selenium.IsTextPresent("Logon"));
//                    Assert.IsFalse(selenium.IsTextPresent("Welcome jadmin!"));
//        }

//        [TestMethod]
//        public void TheNewTest()
//        {
//            // Open Google search engine.
//            selenium.Open("http://www.google.com/");

//            // Assert Title of page.
//            Assert.AreEqual("Google", selenium.GetTitle());

//            // Provide search term as "Selenium OpenQA"
//            selenium.Type("q", "Selenium OpenQA");

//            // Read the keyed search term and assert it.
//            Assert.AreEqual("Selenium OpenQA", selenium.GetValue("q"));

//            // Click on Search button.
//            selenium.Click("btnG");

//            // Wait for page to load.
//            selenium.WaitForPageToLoad("5000");

//            // Assert that "www.openqa.org" is available in search results.
//            Assert.IsTrue(selenium.IsTextPresent("www.openqa.org"));

//            // Assert that page title is - "Selenium OpenQA - Google Search"
//            Assert.AreEqual("Selenium OpenQA - Google Search",
//                         selenium.GetTitle());
//        }

//    }
//}

