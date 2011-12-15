using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Threading;
using Machete.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Interactions;


namespace Machete.Test
{
    class sharedUI
    {
        IWebDriver _d;
        string _url;
        public sharedUI(IWebDriver driver, string url)
        {
            _d = driver;
            _url = url;
        }
        public bool login()
        {
            _d.Navigate().GoToUrl(_url);

            _d.FindElement(By.LinkText("Logon")).Click();
            WaitForText("Account Information", 60);
            _d.FindElement(By.Id("UserName")).Clear();
            _d.FindElement(By.Id("UserName")).SendKeys("jadmin");
            _d.FindElement(By.Id("Password")).Clear();
            _d.FindElement(By.Id("Password")).SendKeys("machete");
            _d.FindElement(By.Name("logonB")).Click();
            WaitForText("Welcome", 60);
            return true;
        }
        #region employers
        public bool employerCreate(Employer _emp)
        {
            string prefix = "employer0-";
            _emp.name = RandomString(7);
            // go to person page
            WaitThenClickElement(By.Id("menulinkemployer"));
            // go to create person tab
            WaitThenClickElement(By.Id("employerCreateTab"));
            WaitForElement(By.Id(prefix + "name"));
            ReplaceElementText(By.Id(prefix + "name"), _emp.name);
            ReplaceElementText(By.Id(prefix + "address1"), _emp.address1);
            ReplaceElementText(By.Id(prefix + "address2"), _emp.address2);
            ReplaceElementText(By.Id(prefix + "city"), _emp.city);
            ReplaceElementText(By.Id(prefix + "zipcode"), _emp.zipcode);
            ReplaceElementText(By.Id(prefix + "phone"), _emp.phone);
            ReplaceElementText(By.Id(prefix + "cellphone"), _emp.cellphone);
            // select lists
            //http://stackoverflow.com/questions/4672658/how-do-i-set-a-an-option-as-selected-using-selenium-webdriver-selenium-2-0-cli
            //ReplaceElementText(By.Id(prefix + "referredby"), _emp.referredby.ToString());
            ReplaceElementText(By.Id(prefix + "email"), _emp.email);
            ReplaceElementText(By.Id(prefix + "notes"), _emp.notes);
            ReplaceElementText(By.Id(prefix + "referredbyOther"), _emp.referredbyOther);
            _d.FindElement(By.Id(prefix + "SaveBtn")).Click();
            //
            // look for new open tab with class: .employer.ui-tabs-selected
            var selectedTab = WaitForElement(By.CssSelector("li.employer.ui-tabs-selected"));
            Assert.IsNotNull(selectedTab, "Failed to find Employer selected tab element");
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            Assert.IsNotNull(tabAnchor, "Failed to find Employer selected tab element anchor");
            Assert.IsTrue(tabAnchor.Text == _emp.name, "Employer anchor label doesn't match employer name");
            _emp.ID = Convert.ToInt32(tabAnchor.GetAttribute("recordid"));
            return true;
        }

        public bool employerValidate(Employer _emp)
        {

            //
            // get recordid for finding new record. ID is {recType}{recID}-{field}

            string prefix = "employer" + _emp.ID.ToString() + "-";
            Func<string, string, bool> getAttributeAssertEqual = ((p, q) =>
            {
                Assert.AreEqual(p,
                    WaitForElement(By.Id(prefix + q)).GetAttribute("value"),
                    "New employer " + q + "doesn't match original.");
                return true;
            });
            getAttributeAssertEqual(_emp.name, "name");
            getAttributeAssertEqual(_emp.address1, "address1");
            getAttributeAssertEqual(_emp.address2, "address2");
            getAttributeAssertEqual(_emp.city, "city");
            getAttributeAssertEqual(_emp.zipcode, "zipcode");
            getAttributeAssertEqual(_emp.phone, "phone");
            getAttributeAssertEqual(_emp.cellphone, "cellphone");
            //getAttributeAssertEqual(_emp.referredby.ToString(), "referredby");
            getAttributeAssertEqual(_emp.email, "email");
            getAttributeAssertEqual(_emp.notes, "notes");
            getAttributeAssertEqual(_emp.referredbyOther, "referredbyOther");
            return true;
        }

        #endregion

        #region workorders
        public bool workOrderCreate(Employer _emp, WorkOrder _wo)
        {
            string prefix = "WO0-";
            WaitThenClickElement(By.Id("workOrderCreateTab_" + _emp.ID));
            WaitForElement(By.Id(prefix + "contactName"));
            ReplaceElementText(By.Id(prefix + "contactName"), _wo.contactName);
            //ReplaceElementText(By.Id(prefix + "dateTimeofWork"), _wo.dateTimeofWork);
            ReplaceElementText(By.Id(prefix + "paperOrderNum"), _wo.paperOrderNum.ToString());
            //ReplaceElementText(By.Id(prefix + "timeFlexible"), _wo.timeFlexible.ToString());
            //ReplaceElementText(By.Id(prefix + "permanentPlacement"), _wo.permanentPlacement);
            ReplaceElementText(By.Id(prefix + "workSiteAddress1"), _wo.workSiteAddress1);
            ReplaceElementText(By.Id(prefix + "workSiteAddress2"), _wo.workSiteAddress2);
            //ReplaceElementText(By.Id(prefix + "englishRequired"), _wo.englishRequired);
            ReplaceElementText(By.Id(prefix + "phone"), _wo.phone);
            //ReplaceElementText(By.Id(prefix + "lunchSupplied"), _wo.lunchSupplied);
            ReplaceElementText(By.Id(prefix + "city"), _wo.city);
            ReplaceElementText(By.Id(prefix + "state"), _wo.state);
            //ReplaceElementText(By.Id(prefix + "transportMethodID"), _wo.transportMethodID);
            ReplaceElementText(By.Id(prefix + "zipcode"), _wo.zipcode);
            //ReplaceElementText(By.Id(prefix + "transportFee"), _wo.transportFee);
            //ReplaceElementText(By.Id(prefix + "transportFeeExtra"), _wo.transportFeeExtra);
            //ReplaceElementText(By.Id(prefix + "englishRequiredNote"), _wo.englishRequiredNote);
            ReplaceElementText(By.Id(prefix + "description"), _wo.description);
            //
            // save work order
            _d.FindElement(By.Id(prefix + "SaveBtn")).Click();
            //
            // Find new work order tab (css class "WO"), get embedded WOID, populate
            // WO object

            _wo.ID = getSelectedTabRecordID("WO");
            Assert.IsTrue(_d.FindElement(By.CssSelector("li.WO.ui-tabs-selected > a"))
                                            .Text == Machete.Web.Resources.WorkOrders.tabprefix + _wo.getTabLabel(), 
                "Work order anchor label doesn't match work order");
            
            return true;
        }

        public int getSelectedTabRecordID(string cssClass)
        {
            var selectedTab = WaitForElement(By.CssSelector("li." + cssClass + ".ui-tabs-selected"));
            Assert.IsNotNull(selectedTab, "Failed to find " + cssClass + " selected tab element");
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            Assert.IsNotNull(tabAnchor, "Failed to find " + cssClass + " selected tab element anchor");
            return Convert.ToInt32(tabAnchor.GetAttribute("recordid"));
        }

        public bool workOrderValidate(WorkOrder _wo) 
        {
            string prefix = "WO" + _wo.ID.ToString() + "-";
            Func<string, string, bool> getAttributeAssertEqual = ((p, q) =>
            {
                Assert.AreEqual(p,
                    WaitForElement(By.Id(prefix + q)).GetAttribute("value"),
                    "New work order " + q + "doesn't match original.");
                return true;
            });
            getAttributeAssertEqual(_wo.contactName, "contactName");
            Assert.IsTrue(WaitForElement(By.Id(prefix + "paperOrderNum")).GetAttribute("value") != "", "paper order number is empty");
            //getAttributeAssertEqual(_wo.paperOrderNum.ToString(), "paperOrderNum");
            getAttributeAssertEqual(_wo.workSiteAddress1, "workSiteAddress1");
            getAttributeAssertEqual(_wo.workSiteAddress2, "workSiteAddress2");
            getAttributeAssertEqual(_wo.phone, "phone");
            getAttributeAssertEqual(_wo.city, "city");
            getAttributeAssertEqual(_wo.state, "state");
            getAttributeAssertEqual(_wo.zipcode, "zipcode");
            getAttributeAssertEqual(_wo.description, "description");
            return true;
        }
        #endregion

        //
        //
        #region utilfunctions
        public bool WaitAndDoubleClick(By by)
        {
            WaitForElement(by);
            IWebElement rowrecord = _d.FindElement(by);
            Actions actionProvider = new Actions(_d);
            IAction doubleClick = actionProvider.DoubleClick(rowrecord).Build();
            doubleClick.Perform();
            return true;
        }

        public bool ReplaceElementText(By by, string text)
        {
            var elem = _d.FindElement(by);
            try
            {
                
                elem.Clear();
                elem.SendKeys(text);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool WaitThenClickElement(By by)
        {
            IWebElement elem = WaitForElement(by);
            if (elem != null) 
            {
                elem.Click();
                return true;
            }
            else 
            {
                return false;
            }
        }

        public IWebElement WaitForElement(By by)
        {
            IWebElement elem;
            for (int second = 0; second < 60; second++)
            {
                try
                {
                    elem = IsElementPresent(by);
                    if (elem != null) return elem;
                }
                catch (Exception)
                { return null; }
                Thread.Sleep(1000);
            }
            return null;
        }
        //
        //
        public bool WaitForElementValue(By by, string value)
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
        public bool WaitForText(String what, int waitfor)
        {
            for (int second = 0; second < waitfor; second++)
            {
                try
                {
                    if (isTextPresent(what, _d)) return true;
                }
                catch (Exception)
                { return false; }
                Thread.Sleep(1000);
            }
            return false;
        }
        #endregion
        #region privatemethods
        private IWebElement IsElementPresent(By by)
        {
            try
            {
                return _d.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
        //
        //
        private bool IsElementValuePresent(By by, string value)
        {
            try
            {
                IWebElement elem = _d.FindElement(by);
                if (elem.Text == value) return true;
                else return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
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
        #endregion 
        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
