using Ministry.WebDriver.Extensions;
using OpenQA.Selenium;

namespace WebDriver.Google.Search.UIAutomation
{
    /// <summary>
    /// Class to provide page wrappers.
    /// </summary>
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
        public override sealed void InitialisePageObjectTree()
        {
            Home = new Home(Browser);
        }

        #endregion

        /// <summary>
        /// The site home page.
        /// </summary>
        public Home Home { get; private set; }
    }
}
