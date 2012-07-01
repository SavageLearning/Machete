using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Threading;
using Machete.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Machete.Data;


namespace Machete.Test
{
    class sharedUI
    {
        IWebDriver _d;
        string _url;
        int maxwait = 10; // seconds
        int sleepFor = 1000; //milliseconds
        public sharedUI(IWebDriver driver, string url)
        {
            _d = driver;
            _url = url;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool login()
        {
            _d.Navigate().GoToUrl(_url);

            _d.FindElement(By.LinkText("Logon")).Click();
            WaitForText("Account Information", maxwait);
            _d.FindElement(By.Id("UserName")).Clear();
            _d.FindElement(By.Id("UserName")).SendKeys("jadmin");
            _d.FindElement(By.Id("Password")).Clear();
            _d.FindElement(By.Id("Password")).SendKeys("machete");
            _d.FindElement(By.Name("logonB")).Click();
            WaitForText("Welcome", maxwait);
            return true;
        }
        #region persons
        public bool personCreate(Person _per)
        {
            string prefix = "person" + _per.ID + "-";
            _per.firstname1 = RandomString(4);
            _per.lastname1 = RandomString(8);
            WaitThenClickElement(By.Id("menulinkperson"));
            WaitThenClickElement(By.Id("personCreateTab"));
            WaitThenClickElement(By.Id("firstname1"));
            _d.FindElement(By.Id("firstname1")).Clear();
            _d.FindElement(By.Id("firstname1")).SendKeys(_per.firstname1);
            _d.FindElement(By.Id("lastname1")).Clear();
            _d.FindElement(By.Id("lastname1")).SendKeys(_per.lastname1);
            WaitThenClickElement(By.Id(prefix + "SaveBtn"));

            //
            //WaitForElement(By.Id("personSearchBox")).SendKeys(_per.lastname1);
            //WaitForElementValue(By.XPath("//table[@id='personTable']/tbody/tr/td[4]"), _per.lastname1);
            //WaitAndDoubleClick(By.XPath("//table[@id='personTable']/tbody/tr/td[6]"));

            //
            var selectedTab = WaitForElement(By.CssSelector("li.person.ui-tabs-selected"));
            Assert.IsNotNull(selectedTab, "Failed to find Person selected tab element");
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            Assert.IsNotNull(tabAnchor, "Failed to find Person selected tab element anchor");
            var name = _per.firstname1 + " " + _per.lastname1;

            Assert.IsTrue(tabAnchor.Text == name, "Person anchor label doesn't match person name");
            _per.ID = Convert.ToInt32(tabAnchor.GetAttribute("recordid"));
            return true;
        }

        public bool workerCreate(Worker _wkr, string imagepath)
        {
            string prefix = "worker"+_wkr.ID+"-";
            WaitThenClickElement(By.Id("workerCreateTab"));
            WaitForElement(By.Id(prefix + "dateOfMembership"));            
            _d.FindElement(By.Id(prefix + "dateOfMembership")).SendKeys(_wkr.dateOfMembership.ToShortDateString());            
            _d.FindElement(By.Id(prefix + "dateOfBirth")).SendKeys(_wkr.dateOfBirth.ToShortDateString());
            _d.FindElement(By.Id(prefix + "dateinUSA")).SendKeys(((DateTime)_wkr.dateinUSA).ToShortDateString());
            _d.FindElement(By.Id(prefix + "dateinseattle")).SendKeys(((DateTime)_wkr.dateinseattle).ToShortDateString());
            _d.FindElement(By.Id(prefix + "memberexpirationdate")).SendKeys(_wkr.memberexpirationdate.ToShortDateString());
            _d.FindElement(By.Id(prefix + "height")).SendKeys(_wkr.height);
            _d.FindElement(By.Id(prefix + "weight")).SendKeys(_wkr.weight);
            _d.FindElement(By.Id(prefix + "dwccardnum")).Clear();
            _d.FindElement(By.Id(prefix + "dwccardnum")).SendKeys(_wkr.dwccardnum.ToString());

            SelectOption(By.Id(prefix + "memberStatus"), "Active");
            SelectOption(By.Id(prefix + "neighborhoodID"), "Kent");
            SelectOption(By.Id(prefix + "typeOfWorkID"), @"(DWC) Day Worker Center");
            SelectOption(By.Id(prefix + "englishlevelID"), "2");
            SelectOption(By.Id(prefix + "incomeID"), @"Less than $15,000");
            SelectOption(By.Id(prefix + "neighborhoodID"), "Kent");
            _d.FindElement(By.Id(prefix + "imagefile")).SendKeys(imagepath);
            _d.FindElement(By.Id("createWorkerBtn")).Click();
            //
            //
            bool result = WaitForElementValue(By.Id("workerCreateTab"), "Worker information");
            Assert.IsTrue(result, "Create tab label not updated by formSubmit");
            _d.FindElement(By.Id("workerCreateTab")).Click();            
            return true;
        }
        #endregion

        #region employers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_emp"></param>
        /// <returns></returns>
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

            SelectOptionByIndex(By.Id("active"), _emp.active ? 2 : 1);
            SelectOptionByIndex(By.Id("blogparticipate"), _emp.blogparticipate ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "business"), _emp.business ? 2 : 1);
            SelectOption(By.Id(prefix + "referredby"), MacheteLookup.cache.First(c => c.category == "emplrreference" && c.ID == _emp.referredby).text_EN);

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_emp"></param>
        /// <returns></returns>
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
            getAttributeAssertEqual(_emp.state, "state");
            getAttributeAssertEqual(_emp.zipcode, "zipcode");
            getAttributeAssertEqual(_emp.phone, "phone");
            getAttributeAssertEqual(_emp.cellphone, "cellphone");
            //getAttributeAssertEqual(_emp.referredby.ToString(), "referredby");
            getAttributeAssertEqual(_emp.email, "email");
            getAttributeAssertEqual(_emp.notes, "notes");
            getAttributeAssertEqual(_emp.referredbyOther, "referredbyOther");

            WaitForElement(By.Id("active"));
            Assert.AreEqual(_emp.active ? 2 : 1, GetOptionIndex(By.Id("active")) );
            WaitForElement(By.Id("blogparticipate"));
            Assert.AreEqual(_emp.blogparticipate ? 2 : 1, GetOptionIndex(By.Id("blogparticipate")));
            WaitForElement(By.Id(prefix + "business"));
            Assert.AreEqual(_emp.business ? 2 : 1, GetOptionIndex(By.Id(prefix + "business")));
            WaitForElement(By.Id(prefix + "refferedby"));
            Assert.AreEqual(_emp.referredby, MacheteLookup.cache.First(c => c.category == "emplrreference" && c.text_EN == GetOptionText(By.Id(prefix + "referredby"))).ID);
            return true;
        }

        public bool employerDelete(Employer _emp)
        {
            WaitThenClickElement(By.Id("deleteEmployerButton-" + _emp.ID.ToString()));
            
            //Thread.Sleep(5);
            //WaitForElement(By.Id("popup_container"));
            //_d.FindElement(By.Id("popup_ok"));
            WaitThenClickElement(By.Id("popup_ok"));
            
            //WaitForElement(By.Id("employerTable_searchbox"));
            Thread.Sleep(5000);
            //var elem = _d.FindElement(By.Id("employerTable_searchbox"));
            //var foo1 = elem.GetAttribute("disabled");
            //var foo2 = elem.GetAttribute("readonly");
            //Assert.IsTrue(elem.Displayed);
            //Assert.IsTrue(elem.Enabled);
            WaitForElement(By.Id("employerTable_searchbox")).SendKeys(_emp.name);
            bool result = WaitForElementValue(By.XPath("//table[@id='employerTable']/tbody/tr/td[1]"), "No matching records found");
            Assert.IsTrue(result, "Employer not deleted properly");
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

            SelectOption(By.Id(prefix + "status"), MacheteLookup.cache.First(c => c.category == "orderstatus" && c.ID == _wo.status).text_EN);
            SelectOptionByIndex(By.Id(prefix + "transportMethodID"), _wo.transportMethodID);
            SelectOptionByIndex(By.Id(prefix + "timeFlexible"), _wo.timeFlexible ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "permanentPlacement"), _wo.permanentPlacement ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "englishRequired"), _wo.englishRequired ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "lunchSupplied"), _wo.lunchSupplied ? 2 : 1);
            if (_wo.workerRequests != null)
                foreach (var request in _wo.workerRequests)
                {
                    WaitThenClickElement(By.Id("addRequestBtn-" + _wo.ID));
                    ReplaceElementText(By.XPath("//*[@id='workerTable-0_filter']/label/input"), request.ID.ToString());
                    WaitThenClickElement(By.XPath("//*[@id='workerTable-0']/tbody/tr/td[1]"));
                }

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

            WaitForElement(By.Id(prefix + "status"));
            string optionText = GetOptionText(By.Id(prefix + "status"));
            Assert.AreEqual(_wo.status, MacheteLookup.cache.First(c => c.category == "orderstatus" && c.text_EN == optionText).ID);
            WaitForElement(By.Id(prefix + "timeFlexible"));
            Assert.AreEqual(_wo.timeFlexible ? 2:1, GetOptionIndex(By.Id(prefix + "timeFlexible")));
            WaitForElement(By.Id(prefix + "permanentPlacement"));
            Assert.AreEqual(_wo.permanentPlacement ? 2 : 1, GetOptionIndex(By.Id(prefix + "permanentPlacement")));
            WaitForElement(By.Id(prefix + "englishRequired"));
            Assert.AreEqual(_wo.englishRequired ? 2 : 1, GetOptionIndex(By.Id(prefix + "englishRequired")));
            WaitForElement(By.Id(prefix + "lunchSupplied"));
            Assert.AreEqual(_wo.lunchSupplied ? 2 : 1, GetOptionIndex(By.Id(prefix + "lunchSupplied")));
            WaitForElement(By.Id(prefix + "transportationMethodID"));
            Assert.AreEqual(_wo.transportMethodID, GetOptionIndex(By.Id(prefix + "transportMethodID")));
            WaitForElement(By.Id("workerRequests2_WO-" + _wo.ID.ToString()));
            if (_wo.workerRequests != null)
                foreach (var request in _wo.workerRequests)
                {
                    WaitForElementValue(By.XPath("//*[@id='workerRequests2_WO-" + _wo.ID + "']/option"), request.fullNameAndID);
                }
            return true;
        }
        #endregion

        #region WorkAssignments
        public bool WorkAssignmentCreate(Employer _emp, WorkOrder _wo, WorkAssignment _wa)
        {
            WaitThenClickElement(By.Id("wact-" + _wo.ID)); //the ID here is the WorkOrder.ID, not the Employer.ID
            WaitForElement(By.Id("description"));
            SelectOption(By.Id("englishLevelID"), _wa.englishLevelID.ToString());
            SelectOptionByValue(By.Id("skillID"), _wa.skillID.ToString());
            ReplaceElementText(By.Id("hourlyWage"), _wa.hourlyWage.ToString());
            SelectOption(By.Id("hours"), _wa.hours.ToString());
            if (_wa.hourRange.ToString().Length > 0)
                SelectOption(By.Id("hourRange"), _wa.hourRange.ToString());
            SelectOption(By.Id("days"), _wa.days.ToString());
            ReplaceElementText(By.Id("description"), _wa.description);
            _d.FindElement(By.Id("WO" + _wo.ID + "-waCreateBtn")).Click();
            return true;
        }
        public bool WorkAssignmentValidate(Employer _emp, WorkOrder _wo, WorkAssignment _wa)
        {
            WaitForElement(By.Id("workOrderTable_" + _emp.ID + "_searchbox"));
            WaitThenClickElement(By.Id("walt-" + _wo.ID));
            WaitForElement(By.Id("workAssignTable-wo-" + _wo.ID + "_searchbox"));
            string idString = _wo.ID.ToString("D5") + "-" + ((int)_wa.pseudoID).ToString("D2");
            ReplaceElementText(By.Id("workAssignTable-wo-" + _wo.ID + "_searchbox"), idString);
            string xpath = "//*[@id='workAssignTable-wo-" + _wo.ID + "']/tbody/tr/td[.='" + idString + "']";
            Assert.IsTrue(WaitAndDoubleClick(By.XPath(xpath)),
                "Cannot find work assignment row to click.");

            //Now, check each of the fields
            WaitForElement(By.Id("englishLevelID"));
            Assert.AreEqual(_wa.englishLevelID + 1,GetOptionIndex(By.Id("englishLevelID")));
            Assert.AreEqual(_wa.hours, GetOptionIndex(By.Id("hours")));
            if (_wa.hourRange != null)
                Assert.AreEqual(_wa.hourRange, GetOptionIndex(By.Id("hourRange")) + 6);
            Assert.AreEqual(_wa.days, GetOptionIndex(By.Id("days")));
            WaitForElement(By.Id("skillID"));
            string skillIDText = GetOptionText(By.Id("skillID"));
            Assert.AreEqual(_wa.skillID, MacheteLookup.cache.First(c => c.category == "skill" && c.text_EN == skillIDText).ID);
            Assert.AreEqual(_wa.hourlyWage.ToString("##.##"), WaitForElement(By.Id("hourlyWage")).Text);

            return true;
        }
        #endregion

        #region activities
        public bool activityCreate(Activity _act)
        {
            string prefix = "activity0-";
            //Go to activites page
            WaitThenClickElement(By.Id("menulinkactivity"));
            //Go to create an activity tab
            WaitThenClickElement(By.Id("activityCreateTab"));
            //Wait for page to load
            WaitForElement(By.Id(prefix + "name"));
            //Enter information
            SelectOptionByIndex(By.Id(prefix + "name"), _act.name - 97);
            SelectOptionByIndex(By.Id(prefix + "type"), _act.type - 100);
            SelectOption(By.Id(prefix + "teacher"), _act.teacher);
            ReplaceElementText(By.Id(prefix + "notes"), _act.notes);
            //Hit the save button
            _d.FindElement(By.Id(prefix + "SaveBtn")).Click();
            //Look for new tab with class: activity.ui-tabs-selected
            var selectedTab = WaitForElement(By.CssSelector("li.activity.ui-tabs-selected"));
            Assert.IsNotNull(selectedTab, "Failed to find Activity selected tab element");
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            Assert.IsNotNull(tabAnchor, "Failed to find Activity selected tab element anchor");
            _act.ID = Convert.ToInt32(tabAnchor.GetAttribute("recordid"));


            return true;
            
        }
        public bool activityValidate(Activity _act)
        {
            string prefix = "activity" + _act.ID + "-";
            //Wait for the page to load
            WaitForElement(By.Id(prefix + "name"));

            Assert.AreEqual(_act.name - 97, GetOptionIndex(By.Id(prefix + "name")));
            Assert.AreEqual(_act.type - 100, GetOptionIndex(By.Id(prefix + "type")));
            Assert.AreEqual(_act.teacher, GetOptionText(By.Id(prefix + "teacher")));
            Assert.AreEqual(_act.notes, WaitForElement(By.Id(prefix + "notes")).GetAttribute("value"));
            return true;
        }
        public bool activitySignIn(int dwccardnum, int rowcount)
        {
            return activitySignIn(dwccardnum, false);
        }
        public bool activitySignIn(int dwccardnum, bool debuggging)
        {
            WaitForElement(By.Id("dwccardnum"));
            ReplaceElementText(By.Id("dwccardnum"), dwccardnum.ToString());
            WaitForElement(By.Id("dwccardnum")).Submit();

            WaitForElement(By.XPath("//*[@id='wsiTable_filter']/label/input"));
            ReplaceElementText(By.XPath("//*[@id='wsiTable_filter']/label/input"), dwccardnum.ToString());

            var tablecell = WaitForElement(By.XPath("//table[@id='wsiTable']/tbody/tr/td[2]"));
            if (debuggging)
                Assert.IsNotNull(tablecell);
            bool result = WaitForElementValue(By.XPath("//table[@id='wsiTable']/tbody/tr/td[2]"), dwccardnum.ToString());
            var tablecell2 = WaitForElement(By.XPath("//table[@id='wsiTable']/tbody/tr/td[2]"));
            if (debuggging)
                Assert.IsTrue(result);
            return result;
            // How will this test catch an error? need to wait for and validate 
            // name that is presented at sign-in.
            //$x("//table[@id='wsiTable']/tbody/tr[1]/td[2]")
            return WaitForElementValue(By.XPath("//table[@id='wsiTable']/tbody/tr["+rowcount+"]/td[2]"), dwccardnum.ToString());
            //return true;
        }

        #endregion

        #region dropDowns
        public bool SelectOption(By by, string opttext)
        {
            var dropdown = _d.FindElement(by);
            var selectElem = new SelectElement(dropdown);
            selectElem.SelectByText(opttext);
            return true;
        }
        public bool SelectOptionByValue(By by, string optvalue)
        {
            var dropdown = _d.FindElement(by);
            var selectElem = new SelectElement(dropdown);
            selectElem.SelectByValue(optvalue);
            return true;
        }
        public bool SelectOptionByIndex(By by, int index)
        {
            var dropdown = _d.FindElement(by);
            var selectElem = new SelectElement(dropdown);
            selectElem.SelectByIndex(index);
            return true;
        }
        public string GetOptionText(By by)
        {
            var dropdown = _d.FindElement(by);
            var selectElem = new SelectElement(dropdown);
            return selectElem.SelectedOption.Text;
        }
        public int GetOptionIndex(By by)
        {
            var dropdown = _d.FindElement(by);
            var selectElem = new SelectElement(dropdown);
            return selectElem.Options.IndexOf(selectElem.SelectedOption);
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
            for (int second = 0; second < maxwait; second++)
            {
                try
                {
                    elem = IsElementPresent(by);
                    if (elem != null) return elem;
                }
                catch (Exception)
                { return null; }
                Thread.Sleep(sleepFor);
            }
            return null;
        }
        //
        //
        public bool WaitForElementValue(By by, string value)
        {
            for (int second = 0; second < maxwait; second++)
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
                Thread.Sleep(sleepFor);
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
                Thread.Sleep(sleepFor);
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
            catch (Exception)
            {
                return null;
            }
            //catch (ElementNotVisibleException)
            //{
            //    return null;
            //}
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
            catch (NoSuchElementException)
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
