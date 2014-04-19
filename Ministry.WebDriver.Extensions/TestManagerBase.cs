using System;
using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// Abstract definition for a key class to provide access to test objects in the site.
    /// </summary>
    public abstract class TestManagerBase<TPageFactory>
        where TPageFactory : PageFactoryBase, new()
    {
        #region | Construction |

        /// <summary>
        /// Creates a test manager for a specific browser.
        /// </summary>
        /// <param name="browserName">The name of the browser to test with</param>
        protected TestManagerBase(string browserName)
        {
            Browser = WebDriverTools.GetBrowser(browserName);
            Pages = new TPageFactory { Browser = Browser };
            Pages.InitialisePageObjectTree();
        }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="browser">The type of the web driver implementation to test with</param>
        protected TestManagerBase(IWebDriver browser)
        {
            Browser = browser;
            Pages = new TPageFactory {Browser = browser};
            Pages.InitialisePageObjectTree();
        }

        #endregion

        /// <summary>
        /// Gets the site root.
        /// </summary>
        public static string SiteRoot { get { return String.Empty; } }

        /// <summary>
        /// The browser driver object.
        /// </summary>
        public IWebDriver Browser { get; private set; }

        /// <summary>
        /// The pages within the site.
        /// </summary>
        public TPageFactory Pages { get; protected set; }
    }
}
