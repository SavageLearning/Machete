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
        public void Se_Employer_Create_record()
        {
            Employer _emp = (Employer)Records.employer.Clone();
            string prefix = "employer0-";
            _emp.name = ui.RandomString(7);
            // go to person page
            ui.WaitThenClickElement(By.Id("menulinkemployer"));
            // go to create person tab
            ui.WaitThenClickElement(By.Id("employerCreateTab"));
            ui.WaitForElement(By.Id(prefix+"name"));
            ui.ReplaceElementText(By.Id(prefix + "name"), _emp.name);
            ui.ReplaceElementText(By.Id(prefix + "address1"), _emp.address1);
            ui.ReplaceElementText(By.Id(prefix + "city"), _emp.city);
            ui.ReplaceElementText(By.Id(prefix + "zipcode"), _emp.zipcode);
            ui.ReplaceElementText(By.Id(prefix + "phone"), _emp.phone);
            ui.ReplaceElementText(By.Id(prefix + "cellphone"), _emp.cellphone);
            // select lists
            //http://stackoverflow.com/questions/4672658/how-do-i-set-a-an-option-as-selected-using-selenium-webdriver-selenium-2-0-cli
            //ui.ReplaceElementText(By.Id(prefix + "referredby"), _emp.referredby.ToString());
            ui.ReplaceElementText(By.Id(prefix + "email"), _emp.email);
            ui.ReplaceElementText(By.Id(prefix + "notes"), _emp.notes);
            ui.ReplaceElementText(By.Id(prefix + "referredbyOther"), _emp.referredbyOther);

            driver.FindElement(By.Id(prefix + "Save")).Click();
            //
            // look for new open tab with class: .employer.ui-tabs-selected
            var selectedTab = ui.WaitForElement(By.CssSelector("li.employer.ui-tabs-selected"));
            Assert.IsNotNull(selectedTab, "Failed to find Employer selected tab element");
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            Assert.IsNotNull(tabAnchor, "Failed to find Employer selected tab element anchor");
            Assert.IsTrue(tabAnchor.Text == _emp.name, "Employer anchor label doesn't match employer name" );
            //
            // get recordid for finding new record. ID is {recType}{recID}-{field}
            _emp.ID = Convert.ToInt32(tabAnchor.GetAttribute("recordid"));
            prefix = "employer" + _emp.ID.ToString() + "-";
            Func<string, string, bool> getAttributeAssertEqual = ((p, q) => { 
                Assert.AreEqual(p, 
                    ui.WaitForElement(By.Id(prefix + q)).GetAttribute("value"), 
                    "New employer " + q + "doesn't match original."); 
                return true; 
            });
            getAttributeAssertEqual(_emp.name, "name");
            getAttributeAssertEqual(_emp.address1, "address1");
            getAttributeAssertEqual(_emp.city, "city");
            getAttributeAssertEqual(_emp.zipcode, "zipcode");
            getAttributeAssertEqual(_emp.phone, "phone");
            getAttributeAssertEqual(_emp.cellphone, "cellphone");
            //getAttributeAssertEqual(_emp.referredby.ToString(), "referredby");
            getAttributeAssertEqual(_emp.email, "email");
            getAttributeAssertEqual(_emp.notes, "notes");
            getAttributeAssertEqual(_emp.referredbyOther, "referredbyOther");

            //Assert.IsTrue(false);
        }
        [TestMethod]
        public void Se_Person_Change_Zip()
        {
        }
        //
        //
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