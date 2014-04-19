using Ministry.WebDriver.Extensions;
using OpenQA.Selenium;

namespace WebDriver.Google.Search.UIAutomation
{
    /// <summary>
    /// Home page definition
    /// </summary>
    public class Home : AutomationPage
    {

        /// <summary>
        /// Creates a home page implementation.
        /// </summary>
        /// <param name="driver">The web driver implementation to automate with.</param>
        public Home(IWebDriver driver)
            : base(driver)
        {}

        public override string Url { get { return TestManager.SiteRoot; } }

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
