using System.Linq;
using System.Runtime.Remoting.Messaging;
using Ministry.WebDriver.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using WebDriver.Google.Search.UIAutomation;

namespace WebDriver.Google.Search.UITests
{
    [Category("Google Tests")]
    [TestFixture("Chrome")]
    [TestFixture("Firefox")]
    [TestFixture("PhantomJS")]
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
            // ReSharper disable once EmptyGeneralCatchClause
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
            tm.Pages.Home.SearchButton.Click();

            if (tm.Browser.GetType() == typeof(PhantomJSDriver))
            {
                // PhantomJS and Google do NOT get along.
                Assert.That(tm.Browser.Title.Contains(searchString));
            }
            else
            {
                Assert.That(tm.Pages.Home.HasResults, "No results were loaded");
                Assert.That(tm.Pages.Home.HasResultsFor(searchString),
                    "The results found do not appear to be for the provided search query");
            }
        }

    }
}
