using System;
using System.Diagnostics.CodeAnalysis;
using Google.Search.UIAutomation;
using Ministry.WebDriver.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Xunit;

namespace Google.Search.UITests
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SearchChromeTests : SearchCrossBrowserTests
    {
        public SearchChromeTests() : base("Chrome")
        {}
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SearchFirefoxTests : SearchCrossBrowserTests
    {
        public SearchFirefoxTests() : base("Firefox")
        { }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SearchPhantomJSTests : SearchCrossBrowserTests
    {
        public SearchPhantomJSTests() : base("PhantomJS")
        { }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class SearchCrossBrowserTests : IDisposable
    {
        private readonly TestManager tm;

        #region | Setup / TearDown |

        protected SearchCrossBrowserTests(string browserName)
        {
            tm = new TestManager(browserName);
        }

        public void Dispose()
        {
            try
            {
                tm.Browser.CloseAndQuit();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        #endregion

        [Theory]
        [InlineData("Cheese")]
        [InlineData("Baconnaise")]
        [InlineData("Crab")]
        public void TestThatICanSearchForSomething(string searchString)
        {
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            tm.Pages.Home.SearchBox.SendKeys(searchString);
            tm.Pages.Home.SearchBox.SendKeys(Keys.Enter);
            tm.Pages.Home.SearchButton.Click();

            if (tm.Browser.GetType() == typeof(PhantomJSDriver))
            {
                // PhantomJS and Google do NOT get along.
                Assert.True(tm.Browser.Title.Contains(searchString));
            }
            else
            {
                Assert.True(tm.Pages.Home.HasResults, "No results were loaded");
                Assert.True(tm.Pages.Home.HasResultsFor(searchString),
                    "The results found do not appear to be for the provided search query");
            }
        }

    }
}
