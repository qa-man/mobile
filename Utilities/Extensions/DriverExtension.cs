using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace Utilities.Extensions
{
    public static class DriverExtension
    {
        public static AndroidDriver<AppiumWebElement> WaitElement(this AndroidDriver<AppiumWebElement> pda, By locator)
        {
            var wait = new WebDriverWait(pda, TimeSpan.FromMinutes(1))
            {
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };
            wait.IgnoreExceptionTypes(typeof(WebDriverException));
            wait.Until(ExpectedConditions.ElementExists(locator));
            return pda;
        }

        public static AndroidDriver<AppiumWebElement> ClickElement(this AndroidDriver<AppiumWebElement> pda, By locator)
        {
            WaitElement(pda, locator);
            pda.FindElement(locator).Click();
            return pda;
        }
    }
}