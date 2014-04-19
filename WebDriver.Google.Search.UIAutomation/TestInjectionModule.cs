using Ministry.WebDriver.Extensions;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Configuration;

namespace WebDriver.Google.Search.UIAutomation
{

    /// <summary>
    /// Default class for determining test browser.
    /// </summary>
    public class TestInjectionModule : NinjectModule
    {
        public override void Load()
        {
            var defaultBrowser = ConfigurationManager.AppSettings["DefaultBrowser"];
            if (defaultBrowser != null)
            {
                var driverType = WebDriverTools.GetBrowserType(defaultBrowser);
                Bind<IWebDriver>().To(driverType);
            }
            else
            {
                Bind<IWebDriver>().To<FirefoxDriver>();
            }
        }
    }
}
