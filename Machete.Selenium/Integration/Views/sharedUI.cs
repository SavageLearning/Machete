using AutoMapper;
using Machete.Data;
using Machete.Web.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Machete.Data.Initialize;
using Machete.Test.Integration.Fluent;
using Machete.Web.Helpers;

namespace Machete.Test.Selenium.View
{
    class sharedUI
    {
        public IWebDriver _d;
        string _url;
        public int maxwait = 6; // seconds
        int _sleepFor = 1200; //milliseconds
        IMapper map;

        public sharedUI(IWebDriver driver, string url, IMapper map)
        {
            _d = driver;
            _url = url;
            this.map = map;
        }

        public bool gotoMachete()
        {
            _d.Navigate().GoToUrl(_url);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool login(string user = "jadmin", string password = "ChangeMe")
        {
            _d.Navigate().GoToUrl(_url);

            //_d.FindElement(By.LinkText("Logon")).Click();
            WaitForText("Account Information", maxwait);
            _d.FindElement(By.Id("UserName")).Clear();
            _d.FindElement(By.Id("UserName")).SendKeys(user);
            _d.FindElement(By.Id("Password")).Clear();
            _d.FindElement(By.Id("Password")).SendKeys(password);
            _d.FindElement(By.Name("logonB")).Click();
            WaitForText("Welcome", maxwait);
            return true;
        }

        public bool logout()
        {
            _d.FindElement(By.LinkText("Logoff")).Click();
            WaitForText("Logon", maxwait);
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
            WaitThenClickElement(By.Id(prefix + "firstname1"));
            _d.FindElement(By.Id(prefix + "firstname1")).Clear();
            _d.FindElement(By.Id(prefix + "firstname1")).SendKeys(_per.firstname1);
            if (_per.firstname2 != null)
            {
                _d.FindElement(By.Id(prefix + "firstname2")).Clear();
                _d.FindElement(By.Id(prefix + "firstname2")).SendKeys(_per.firstname2);
            }
            _d.FindElement(By.Id(prefix + "lastname1")).Clear();
            _d.FindElement(By.Id(prefix + "lastname1")).SendKeys(_per.lastname1);
            if (_per.lastname2 != null)
            {
                _d.FindElement(By.Id(prefix + "lastname2")).Clear();
                _d.FindElement(By.Id(prefix + "lastname2")).SendKeys(_per.lastname2);
            }
            _d.FindElement(By.Id(prefix + "address1")).Clear();
            _d.FindElement(By.Id(prefix + "address1")).SendKeys(_per.address1);
            _d.FindElement(By.Id(prefix + "address2")).Clear();
            _d.FindElement(By.Id(prefix + "address2")).SendKeys(_per.address2);
            _d.FindElement(By.Id(prefix + "phone")).Clear();
            _d.FindElement(By.Id(prefix + "phone")).SendKeys(_per.phone);
            SelectOption(By.Id(prefix + "gender"), MacheteLookups.cache.First(c => c.category == "gender" && c.ID == _per.gender).text_EN);
            _d.FindElement(By.Id(prefix + "city")).Clear();
            _d.FindElement(By.Id(prefix + "city")).SendKeys(_per.city);
            if (_per.genderother != null)
            {
                _d.FindElement(By.Id(prefix + "genderOther")).Clear();
                _d.FindElement(By.Id(prefix + "genderOther")).SendKeys(_per.genderother);
            }
            WaitForElement(By.Id(prefix + "state")).Clear();
            WaitForElement(By.Id(prefix + "state")).SendKeys(_per.state);
            if (!_per.active)
                WaitForElement(By.Id(prefix + "active")).Click();
            WaitForElement(By.Id(prefix + "zipcode")).Clear();
            WaitForElement(By.Id(prefix + "zipcode")).SendKeys(_per.zipcode);
            WaitThenClickElement(By.Id(prefix + "SaveBtn"));
            //
            //WaitForElement(By.Id("personSearchBox")).SendKeys(_per.lastname1);
            //WaitForElementValue(By.XPath("//table[@id='personTable']/tbody/tr/td[4]"), _per.lastname1);
            //WaitAndDoubleClick(By.XPath("//table[@id='personTable']/tbody/tr/td[6]"));
            //
            var selectedTab = WaitForElement(By.CssSelector("li.person.ui-tabs-selected"));
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            _per.ID = Convert.ToInt32(tabAnchor.GetAttribute("recordid"));
            //
            return true;
        }
        public bool personValidate(Person _per)
        {
            string prefix = "person" + _per.ID + "-";
            var selectedTab = WaitForElement(By.CssSelector("li.person.ui-tabs-selected"));
            Assert.IsNotNull(selectedTab, "Failed to find Person selected tab element");
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            Assert.IsNotNull(tabAnchor, "Failed to find Person selected tab element anchor");
            var name = _per.firstname1 + " " + (_per.firstname2 != null ? _per.firstname2 + " " : "") + _per.lastname1 + (_per.lastname2 != null ? " " + _per.lastname2: "");

            Assert.IsTrue(tabAnchor.Text == name, "Person anchor label doesn't match person name");

            Assert.AreEqual(_per.firstname1, WaitForElement(By.Id(prefix + "firstname1")).GetAttribute("value"));
            Assert.AreEqual(_per.firstname2 == null ? "" : _per.firstname2, WaitForElement(By.Id(prefix + "firstname2")).GetAttribute("value"));
            Assert.AreEqual(_per.lastname1, WaitForElement(By.Id(prefix + "lastname1")).GetAttribute("value"));
            Assert.AreEqual(_per.lastname2 == null ? "" : _per.lastname2, WaitForElement(By.Id(prefix + "lastname2")).GetAttribute("value"));
            Assert.AreEqual(_per.address1, WaitForElement(By.Id(prefix + "address1")).GetAttribute("value"));
            Assert.AreEqual(_per.address2 == null ? "" : _per.address2, WaitForElement(By.Id(prefix + "address2")).GetAttribute("value"));
            Assert.AreEqual(_per.phone, WaitForElement(By.Id(prefix + "phone")).GetAttribute("value"));
            Assert.AreEqual(_per.city, WaitForElement(By.Id(prefix + "city")).GetAttribute("value"));
            Assert.AreEqual(_per.genderother == null ? "" : _per.genderother, WaitForElement(By.Id(prefix + "genderOther")).GetAttribute("value"));
            Assert.AreEqual(_per.state, WaitForElement(By.Id(prefix + "state")).GetAttribute("value"));
            Assert.AreEqual(_per.zipcode, WaitForElement(By.Id(prefix + "zipcode")).GetAttribute("value"));
            //Assert.AreEqual(_per.active, WaitForElement(By.Id(prefix + "active")).Selected);
            Assert.AreEqual(_per.gender.ToString(), GetOptionValue(By.Id(prefix + "gender"))); 
            return true;
        }

        public bool workerCreate(Worker _wkr, string imagepath)
        {
            string prefix = "worker"+_wkr.ID+"-";
            WaitThenClickElement(By.Id("workerCreateTab"));
            WaitForElement(By.Id(prefix + "dateOfMembership"));            
            _d.FindElement(By.Id(prefix + "dateOfMembership")).SendKeys(_wkr.dateOfMembership.ToShortDateString());
            _d.FindElement(By.Id(prefix + "dateOfBirth")).SendKeys(((DateTime)_wkr.dateOfBirth.Value).ToShortDateString());
            _d.FindElement(By.Id(prefix + "dateinUSA")).SendKeys(((DateTime)_wkr.dateinUSA.Value).ToShortDateString());
            _d.FindElement(By.Id(prefix + "dateinseattle")).SendKeys(((DateTime)_wkr.dateinseattle.Value).ToShortDateString());
            _d.FindElement(By.Id(prefix + "memberexpirationdate")).SendKeys(_wkr.memberexpirationdate.ToShortDateString());
            _d.FindElement(By.Id(prefix + "height")).SendKeys(_wkr.height);
            _d.FindElement(By.Id(prefix + "weight")).SendKeys(_wkr.weight);
            _d.FindElement(By.Id(prefix + "dwccardnum")).Clear();
            _d.FindElement(By.Id(prefix + "dwccardnum")).SendKeys(_wkr.dwccardnum.ToString());

            SelectOption(By.Id(prefix + "memberStatusID"), "Active");
            SelectOption(By.Id(prefix + "neighborhoodID"), "Primary City");
            SelectOption(By.Id(prefix + "typeOfWorkID"), @"Day Worker Center");
            SelectOption(By.Id(prefix + "englishlevelID"), "1");
            SelectOption(By.Id(prefix + "incomeID"), @"Poor (Less than $15,000)");
            _d.FindElement(By.Id(prefix + "imagefile")).SendKeys(imagepath);
            _d.FindElement(By.Id("createWorkerBtn")).Click();
            //
            //
            _d.FindElement(By.Id("workerCreateTab")).Click();            
            return true;
        }

        public bool workerSanction(Worker _wkr)
        {
            WaitThenClickElement(By.Id("workerCreateTab"));
            WaitForElement(By.Id(_wkr.idPrefix + "memberStatusID"));
            
            Thread.Sleep(5000);
            
            WaitThenClickElement(By.Id(_wkr.idPrefix + "memberStatusID"));
            SelectOption(By.Id(_wkr.idPrefix + "memberStatusID"), "Sanctioned");
            WaitThenClickElement(By.Id(_wkr.idPrefix + "SaveButton"));
            _d.FindElement(By.Id("workerCreateTab")).Click();  
            return true;
        }

        public bool workerValidate(Worker _wkr)
        {
            string prefix = "worker"+_wkr.ID+"-";
            bool result = WaitForElementValue(By.Id("workerCreateTab"), "Worker information");
            Assert.IsTrue(result, "Create tab label not updated by formSubmit");            
            Assert.AreEqual(_wkr.dateOfMembership.ToShortDateString(), WaitForElement(By.Id(prefix + "dateOfMembership")).GetAttribute("value"));
            Assert.AreEqual(((DateTime)_wkr.dateOfBirth.Value).ToShortDateString(), WaitForElement(By.Id(prefix + "dateOfMembership")).GetAttribute("value"));
            
            Thread.Sleep(5000);
            
            Assert.AreEqual(((DateTime)_wkr.dateinUSA).ToShortDateString(), WaitForElement(By.Id(prefix + "dateinUSA")).GetAttribute("value"));
            Assert.AreEqual(((DateTime)_wkr.dateinseattle).ToShortDateString(), WaitForElement(By.Id(prefix + "dateinseattle")).GetAttribute("value"));
            Assert.AreEqual(_wkr.memberexpirationdate.ToShortDateString(), WaitForElement(By.Id(prefix + "memberexpirationdate")).GetAttribute("value"));
            Assert.AreEqual(_wkr.height, WaitForElement(By.Id(prefix + "height")).GetAttribute("value"));
            Assert.AreEqual(_wkr.weight, WaitForElement(By.Id(prefix + "weight")).GetAttribute("value"));
            
            Thread.Sleep(5000);
            
            Assert.AreEqual(_wkr.dwccardnum.ToString(), WaitForElement(By.Id(prefix + "dwccardnum")).GetAttribute("value"));
            Assert.AreEqual("Active", GetOptionText(WaitForElement(By.Id(prefix + "memberStatusID"))));
            Assert.AreEqual("Primary City", GetOptionText(WaitForElement(By.Id(prefix + "neighborhoodID"))));
            Assert.AreEqual(@"Day Worker Center", GetOptionText(WaitForElement(By.Id(prefix + "typeOfWorkID"))));
            Assert.AreEqual("1", GetOptionText(WaitForElement(By.Id(prefix + "englishlevelID"))));
            Assert.AreEqual(@"Poor (Less than $15,000)", GetOptionText(WaitForElement(By.Id(prefix + "incomeID"))));
            return true;
        }

        public bool workerDelete(Worker _wkr)
        {
            WaitThenClickElement(By.Id("deleteWorkerButton-" + _wkr.ID.ToString()));
            WaitThenClickElement(By.Id("popup_ok"));

            return true;
        }

        public bool confirmWorkerDeleted()
        {
            Thread.Sleep(5000);
            bool result = WaitForElementValue(By.Id("workerCreateTab"), "Create Worker");
            Assert.IsTrue(result, "Create tab label not updated by formSubmit");

            return true;
        }

        public bool createSomeWorkers(int numWorkers, DbSet<Worker> workers)
        {
            for (int i = numWorkers; i >= 0; --i)
            {
                Person _per = (Person)Records.person.Clone();
                Worker _wkr = (Worker)Records.worker.Clone();
                Random rnd = new Random();

                while (workers.Where(q => q.dwccardnum == _wkr.dwccardnum).Count() > 0)
                {
                    _wkr.dwccardnum = rnd.Next(30000, 32000);
                }

                //personCreate(_per);
                
                _wkr.ID = _per.ID;
                //workerCreate(_wkr, SolutionDirectory() + "\\Machete.test\\jimmy_machete.jpg");
            }
            return true;
        }

        public bool eventCreate(Event _ev)
        {
            string prefix = "event"+ _ev.ID +"-";
            WaitThenClickElement(By.Id("eventCreateTab"));
            WaitForElement(By.Id(prefix + "eventType"));
            SelectOptionByValue(By.Id(prefix + "eventType"), _ev.eventTypeID.ToString());
            WaitForElement(By.Id(prefix + "dateFrom")).Clear();
            WaitForElement(By.Id(prefix + "dateFrom")).SendKeys(_ev.dateFrom.ToShortDateString());
            if (_ev.dateTo != null)
            {
                WaitForElement(By.Id(prefix + "dateTo")).Clear();
                WaitForElement(By.Id(prefix + "dateTo")).SendKeys(((DateTime)_ev.dateTo).ToShortDateString());
            }
            WaitForElement(By.Id(prefix + "notes")).SendKeys(_ev.notes);
            WaitThenClickElement(By.Id("eventCreateBtn"));
            
            Thread.Sleep(5000); // need tabs to change; should build label and wait on it

            var selectedTab = _d.FindElements(By.CssSelector("li.ui-tabs-selected"))[1];
            IWebElement tabAnchor = selectedTab.FindElement(By.CssSelector("a"));
            _ev.ID = Convert.ToInt32(tabAnchor.GetAttribute("recordid"));
            return true;
        }
        public bool eventValidate(ref Event _ev)
        {
            string prefix = "event" + _ev.ID + "-";
            WaitForElement(By.Id(prefix + "eventType"));
            Assert.AreEqual(_ev.eventTypeID.ToString(), GetOptionValue(By.Id(prefix + "eventType")));
            Assert.AreEqual(_ev.dateFrom.ToShortDateString(), WaitForElement(By.Id(prefix + "dateFrom")).GetAttribute("value"));
            if (_ev.dateTo != null)
                Assert.AreEqual(((DateTime)_ev.dateTo).ToShortDateString(), WaitForElement(By.Id(prefix + "dateTo")).GetAttribute("value"));
            Assert.AreEqual(_ev.notes, WaitForElement(By.Id(prefix + "notes")).GetAttribute("value"));
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
            ReplaceElementText(By.Id(prefix + "state"), _emp.state);
            ReplaceElementText(By.Id(prefix + "zipcode"), _emp.zipcode);
            ReplaceElementText(By.Id(prefix + "phone"), _emp.phone);
            ReplaceElementText(By.Id(prefix + "cellphone"), _emp.cellphone);
            ReplaceElementText(By.Id(prefix + "driverslicense"), _emp.driverslicense);
            ReplaceElementText(By.Id(prefix + "licenseplate"), _emp.licenseplate);
            //ReplaceElementText(By.Id(prefix + "businessname"), _emp.businessname);
            // select lists
            //http://stackoverflow.com/questions/4672658/how-do-i-set-a-an-option-as-selected-using-selenium-webdriver-selenium-2-0-cli
            //ReplaceElementText(By.Id(prefix + "referredby"), _emp.referredby.ToString());
            ReplaceElementText(By.Id(prefix + "email"), _emp.email);
            ReplaceElementText(By.Id(prefix + "notes"), _emp.notes);
            ReplaceElementText(By.Id(prefix + "referredbyOther"), _emp.referredbyOther);

            SelectOptionByIndex(By.Id(prefix + "active"), _emp.active ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "blogparticipate"), _emp.blogparticipate.Value ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "business"), _emp.business ? 2 : 1);
            SelectOption(By.Id(prefix + "referredby"), 
                MacheteLookups.cache.First(c => c.category == "emplrreference" && c.ID == _emp.referredby).text_EN);
            // save employer
            _d.FindElement(By.Id(prefix + "SaveBtn")).Click();
            //
            // check for duplicate dialog
            var dialog = WaitForElementExists(By.Id("duplicatesDialog"));
            if (dialog)
            {
                // This always fails the first time, and I do not know why. Reproduce with completely clean dir;
                // otherwise does not happen. Selenium pls
                WaitForElement(By.Id("duplicateSaveBtn"));
                _d.FindElement(By.Id("duplicateSaveBtn")).Click();
            }
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
            getAttributeAssertEqual(_emp.licenseplate, "licenseplate");
            getAttributeAssertEqual(_emp.driverslicense, "driverslicense");
            //getAttributeAssertEqual(_emp.businessname, "businessname");
            getAttributeAssertEqual(_emp.phone, "phone");
            getAttributeAssertEqual(_emp.cellphone, "cellphone");
            //getAttributeAssertEqual(_emp.referredby.ToString(), "referredby");
            getAttributeAssertEqual(_emp.email, "email");
            getAttributeAssertEqual(_emp.notes, "notes");
            getAttributeAssertEqual(_emp.referredbyOther, "referredbyOther");

            WaitForElement(By.Id(prefix + "active"));
            Assert.AreEqual(_emp.active ? 2 : 1, GetOptionIndex(By.Id(prefix + "active")));
            WaitForElement(By.Id(prefix + "blogparticipate"));
            Assert.AreEqual(_emp.blogparticipate.Value ? 2 : 1, GetOptionIndex(By.Id(prefix + "blogparticipate")));
            WaitForElement(By.Id(prefix + "business"));
            Assert.AreEqual(_emp.business ? 2 : 1, GetOptionIndex(By.Id(prefix + "business")));
            WaitForElement(By.Id(prefix + "referredbyOther"));
            Assert.AreEqual(_emp.referredby, MacheteLookups.cache.First(c => c.category == "emplrreference" && c.text_EN == GetOptionText(By.Id(prefix + "referredby"))).ID);
            return true;
        }

        public bool employerDelete(Employer _emp)
        {
            WaitThenClickElement(By.Id("deleteEmployerButton-" + _emp.ID.ToString()));
            WaitThenClickElement(By.Id("popup_ok"));

            Thread.Sleep(5000);
            WaitForElement(By.Id("employerTable_searchbox")).SendKeys(_emp.name);
            bool result = WaitForElementValue(By.XPath("//table[@id='employerTable']/tbody/tr/td[1]"), "No matching records found");
            Assert.IsTrue(result, "Employer not deleted properly");
            return true;
        }

        #endregion

        #region workorders
        public bool workOrderCreate(Employer _emp, WorkOrderMVC _wo)
        {
            string prefix = "WO0-";
            WaitThenClickElement(By.Id("workOrderCreateTab_" + _emp.ID));
            WaitForElement(By.Id(prefix + "contactName"));
            ReplaceElementText(By.Id(prefix + "contactName"), _wo.contactName);
            //ReplaceElementText(By.Id(prefix + "dateTimeofWork"), String.Format("{MM/dd/yyyy hh:mm tt}", _wo.dateTimeofWork));
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
            ReplaceElementText(By.Id(prefix + "zipcode"), _wo.zipcode);
            //ReplaceElementText(By.Id(prefix + "transportFee"), _wo.transportFee);
            //ReplaceElementText(By.Id(prefix + "transportFeeExtra"), _wo.transportFeeExtra);
            //ReplaceElementText(By.Id(prefix + "englishRequiredNote"), _wo.englishRequiredNote);
            ReplaceElementText(By.Id(prefix + "description"), _wo.description);

           // SelectOption(By.Id(prefix + "status"), MacheteLookup.cache.First(c => c.category == "orderstatus" && c.ID == _wo.status).text_EN);
            SelectOptionByIndex(By.Id(prefix + "transportMethodID"), _wo.transportProviderID);
            //SelectOptionByIndex(By.Id(prefix + "transportTransactType"), _wo.transportTransactType);
            SelectOptionByIndex(By.Id(prefix + "timeFlexible"), _wo.timeFlexible ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "permanentPlacement"), _wo.permanentPlacement ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "englishRequired"), _wo.englishRequired ? 2 : 1);
            SelectOptionByIndex(By.Id(prefix + "lunchSupplied"), _wo.lunchSupplied ? 2 : 1);
            
            Thread.Sleep(1000); //prevent race condition
            if (_wo.workerRequestsAAA != null)
                foreach (var request in _wo.workerRequestsAAA)
                {
                    WaitThenClickElement(By.Id("addRequestBtn-" + _wo.ID));
                    ReplaceElementText(By.XPath("//*[@id='workerTable-0_filter']/label/input"), request.ID.ToString());
                    WaitThenClickElement(By.XPath("//*[@id='workerTable-0']/tbody/tr/td[1]"));
                }

            Thread.Sleep(1000); //prevent race condition
            //
            // save work order
            _d.FindElement(By.Id(prefix + "SaveBtn")).Click();
            //
            // Find new work order tab (css class "WO"), get embedded WOID, populate
            // WO object

            _wo.ID = getSelectedTabRecordID("WO");
            _wo.paperOrderNum = _wo.ID;
            _wo.tablabel = Machete.Web.Resources.WorkOrders.tabprefix +
                                                            Convert.ToString(_wo.paperOrderNum) +
                                                            " @ " + _wo.workSiteAddress1;
            Assert.IsTrue(_d.FindElement(By.CssSelector("li.WO.ui-tabs-selected > a"))
                                            .Text == _wo.tablabel, 
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

        public bool workOrderValidate(Web.ViewModel.WorkOrderMVC _wo) 
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
            //Assert.IsTrue(WaitForElement(By.Id(prefix + "paperOrderNum")).GetAttribute("value") != "", "paper order number is empty");
            //getAttributeAssertEqual(_wo.paperOrderNum.ToString(), "paperOrderNum");
            getAttributeAssertEqual(_wo.workSiteAddress1, "workSiteAddress1");
            getAttributeAssertEqual(_wo.workSiteAddress2, "workSiteAddress2");
            getAttributeAssertEqual(_wo.phone, "phone");
            getAttributeAssertEqual(_wo.city, "city");
            getAttributeAssertEqual(_wo.state, "state");
            getAttributeAssertEqual(_wo.zipcode, "zipcode");
            getAttributeAssertEqual(_wo.description, "description");
            //getAttributeAssertEqual(_wo.transportTransactID, "transportTransactID");

            WaitForElement(By.Id(prefix + "status"));
            string optionText = GetOptionText(By.Id(prefix + "status"));
            Assert.AreEqual(_wo.statusID, MacheteLookups.cache.First(c => c.category == "orderstatus" && c.text_EN == optionText).ID);
            WaitForElement(By.Id(prefix + "timeFlexible"));
            Assert.AreEqual(_wo.timeFlexible ? 2:1, GetOptionIndex(By.Id(prefix + "timeFlexible")));
            WaitForElement(By.Id(prefix + "permanentPlacement"));
            Assert.AreEqual(_wo.permanentPlacement ? 2 : 1, GetOptionIndex(By.Id(prefix + "permanentPlacement")));
            WaitForElement(By.Id(prefix + "englishRequired"));
            Assert.AreEqual(_wo.englishRequired ? 2 : 1, GetOptionIndex(By.Id(prefix + "englishRequired")));
            WaitForElement(By.Id(prefix + "lunchSupplied"));
            Assert.AreEqual(_wo.lunchSupplied ? 2 : 1, GetOptionIndex(By.Id(prefix + "lunchSupplied")));
            WaitForElement(By.Id(prefix + "transportMethodID"));
            Assert.AreEqual(_wo.transportProviderID, GetOptionIndex(By.Id(prefix + "transportMethodID")));
            //WaitForElement(By.Id(prefix + "transportTransactType"));
            //Assert.AreEqual(_wo.transportTransactType, GetOptionIndex(By.Id(prefix + "transportTransactType")));
            WaitForElement(By.Id("workerRequests2_WO-" + _wo.ID));
            if (_wo.workerRequestsAAA != null)
                foreach (var request in _wo.workerRequestsAAA)
                {
                    WaitForElementValue(By.XPath("//*[@id='workerRequests2_WO-" + _wo.ID + "']/option"), request.workerRequested.fullNameAndID);
                }
            return true;
        }
        #endregion

        #region WorkAssignments
        /// <summary>
        /// Create new assignment. Last action executes Create Button, returning to List Tab.
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="wo"></param>
        /// <param name="wa"></param>
        /// <returns></returns>
        public bool workAssignmentCreate(Employer emp, WorkOrderMVC wo, WorkAssignmentMVC wa, FluentRecordBase frb)
        {
            // Click on the assignment Create Tab
            WaitThenClickElement(By.Id("wact-" + wo.ID)); //the ID here is the WorkOrder.ID, not the Employer.ID
            WaitForElement(By.Id(wa.idPrefix + "description"));
            SelectOption(By.Id(wa.idPrefix + "englishLevelID"), wa.englishLevelID.ToString());
            SelectOptionByValue(By.Id(wa.idPrefix + "skillID"), wa.skillID.ToString());
            ReplaceElementText(By.Id(wa.idPrefix + "hourlyWage"), wa.hourlyWage.ToString());
            ReplaceElementText(By.Id(wa.idPrefix + "hours"), wa.hours.ToString());
            if (wa.hourRange.ToString().Length > 0)
                SelectOption(By.Id(wa.idPrefix + "hourRange"), wa.hourRange.ToString());
            SelectOption(By.Id(wa.idPrefix + "days"), wa.days.ToString());
            ReplaceElementText(By.Id(wa.idPrefix + "description"), wa.description);

            WaitForElement(By.Id("WO" + wo.ID + "-waCreateBtn")).Click();
            Thread.Sleep(1000);
            wa.ID = getSelectedTabRecordID("WA");
            wa.pseudoID = frb.ToServ<Machete.Service.IWorkAssignmentService>().Get(wa.ID).pseudoID;
            wa.tablabel = "Assignment #: " + System.String.Format("{0,5:D5}-{1,2:D2}", wo.paperOrderNum, wa.pseudoID);
            WaitForElement(By.Id(wa.idPrefix + "EditTab"));
            WaitThenClickElement(By.Id("walt-" + wo.ID));
            return true;
        }
        /// <summary>
        /// Activate work order. Assumes start at WA ListTab. 
        /// </summary>
        /// <param name="_emp"></param>
        /// <param name="_wo"></param>
        /// <param name="_wa"></param>
        /// <returns></returns>
        public bool workAssignmentActivate(Employer _emp, WorkOrderMVC _wo, WorkAssignmentMVC _wa)
        {
            // Verify we're on the WA ListTab we expected
            WaitForElement(By.Id("walt-" + _wo.ID));
            // Look for WA datatable to have a first row (at least one record)
            By walt = By.XPath("//table[@id='workAssignTable-wo-" + _wo.ID + "']/tbody/tr/td[1]");
            // The #####-## order number from the first column
            
            Thread.Sleep(5000);
            
            var waltText = "Assignment #: " + WaitForElement(walt).Text;
            WaitForElementValue(walt, _wa.tablabel);
            
            Thread.Sleep(5000);
            
            Assert.AreEqual(_wa.tablabel, waltText, "Unexpected PseudoID in assignment's list");
            Thread.Sleep(1000);
            WaitThenClickElement(By.Id("activateWorkOrderButton-" + _wo.ID));
            // todo: find a way to change this hard-coded value assignment
            _wo.statusID = 40; // changing test object to reflect activate status from previous action
            return true;
        }

        public static string GetID(string s, params object[] list)
        {
            return String.Format(s, list);

        }

        public bool workAssignmentValidate(Employer _emp, WorkOrderMVC _wo, WorkAssignmentMVC _wa)
        {
            Thread.Sleep(1000);
            WaitThenClickElement(By.Id("WO" + _wo.ID + "-EditTab"));
            Thread.Sleep(1000);
            WaitThenClickElement(By.Id("WO" + _wo.ID + "-EditTab"));
            Thread.Sleep(1000);
            WaitForElement(By.Id("walt-" + _wo.ID));
            WaitThenClickElement(By.Id("walt-" + _wo.ID));
            Thread.Sleep(1000);
            WaitForElement(By.Id("workAssignTable-wo-" + _wo.ID + "_searchbox"));
            string idString = _wo.ID.ToString("D5") + "-" + ((int)_wa.pseudoID).ToString("D2");
            ReplaceElementText(By.Id("workAssignTable-wo-" + _wo.ID + "_searchbox"), idString);
            Thread.Sleep(1000);
            string xpath = "//*[@id='workAssignTable-wo-" + _wo.ID + "']/tbody/tr/td[.='" + idString + "']";
            Assert.IsTrue(WaitAndDoubleClick(By.XPath(xpath)),
                "Cannot find work assignment row to click.");

            //Now, check each of the fields

            string prefix = "WA" + _wa.ID + "-";

            WaitForElement(By.Id(prefix + "englishLevelID"));
            WaitForElement(By.Id(prefix + "hours")); //Doing a lot of waits up front here because I was having some problems with the englishLevelID getting
            WaitForElement(By.Id(prefix + "hourRange")); //detached from the DOM if I grab it too early. A short sleep seemed to work, but waits make sure.
            WaitForElement(By.Id(prefix + "days"));
            WaitForElement(By.Id(prefix + "skillID"));
            WaitForElement(By.Id(prefix + "hourlyWage"));
            WaitForElement(By.Id(prefix + "total"));
            Thread.Sleep(50);
            Assert.AreEqual(_wa.englishLevelID.ToString(), GetOptionValue(By.Id(prefix + "englishLevelID")));
            Assert.AreEqual(System.String.Format("{0,2:N2}", _wa.hours), WaitForElement(By.Id(prefix + "hours")).GetAttribute("value"));
            if (_wa.hourRange != null)
                Assert.AreEqual(_wa.hourRange, GetOptionIndex(By.Id(prefix + "hourRange")) + 6);
            Assert.AreEqual(_wa.days, GetOptionIndex(By.Id(prefix + "days")));
            string skillIDValue = GetOptionValue(By.Id(prefix + "skillID"));
            Assert.AreEqual(_wa.skillID.ToString(), skillIDValue);
            WaitForElement(By.Id(prefix + "hourlyWage"));
            Assert.AreEqual(_wa.hourlyWage.ToString("F"), WaitForElement(By.Id(prefix + "hourlyWage")).GetAttribute("value"));
            WaitForElement(By.Id(prefix + "total"));
            Assert.AreEqual("$" +(_wa.hourlyWage * _wa.hours * _wa.days).ToString("F"), WaitForElement(By.Id(prefix + "total")).GetAttribute("value"));
            Assert.AreEqual(_wa.pseudoID.ToString(), WaitForElement(By.Id(prefix + "pseudoID")).GetAttribute("Value"));

            // Now change field values and check field relationships

            //reset to default values
            SelectOptionByIndex(By.Id(prefix + "englishLevelID"), 0);
            SelectOptionByIndex(By.Id(prefix + "skillID"), 0);
            ReplaceElementText(By.Id(prefix + "hourlyWage"), "0");
            ReplaceElementText(By.Id(prefix + "hours"), "0");
            SelectOptionByIndex(By.Id(prefix + "hourRange"), 0);
            SelectOptionByIndex(By.Id(prefix + "days"), 0);
            SelectOptionByIndex(By.Id(prefix + "hourRange"), 0);

            //check default form response
            Assert.AreEqual("not calculable", WaitForElement(By.Id(prefix + "total")).GetAttribute("value"));
            Assert.AreEqual("not calculable", WaitForElement(By.Id(prefix + "totalRange")).GetAttribute("value"));
            
            //change skill and make sure wage and hours changed
            SelectOptionByIndex(By.Id(prefix + "skillID"), 1);
            Assert.IsFalse(Convert.ToInt32(WaitForElement(By.Id(prefix + "hourlyWage")).GetAttribute("value")) == 0, "Hourly Wage failed reaction to skill selection");
            Assert.IsFalse(Convert.ToInt32(WaitForElement(By.Id(prefix + "hours")).GetAttribute("value")) == 0, "Hours failed reaction to skill selection");
            //var indexResult = GetOptionIndex(By.Id(prefix + "hours"));
            //Assert.AreEqual(0, indexResult, "Hours dropdown failed reaction to skill selection");

            //set hourly range and days then check total and max total
            SelectOptionByIndex(By.Id(prefix + "hourRange"), 3);
            SelectOptionByIndex(By.Id(prefix + "days"), 1);

            int hourlyWage = Convert.ToInt32(WaitForElement(By.Id(prefix + "hourlyWage")).GetAttribute("value"));
            int hoursWork = Convert.ToInt32(WaitForElement(By.Id(prefix + "hours")).GetAttribute("value"));
            int hourRange = Convert.ToInt32(GetOptionValue(By.Id(prefix + "hourRange")));
            int daysWork = Convert.ToInt32(GetOptionValue(By.Id(prefix + "days")));

            Assert.AreEqual("$" + (hourlyWage * hoursWork * daysWork).ToString("F"), WaitForElement(By.Id(prefix + "total")).GetAttribute("value"), "Total pay doesn't match hour, wage and day calculation");
            Assert.AreEqual("$" + (hourlyWage * hourRange * daysWork).ToString("F"), WaitForElement(By.Id(prefix + "totalRange")).GetAttribute("value"), "Max Total pay doesn't match hourRange, wage and day calculation");

            //select fixed job and verify hourly pay is fixed
            // TODO: Find another way of testing this. I took fixed categories out of the Lookups.
            // Centers should be aware that they *can* fix pay in the config, but Casa Latina is the
            // only center that's had that as a requirement for certain types of jobs.

            //String skillValue = MacheteLookup.cache.First(x => x.category == LCategory.skill && x.text_EN == "DWC Chambita 1hr").ID.ToString();
            //SelectOptionByValue(By.Id(prefix + "skillID"), skillValue);
            //Thread.Sleep(1000); // to avoid race condition
            //Assert.AreEqual("true", WaitForElement(By.Id(prefix + "hourlyWage")).GetAttribute("disabled"), "Hourly Wage should be fixed (disabled) in response to skill selection");

            return true;
        }
        #endregion

        #region activities
        public bool activityMenuLink()
        {
            // find Activity link in menu
            var activityMenuLink = WaitThenClickElement(By.Id("menulinkactivity"));
            Assert.IsNotNull(activityMenuLink, "Failed to find Activity menu link");

            return true;

        }
        public bool activityCreate(Activity _act)
        {
            string prefix = "activity0-";
            //Go to activites page
            WaitThenClickElement(By.Id("menulinkactivity"));
            //Go to create an activity tab
            WaitThenClickElement(By.Id("activityCreateTab"));
            //Wait for page to load
            WaitForElement(By.Id(prefix + "type"));
            //Enter information
            SelectOptionByValue(By.Id(prefix + "type"), _act.typeID.ToString());
            WaitForElement(By.Id(prefix + "name"));
            SelectOptionByValue(By.Id(prefix + "name"), _act.nameID.ToString());
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

            Assert.AreEqual(1, GetOptionIndex(By.Id(prefix + "name")));
            Assert.AreEqual(1, GetOptionIndex(By.Id(prefix + "type")));
            Assert.AreEqual(_act.teacher, GetOptionText(By.Id(prefix + "teacher")));
            Assert.AreEqual(_act.notes, WaitForElement(By.Id(prefix + "notes")).GetAttribute("value"));
            return true;
        }
        public bool activitySignIn(string idPrefix, int dwccardnum)
        {
            var textboxID = idPrefix + "dwccardnum";
            var submitID = idPrefix + "submit";
            WaitForElement(By.Id(textboxID));
            ReplaceElementText(By.Id(textboxID), dwccardnum.ToString());
            WaitThenClickElement(By.Id(submitID));

            return true;
        }
        public bool activitySignInIsSanctioned()
        {
            //var sanctionedBox = WaitForElement(By.ClassName("ui-dialog"));
            if (WaitForElementExists(By.ClassName("ui-dialog"))) {
                var elem = _d.FindElement(By.ClassName("ui-dialog"));
                if (elem.Displayed) {
                    elem.FindElement(By.ClassName("ui-button")).Click();
                    return true;
                }
            }

            return false;
        }
        public bool activitySignInValidate(string idPrefix, int dwccardnum, int rowcount)
        {
            // Example of validating a row that returns from a sign-in
            return WaitForElementValue(By.XPath("//table[@id='"+idPrefix+"asiTable']/tbody/tr[" + rowcount + "]/td[2]"), dwccardnum.ToString());
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
        public string GetOptionText(IWebElement elem)
        {
            var selectElem = new SelectElement(elem);
            return selectElem.SelectedOption.Text;
        }

        public int GetOptionIndex(By by)
        {
            var dropdown = _d.FindElement(by);
            SelectElement selectElem;
            selectElem = new SelectElement(dropdown);
            return selectElem.Options.IndexOf(selectElem.SelectedOption);
        }
        public string GetOptionValue(By by)
        {
            var dropdown = _d.FindElement(by);
            var selectElem = new SelectElement(dropdown);
            return selectElem.SelectedOption.GetAttribute("value");
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
            if (elem != null && elem.Displayed) 
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
                    if (elem != null && elem.Displayed) return elem;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Waiting for element {by}; Exception:\r\n{ex}\r\nInnerException:\r\n{ex.InnerException}");
                }
                Thread.Sleep(_sleepFor);
            }
            throw new NotFoundException();
        }

        public bool WaitForElementExists(By by)
        {
            bool elem = false;
            for (int second = 0; second < maxwait; second++)
            {
                elem = elementExists(by);
                if (elem) 
                    return true;
                else 
                    Thread.Sleep(_sleepFor);
            }
            return false;
        }

        /// <summary>
        /// Wait for both an element to exist and for the specified value to be present.
        /// </summary>
        /// <param name="by"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
                Thread.Sleep(_sleepFor);
            }
            return false;
        }

        public bool WaitForText(String what)
        {
            return WaitForText(what, maxwait);
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
                Thread.Sleep(_sleepFor);
            }
            return false;
        }
        public IWebElement WaitForElementDisplayed(By by)
        {
            IWebElement elem;
            for (int second = 0; second < maxwait; second++)
            {
                try
                {
                    elem = IsElementPresent(by);
                    if (elem != null && elem.Displayed == true) return elem;
                }
                catch (Exception)
                { return null; }
                Thread.Sleep(_sleepFor);
            }
            return null;
        }
        #endregion

        public bool elementExists(By by)
        {
            try
            {
                _d.FindElement(by);
            }
            catch(NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        #region privatemethods
        /// <summary>
        /// Returns element or null if not found. Catches Selenium exceptions.
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
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
        }
        /// <summary>
        /// Find element by the given By object and return true if the value matches the attribute value.
        /// </summary>
        /// <param name="by"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        public static string SolutionDirectory()
        {
            //string solutionDirectory = ((EnvDTE.DTE)System.Runtime
            //                  .InteropServices
            //                  .Marshal
            //                  .GetActiveObject("VisualStudio.DTE.11.0"))
            //       .Solution
            //       .FullName;
            return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public static int nextAvailableDwccardnum(MacheteContext DB)
        {
            int attempt = 30000;
            while (DB.Workers.Any(x => x.dwccardnum == attempt))
            {
                ++attempt;
            }
            return attempt;
        }
    }
}
