using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
using WebDriver.Google.Search.UIAutomation;

namespace Ministry.WebDriver.Extensions.Tests
{
    /// <summary>
    /// NOTE: The IWebDriver interface is not mocked in some of the tests here as the methods they test are only relevant in behaviour when using FireFox.
    /// </summary>
    [Category("WebDriver Extensions Tests")]
    [TestFixture]
    public class WebDriverToolsTests
    {
        private TestManager tm;
        private bool threadSleepCompleted;

        #region | Setup / TearDown |

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

        #region | GetBrowser Tests |

        [Test]
        [Category("Browser Detection Tests")]
        [TestCase("InternetExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("Internet Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("MicrosoftInternetExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("Microsoft Internet Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("WindowsExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("Windows Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("WindowsInternetExplorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("Windows Internet Explorer", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("IE", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("MSIE", "OpenQA.Selenium.IE.InternetExplorerDriver")]
        [TestCase("MozillaFirefox", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [TestCase("Mozilla Firefox", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [TestCase("Firefox", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [TestCase("Mozilla", "OpenQA.Selenium.Firefox.FirefoxDriver")]
        [TestCase("GoogleChrome", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [TestCase("Google Chrome", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [TestCase("Google", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [TestCase("Chrome", "OpenQA.Selenium.Chrome.ChromeDriver")]
        [TestCase("Phantom", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        [TestCase("Ghost", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        [TestCase("PhantomJS", "OpenQA.Selenium.PhantomJS.PhantomJSDriver")]
        public void TestThatICanObtainAWebDriverInstanceFromADescriptiveBrowserString(string browserName, string objectType)
        {
            tm = new TestManager(browserName);
            Assert.AreEqual(objectType, tm.Browser.GetType().ToString());
        }

        [Test]
        [Category("Browser Detection Tests")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestThatAttemptingToObtainAWebDriverInstanceFromAnInvalidDescriptiveBrowserStringThrowsException()
        {
            WebDriverTools.GetBrowser("Invalid Browser");
        }

        [Test]
        [Category("Browser Detection Tests")]
        public void TestThatAttemptingToObtainAWebDriverInstanceWithoutABrowserStringThrowsAnArgumentNullException()
        {
            var anex = Assert.Throws(typeof(ArgumentNullException), () => WebDriverTools.GetBrowser(null)) as ArgumentNullException;
            Assert.IsNotNull(anex);
            Assert.AreEqual("browserName", anex.ParamName, "The invalid parameter name should be 'browserName'");
        }

        [Test]
        [Category("Browser Detection Tests")]
        public void TestThatAttemptingToObtainAWebDriverInstanceWithAnEmptyBrowserStringThrowsAnArgumentException()
        {
            var anex = Assert.Throws(typeof(ArgumentException), () => WebDriverTools.GetBrowser(String.Empty)) as ArgumentException;
            Assert.IsNotNull(anex);
            Assert.AreEqual("browserName", anex.ParamName, "The invalid parameter name should be 'browserName'");
        }

        #endregion

        #region | GoToPage Tests |

        [Test]
        [Category("Navigation Tests")]
        public void TestThatAttemptingToNavigateToAPageWithANullPageObjectThrowsAnArgumentNullException()
        {
            var anex = Assert.Throws(typeof(ArgumentNullException), NavigateToNullPage) as ArgumentNullException;
            Assert.IsNotNull(anex);
            Assert.AreEqual("page", anex.ParamName, "The invalid parameter name should be 'page'");
        }

        private void NavigateToNullPage()
        {
            tm = new TestManager();
            tm.Browser.Navigate().GoToPage(null);
        }

        #endregion

        #region | Wait Tests |

        [Test]
        [Category("Race Condition Management Tests")]
        public void TestThatICanSuspendCodeExecutionForAGivenTimePeriod()
        {
            tm = new TestManager(new Mock<IWebDriver>().Object);
            threadSleepCompleted = false;
            TimerCallback tcb = TimerComplete;
            // ReSharper disable once UnusedVariable
            var threadTimer = new Timer(tcb, null, 0, 11000);

            // Do a vague test to check that the period is roughly correct.
            tm.Browser.Wait(10000);
            threadSleepCompleted = true;
        }

        public void TimerComplete(object state)
        {
            Assert.IsTrue(threadSleepCompleted);
        }

        #endregion

        #region | FindElement Tests |

        [Test]
        [Category("Google Tests")]
        [Category("Element Tests")]
        [TestCase("Firefox")]
        [TestCase("Chrome")]
        [TestCase("PhantomJS")]
        [TestCase("InternetExplorer")]
        public void TestThatFindingAnElementWithALongTimeoutWorksInAllSupportedBrowsers(string browserName)
        {
            tm = new TestManager(browserName);
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            tm.Pages.Home.SearchBox.SendKeys("Cheese");
            tm.Pages.Home.SearchButton.Click();
            var expectedResultItem = tm.Browser.FindElement(By.Name("btnG"), 1000);
            Assert.IsNotNull(expectedResultItem, "The expected result item was not found.");
        }

        [Test]
        [Category("Google Tests")]
        [Category("Element Tests")]
        [ExpectedException(typeof(NoSuchElementException))]
        public void TestThatFindingAnElementWithAShortTimeoutThrowsAnExceptionInFirefox()
        {
            tm = new TestManager("Firefox");
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            tm.Pages.Home.SearchBox.SendKeys("Cheese");
            tm.Pages.Home.SearchButton.Click();
            var expectedResultItem = tm.Browser.FindElement(By.XPath("//li/div/span/h3/a[contains(text(), 'British')]"), 1);
            Assert.IsNotNull(expectedResultItem, "The expected result item was not found.");
        }

        [Test]
        [Category("Google Tests")]
        [Category("Element Tests")]
        public void TestThatFindingAnElementWithNoCriteriaThrowsAnArgumentNullException()
        {
            tm = new TestManager();
            tm.Browser.Navigate().GoToPage(tm.Pages.Home);
            var anex = Assert.Throws(typeof(ArgumentNullException), () => tm.Browser.FindElement(null, 1)) as ArgumentNullException;
            Assert.IsNotNull(anex);
            Assert.AreEqual("elementSearchDefinition", anex.ParamName, "The invalid parameter name should be 'elementSearchDefinition'");
        }

        #endregion
    }
}
