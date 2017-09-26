using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    #region | Interface |

    /// <summary>
    /// A basic definition for a page.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IAutomationPage : IElementInterrogator
    {
        /// <summary>
        /// Gets the URL linked to this 'page'.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Gets a value indicating whether this page is the currently active page.
        /// </summary>
        /// <remarks>
        /// The default implementation here is to wait for a second and check that the URLs match. Recommended usage is to override the method to check an element exists.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is currently active; otherwise, <c>false</c>.
        /// </value>
        bool IsShown { get; }
    }

    #endregion | Interface |

    /// <summary>
    /// A basic definition for a page.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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

        /// <summary>
        /// Gets a value indicating whether this page is the currently active page.
        /// </summary>
        /// <remarks>
        /// The default implementation here is to wait for a second and check that the URLs match, ignoring any query strings and case differences. Recommended usage is to override the method to check an element exists.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is currently active; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsShown
        {
            get
            {
                var cleanUrl = Url.Trim('/').ToLowerInvariant();
                for (var i = 0; i < 10; i++)
                {
                    var currentUrl = Browser.Url.Split('?')[0].Trim('/').ToLowerInvariant();
                    if (currentUrl == cleanUrl) return true;
                    Browser.Wait(1000);
                }

                return false;
            }
        }
    }
}
