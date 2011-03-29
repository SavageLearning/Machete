using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WatiN.Core;

namespace Machete.Test 
{
    [TestClass]
    public class UnitTest1
    {
        public WebServer webServer;

        [TestInitialize]
        public void StartWebServer()
        {
            //var absolutePathToWebsite = ConfigurationManager.AppSettings["absolutePathToWebsite"];
            var absolutePathToWebsite = @"H:\jimmy\Documents\VS2010\Projects\Machete\Machete.Web";
            webServer = new WebServer(absolutePathToWebsite, 42420);
            webServer.Start();
        }

        [TestMethod]
        [STAThread]
        public void Web_test_worker_create()
        {
            //using (var browser = new IE("http://www.google.com"))
            using (var browser = new IE("http://localhost:42420/Account/LogOn"))
            {
                browser.TextField(Find.ByName("UserName")).TypeText("jadmin");
                browser.TextField(Find.ByName("Password")).TypeText("1bud4me");
                //browser.CheckBox(Find.ByName("RememberMe")).Click();
                browser.Button(Find.ByName("logonB")).Click();
                Assert.IsTrue(browser.ContainsText("Welcome jadmin"));
                browser.Link(Find.ById("menulinkworker")).Click();
                browser.Link(Find.ById("createlink")).Click();
                Assert.IsTrue(browser.ContainsText("Person information"));
                //Person inputs
                browser.TextField(Find.ById("person_firstname1")).TypeText("Web test");
                browser.TextField(Find.ById("person_lastname1")).TypeText("Web_test_worker_create");
                browser.SelectList(Find.ById("person_gender")).SelectByValue("M");
                //Worker inputs
                browser.SelectList(Find.ById("worker_RaceID")).SelectByValue("3");
                browser.TextField(Find.ById("worker_height")).TypeText("Web test");
                browser.TextField(Find.ById("worker_weight")).TypeText("wkr_create");
                browser.TextField(Find.ById("worker_dateinUSA")).TypeText("1/1/2011");
                browser.TextField(Find.ById("worker_dateinseattle")).TypeText("1/1/2011");
                browser.SelectList(Find.ById("worker_englishlevelID")).SelectByValue("1");
                browser.SelectList(Find.ById("worker_incomeID")).SelectByValue("1");
                browser.TextField(Find.ById("worker_dwccardnum")).TypeText("10123");
                browser.SelectList(Find.ById("worker_neighborhoodID")).SelectByValue("1");
                browser.TextField(Find.ById("worker_countryoforigin")).TypeText("USA");
                browser.TextField(Find.ById("worker_memberexpirationdate")).TypeText("1/1/2012");
                browser.Button(Find.ByValue("Create")).Click();
                //browser.Close();
            }
        }
        [TestCleanup]
        public void StopWebServer()
        {
            webServer.Stop();
        }
    }
}
