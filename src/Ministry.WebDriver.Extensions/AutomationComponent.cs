﻿using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;

namespace Ministry.WebDriver.Extensions
{
    #region | Interface |

    /// <summary>
    /// A basic definition for a component with its own class.
    /// </summary>
    /// <remarks>
    /// This is useful for elements shared between pages, such as headers, footers and sidebars or for an SPA style app.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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

        /// <summary>
        /// Gets a value indicating whether this component is currently shown.
        /// </summary>
        /// <remarks>
        /// The default implementation here is to check that the container element is not empty. This may not work in many cases so overriding is recommended.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is shown; otherwise, <c>false</c>.
        /// </value>
        bool IsShown { get; }
    }

    #endregion | Interface |

    /// <summary>
    /// A basic definition for a component of a page with its own class.
    /// </summary>
    /// <remarks>
    /// This is useful for elements shared between pages, such as headers, footers and sidebars or for an SPA style app.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
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
                    return ContainerElement != null;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this component is currently shown.
        /// </summary>
        /// <remarks>
        /// The default implementation here is to check that the container element is not empty. This may not work in many cases so overriding is recommended.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is shown; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsShown
        {
            get
            {
                try
                {
                    return ContainerElement != null && Displayed &&
                           !string.IsNullOrWhiteSpace(ContainerElement.InnerHtml());
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
        public bool Displayed => ContainerElement.Displayed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="AutomationComponent"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled => ContainerElement.Enabled;
    }
}
