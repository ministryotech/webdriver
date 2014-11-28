using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// A basic definition for a component of a page with its own class.
    /// </summary>
    /// <remarks>
    /// This is useful for elements shared between pages, such as headers, footers and sidebars.
    /// </remarks>
    public interface IAutomationComponent : IElementInterrogator
    {
        /// <summary>
        /// Gets the container element.
        /// </summary>
        IWebElement ContainerElement { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is rendered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is rendered; otherwise, <c>false</c>.
        /// </value>
        bool IsRendered { get; }
    }

    /// <summary>
    /// A basic definition for a component of a page with its own class.
    /// </summary>
    /// <remarks>
    /// This is useful for elements shared between pages, such as headers, footers and sidebars.
    /// </remarks>
    public abstract class AutomationComponent : AutomationBase, IAutomationComponent
    {
        #region | Construction |

        /// <summary>
        /// Creates a base class implementation of an automated component.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        protected AutomationComponent(IWebDriver browser)
            : base(browser)
        { }

        #endregion

        /// <summary>
        /// Gets the container element.
        /// </summary>
        public abstract IWebElement ContainerElement { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is rendered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is rendered; otherwise, <c>false</c>.
        /// </value>
        public bool IsRendered
        {
            get
            {
                try
                {
                    return (ContainerElement != null);
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="AutomationComponent"/> is displayed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if displayed; otherwise, <c>false</c>.
        /// </value>
        public bool Displayed { get { return ContainerElement.Displayed; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="AutomationComponent"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get { return ContainerElement.Enabled; } }
    }
}
