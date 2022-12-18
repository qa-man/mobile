using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using Utilities.Extensions;
using Tests.Basics;

namespace Tests.Pages
{
    public class BasePage
    {
        private By example = By.Id("com.example.app:id/element");

        private AndroidDriver<AppiumWebElement> _driver;

        public BasePage()
        {
            _driver = Base.Driver;
        }

        public void ClickExampleButton()
        {
            _driver.ClickElement(example);
        }
    }
}
