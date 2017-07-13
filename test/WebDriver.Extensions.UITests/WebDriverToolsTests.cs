using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Google.Search.UIAutomation;
using OpenQA.Selenium;
using Xunit;

namespace Ministry.WebDriverCore.UITests
{
    /// <summary>
    /// NOTE: The IWebDriver interface is not mocked in some of the tests here as the methods they test are only relevant in behaviour when using FireFox.
    /// </summary>
    /// <remarks>
    /// These are E2E Tests on the library itself.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class WebDriverToolsTests : IDisposable
    {
        private TestManager tm;

        #region | Setup / TearDown |

        /// <summary>
        /// Tears down the test class.
        /// </summary>
        public void Dispose()
        {
            try
            {
                tm.Browser.CloseAndQuit();
            }
            // ReSharper disable once RedundantEmptyFinallyBlock
            finally
            { }
        }

        #endregion

        #region | GetBrowser Tests |

        [Theory]
        [InlineData("InternetExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("Internet Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("MicrosoftInternetExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("Microsoft Internet Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("WindowsExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("Windows Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("WindowsInternetExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("Windows Internet Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("IE", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [InlineData("MSIE", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        public void TestThatICanObtainAWebDriverInstanceForIE(string browserName, string objectType)
        {
            tm = new TestManager(browserName);
            Assert.Equal(objectType, tm.Browser.GetType().ToString());
        }

        [Theory]
        [InlineData("Microsoft Edge", "OpenQA.Selenium.WindowsWebDriver")]
        [InlineData("Edge", "OpenQA.Selenium.WindowsWebDriver")]
        public void TestThatICanObtainAWebDriverInstanceForEdge(string browserName, string objectType)
        {
            tm = new TestManager(browserName);
            Assert.Equal(objectType, tm.Browser.GetType().ToString());
        }

        [Theory]
        [InlineData("MozillaFirefox", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [InlineData("Mozilla Firefox", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [InlineData("Firefox", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [InlineData("Mozilla", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [InlineData("Gecko", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        public void TestThatICanObtainAWebDriverInstanceForFirefox(string browserName, string objectType)
        {
            tm = new TestManager(browserName);
            Assert.Equal(objectType, tm.Browser.GetType().ToString());
        }

        [Theory]
        [InlineData("GoogleChrome", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [InlineData("Google Chrome", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [InlineData("Google", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [InlineData("Chrome", "OpenQA.Selenium.Chrome.ChromeDriver")]
        public void TestThatICanObtainAWebDriverInstanceForChrome(string browserName, string objectType)
        {
            tm = new TestManager(browserName);
            Assert.Equal(objectType, tm.Browser.GetType().ToString());
        }

        [Theory]
        [InlineData("Phantom", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        [InlineData("Ghost", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        [InlineData("PhantomJS", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        [InlineData("Headless", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        public void TestThatICanObtainAWebDriverInstanceForPhantomJS(string browserName, string objectType)
        {
            tm = new TestManager(browserName);
            Assert.Equal(objectType, tm.Browser.GetType().ToString());
        }

        #endregion

        #region | GoToPage Tests |

        [Fact]
        [Category("Navigation Tests")]
        public void TestThatAttemptingToNavigateToAPageWithANullPageObjectThrowsAnArgumentNullException()
            => Assert.Throws<ArgumentNullException>("page", () => {
                tm = new TestManager("Chrome");
                tm.Browser.Navigate().GoToPage(null);
            });

        #endregion

        #region | Wait Tests |

        [Fact]
        public void TestThatICanSuspendCodeExecutionForAGivenTimePeriod()
        {
            tm = new TestManager("Chrome");
            tm.Browser.Wait(1000);
        }

        #endregion

        #region | FindElement Tests |

        [Theory]
        [InlineData("Firefox")]
        [InlineData("Chrome")]
        [InlineData("PhantomJS")]
        [InlineData("InternetExplorer")]
        [InlineData("Edge")]
        public void TestThatFindingAnElementWithALongTimeoutWorksInAllSupportedBrowsers(string browserName)
        {
            try
            {
                tm = new TestManager(browserName);
                tm.Browser.Navigate().GoToPage(tm.Pages.Home);
                tm.Pages.Home.SearchBox.SendKeys("Cheese");
                tm.Pages.Home.SearchButton.Click();
                var expectedResultItem = tm.Browser.FindElement(By.Name("btnG"), 1000);

                Assert.NotNull(expectedResultItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"Test run failed for browser '{browserName}': [{ex.GetType()}] {ex.Message}.", ex);
            }
        }

        [Fact]
        public void TestThatFindingAnElementWithAShortTimeoutThrowsAnExceptionInFirefox()
        {
            tm = new TestManager("Firefox");
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            tm.Pages.Home.SearchBox.SendKeys("Cheese");
            tm.Pages.Home.SearchButton.Click();

            Assert.Throws<NoSuchElementException>(() => tm.Browser.FindElement(By.XPath("//li/div/span/h3/a[contains(text(), 'British')]"), 1));
        }

        [Fact]
        public void TestThatFindingAnElementWithNoCriteriaThrowsAnArgumentNullException()
        {
            tm = new TestManager("Chrome");
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            Assert.Throws<ArgumentNullException>("elementSearchDefinition", () => tm.Browser.FindElement(null, 1));
        }

        #endregion
    }
}
