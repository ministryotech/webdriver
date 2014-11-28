using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// A basic definition for a page.
    /// </summary>
    public interface IAutomationPage : IElementInterrogator
    {
        /// <summary>
        /// Gets the URL linked to this 'page'.
        /// </summary>
        string Url { get; }
    }

    /// <summary>
    /// A basic definition for a page.
    /// </summary>
    public abstract class AutomationPage : AutomationBase, IAutomationPage
    {
        #region | Construction |

        /// <summary>
        /// Creates a base class implementation of an automated page.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        protected AutomationPage(IWebDriver browser)
            : base(browser)
        { }

        #endregion

        /// <summary>
        /// Gets the URL linked to this 'page'.
        /// </summary>
        public abstract string Url { get; }
    }
}
