//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using System.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using WatiN.Core;

//namespace Machete.Test 
//{
//    public static class WatiNExtensions 
//    {
//        //http://blog.dbtracer.org/2010/08/05/speed-up-typing-text-with-watin/
//        public static void TypeTextQuickly(this TextField textField, string text)
//        {
//            textField.SetAttributeValue("value", text);
//        }
//    }

//    [TestClass]
//    public class UnitTest1
//    {
//        public WebServer webServer;

//        [TestInitialize]
//        public void StartWebServer()
//        {
//            //var absolutePathToWebsite = ConfigurationManager.AppSettings["absolutePathToWebsite"];
//            var absolutePathToWebsite = @"H:\jimmy\Documents\VS2010\Projects\Machete\Machete.Web";
//            webServer = new WebServer(absolutePathToWebsite, 42420);
//            webServer.Start();
//        }

//        [TestMethod]
//        [STAThread]
//        public void Web_test_worker_create()
//        {
//            //using (var browser = new IE("http://www.google.com"))
//            using (var browser = new IE("http://localhost:42420/Account/LogOn"))
//            {
                
//                browser.TextField(Find.ByName("UserName")).TypeTextQuickly("jadmin");
//                browser.TextField(Find.ByName("Password")).TypeTextQuickly("1bud4me");
//                //browser.CheckBox(Find.ByName("RememberMe")).Click();
//                browser.Button(Find.ByName("logonB")).Click();
//                Assert.IsTrue(browser.ContainsText("Welcome jadmin"));
//                browser.Link(Find.ById("menulinkworker")).Click();
//                browser.Link(Find.ById("createlink")).Click();
//                Assert.IsTrue(browser.ContainsText("Person information"));
//                //Person inputs
//                browser.TextField(Find.ById("person_firstname1")).TypeTextQuickly("Web test");
//                browser.TextField(Find.ById("person_lastname1")).TypeTextQuickly("Web_test_worker_create");
//                browser.SelectList(Find.ById("person_gender")).SelectByValue("M");
//                //Worker inputs
//                browser.SelectList(Find.ById("worker_RaceID")).SelectByValue("3");
//                browser.TextField(Find.ById("worker_height")).TypeTextQuickly("Web test");
//                browser.TextField(Find.ById("worker_weight")).TypeTextQuickly("wkr_create");
//                browser.TextField(Find.ById("worker_dateinUSA")).TypeTextQuickly("1/1/2011");
//                browser.TextField(Find.ById("worker_dateinseattle")).TypeTextQuickly("1/1/2011");
//                browser.SelectList(Find.ById("worker_englishlevelID")).SelectByValue("1");
//                browser.SelectList(Find.ById("worker_incomeID")).SelectByValue("1");
//                browser.TextField(Find.ById("worker_dwccardnum")).TypeTextQuickly("10123");
//                browser.SelectList(Find.ById("worker_neighborhoodID")).SelectByValue("1");
//                browser.TextField(Find.ById("worker_countryoforigin")).TypeTextQuickly("USA");
//                browser.TextField(Find.ById("worker_memberexpirationdate")).TypeTextQuickly("1/1/2012");
//                browser.Button(Find.ByValue("Create")).Click();
//                browser.TextField(Find.By("type","test")).TypeTextQuickly("Web");
//                //browser.Close();
                
//            }
//        }
//        [TestCleanup]
//        public void StopWebServer()
//        {
//            //webServer.Stop();
//        }
//    }
//}
