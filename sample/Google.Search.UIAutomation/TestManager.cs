using System.Diagnostics.CodeAnalysis;
using Ministry.WebDriverCore;
using OpenQA.Selenium;

namespace Google.Search.UIAutomation
{
    /// <summary>
    /// Class to provide access to test objects in the site.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class TestManager : PageTestManagerBase<GooglePageFactory>
    {
        #region | Construction |

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
