using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Threading;

namespace Machete.Test
{
    class sharedUI
    {
        IWebDriver _d;
        string _url;
        public sharedUI(IWebDriver driver, string url)
        {
            _d = driver;
            _url = url;
        }
        public bool login()
        {
            _d.Navigate().GoToUrl(_url);

            _d.FindElement(By.LinkText("Logon")).Click();
            WaitForText("Account Information", _d, 60);
            _d.FindElement(By.Id("UserName")).Clear();
            _d.FindElement(By.Id("UserName")).SendKeys("jadmin");
            _d.FindElement(By.Id("Password")).Clear();
            _d.FindElement(By.Id("Password")).SendKeys("machete");
            _d.FindElement(By.Name("logonB")).Click();
            WaitForText("Welcome", _d, 60);
            return true;
        }
        //
        //
        #region utilfunctions
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
            if (elem != null) 
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
            for (int second = 0; second < 60; second++)
            {
                try
                {
                    elem = IsElementPresent(by);
                    if (elem != null) return elem;
                }
                catch (Exception)
                { return null; }
                Thread.Sleep(1000);
            }
            return null;
        }
        //
        //
        public bool WaitForElementValue(By by, string value)
        {
            for (int second = 0; second < 60; second++)
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
                Thread.Sleep(1000);
            }
            return false;
        }
        public bool WaitForText(String what, IWebDriver driver, int waitfor)
        {
            for (int second = 0; second < waitfor; second++)
            {
                try
                {
                    if (isTextPresent(what, driver)) return true;
                }
                catch (Exception)
                { return false; }
                Thread.Sleep(1000);
            }
            return false;
        }
        #endregion
        #region privatemethods
        private IWebElement IsElementPresent(By by)
        {
            try
            {
                return _d.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
        //
        //
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
            catch (NoSuchElementException e)
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
    }
}
