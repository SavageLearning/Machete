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
using Machete.Data;
using System.Linq;
using System.Collections.Generic;
using Machete.Service;
using Machete.Data.Infrastructure;
using System.Data.Entity;

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
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            DB = new MacheteContext("machete"); //name of DB in sql server
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
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);
            ui.activityValidate(_act);
        }
        [TestMethod]
        public void SeActivity_Create_ManySignins()
        {
            //Arrange
            MacheteContext DB = new MacheteContext("machete"); //maps to Machete.Text\app.config ConnectionString
            Random rand = new Random();
            // There's a lot in this one line. Ask questions about it.
            //                         [DbSet] (LINQ)(Lambda Expression) (SQL-ish)
            IEnumerable<int> list = DB.Workers.Select(q => q.dwccardnum).Distinct();
            var count = list.Count();
            Assert.IsTrue(count > 10, "There were not enough workers found whose dwccardnumbers we can use! There were only: " + count);
            int numberOfSignins = rand.Next(count / 10);
            Assert.IsTrue(numberOfSignins > 0, "We decided not to sign anyone in?");
            IEnumerable<int> list1 = list.Take<int>(numberOfSignins);
            
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);
            foreach (var i in list1)
            {
                bool result = ui.activitySignIn(i);
                var sanctionedBox = ui.WaitForElement(By.XPath("/html/body/div[3]"));
                if (sanctionedBox != null && sanctionedBox.GetCssValue("display") == "block")
                {
                    Assert.IsTrue(ui.WaitThenClickElement(By.XPath("/html/body/div[3]/div[1]/a")), "Couldn't find button to close sanctionbox");
                    --numberOfSignins;
                    Thread.Sleep(2000); // cheap hack; replace with a Selenium WaitFor
                    continue;
                }
                Assert.IsTrue(result, "Sign in for worker " + i + " failed!");
                int numWorkerMatches = DB.Workers.Where(q => q.dwccardnum == i).Count();
                numberOfSignins += numWorkerMatches - 1;
                Thread.Sleep(2000); // cheap hack; replace with a Selenium WaitFor
            }
            ui.WaitThenClickElement(By.Id("activityListTab"));
            ui.SelectOption(By.XPath("//*[@id='activityTable_length']/label/select"), "100");
            Assert.IsTrue(ui.WaitForElementValue(By.XPath("//table[@id='activityTable']/tbody/tr[@recordid='" + _act.ID + "']/td[4]"), numberOfSignins.ToString()), "Not the right number of workers signed in.");
        }
        
        [TestMethod]
        public void SeActivity_Create_signin_simple()
        {
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);

            ActivitySignin _asi = (ActivitySignin)Records.activitysignin.Clone();
            var workers = DB.Workers;
            Assert.IsTrue(ui.activitySignIn(workers.First().dwccardnum));
        }

        [TestMethod]
        public void SeActivity_Signin_random_worker()
        {
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);

            var workers = DB.Workers;
            IEnumerable<int> cardList = workers.Select(q => q.dwccardnum).Distinct();
            Random rand = new Random();
            int randCardIndex = rand.Next(cardList.Count());
            int randCard = cardList.ElementAt(randCardIndex);

            TryRandomSignins(cardList, rand);
        }
        void TryRandomSignins(IEnumerable<int> cardList, Random rand)
        {
            int randCardIndex = rand.Next(cardList.Count());
            int randCard = cardList.ElementAt(randCardIndex);
            bool result = ui.activitySignIn(randCard);
            var sanctionedBox = ui.WaitForElement(By.XPath("/html/body/div[3]")); 
            if (sanctionedBox != null && sanctionedBox.GetCssValue("display") == "block") //TODO: Find a reliable way to recursively call this function if we hit a sanctioned worker.
            {
                ui.WaitThenClickElement(By.XPath("/html/body/div[3]/div[1]/a"));
                TryRandomSignins(cardList, rand);
            }
            else Assert.IsTrue(result);
        }

        [TestMethod]
        public void SeActivity_Signin_random_sactioned_worker()
        {
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);

            var workers = DB.Workers;
            IEnumerable<int> cardList = workers.Where(q => q.memberStatus == Worker.iSanctioned || q.memberStatus == Worker.iExpelled).Select(q => q.dwccardnum).Distinct();
            Assert.AreNotEqual(0, cardList.Count());
            Random rand = new Random();
            int randCardIndex = rand.Next(cardList.Count());
            int randCard = cardList.ElementAt(randCardIndex);

            Assert.IsFalse(ui.activitySignIn(randCard));
            var sanctionedBox = ui.WaitForElement(By.XPath("/html/body/div[3]"));
            Assert.IsTrue(sanctionedBox != null && sanctionedBox.GetCssValue("display") == "block", "Sanctioned worker box is not visible like it should be.");
            ui.WaitThenClickElement(By.XPath("/html/body/div[3]/div[1]/a"));
        }
    }
}
