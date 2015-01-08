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
        /// <param name="options">The options for browser usage.</param>
        /// <returns>
        /// The implementation of IWebDriver for the specified string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static IWebDriver GetBrowser(string browserName, BrowserOptions options = null)
        {
            var browserType = GetBrowserType(browserName);
            if (browserType == typeof (PhantomJSDriver))
            {
                var pjsService = PhantomJSDriverService.CreateDefaultService();
                if (options == null)
                    options = new BrowserOptions();

                pjsService.IgnoreSslErrors = options.IgnoreSslErrors;
                pjsService.LoadImages = options.LoadImages;
                pjsService.ProxyType = "none";

                return new PhantomJSDriver(pjsService);
            }
            if (browserType == typeof(InternetExplorerDriver))
            {
                var ieService = InternetExplorerDriverService.CreateDefaultService();
                ieService.HideCommandPromptWindow = true;

                var ieOptions = new InternetExplorerOptions();

                return new InternetExplorerDriver(ieService, ieOptions);
            }
            if (browserType == typeof(FirefoxDriver))
            {
                return new FirefoxDriver();
            }
            if (browserType == typeof(ChromeDriver))
            {
                var chromeService = ChromeDriverService.CreateDefaultService();
                chromeService.HideCommandPromptWindow = true;

                var chromeOptions = new ChromeOptions
                {
                    LeaveBrowserRunning = false
                };

                return new ChromeDriver(chromeService, chromeOptions);
            }

            throw new ArgumentOutOfRangeException("browserName", "The browser '" + browserName + "' is not supported by WebDriver");
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
        public static void GoToPage(this INavigation navigation, IAutomationPage page)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (page == null) throw new ArgumentNullException("page");

            navigation.GoToUrl(page.Url);
        }

        /// <summary>
        /// Bypasses the HTTPS warning shown by IE.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="timeout">The timeout.</param>
        public static void BypassHttpsWarning(this InternetExplorerDriver browser, int timeout = 1000)
        {
            try
            {
                var warning = browser.FindElement(By.Id("overridelink"), timeout);
                if (warning != null) warning.Click();
            }
            catch (NoSuchElementException)
            { }
        }

        /// <summary>
        /// Navigates to a page.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="page">The page.</param>
        public static void NavigateTo(this IWebDriver browser, IAutomationPage page)
        {
            browser.Navigate().GoToPage(page);

            if (page.Url.Contains("https") && (browser.GetType() == typeof (InternetExplorerDriver)))
            {
                BypassHttpsWarning(browser as InternetExplorerDriver);
            }
        }

        /// <summary>
        /// Navigates to a URL.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="url">The URL.</param>
        public static void NavigateTo(this IWebDriver browser, string url)
        {
            browser.Navigate().GoToUrl(url);

            if (url.Contains("https") && (browser.GetType() == typeof(InternetExplorerDriver)))
            {
                BypassHttpsWarning(browser as InternetExplorerDriver);
            }
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
        /// <param name="requireVisible">if set to <c>true</c> requires that the elements are visible.</param>
        /// <returns>
        /// The element searched for.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="OpenQA.Selenium.NoSuchElementException">WebDriver timed out while waiting for element ' + elementSearchDefinition + '</exception>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when the item is not found in the given time period.</exception>
        public static IWebElement FindElement(this IWebDriver browser, By elementSearchDefinition, int millisecondsTimeout, bool requireVisible = false)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (elementSearchDefinition == null) throw new ArgumentNullException("elementSearchDefinition");

            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementToWaitFor = browser.FindElement(elementSearchDefinition);

                    if (elementToWaitFor != null && (!requireVisible || elementToWaitFor.Displayed))
                        return elementToWaitFor;

                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
                catch (WebDriverException wex)
                {
                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                    lastWex = wex;
                }
            }

            if (lastWex != null)
            {
                throw new NoSuchElementException(
                    "WebDriver timed out while waiting for element '" + elementSearchDefinition + "'", lastWex);
            }
            throw new NoSuchElementException("WebDriver timed out while waiting for element '" + elementSearchDefinition + "'");
        }

        /// <summary>
        /// Suspends the browser until the given element exists and is visible unless the timeout value expires.
        /// </summary>
        /// <param name="parentElement">The browser instance to wait.</param>
        /// <param name="elementSearchDefinition">The criteria used tio locate the element to wait for.</param>
        /// <param name="millisecondsTimeout">The period at which the wait will throw an exception.</param>
        /// <param name="requireVisible">if set to <c>true</c> requires that the elements are visible.</param>
        /// <returns>
        /// The element searched for.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="OpenQA.Selenium.NoSuchElementException">WebDriver timed out while waiting for element ' + elementSearchDefinition + '</exception>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when the item is not found in the given time period.</exception>
        public static IWebElement FindElement(this IWebElement parentElement, By elementSearchDefinition, int millisecondsTimeout, bool requireVisible = false)
        {
            if (parentElement == null) throw new ArgumentNullException("parentElement");
            if (elementSearchDefinition == null) throw new ArgumentNullException("elementSearchDefinition");

            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementToWaitFor = parentElement.FindElement(elementSearchDefinition);

                    if (elementToWaitFor != null && (!requireVisible || elementToWaitFor.Displayed))
                        return elementToWaitFor;

                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
                catch (WebDriverException wex)
                {
                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                    lastWex = wex;
                }
            }

            if (lastWex != null)
            {
                throw new NoSuchElementException(
                    "WebDriver timed out while waiting for element '" + elementSearchDefinition + "'", lastWex);
            }
            throw new NoSuchElementException("WebDriver timed out while waiting for element '" + elementSearchDefinition + "'");
        }

        /// <summary>
        /// Suspends the browser until the given elements exist and are visible unless the timeout value expires.
        /// </summary>
        /// <param name="browser">The browser instance to wait.</param>
        /// <param name="elementSearchDefinition">The criteria used tio locate the element to wait for.</param>
        /// <param name="millisecondsTimeout">The period at which the wait will throw an exception.</param>
        /// <param name="requireVisible">if set to <c>true</c> requires that the elements are visible.</param>
        /// <returns>
        /// The element searched for.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="OpenQA.Selenium.NoSuchElementException">WebDriver timed out while waiting for elements ' + elementSearchDefinition + '</exception>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when the items are not found in the given time period.</exception>
        public static IList<IWebElement> FindElements(this IWebDriver browser, By elementSearchDefinition, int millisecondsTimeout, bool requireVisible = false)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (elementSearchDefinition == null) throw new ArgumentNullException("elementSearchDefinition");

            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementsToWaitFor = browser.FindElements(elementSearchDefinition);

                    if (elementsToWaitFor.Any())
                    {
                        if (requireVisible)
                        {
                            var displayedItems = (from e in elementsToWaitFor
                                where e.Displayed
                                select e).ToList();
                            if (displayedItems.Any()) return displayedItems;
                        }
                        else
                        {
                            return elementsToWaitFor;
                        }
                    }

                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
                catch (WebDriverException wex)
                {
                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                    lastWex = wex;
                }
            }

            if (lastWex != null)
            {
                throw new NoSuchElementException(
                    "WebDriver timed out while waiting for element '" + elementSearchDefinition + "'", lastWex);
            }
            throw new NoSuchElementException("WebDriver timed out while waiting for elements '" + elementSearchDefinition + "'");
        }

        /// <summary>
        /// Suspends the browser until the given elements exist and are visible unless the timeout value expires.
        /// </summary>
        /// <param name="parentElement">The browser instance to wait.</param>
        /// <param name="elementSearchDefinition">The criteria used tio locate the element to wait for.</param>
        /// <param name="millisecondsTimeout">The period at which the wait will throw an exception.</param>
        /// <param name="requireVisible">if set to <c>true</c> requires that the elements are visible.</param>
        /// <returns>
        /// The element searched for.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="OpenQA.Selenium.NoSuchElementException">WebDriver timed out while waiting for elements ' + elementSearchDefinition + '</exception>
        /// <exception cref="OpenQA.Selenium.WebDriverException">Thrown when the items are not found in the given time period.</exception>
        public static IList<IWebElement> FindElements(this IWebElement parentElement, By elementSearchDefinition, int millisecondsTimeout, bool requireVisible = false)
        {
            if (parentElement == null) throw new ArgumentNullException("parentElement");
            if (elementSearchDefinition == null) throw new ArgumentNullException("elementSearchDefinition");

            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementsToWaitFor = parentElement.FindElements(elementSearchDefinition);

                    if (elementsToWaitFor.Any())
                    {
                        if (requireVisible)
                        {
                            var displayedItems = (from e in elementsToWaitFor
                                                  where e.Displayed
                                                  select e).ToList();
                            if (displayedItems.Any()) return displayedItems;
                        }
                        else
                        {
                            return elementsToWaitFor;
                        }
                    }

                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                }
                catch (WebDriverException wex)
                {
                    currentPeriod = WaitForElement(millisecondsTimeout, currentPeriod, waitPeriod);
                    lastWex = wex;
                }
            }

            if (lastWex != null)
            {
                throw new NoSuchElementException(
                    "WebDriver timed out while waiting for element '" + elementSearchDefinition + "'", lastWex);
            }
            throw new NoSuchElementException("WebDriver timed out while waiting for elements '" + elementSearchDefinition + "'");
        }

        /// <summary>
        /// Gets the inner HTML content value of the element whether visible or not..
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The text.</returns>
        public static string InnerHtml(this IWebElement element)
        {
            return element.GetAttribute("innerHTML");
        }

        /// <summary>
        /// Gets a collection of the Inner HTML for a selection of elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="trimmed">if set to <c>true</c> trim the text.</param>
        /// <returns></returns>
        public static IList<String> InnerHtmlList(this IList<IWebElement> elements, bool trimmed = false)
        {
            var list = (from el in elements
                        select el.InnerHtml()).ToList();

            return trimmed ? list.Select(item => item.Trim()).ToList() : list;
        }

        /// <summary>
        /// Gets a collection of the Inner HTML for a selection of elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="trimmed">if set to <c>true</c> trim the text.</param>
        /// <returns></returns>
        public static IQueryable<String> InnerHtmlList(this IQueryable<IWebElement> elements, bool trimmed = false)
        {
            var list = (from el in elements
                        select el.InnerHtml());

            return trimmed ? list.Select(item => item.Trim()) : list;
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
