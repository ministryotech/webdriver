using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// Class that acts as a base page factory.
    /// </summary>
    /// <remarks>
    /// Inherit from this class and add properties for each new AutomationPage type.
    /// </remarks>
    public abstract class PageFactoryBase
    {
        #region | Construction |

        /// <summary>
        /// Creates a page factory with no browser instance set.
        /// </summary>
        /// <remarks>
        /// Implement the child constructor in the same way.
        /// </remarks>
        protected PageFactoryBase()
        { }

        /// <summary>
        /// Creates a page factory.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        /// <remarks>
        /// Implement the child constructor passing the browser IWebDriver instance into each page.
        /// </remarks>
        /// <example>
        /// InitialisePageObjectTree();
        /// </example>
        protected PageFactoryBase(IWebDriver browser)
        {
            Browser = browser;
        }

        #endregion

        /// <summary>
        /// Gets the site root.
        /// </summary>
        public string SiteRoot { get; set; }

        /// <summary>
        /// Gets or sets the browser instance.
        /// </summary>
        public IWebDriver Browser { get; set; }

        /// <summary>
        /// Initialises the page object tree.
        /// </summary>
        public abstract void InitialisePageObjectTree();
    }
}
