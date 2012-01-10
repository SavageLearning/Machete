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
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL);
            verificationErrors = new StringBuilder();
            ui.login();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            //
            // Loggoff
            Assert.AreEqual("", verificationErrors.ToString());
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

        }
        [ClassCleanup]
        public static void ClassCleanup() { }
        /// <summary>
        /// 
        /// 
        /// </summary>
        [TestMethod]
        public void SeEmployer_Create_employer()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            ui.employerCreate(_emp);
            ui.employerValidate(_emp);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SeEmployer_Create_workorder()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            WorkOrder _wo = (WorkOrder)Records.order.Clone();
            
            ui.employerCreate(_emp);
            ui.workOrderCreate(_emp, _wo);
            ui.workOrderValidate(_wo);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SeEmployer_Create_workorder_copyinfo()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            WorkOrder _wo = new WorkOrder();

            ui.employerCreate(_emp);
            ui.WaitThenClickElement(By.Id("workOrderCreateTab_" + _emp.ID));
            ui.WaitThenClickElement(By.Id("WO0-copyEmployerInfo"));
            ui.WaitThenClickElement(By.Id("WO0-SaveBtn"));
            _wo.ID = ui.getSelectedTabRecordID("WO");
            _wo.contactName = _emp.name;
            _wo.workSiteAddress1 = _emp.address1;
            _wo.workSiteAddress2 = _emp.address2;
            _wo.city = _emp.city;
            _wo.state = _emp.state;
            _wo.zipcode = _emp.zipcode;
            _wo.phone = _emp.phone;
            _wo.description = "";
            ui.workOrderValidate(_wo);
        }
        [TestMethod]
        public void SeEmployer_Create_and_move_Workorder()
        {
            Employer _emp1 = (Employer)Records.employer.Clone();
            Employer _emp2 = (Employer)Records.employer.Clone();
            WorkOrder _wo = (WorkOrder)Records.order.Clone();
            _wo.contactName = ui.RandomString(10);
            // create first worker
            ui.employerCreate(_emp1);
            //  create 2nd worker
            ui.employerCreate(_emp2);
            // create workorder for employer 2
            ui.workOrderCreate(_emp2, _wo);
            //
            //
            string prefix = "WO" + _wo.ID + "-";
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            string title = (string)js.ExecuteScript("if (window.screen){window.moveTo(0, 0);window.resizeTo(window.screen.availWidth,window.screen.availHeight);};");
            // click change button
            ui.WaitThenClickElement(By.Id(prefix + "changeEmployerBtn"));
            // find new employer
            ui.WaitForElement(By.Id("employerSelectTable_searchbox")).SendKeys(_emp1.name);
            // check for name in popup table column
            ui.WaitForElementValue(By.XPath("//table[@id='employerSelectTable']/tbody/tr/td[2]"), _emp1.name);
            // doubleclick on row (using elem 6 b/c first elems might be off screen)
            ui.WaitAndDoubleClick(By.XPath("//table[@id='employerSelectTable']/tbody/tr/td[6]"));
            //
            // confirm dialog
            ui.WaitThenClickElement(By.Id("popup_ok"));
            ui.WaitThenClickElement(By.Id("employerListTab"));
            ui.WaitForElement(By.Id("employerTable_searchbox")).SendKeys(_emp1.name);
            // search for employer 1
            ui.WaitForElementValue(By.XPath("//table[@id='employerTable']/tbody/tr/td[2]"), _emp1.name);
            ui.WaitAndDoubleClick(By.XPath("//table[@id='employerTable']/tbody/tr/td[6]"));
            var selectedTab = ui.WaitForElement(By.CssSelector("li.employer.ui-tabs-selected a"));
            var recID = Convert.ToInt32(selectedTab.GetAttribute("recordid"));
            ui.WaitForElement(By.Id("workOrderList"));
            Assert.IsTrue(ui.WaitForElementValue(By.XPath("//table[@id='workOrderTable_" + recID.ToString() + "']/tbody/tr/td[5]"), _wo.contactName));
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