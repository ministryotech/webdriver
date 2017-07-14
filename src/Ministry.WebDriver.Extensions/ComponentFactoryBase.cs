using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// Class that acts as a base component factory for SPA component based sites or hybrids.
    /// </summary>
    /// <remarks>
    /// Inherit from this class and add properties for each new AutomationComponent type that is not a page.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class ComponentFactoryBase
    {
        #region | Construction |

        /// <summary>
        /// Creates a component factory with no browser instance set.
        /// </summary>
        /// <remarks>
        /// Implement the child constructor in the same way.
        /// </remarks>
        protected ComponentFactoryBase()
        { }

        /// <summary>
        /// Creates a component factory.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        /// <remarks>
        /// Implement the child constructor passing the browser IWebDriver instance into each component.
        /// </remarks>
        /// <example>
        /// InitialiseComponentObjectTree();
        /// </example>
        protected ComponentFactoryBase(IWebDriver browser)
        {
            Browser = browser;
        }

        #endregion

        /// <summary>
        /// Gets or sets the browser instance.
        /// </summary>
        public IWebDriver Browser { get; set; }

        /// <summary>
        /// Initialises the page object tree.
        /// </summary>
        public abstract void InitialiseComponentObjectTree();
    }
}
