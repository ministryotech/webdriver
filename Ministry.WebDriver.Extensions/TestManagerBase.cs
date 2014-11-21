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
        /// <param name="siteRoot">The site root.</param>
        protected TestManagerBase(string browserName, string siteRoot = "")
        {
            Browser = WebDriverTools.GetBrowser(browserName);
            Pages = new TPageFactory { Browser = Browser, SiteRoot = siteRoot };
            Pages.InitialisePageObjectTree();
        }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="browser">The type of the web driver implementation to test with</param>
        /// <param name="siteRoot">The site root.</param>
        protected TestManagerBase(IWebDriver browser, string siteRoot = "")
        {
            Browser = browser;
            Pages = new TPageFactory { Browser = browser, SiteRoot = siteRoot };
            Pages.InitialisePageObjectTree();
        }

        #endregion

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
