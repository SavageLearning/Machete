using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading;
using Machete.Web.Maps;
using Machete.Test.Integration.Fluent;

namespace Machete.Test.Selenium.View
{
    [TestClass]
    public class ActivityTests 
    {
        static IWebDriver driver;
        private StringBuilder verificationErrors;
        static string baseURL;
        private sharedUI ui;
        FluentRecordBase frb;
        static IMapper map;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            WebServer.StartIis();
            var webMapperConfig = new MapperConfiguration(config => config.ConfigureMvc());
            map = webMapperConfig.CreateMapper();
        }

        [TestInitialize]
        public void SetupTest()
        {
            frb = new FluentRecordBase();
            driver = new ChromeDriver(ConfigurationManager.AppSettings["CHROMEDRIVERPATH"]);
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL, map);
            verificationErrors = new StringBuilder();
            ui.login();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            //Logon
            //ui.login();

            //Logoff
            Assert.AreEqual("", verificationErrors.ToString());
            ui.WaitForElement(By.LinkText("Logoff"));
            driver.FindElement(By.LinkText("Logoff")).Click();
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                //ignoring errors if we can't close the browser.
            }
        }
        [ClassCleanup]
        public static void ClassCleanup() { WebServer.StopIis(); }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_Create_Validate()
        {
            //Arrange
            var _act = (Web.ViewModel.Activity)ViewModelRecords.activity.Clone();
            //Act
            ui.activityCreate(_act);
            //Assert
            ui.activityValidate(_act);
        }

        [Ignore, TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_Create_ManySignins()
        {
            //Arrange
            int rowcount = 1;                        
            Random rand = new Random();

            if (frb.ToFactory().Workers.Select(q => q.dwccardnum).Distinct().Count() <= 10)
            {
                frb.AddWorker();
            }
            //
            //
            IEnumerable<int> list = frb.ToFactory().Workers.Select(q => q.dwccardnum).Distinct().ToList();
            var count = list.Count();
            int numberOfSignins = rand.Next(count / 10) + 1; //+1 will never lead to zero here
            int numberSignedIn = numberOfSignins;
            IEnumerable<int> list1 = list.Take<int>(numberOfSignins);
            var _act = (Web.ViewModel.Activity)ViewModelRecords.activity.Clone();

            //Act
            ui.activityCreate(_act);
            var idPrefix = "asi"+_act.ID+"-"; 
 
            for (var i = 0; i < numberOfSignins; i++)
            {
                int cardNum = list1.ElementAt(i);
                ui.activitySignIn(idPrefix, cardNum);
                Thread.Sleep(1000);//prevent race condition
                if (ui.activitySignInIsSanctioned()) {
                    --numberSignedIn;
                    continue;
                }
                
                //Assert
                Thread.Sleep(1000);//prevent race condition
                Assert.IsTrue(ui.activitySignInValidate(idPrefix, cardNum, rowcount), "Sign in for worker " + i + " with cardNum " + cardNum + " failed!");

                //This line ensures the test doesn't break if we try to sign in an ID that has multiple workers attached to it.
                //rowcount increments by the number of records found in the database matching that cardNum
                rowcount += frb.ToFactory().Workers.Where(q => q.dwccardnum == cardNum).Count();
            }
            ui.WaitThenClickElement(By.Id("activityListTab"));
            //ui.SelectOption(By.XPath("//*[@id='activityTable_length']/label/select"), "100");
            
            //Assert

            // Chaim 4/2/2014 
            // This isn't working because I disabled auto-reload. It was making
            // the table appear strangely when I disabled pagination. I did that
            // because pagination was unpopular with users.

            // Todo: Either make auto-reload work or find another way of reloading
            // the page.

            //Locate record within activitylist datatable and compare the count (column 4) with numberSignedIn
            Assert.AreEqual(numberSignedIn.ToString(), ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']/td[4]")).Text);

            //walk through pagination to search for recordid
            //var activityRecordCount = "what";
            //bool tableRecordMatch = false;
            //while (tableRecordMatch == false) {
            //    if (ui.WaitForElementExists(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']"))) {
            //        tableRecordMatch = true;
            //        activityRecordCount = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']/td[4]")).Text;
            //    } else {
                    //check for #activityTable_next.paginate_disabled_next
                    //Assert.IsTrue(ui.WaitThenClickElement(By.CssSelector("#activityTable_next.paginate_enabled_next")), "Could not locate record in table pagination");
                //}
            //}

            //Assert.AreEqual(numberSignedIn.ToString(), activityRecordCount);
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_Create_signin_simple()
        {
            // Arrange
            int rowcount = 1;
            var _act = (Web.ViewModel.Activity)ViewModelRecords.activity.Clone();
            frb.ToServ<ILookupService>().populateStaticIds();
            var _asi = (Web.ViewModel.ActivitySignin)ViewModelRecords.activitysignin.Clone();
            var worker = frb.AddWorker(status: Worker.iActive).ToWorker();
            // Act
            ui.gotoMachete();
            ui.activityCreate(_act);
            var idPrefix = "asi" + _act.ID + "-"; 
            ui.activitySignIn(idPrefix, worker.dwccardnum);
            var result = ui.activitySignInValidate(idPrefix, worker.dwccardnum, rowcount);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_Signin_random_worker()
        {
            //Arrange
            var _act = (Web.ViewModel.Activity)ViewModelRecords.activity.Clone();
            IEnumerable<int> cardList = frb.ToFactory().Workers.Select(q => q.dwccardnum).Distinct();
            Random rand = new Random();
            int randCardIndex = rand.Next(cardList.Count());
            int rowCount = 1;
            int randCard = cardList.ElementAt(randCardIndex);
            while (frb.ToFactory().Workers.First(c => c.dwccardnum == randCard).isSanctioned)
            {
                randCardIndex = rand.Next(cardList.Count());
                randCard = cardList.ElementAt(randCardIndex);
            }
            //Act
            ui.activityCreate(_act);
            var idPrefix = "asi" + _act.ID + "-"; 
            ui.activitySignIn(idPrefix, randCard);

            //Assert
            Assert.IsTrue(ui.activitySignInValidate(idPrefix, randCard,rowCount));
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_Signin_random_sactioned_worker()
        {
            //Arrange
            var _act = (Web.ViewModel.Activity)ViewModelRecords.activity.Clone();
            var _sanctionedW = frb.AddWorker(status: Domain.Worker.iSanctioned, memberexpirationdate: DateTime.Now.AddDays(-1)).ToWorker();
            ui.gotoMachete();
            ui.activityCreate(_act);
            var idPrefix = "asi" + _act.ID + "-"; 
            ui.activitySignIn(idPrefix, _sanctionedW.dwccardnum);

            //Assert
            var result = ui.activitySignInValidate(idPrefix, _sanctionedW.dwccardnum, 1);
            Assert.IsFalse(result);
            Assert.IsTrue(ui.activitySignInIsSanctioned(), "Sanctioned worker box is not visible like it should be.");
        }

        [Ignore] // Pagination disabled on Activities page
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_test_pagination()
        {
            // Arrange
            FluentRecordBase SeDB = new FluentRecordBase();
            int count = SeDB.ToServ<IActivityService>().GetAll().Count();
            if (count < 20)
            {
                Domain.Person _person = (Domain.Person)Records.person.Clone();
                SeDB.ToServ<IPersonService>().Create(_person, "ME");
            }

            // Act
            ui.WaitThenClickElement(By.Id("menulinkactivity"));
            var recordID = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr")).GetAttribute("recordid");
            ui.WaitThenClickElement(By.CssSelector("#activityTable_next.paginate_enabled_next"));
            Thread.Sleep(1000); // Prevent race condition
            var recordIDPage = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr")).GetAttribute("recordid");

            // Assert
            Assert.AreNotEqual(recordID, recordIDPage, "Pagination for Activities List appears to not be working");
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_test_search()
        {
            // Arrange
            FluentRecordBase SeDB = new FluentRecordBase();
            int count = SeDB.ToServ<IActivityService>().GetAll().Count();
            if (count < 20)
            {
                var _activity = (Domain.Activity)Records.activity.Clone();
                SeDB.ToServ<IActivityService>().Create(_activity, "ME");
            }

            // Act
            ui.WaitThenClickElement(By.Id("menulinkactivity"));
            
            // Test bad search first
            ui.WaitForElement(By.Id("activityTable_searchbox")).SendKeys("bk45kjdsgjk4j3lkt6j3lkjgre");
            bool result =ui.WaitForElementValue(By.XPath("//table[@id='activityTable']/tbody/tr/td[1]"), "No matching records found");
            Assert.IsTrue(result, "Activity search results should be empty");

            // Test good search first
            ui.WaitForElement(By.Id("activityTable_searchbox")).Clear();
            ui.WaitForElement(By.Id("activityTable_searchbox")).SendKeys("jadmin");
            result = ui.WaitForElementValue(By.XPath("//table[@id='activityTable']/tbody/tr[5]/td[3]"), "jadmin");
            Assert.IsTrue(result, "Activities search not returning proper results");
        }

        [Ignore] // Pagination has been disabled
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_test_record_limit()
        {
            // Arrange
            int count = frb.ToServ<IActivityService>().GetAll().Count();
            while (count < 100)
            {
                var _activity = (Domain.Activity)Records.activity.Clone();
                frb.ToServ<IActivityService>().Create(_activity, "ME");
                count = frb.ToServ<IActivityService>().GetAll().Count();
            }

            // Act
            ui.WaitThenClickElement(By.Id("menulinkactivity"));

            // Test default
            ui.WaitForText("Showing (1 to 10)");
            //Thread.Sleep(3000); //prevent race condition
            int recCount = ui._d.FindElements(By.XPath("//table[@id='activityTable']/tbody/tr")).Count;
            Assert.AreEqual(recCount, 10, "Default record limiter is not set to 10");

            // Test 25
            ui.SelectOption(By.XPath("//*[@id='activityTable_length']/label/select"), "25");
            Thread.Sleep(1000); //prevent race condition
            recCount = ui._d.FindElements(By.XPath("//table[@id='activityTable']/tbody/tr")).Count;
            Assert.AreEqual(recCount, 25, "Record limiter set to 25 is not working");

            // Test 50
            ui.SelectOption(By.XPath("//*[@id='activityTable_length']/label/select"), "50");
            Thread.Sleep(1000); //prevent race condition
            recCount = ui._d.FindElements(By.XPath("//table[@id='activityTable']/tbody/tr")).Count;
            Assert.AreEqual(recCount, 50, "Record limiter set to 50 is not working");

            // Test 100
            ui.SelectOption(By.XPath("//*[@id='activityTable_length']/label/select"), "100");
            Thread.Sleep(1000); //prevent race condition
            recCount = ui._d.FindElements(By.XPath("//table[@id='activityTable']/tbody/tr")).Count;
            Assert.AreEqual(recCount, 100, "Record limiter set to 100 is not working");
        }

       [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Activities)]
        public void SeActivity_test_column_sorting()
        {
            // Arrange
            int count = frb.ToServ<IActivityService>().GetAll().Count();
            if (count < 100)
            {
                var _activity = (Domain.Activity)Records.activity.Clone();
                frb.ToServ<IActivityService>().Create(_activity, "ME");
            }

            // Act
            ui.WaitThenClickElement(By.Id("menulinkactivity"));
            Thread.Sleep(1000);
            // Test default - End Time column should be sort ascending
            IWebElement defaultSortCol = ui.WaitForElement(By.XPath("//th[contains(@class, 'sorting_desc')]"));
            Assert.AreEqual("Start time", defaultSortCol.Text, "Activity Start time isn't the default sort column");

            //Test Attendance Ascending
            ui.WaitThenClickElement(By.XPath("//th[contains(.,'Attendance')]"));
            Thread.Sleep(1000);
            int recVal = Convert.ToInt32(ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr/td[4]")).Text);
            Assert.AreEqual(0, recVal, "Activity attendance ascending sort isn't working");

            //Test Attendance Descending
            ui.WaitThenClickElement(By.XPath("//th[contains(.,'Attendance')]"));
            Thread.Sleep(1000);
            int recValDesc = Convert.ToInt32(ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr/td[4]")).Text);
            Assert.IsTrue(recValDesc > recVal, "Activity attendance desccending sort isn't working");

        }

    }

}