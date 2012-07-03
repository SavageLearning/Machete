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
using Machete.Data;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace Machete.Test
{
    [TestClass]
    public class WorkerSigninTests
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private static sharedUI ui;
        private static string testdir;
        private static string testimagefile;
        private static MacheteContext DB;
        private static DbSet<Worker> wSet;
        private static DbSet<WorkerSignin> wsiSet;
        private static DbSet<Person> pSet;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // getting Project path for dummy image
            string solutionDirectory = sharedUI.SolutionDirectory();
            testdir = solutionDirectory + "\\Machete.test\\";
            testimagefile = testdir + "jimmy_machete.jpg";
            driver = new FirefoxDriver();
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL);
            DB = new MacheteContext();
            wsiSet = DB.Set<WorkerSignin>();
            wSet = DB.Set<Worker>();
            pSet = DB.Set<Person>();
        }
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void SetupTest()
        {
            //driver = new FirefoxDriver();
            //baseURL = "http://localhost:4213/";
            //ui = new sharedUI(driver, baseURL);
            verificationErrors = new StringBuilder();
            ui.login();

        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup() { }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SeWSI_create_signin()
        {
            //IEnumerable<WorkerSignin> wList = wsiSet.Where(w => w.dwccardnum < 30000).AsEnumerable();
        }
    }
}