using System.Diagnostics.CodeAnalysis;
using Ministry.WebDriverCore;
using OpenQA.Selenium;

namespace Google.Search.UIAutomation
{
    /// <summary>
    /// Class to provide page wrappers.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class GooglePageFactory : PageFactoryBase
    {
        #region | Construction |

        /// <summary>
        /// Creates an empty page factory.
        /// </summary>
        public GooglePageFactory()
        { }

        /// <summary>
        /// Creates a page factory.
        /// </summary>
        /// <param name="browser">The web driver implementation to automate with.</param>
        public GooglePageFactory(IWebDriver browser)
            : base (browser)
        {
            InitialisePageObjectTree();
        }

        /// <summary>
        /// Initialises the page object tree.
        /// </summary>
        public sealed override void InitialisePageObjectTree()
        {
            Home = new Home(Browser, SiteRoot);
        }

        #endregion

        /// <summary>
        /// The site home page.
        /// </summary>
        public Home Home { get; private set; }
    }
}
