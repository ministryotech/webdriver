using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// Elements used by a base Automation element for interrogating element states.
    /// </summary>
    public interface IElementInterrogator
    {
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
        bool DoesElementExist(Func<IWebElement> elementPropertyFunc);

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
        bool DoesElementExist(Func<IEnumerable<IWebElement>> elementPropertyFunc);

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
        bool DoesElementExistAndIsVisible(Func<IWebElement> elementPropertyFunc);

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
        bool DoesElementExistAndIsVisible(Func<IEnumerable<IWebElement>> elementPropertyFunc);
    }
}