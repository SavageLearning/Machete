using AutoMapper;
using Machete.Data;
using Machete.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Machete.Test.Selenium.View
{
    [TestClass]
    public class PersonTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private sharedUI ui;
        //private static string testdir;
        private static string testimagefile;
        FluentRecordBase frb;
        static IMapper map;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            string solutionDirectory = sharedUI.SolutionDirectory();
            //testdir = solutionDirectory + "\\Machete.test\\";
            testimagefile = solutionDirectory + "\\jimmy_machete.jpg";
            map = new Machete.Web.MapperConfig().get();

            WebServer.StartIis();
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
            ////
            //// Loggoff
            ui.WaitForElement(By.LinkText("Logoff"));
            driver.FindElement(By.LinkText("Logoff")).Click();
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser/.ujhkmn,.
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        [ClassCleanup]
        public static void ClassCleanup() { WebServer.StopIis();  }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Persons)]
        public void SePerson_create_person()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            //Act
            ui.personCreate(_per);
            //Assert
            ui.personValidate(_per);
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Persons)]
        public void SePerson_create_worker()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Worker _wkr = (Worker)Records.worker.Clone();
            _wkr.memberexpirationdate = DateTime.Now.AddYears(1);
            //Act
            ui.personCreate(_per);
            _wkr.ID = _per.ID;
            _wkr.dwccardnum = frb.GetNextMemberID();
            ui.workerCreate(_wkr, testimagefile);

            //Assert
            ui.workerValidate(_wkr);
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Persons)]
        public void SePerson_delete_worker()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Worker _wkr = (Worker)Records.worker.Clone();
            _wkr.memberexpirationdate = DateTime.Now.AddYears(1);
            //Act
            ui.personCreate(_per);
            _wkr.ID = _per.ID;
            _wkr.dwccardnum = frb.GetNextMemberID();
            ui.workerCreate(_wkr, testimagefile);
            ui.workerDelete(_wkr);

            //Assert
            ui.confirmWorkerDeleted();
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Persons)]
        public void SePerson_create_event()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Event _ev = (Event)Records.event1.Clone();
            _ev.Person = _per;

            //Act
            ui.personCreate(_per);
            _ev.PersonID = _per.ID;
            ui.eventCreate(_ev);

            //Assert
            ui.eventValidate(ref _ev);
        }

        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.Persons)]
        public void SePerson_signin_sanctioned_worker_fails()
        {
            //Arrange
            Person _per = (Person)Records.person.Clone();
            Worker _wkr = (Worker)Records.worker.Clone();
            _wkr.memberexpirationdate = DateTime.Now.AddYears(1);
            _wkr.dwccardnum = sharedUI.nextAvailableDwccardnum(frb.ToFactory().Get());
            Event _san = (Event)Records.event1.Clone();
            _san.Person = _per;
            _san.eventType = MacheteLookup.cache.First(x => x.category == "eventtype" && x.text_EN == "Sanction").ID;
            Activity _act = (Activity)Records.activity.Clone();

            //Act
            ui.personCreate(_per);
            _wkr.ID = _per.ID;
            ui.workerCreate(_wkr, testimagefile);
            //
            ui.workerSanction(_wkr);
            //
            _san.PersonID = _per.ID;
            ui.eventCreate(_san);
            ui.activityCreate(_act);
            // refactored other test to identify idPrefix-dwccardnum
            ui.activitySignIn(_act.idChild,_wkr.dwccardnum);
            //Assert
            Assert.IsTrue(ui.activitySignInIsSanctioned());
        }
    }
}
