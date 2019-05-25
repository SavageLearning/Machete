using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using Machete.Web.Maps;
using Machete.Test.Integration.Fluent;

namespace Machete.Test.Selenium.View
{
    [TestClass]
    public class EmployerTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private sharedUI ui;
        private FluentRecordBase frb;
        private static IMapper map;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var webMapperConfig = new MapperConfiguration(config => { config.ConfigureMvc(); });
            map = webMapperConfig.CreateMapper();
        }

        [TestInitialize]
        public void SetupTest()
        {
            frb = FluentRecordBaseFactory.Get();
            driver = new ChromeDriver("/usr/local/bin");
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL, map);
            verificationErrors = new StringBuilder();
            ui.login();
        }

        [TestCleanup]
        public void TeardownTest()
        {
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

        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Employers)]
        public void SeEmployer_Create_Validate_Delete()
        {
            //Arrange
            var _emp = frb.CloneEmployer();
            var _emp1 = frb.CloneEmployer();
            //Act
            //starts with /Employer/Create
            ui.employerCreate(_emp1);
            ui.employerCreate(_emp);
            //Assert
            //save from /Create opens /Employer/Edit
            ui.employerValidate(_emp);
            ui.employerDelete(_emp);
            // TODO: Validate emp1 and verify business name exists (good intentions marinated in panic)
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Employers)]
        public void SeEmployer_Create_workorder()
        {
            //Arrange
            var _emp = frb.CloneEmployer();
            var _wo = frb.CloneWorkOrder();
            _wo.statusID = frb.ToServ<ILookupService>().GetByKey(LCategory.orderstatus, LOrderStatus.Pending).ID; // start work order off as pending
            //Act
            ui.employerCreate(_emp);
            ui.workOrderCreate(_emp, _wo);

            //Assert
            ui.workOrderValidate(_wo);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Employers)]
        public void SeEmployer_Create_workorder_copyinfo()
        {
            //Arrange
            var _emp = frb.CloneEmployer();
            var _wo = frb.CloneWorkOrder(); 

            //Act
            ui.employerCreate(_emp);
            ui.WaitThenClickElement(By.Id("workOrderCreateTab_" + _emp.ID));
            ui.WaitThenClickElement(By.Id("WO0-copyEmployerInfo"));
            ui.SelectOptionByIndex(By.Id("WO0-transportMethodID"), 1);
            ui.WaitThenClickElement(By.Id("WO0-SaveBtn"));
            //_wo.ID = ui.getSelectedTabRecordID("WO");
            _wo.contactName = _emp.name;
            _wo.workSiteAddress1 = _emp.address1;
            _wo.workSiteAddress2 = _emp.address2;
            _wo.city = _emp.city;
            _wo.state = _emp.state;
            _wo.zipcode = _emp.zipcode;
            _wo.phone = _emp.phone;
            _wo.description = "";
            _wo.statusID = frb.ToServ<ILookupService>().GetByKey(LCategory.orderstatus, LOrderStatus.Pending).ID;
            _wo.ID = ui.getSelectedTabRecordID("WO");
            //Assert
            ui.workOrderValidate(_wo);
        }
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Employers)]
        public void SeEmployer_Create_and_Activate_WorkAssignment()
        {
            //Arrange
            var _employer1 = frb.CloneEmployer();
            var _wo = frb.CloneWorkOrder();
            var _wa1 = frb.CloneWorkAssignment();
            _wo.contactName = ui.RandomString(10);
            _wo.statusID = frb.ToServ<ILookupService>().GetByKey(LCategory.orderstatus, LOrderStatus.Pending).ID; // status = pending
            //
            // Create employer
            ui.employerCreate(_employer1);
            // Create work order
            ui.workOrderCreate(_employer1, _wo);
            // create assignment
            ui.workAssignmentCreate(_employer1, _wo, _wa1, frb);
            //Get WA ID and arrange pseudoID information
            //_wa1.workOrder = _wo;
            _wa1.workOrderID = _wo.ID;
            // pseudoID needs to be updated; created on save above
            _wa1.pseudoID = frb.ToServ<IWorkAssignmentService>().Get(_wa1.ID).pseudoID;
            // Activate assignment
            ui.workAssignmentActivate(_employer1, _wo, _wa1);
            //
            ui.workAssignmentValidate(_employer1, _wo, _wa1);
            ui.workOrderValidate(_wo);
            // TODO: Selenium: test duplicate (pseudoID increment is visible in table)
            // TODO: Selenium: test DispatchOption / Change Worker dialog. 
            // TODO: Selenium: test Skill dropdown for Chambita/specialized skill, test total changes
        }
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Employers)]
        public void SeEmployer_Create_and_move_Workorder()
        {
            var _emp1 = frb.CloneEmployer();
            var _emp2 = frb.CloneEmployer();
            var _wo = frb.CloneWorkOrder();
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
            // click change button
            Thread.Sleep(3000);
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
            ui.WaitForElement(By.Id("workOrderListTab_" + _emp1.ID));
            Assert.IsTrue(ui.WaitForElementValue(By.XPath("//table[@id='workOrderTable_" + recID.ToString() + "']/tbody/tr/td[5]"), _wo.contactName));
        }
    }
}
