﻿using System;
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
            IEnumerable<int> list1 = list.Take<int>(rand.Next(count/10));
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);
            foreach (var i in list1)
            {
                ui.activitySignIn(i);
                Thread.Sleep(2000); // cheap hack; replace with a Selenium WaitFor
            }

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
            if (ui.WaitForElement(By.Id("dwcardnum")) != null)
                TryRandomSignins(cardList, rand);
            else Assert.IsTrue(result);
        }

        [TestMethod]
        public void SeActivity_Signin_random_sactioned_worker()
        {
            Activity _act = (Activity)Records.activity.Clone();
            ui.activityCreate(_act);

            var workers = DB.Workers;
            IEnumerable<int> cardList = workers.Where(q => q.memberStatus == Worker.iSanctioned || q.memberStatus == Worker.iExpelled).Select(q => q.dwccardnum).Distinct();
            Random rand = new Random();
            int randCardIndex = rand.Next(cardList.Count());
            int randCard = cardList.ElementAt(randCardIndex);

            Assert.IsFalse(ui.activitySignIn(randCard));
            Assert.IsNull(ui.WaitForElement(By.Id("dwcardnum")));
        }
    }
}
