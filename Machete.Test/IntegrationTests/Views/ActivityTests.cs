using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Machete.Test
{
    [TestClass]
    public class ActivityTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private sharedUI ui;
        private DatabaseFactory _dbfactory;
        private WorkerService _wserv;
        private WorkerRepository _wRepo;
        private IUnitOfWork _unitofwork;
        private MacheteContext DB;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) { }

        [TestInitialize]
        public void SetupTest()
        {
            Database.SetInitializer<MacheteContext>(new MacheteInitializer());
            DB = new MacheteContext();
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            _dbfactory = new DatabaseFactory();
            _wRepo = new WorkerRepository(_dbfactory);
            _unitofwork = new UnitOfWork(_dbfactory);
            _wserv = new WorkerService(_wRepo, _unitofwork);
            driver = new FirefoxDriver();
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL);
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
        public static void ClassCleanup() { }

        [TestMethod]
        public void SeActivity_Create_Validate()
        {
            //Arrange
            Activity _act = (Activity)Records.activity.Clone();
            //Act
            ui.activityCreate(_act);
            //Assert
            ui.activityValidate(_act);
        }

        [TestMethod]
        public void SeActivity_Create_ManySignins()
        {
            //Arrange
            int rowcount = 1;                        
            MacheteContext DB = new MacheteContext(); //maps to Machete.Text\app.config ConnectionString
            IEnumerable<int> cardlist = DB.Workers.Select(q => q.dwccardnum).Distinct();
            Random rand = new Random();
            // There's a lot in this one line. Ask questions about it.
            //                         [DbSet] (LINQ)(Lambda Expression) (SQL-ish)
            IEnumerable<int> list = DB.Workers.Select(q => q.dwccardnum).Distinct().ToList();
             var count = list.Count();
            if (count < 10)
                ui.createSomeWorkers(10, DB.Workers);
            int numberOfSignins = rand.Next(count / 10) + 1; //+1 will never lead to zero here
            int numberSignedIn = numberOfSignins;
            IEnumerable<int> list1 = list.Take<int>(numberOfSignins);
            Activity _act = (Activity)Records.activity.Clone();

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
                Thread.Sleep(4000);//prevent race condition
                Assert.IsTrue(ui.activitySignInValidate(idPrefix, cardNum, rowcount), "Sign in for worker " + i + " with cardNum " + cardNum + " failed!");

                //This line ensures the test doesn't break if we try to sign in an ID that has multiple workers attached to it.
                //rowcount increments by the number of records found in the database matching that cardNum
                rowcount += DB.Workers.Where(q => q.dwccardnum == cardNum).Count();
            }
            ui.WaitThenClickElement(By.Id("activityListTab"));
            ui.SelectOption(By.XPath("//*[@id='activityTable_length']/label/select"), "100");
            
            //Assert

            //Locate record within activitylist datatable and compare the count (column 4) with numberSignedIn
            //Assert.AreEqual(numberSignedIn.ToString(), ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']/td[4]")).Text);

            //walk through pagination to search for recordid
            var activityRecordCount = "what";
            bool tableRecordMatch = false;
            while (tableRecordMatch == false) {
                if (ui.WaitForElementExists(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']"))) {
                    tableRecordMatch = true;
                    activityRecordCount = ui.WaitForElement(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']/td[4]")).Text;
                } else {
                    //check for #activityTable_next.paginate_disabled_next
                    Assert.IsTrue(ui.WaitThenClickElement(By.CssSelector("#activityTable_next.paginate_enabled_next")), "Could not locate record in table pagination");
                }
            }

            Assert.AreEqual(numberSignedIn.ToString(), activityRecordCount);
        }
        
        [TestMethod]
        public void SeActivity_Create_signin_simple()
        {
            // Arrange
            Random rand = new Random();
            int rowcount = 1;
            MacheteContext DB = new MacheteContext();
            var workers = DB.Workers;
            Activity _act= (Activity)Records.activity.Clone();
            ActivitySignin _asi = (ActivitySignin)Records.activitysignin.Clone();
            IEnumerable<int> cardlist = DB.Workers.Select(q => q.dwccardnum).Distinct();
            int firstCardNum = workers.First().dwccardnum;
            // Act
            ui.activityCreate(_act);
            var idPrefix = "asi" + _act.ID + "-"; 
            ui.activitySignIn(idPrefix, firstCardNum);
            // Assert
            Assert.IsTrue(ui.activitySignInValidate(idPrefix, firstCardNum, rowcount));
        }

        [TestMethod]
        public void SeActivity_Signin_random_worker()
        {
            //Arrange
            Activity _act = (Activity)Records.activity.Clone();
            var workers = DB.Workers;
            IEnumerable<int> cardList = workers.Select(q => q.dwccardnum).Distinct();
            Random rand = new Random();
            int randCardIndex = rand.Next(cardList.Count());
            int rowCount = 1;
            int randCard = cardList.ElementAt(randCardIndex);
            while (workers.First(c => c.dwccardnum == randCard).isSanctioned)
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

        [TestMethod]
        public void SeActivity_Signin_random_sactioned_worker()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Worker _sanctionedW = (Worker)Records.worker.Clone();
            Activity _act = (Activity)Records.activity.Clone();

            var workers = DB.Workers;
            IEnumerable<int> cardList = workers.Where(q => q.memberStatus == Worker.iSanctioned || q.memberStatus == Worker.iExpelled).Select(q => q.dwccardnum).Distinct();
            Assert.AreNotEqual(0, cardList.Count()); //pre-condition. 
            Random rand = new Random();
            int randCardIndex = rand.Next(cardList.Count());
            int randCard = cardList.ElementAt(randCardIndex);
            int rowcount = 1;

            //Act
            ui.personCreate(_per);
            _sanctionedW.ID = _per.ID;
            _sanctionedW.memberStatus = Worker.iSanctioned; 

            ui.workerCreate(_sanctionedW, sharedUI.SolutionDirectory() + "\\Machete.test\\jimmy_machete.jpg");
            ui.activityCreate(_act);
            var idPrefix = "asi" + _act.ID + "-"; 
            ui.activitySignIn(idPrefix, randCard);

            //Assert
            Assert.IsFalse(ui.activitySignInValidate(idPrefix, randCard, rowcount));
            Assert.IsTrue(ui.activitySignInIsSanctioned(), "Sanctioned worker box is not visible like it should be.");
        }

        [TestMethod]
        public void SeActivity_test_pagination()
        {
            // Arrange
            ServiceTest SeDB = new ServiceTest().LoadContext();
            int count = SeDB._aServ.GetAll().Count();
            if (count < 20)
            {
                Person _person = (Person)Records.person.Clone();
                SeDB._pServ.Create(_person, "ME");
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

        [TestMethod]
        public void SeActivity_test_search()
        {
            // Arrange
            ServiceTest SeDB = new ServiceTest().LoadContext();
            int count = SeDB._aServ.GetAll().Count();
            if (count < 20)
            {
                Activity _activity = (Activity)Records.activity.Clone();
                SeDB._aServ.Create(_activity, "ME");
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

        [TestMethod]
        public void SeActivity_test_record_limit()
        {
            // Arrange
            ServiceTest SeDB = new ServiceTest().LoadContext();
            int count = SeDB._aServ.GetAll().Count();
            while (count < 100)
            {
                Activity _activity = (Activity)Records.activity.Clone();
                SeDB._aServ.Create(_activity, "ME");
                count = SeDB._aServ.GetAll().Count();
            }

            // Act
            ui.WaitThenClickElement(By.Id("menulinkactivity"));

            // Test default
            Thread.Sleep(3000); //prevent race condition
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

        [TestMethod]
        public void SeActivity_test_column_sorting()
        {
            // Arrange
            ServiceTest SeDB = new ServiceTest().LoadContext();
            int count = SeDB._aServ.GetAll().Count();
            if (count < 100)
            {
                Activity _activity = (Activity)Records.activity.Clone();
                SeDB._aServ.Create(_activity, "ME");
            }

            // Act
            ui.WaitThenClickElement(By.Id("menulinkactivity"));

            // Test default - End Time column should be sort ascending
            IWebElement defaultSortCol = ui.WaitForElement(By.XPath("//th[@class='sorting_asc']"));
            Assert.AreEqual("End time", defaultSortCol.Text, "Activity End time isn't the default sort column");

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