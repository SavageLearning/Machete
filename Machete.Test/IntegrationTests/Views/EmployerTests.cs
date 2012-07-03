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
        public void SeEmployer_Create_Validate_Delete()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            Employer _emp1 = (Employer)Records.employer.Clone();
            //starts with /Employer/Create
            ui.employerCreate(_emp1);
            ui.employerCreate(_emp);
            //save from /Create opens /Employer/Edit
            ui.employerValidate(_emp);
            ui.employerDelete(_emp);

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
            WorkOrder _wo = (WorkOrder)Records._workOrder1.Clone(); //Made this a clone so fields like workorder status and tranportation method would be filled in properly for validation.

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
            _wo.status = 43;
            ui.workOrderValidate(_wo);
        }
        [TestMethod]
        public void SeEmployer_Create_and_Activate_WorkAssignment()
        {
            Employer _employer1 = (Employer)Records.employer.Clone();
            WorkOrder _wo = (WorkOrder)Records.order.Clone();
            WorkAssignment _wa1 = (WorkAssignment)Records.assignment.Clone();

            _wo.contactName = ui.RandomString(10);
            ui.employerCreate(_employer1);
            ui.workOrderCreate(_employer1, _wo);
            //_wo.ID = ui.getSelectedTabRecordID("WO");
            ui.WorkAssignmentCreate(_employer1, _wo, _wa1);
            //Get WA ID and arrange pseudoID information
            _wa1.ID = ui.getSelectedTabRecordID("WA");
            _wa1.workOrder = _wo;
            _wa1.workOrderID = _wo.ID;
            _wa1.incrPseudoID();
            //_wa1.workOrder.waPseudoIDCounter++; // from WorkASsignment
            //_wa1.pseudoID = _wa1.workOrder.waPseudoIDCounter; //TODO: refactor into base class
            //
            ui.WaitThenClickElement(By.Id("walt-" + _wo.ID));
            Thread.Sleep(2000);
            By walt = By.XPath("//table[@id='workAssignTable-wo-" + _wo.ID + "']/tbody/tr/td[1]");
            ui.WaitForElementValue(walt, _wa1.getFullPseudoID());
            Assert.AreEqual(_wa1.getFullPseudoID(), driver.FindElement(walt).Text, "Unexpected PseudoID in assignment's list");
            //
            ui.WaitThenClickElement(By.Id("activateWorkOrderButton-"+ _wo.ID));

            ui.WorkAssignmentValidate(_employer1, _wo, _wa1);
            //
            // TODO: Selenium: check WO status and recid of order
            // TODO: Selenium: test duplicate (pseudoID increment is visible in table)
            // TODO: Selenium: test DispatchOption / Change Worker dialog. 
            // TODO: Selenium: test Skill dropdown for Chambita/specialized skill, test total changes


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
            //IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            //string title = (string)js.ExecuteScript("if (window.screen){window.moveTo(0, 0);window.resizeTo(window.screen.availWidth,window.screen.availHeight);};");
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