using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using OpenQA.Selenium;

namespace Ministry.WebDriverCore
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public abstract class AutomationBase : IElementInterrogator
    {
        #region | Construction |

        /// <summary>
        /// Creates a base class implementation of an automated component.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        protected AutomationBase(IWebDriver browser)
        {
            Browser = browser;
        }

        #endregion

        /// <summary>
        /// Gets the browser instance passed into the page.
        /// </summary>
        protected IWebDriver Browser { get; }

        #region | Element status checkers |

        /// <summary>
        /// Does the element exist.
        /// </summary>
        /// <param name="elementPropertyFunc">The element property function.</param>
        /// <returns>A flag to indicate if the given element exists.</returns>
        /// <remarks>
        /// Use Lambda syntax to pass in a property
        /// </remarks>
        /// <example>
        /// homePage.DoesElementExist(() => homePage.Header);
        /// </example>
        public bool DoesElementExist(Func<IWebElement> elementPropertyFunc)
        {
            try
            {
                return elementPropertyFunc() != null;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Does the element exist.
        /// </summary>
        /// <param name="elementPropertyFunc">The element property function.</param>
        /// <returns>A flag to indicate if the given element exists.</returns>
        /// <remarks>
        /// Use Lambda syntax to pass in a property
        /// </remarks>
        /// <example>
        /// homePage.DoesElementExist(() => homePage.HeaderLinks);
        /// </example>
        public bool DoesElementExist(Func<IEnumerable<IWebElement>> elementPropertyFunc)
        {
            try
            {
                var elements = elementPropertyFunc();
                return elements != null && elements.Any();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Does the element exist and is it visible.
        /// </summary>
        /// <param name="elementPropertyFunc">The element property function.</param>
        /// <returns>A flag to indicate if the given element exists.</returns>
        /// <remarks>
        /// Use Lambda syntax to pass in a property
        /// </remarks>
        /// <example>
        /// homePage.DoesElementExistAndIsVisible(() => homePage.Header);
        /// </example>
        public bool DoesElementExistAndIsVisible(Func<IWebElement> elementPropertyFunc)
        {
            return DoesElementExist(elementPropertyFunc) && elementPropertyFunc().Displayed;
        }

        /// <summary>
        /// Doeses the element exist and is it visible.
        /// </summary>
        /// <param name="elementPropertyFunc">The element property function.</param>
        /// <returns>A flag to indicate if the given element exists.</returns>
        /// <remarks>
        /// Use Lambda syntax to pass in a property
        /// </remarks>
        /// <example>
        /// homePage.DoesElementExistAndIsVisible(() => homePage.HeaderLinks);
        /// </example>
        public bool DoesElementExistAndIsVisible(Func<IEnumerable<IWebElement>> elementPropertyFunc)
        {
            return DoesElementExist(elementPropertyFunc) && elementPropertyFunc().Any(webElement => webElement.Displayed);
        }

        #endregion
    }
}