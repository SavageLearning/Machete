using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selenium;

namespace Machete.Test.SeleniumTests
{
	[TestClass]
	public class SeEmployerTests
	{
		private ISelenium selenium;
		private StringBuilder verificationErrors;
		
		[TestInitialize]
		public void SetupTest()
		{
			selenium = new DefaultSelenium("localhost", 4444, "*chrome", "http://localhost:4242/");
			selenium.Start();
			verificationErrors = new StringBuilder();
		}
		
		[TestCleanup]
		public void TeardownTest()
		{
			try
			{
				selenium.Stop();
			}
			catch (Exception)
			{
				// Ignore errors if unable to close the browser
			}
			Assert.AreEqual("", verificationErrors.ToString());
		}
		
		[TestMethod]
		public void EmployerCreate_Test_validation()
		{
			selenium.Open("/");
			selenium.Click("link=Logon");
			selenium.WaitForPageToLoad("30000");
			selenium.Type("UserName", "jphonedesk");
			selenium.Type("Password", "1bud4me");
			selenium.Click("logonB");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("menulinkemployer");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("employerCreateTab");
			for (int second = 0;; second++) {
				if (second >= 60) Assert.Fail("timeout");
				try
				{
					if (selenium.IsElementPresent("employerCreateForm")) break;
				}
				catch (Exception)
				{}
				Thread.Sleep(1000);
			}
			selenium.Click("employerCreateButton");
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("A name is required"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("An adddress is required"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("A city is required"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("A state is required"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("Zip code"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("A phone number is required"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
			selenium.Click("link=Logoff");
			selenium.WaitForPageToLoad("30000");
			try
			{
				Assert.IsTrue(selenium.IsTextPresent("Welcome to Machete"));
			}
            catch (Exception e)
			{
				verificationErrors.Append(e.Message);
			}
		}

        [TestMethod]
        public void EmployerCED_Test()
        {
            selenium.Open("/");
            selenium.Click("link=Logon");
            selenium.WaitForPageToLoad("30000");
            selenium.Type("UserName", "jphonedesk");
            selenium.Type("Password", "1bud4me");
            selenium.Click("logonB");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("menulinkemployer");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("employerCreateTab");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsElementPresent("employerCreateForm")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.Type("name", "selenium test 1");
            selenium.Type("address1", "Selenium Drive");
            selenium.Type("city", "Seattle");
            selenium.Type("state", "WA");
            selenium.Type("zipcode", "98123");
            selenium.Type("phone", "123-456-7890");
            selenium.MouseOver("id=employerCreateButton");
            selenium.Submit("css=#employerCreateForm");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsVisible("id=employerTable_wrapper")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.DoubleClick("//table[@id='employerTable']/tbody/tr[1]/td[2]");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsElementPresent("css=a[href]")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.Click("link=selenium test 1");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsElementPresent("css=form[id^='EmployerTab']")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if ("selenium test 1" == selenium.GetValue("//input[@id='name' and @name='name' and @value='selenium test 1']")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.Type("xpath=//form[@^id='EmployerTab']/div/div[1]/div[2]/input", "old--selenium test 1");
            selenium.Click("//input[@value='Save']");
            selenium.Click("xpath=//form[@id^='EmployerTab-']/input[2]");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsVisible("id=employerTable_wrapper")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.DoubleClick("//table[@id='employerTable']/tbody/tr[1]/td[3]");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsVisible("css=input[id^='deleteEmployerButton']")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.Click("css=input[id^='deleteEmployerButton']");
            selenium.Click("id=popup_ok");
            selenium.Click("link=Logoff");
        }
	}
}
