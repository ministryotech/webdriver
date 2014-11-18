using Ministry.WebDriver.Extensions;
using Ninject;
using OpenQA.Selenium;

namespace WebDriver.Google.Search.UIAutomation
{
    /// <summary>
    /// Class to provide access to test objects in the site.
    /// </summary>
    public class TestManager : TestManagerBase<GooglePageFactory>
    {

        #region | Construction |

        /// <summary>
        /// Creates a test manager.
        /// </summary>
        public TestManager()
            : base(new StandardKernel(new TestInjectionModule()).Get<IWebDriver>(), "http://www.google.com/")
        { }

        /// <summary>
        /// Creates a test manager for a specific browser.
        /// </summary>
        /// <param name="browserName">The name of the browser to test with</param>
        public TestManager(string browserName)
            : base(browserName, "http://www.google.com/")
        { }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="driver">The type of the web driver implementation to test with</param>
        public TestManager(IWebDriver driver)
            : base(driver, "http://www.google.com/")
        { }

        #endregion

    }
}
