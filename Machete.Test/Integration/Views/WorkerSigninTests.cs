#region COPYRIGHT
// File:     WorkerSigninTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test.Old
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using AutoMapper;
using Machete.Data;
using Machete.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using Machete.Web.Maps;

namespace Machete.Test.Selenium.View
{
    [TestClass]
    public class WorkerSigninTests
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private static sharedUI ui;
        //private static string testdir;
        private static string testimagefile;
        private static MacheteContext DB;
        private static DbSet<Worker> wSet;
        private static DbSet<WorkerSignin> wsiSet;
        private static DbSet<Person> pSet;
        static IMapper map;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            WebServer.StartIis();
            // getting Project path for dummy image
            string solutionDirectory = sharedUI.SolutionDirectory();
            //testdir = solutionDirectory + "\\Machete.test\\";
            testimagefile = solutionDirectory + "\\jimmy_machete.jpg";
            var mapperConfig = new MvcMapperConfiguration().Config;
            map = mapperConfig.CreateMapper();
            
            driver = new ChromeDriver(ConfigurationManager.AppSettings["CHROMEDRIVERPATH"]);
            baseURL = "http://localhost:4213/";
            ui = new sharedUI(driver, baseURL, map);

            // fake
            DbContextOptions<MacheteContext> config = new DbContextOptions<MacheteContext>();
            DB = new MacheteContext(config);
            wsiSet = DB.Set<WorkerSignin>();
            wSet = DB.Set<Worker>();
            pSet = DB.Set<Person>();
        }
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void SetupTest()
        {
            //ui.iisX.Start();
            //driver = new FirefoxDriver();
            //baseURL = "http://localhost:4213/";
            //ui = new sharedUI(driver, baseURL);
            verificationErrors = new StringBuilder();
            ui.login();

        }
        /// <summary>
        /// 
        /// </summary>
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
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup() { WebServer.StopIis(); }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.SE), TestCategory(TC.View), TestCategory(TC.WSIs)]
        public void SeWSI_create_signin()
        {
            IEnumerable<WorkerSignin> wList = wsiSet.Where(w => w.dwccardnum < 30000).AsEnumerable();
        }
    }
}