using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace Ministry.WebDriver.Extensions
{

    /// <summary>
    /// Extension methods to enhance the available tools for writing WebDriver tests.
    /// </summary>
    public static class WebDriverTools
    {
        /// <summary>
        /// Obtains an IWebDriver implementation by providing a descriptive string.
        /// </summary>
        /// <param name="browserName">The name of the browser to return.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <returns>The implementation of IWebDriver for the specified string.</returns>
        public static IWebDriver GetBrowser(string browserName)
        {
            return (IWebDriver)Activator.CreateInstance(GetBrowserType(browserName));
        }

        /// <summary>
        /// Obtains the type of IWebDriver implementation created by providing a descriptive string.
        /// </summary>
        /// <param name="browserName">The name of the browser to return.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <returns>The implementation of IWebDriver for the specified string.</returns>
        public static Type GetBrowserType(string browserName)
        {
            if (browserName == null) throw new ArgumentNullException("browserName");
            if (browserName == String.Empty) throw new ArgumentException("The 'browserName' parameter cannot be empty", "browserName");

            var testVal = browserName.Replace(" ", "").ToLower();
            switch (testVal)
            {
                case "internetexplorer":
                case "explorer":
                case "ie":
                case "msie":
                case "microsoftinternetexplorer":
                case "windowsexplorer":
                case "windowsinternetexplorer":
                    return typeof(InternetExplorerDriver);
                case "firefox":
                case "mozilla":
                case "mozillafirefox":
                    return typeof(FirefoxDriver);
                case "chrome":
                case "google":
                case "googlechrome":
                    return typeof(ChromeDriver);
                case "phantom":
                case "ghost":
                case "phantomjs":
                    return typeof (PhantomJSDriver);
                default:
                    throw new ArgumentOutOfRangeException("browserName", "The browser '" + browserName + "' is not supported by WebDriver");
            }
        }

        /// <summary>
        /// Navigates directly to a provided page object.
        /// </summary>
        /// <param name="navigation">The navigation item to navigate with.</param>
        /// <param name="page">The page to navigate to.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static void GoToPage(this INavigation navigation, AutomationPage page)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (page == null) throw new ArgumentNullException("page");

            navigation.GoToUrl(page.Url);
        }

        /// <summary>
        /// Suspends the browser for the specified period of time.
        /// </summary>
        /// <param name="browser">The browser instance to wait.</param>
        /// <param name="millisecondsTimeout">The period to wait for.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static void Wait(this IWebDriver browser, int millisecondsTimeout)
        {
            if (browser == null) throw new ArgumentNullException("browser");

            Thread.Sleep(millisecondsTimeout);
        }

        /// <summary>
        /// Suspends the browser until the given element exists and is visible unless the timeout value expires.
        /// </summary>
        /// <param name="browser">The browser instance to wait.</param>
        /// <param name="elementSearchDefinition">The criteria used tio locate the element to wait for.</param>
        /// <param name="millisecondsTimeout">The period at which the wait will throw an exception.</param>
        /// <returns>The element searched for.</returns>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when the item is not found in the given time period.</exception>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static IWebElement FindElement(this IWebDriver browser, By elementSearchDefinition, int millisecondsTimeout)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (elementSearchDefinition == null) throw new ArgumentNullException("elementSearchDefinition");

            var currentPeriod = 0;
            const int waitPeriod = 500;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementToWaitFor = browser.FindElement(elementSearchDefinition);

                    if (elementToWaitFor != null && elementToWaitFor.Displayed)
                        return elementToWaitFor;

                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
                catch (WebDriverException)
                {
                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
            } 

            throw new NoSuchElementException("WebDriver timed out while waiting for element '" + elementSearchDefinition + "'");
        }

        /// <summary>
        /// Suspends the browser until the given elements exist and are visible unless the timeout value expires.
        /// </summary>
        /// <param name="browser">The browser instance to wait.</param>
        /// <param name="elementSearchDefinition">The criteria used tio locate the element to wait for.</param>
        /// <param name="millisecondsTimeout">The period at which the wait will throw an exception.</param>
        /// <returns>The element searched for.</returns>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when the items are not found in the given time period.</exception>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static IList<IWebElement> FindElements(this IWebDriver browser, By elementSearchDefinition, int millisecondsTimeout)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (elementSearchDefinition == null) throw new ArgumentNullException("elementSearchDefinition");

            var currentPeriod = 0;
            const int waitPeriod = 500;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementsToWaitFor = browser.FindElements(elementSearchDefinition);

                    if (elementsToWaitFor.Any())
                    {
                        var displayedItems = (from e in elementsToWaitFor
                                              where e.Displayed
                                              select e).ToList();
                        if (displayedItems.Any()) return displayedItems;
                    }

                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
                catch (WebDriverException)
                {
                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
            }

            throw new NoSuchElementException("WebDriver timed out while waiting for elements '" + elementSearchDefinition + "'");
        }


        #region | Private Methods |

        /// <summary>
        /// Waits for a given period and returns the total period waited for overall.
        /// </summary>
        /// <param name="millisecondsTimeoutOverride">The timeout override.</param>
        /// <param name="currentTotalWaitPeriod">The total wait period accrued so far.</param>
        /// <param name="waitPeriod">The individual wait period.</param>
        /// <returns>The combined total wait period after the event.</returns>
        private static int WaitForElement(int millisecondsTimeoutOverride, int currentTotalWaitPeriod, int waitPeriod)
        {
            Thread.Sleep(waitPeriod < millisecondsTimeoutOverride ? waitPeriod : millisecondsTimeoutOverride);
            currentTotalWaitPeriod += waitPeriod;
            return currentTotalWaitPeriod;
        }

        #endregion
    }

}
