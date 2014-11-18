using Ministry.WebDriver.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using WebDriver.Google.Search.UIAutomation;

namespace WebDriver.Google.Search.UITests
{
    [Category("Google Tests")]
    [TestFixture("Chrome")]
    [TestFixture("Firefox")]
    [TestFixture("IE")]
    public class SearchTests
    {

        private TestManager tm;
        private readonly string browserName;
        
        #region | Setup / TearDown |

        public SearchTests(string browserName)
        {
            this.browserName = browserName;
        }

        [SetUp]
        public void SetUp()
        {
            tm = new TestManager(browserName);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                tm.Browser.Quit();
            }
            catch { }
        }

        #endregion

        [Test]
        [TestCase("Cheese")]
        [TestCase("Baconnaise")]
        [TestCase("Crab")]
        public void TestThatICanSearchForSomething(string searchString)
        {
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            tm.Pages.Home.SearchBox.SendKeys(searchString);
            tm.Pages.Home.SearchBox.SendKeys(Keys.Enter);
            //tm.Pages.Home.SearchButton.Click();
            Assert.IsNotNull(tm.Pages.Home.ResultItem(searchString), "A result item was not found.");
        }

    }
}
