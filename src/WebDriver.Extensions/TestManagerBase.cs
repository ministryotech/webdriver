using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;

namespace Ministry.WebDriverCore
{
    #region | Abstractions |

    /// <summary>
    /// Abstract definition for a key class to provide access to test objects in the site.
    /// </summary>
    /// <remarks>
    /// This is a root class - Use one of the generic implementations. If you are unsure use HybridTestManagerBase.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public abstract class TestManagerBase
    {
        #region | Construction |

        /// <summary>
        /// Creates a test manager for a specific browser.
        /// </summary>
        /// <param name="browserName">The name of the browser to test with</param>
        protected TestManagerBase(string browserName)
        {
            Browser = WebDriverTools.GetBrowser(browserName);
        }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="browser">The type of the web driver implementation to test with</param>
        protected TestManagerBase(IWebDriver browser)
        {
            Browser = browser;
        }

        #endregion

        /// <summary>
        /// The browser driver object.
        /// </summary>
        public IWebDriver Browser { get; }
    }

    #endregion | Abstractions |

    /// <summary>
    /// Abstract definition for a key class to provide access to test objects on a component based site.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class ComponentTestManagerBase<TComponentFactory> : TestManagerBase
        where TComponentFactory : ComponentFactoryBase, new()
    {
        #region | Construction |

        /// <summary>
        /// Creates a test manager for a specific browser.
        /// </summary>
        /// <param name="browserName">The name of the browser to test with</param>
        protected ComponentTestManagerBase(string browserName)
            : base(browserName)
        {
            Components = new TComponentFactory { Browser = Browser };
            Components.InitialiseComponentObjectTree();
        }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="browser">The type of the web driver implementation to test with</param>
        protected ComponentTestManagerBase(IWebDriver browser)
            : base(browser)
        {
            Components = new TComponentFactory { Browser = browser };
            Components.InitialiseComponentObjectTree();
        }

        #endregion

        /// <summary>
        /// The components used within the site.
        /// </summary>
        public TComponentFactory Components { get; }
    }

    /// <summary>
    /// Abstract definition for a key class to provide access to test objects on a traditional page based site.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class PageTestManagerBase<TPageFactory> : TestManagerBase
        where TPageFactory : PageFactoryBase, new()
    {
        #region | Construction |

        /// <summary>
        /// Creates a test manager for a specific browser.
        /// </summary>
        /// <param name="browserName">The name of the browser to test with</param>
        /// <param name="siteRoot">The site root.</param>
        protected PageTestManagerBase(string browserName, string siteRoot = "")
            : base(browserName)
        {
            Pages = new TPageFactory { Browser = Browser, SiteRoot = siteRoot };
            Pages.InitialisePageObjectTree();
        }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="browser">The type of the web driver implementation to test with</param>
        /// <param name="siteRoot">The site root.</param>
        protected PageTestManagerBase(IWebDriver browser, string siteRoot = "")
            : base(browser)
        {
            Pages = new TPageFactory { Browser = browser, SiteRoot = siteRoot };
            Pages.InitialisePageObjectTree();
        }

        #endregion

        /// <summary>
        /// The pages within the site.
        /// </summary>
        public TPageFactory Pages { get; }
    }

    /// <summary>
    /// Abstract definition for a key class to provide access to test objects on a hybrid site consisting of pages and components.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class HybridTestManagerBase<TComponentFactory, TPageFactory> : TestManagerBase
        where TComponentFactory : ComponentFactoryBase, new()
        where TPageFactory : PageFactoryBase, new()
    {
        #region | Construction |

        /// <summary>
        /// Creates a test manager for a specific browser.
        /// </summary>
        /// <param name="browserName">The name of the browser to test with</param>
        /// <param name="siteRoot">The site root.</param>
        protected HybridTestManagerBase(string browserName, string siteRoot = "")
            : base(browserName)
        {
            Components = new TComponentFactory { Browser = Browser };
            Pages = new TPageFactory { Browser = Browser, SiteRoot = siteRoot };
            Components.InitialiseComponentObjectTree();
            Pages.InitialisePageObjectTree();
        }

        /// <summary>
        /// Creates a test manager for a specific driver type.
        /// </summary>
        /// <param name="browser">The type of the web driver implementation to test with</param>
        /// <param name="siteRoot">The site root.</param>
        protected HybridTestManagerBase(IWebDriver browser, string siteRoot = "")
            : base(browser)
        {
            Components = new TComponentFactory { Browser = Browser };
            Pages = new TPageFactory { Browser = browser, SiteRoot = siteRoot };
            Components.InitialiseComponentObjectTree();
            Pages.InitialisePageObjectTree();
        }

        #endregion

        /// <summary>
        /// The components used within the site.
        /// </summary>
        public TComponentFactory Components { get; }

        /// <summary>
        /// The pages within the site.
        /// </summary>
        public TPageFactory Pages { get; }
    }
}
