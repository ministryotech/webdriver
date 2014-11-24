using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// A basic definition for a page.
    /// </summary>
    public abstract class AutomationPage
    {
        #region | Construction |

        /// <summary>
        /// Creates a base class implementation of an automated page.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        protected AutomationPage(IWebDriver browser)
        {
            Browser = browser;
        }

        #endregion

        /// <summary>
        /// Gets the URL linked to this 'page'.
        /// </summary>
        public abstract string Url { get; }

        /// <summary>
        /// Gets the browser instance passed into the page.
        /// </summary>
        protected IWebDriver Browser { get; private set; } 
    }
}
