using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace ContactFormTests.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver, int timeoutSeconds = 10)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        }

        public void Open(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        protected IWebElement Find(By locator)
        {
            return Wait.Until(d =>
            {
                try { return d.FindElement(locator); }
                catch (NoSuchElementException) { return null; }
            });
        }

        protected void Click(By locator)
        {
            Find(locator).Click();
        }

        protected void Type(By locator, string text)
        {
            var element = Find(locator);
            element.Clear();
            element.SendKeys(text);
        }

        protected string GetText(By locator)
        {
            return Find(locator).Text.Trim();
        }

        protected bool IsVisible(By locator)
        {
            try
            {
                return Wait.Until(d =>
                {
                    try
                    {
                        var el = d.FindElement(locator);
                        return el.Displayed;
                    }
                    catch (NoSuchElementException) { return false; }
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}