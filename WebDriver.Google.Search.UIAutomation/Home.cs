using Ministry.WebDriver.Extensions;
using OpenQA.Selenium;

namespace WebDriver.Google.Search.UIAutomation
{
    /// <summary>
    /// Home page definition
    /// </summary>
    public class Home : AutomationPage
    {
        private readonly string siteRoot;

        /// <summary>
        /// Creates a home page implementation.
        /// </summary>
        /// <param name="driver">The web driver implementation to automate with.</param>
        /// <param name="siteRoot">The site root.</param>
        public Home(IWebDriver driver, string siteRoot)
            : base(driver)
        {
            this.siteRoot = siteRoot;
        }

        public override string Url { get { return siteRoot; } }

        public IWebElement SearchBox { get { return Browser.FindElement(By.Name("q")); } }
        public IWebElement SearchButton { get { return Browser.FindElement(By.Name("btnG"), 1000); } }

        public IWebElement ResultItem(string linkText)
        {
            try
            {
                return Browser.FindElement(By.XPath("//li/div/h3/a[contains(text(), '" + linkText + "')]"), 5000);
            }
            catch (NoSuchElementException)
            {
                return Browser.FindElement(By.XPath("//li/div/h3/a/em[contains(text(), '" + linkText + "')]"), 5000);
            }
        }

    }
}
