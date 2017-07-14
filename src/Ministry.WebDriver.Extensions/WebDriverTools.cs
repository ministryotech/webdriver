using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// Extension methods to enhance the available tools for writing WebDriver tests.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class WebDriverTools
    {
        #region | Browser |

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
            => GetBrowser(GetBrowserType(browserName), options);

        /// <summary>
        /// Obtains an IWebDriver implementation by providing a descriptive string.
        /// </summary>
        /// <param name="browserType">The type of the browser driver to return.</param>
        /// <param name="options">The options for browser usage.</param>
        /// <returns>
        /// The implementation of IWebDriver for the specified string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static IWebDriver GetBrowser(Type browserType, BrowserOptions options = null)
        {
            if (browserType == typeof(PhantomJSDriver))
            {
                return GetPhantomJSBrowser(options);
            }
            if (browserType == typeof(ChromeDriver))
            {
                return GetChromeBrowser();
            }
            if (browserType == typeof(EdgeDriver))
            {
                return GetEdgeBrowser();
            }
            if (browserType == typeof(InternetExplorerDriver))
            {
                return GetInternetExplorerBrowser();
            }
            if (browserType == typeof(FirefoxDriver))
            {
                return GetFirefoxBrowser();
            }

            throw new ArgumentException(nameof(browserType), $"The specified browser type {browserType} is not supported.");
        }

        /// <summary>
        /// Gets the Chrome Browser.
        /// </summary>
        /// <returns>A browser instance.</returns>
        private static IWebDriver GetChromeBrowser()
        {
            var chromeService = ChromeDriverService.CreateDefaultService(GetExecutionPath());
            chromeService.HideCommandPromptWindow = true;

            var chromeOptions = new ChromeOptions
            {
                LeaveBrowserRunning = false
            };

            return new ChromeDriver(chromeService, chromeOptions);
        }

        /// <summary>
        /// Gets the Firefox Browser.
        /// </summary>
        /// <returns>A browser instance.</returns>
        private static IWebDriver GetFirefoxBrowser()
        {
            var foxService = FirefoxDriverService.CreateDefaultService(GetExecutionPath());
            foxService.HideCommandPromptWindow = true;

            var foxOptions = new FirefoxOptions();

            return new FirefoxDriver(foxService, foxOptions, new TimeSpan(0,0,20));
        }

        /// <summary>
        /// Gets the Edge Browser.
        /// </summary>
        /// <returns>A browser instance.</returns>
        private static IWebDriver GetEdgeBrowser()
        {
            var edgeService = EdgeDriverService.CreateDefaultService(GetExecutionPath());
            edgeService.HideCommandPromptWindow = true;

            var edgeOptions = new EdgeOptions();

            return new EdgeDriver(edgeService, edgeOptions);
        }

        /// <summary>
        /// Gets the IE Browser.
        /// </summary>
        /// <returns>A browser instance.</returns>
        private static IWebDriver GetInternetExplorerBrowser()
        {
            var ieService = InternetExplorerDriverService.CreateDefaultService(GetExecutionPath());
            ieService.HideCommandPromptWindow = true;

            var ieOptions = new InternetExplorerOptions();

            return new InternetExplorerDriver(ieService, ieOptions);
        }

        /// <summary>
        /// Gets the PhantomJS Browser.
        /// </summary>
        /// <returns>A browser instance.</returns>
        private static IWebDriver GetPhantomJSBrowser(BrowserOptions options)
        {
            var pjsService = PhantomJSDriverService.CreateDefaultService(GetExecutionPath());
            if (options == null)
                options = new BrowserOptions();

            pjsService.IgnoreSslErrors = options.IgnoreSslErrors;
            pjsService.LoadImages = options.LoadImages;
            pjsService.ProxyType = "none";

            return new PhantomJSDriver(pjsService);
        }

        /// <summary>
        /// Gets the execution path for the drivers.
        /// </summary>
        /// <returns>They should be present in the executing directory.</returns>
        private static string GetExecutionPath() => Directory.GetCurrentDirectory();

        /// <summary>
        /// Obtains the type of IWebDriver implementation created by providing a descriptive string.
        /// </summary>
        /// <param name="browserName">The name of the browser to return.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        /// <returns>The implementation of IWebDriver for the specified string.</returns>
        public static Type GetBrowserType(string browserName)
        {
            switch (browserName.ThrowIfNullOrEmpty(nameof(browserName)).Replace(" ", "").ToLower())
            {
                case "phantom":
                case "ghost":
                case "phantomjs":
                case "headless":
                    return typeof(PhantomJSDriver);
                case "chrome":
                case "google":
                case "googlechrome":
                    return typeof(ChromeDriver);
                case "edge":
                case "microsoftedge":
                    return typeof(EdgeDriver);
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
                case "gecko":
                    return typeof(FirefoxDriver);
                default:
                    throw new ArgumentOutOfRangeException(nameof(browserName), $"The browser '{browserName}' is not supported by WebDriver");
            }
        }

        #endregion | GetBrowser |

        #region | Navigation |

        /// <summary>
        /// Navigates directly to a provided page object.
        /// </summary>
        /// <param name="navigation">The navigation item to navigate with.</param>
        /// <param name="page">The page to navigate to.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static void GoToPage(this INavigation navigation, IAutomationPage page)
            => navigation.ThrowIfNull(nameof(navigation)).GoToUrl(page.ThrowIfNull(nameof(page)).Url);

        /// <summary>
        /// Navigates to a page.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="page">The page.</param>
        public static void NavigateTo(this IWebDriver browser, IAutomationPage page)
        {
            browser.Navigate().GoToPage(page);

            if (page.Url.Contains("https") && browser.GetType() == typeof(InternetExplorerDriver))
                BypassHttpsWarning(browser as InternetExplorerDriver);
        }

        /// <summary>
        /// Navigates to a URL.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="url">The URL.</param>
        public static void NavigateTo(this IWebDriver browser, string url)
        {
            browser.Navigate().GoToUrl(url);

            if (url.Contains("https") && browser.GetType() == typeof(InternetExplorerDriver))
                BypassHttpsWarning(browser as InternetExplorerDriver);
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
                warning?.Click();
            }
            catch (NoSuchElementException)
            { }
        }

        #endregion | Navigation |

        /// <summary>
        /// Closes the browser and quits the driver instance.
        /// </summary>
        /// <remarks>
        /// Firefox has a tendency to leave orphans floating about if Quit is used alone.
        /// </remarks>
        /// <param name="browser">The browser.</param>
        public static void CloseAndQuit(this IWebDriver browser)
        {
            browser.Close();
            browser.Quit();
        }

        /// <summary>
        /// Suspends the browser for the specified period of time.
        /// </summary>
        /// <param name="browser">The browser instance to wait.</param>
        /// <param name="millisecondsTimeout">The period to wait for.</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null.</exception>
        public static void Wait(this IWebDriver browser, int millisecondsTimeout)
        {
            browser.ThrowIfNull(nameof(browser));
            Thread.Sleep(millisecondsTimeout);
        }

        #region | Element Searching |

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
            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementToWaitFor = browser.ThrowIfNull(nameof(browser))
                        .FindElement(elementSearchDefinition.ThrowIfNull(nameof(elementSearchDefinition)));

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
            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementToWaitFor = parentElement.ThrowIfNull(nameof(parentElement))
                        .FindElement(elementSearchDefinition.ThrowIfNull(nameof(elementSearchDefinition)));

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
            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementsToWaitFor = browser.ThrowIfNull(nameof(browser))
                        .FindElements(elementSearchDefinition.ThrowIfNull(nameof(elementSearchDefinition)));

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
            var currentPeriod = 0;
            const int waitPeriod = 500;
            WebDriverException lastWex = null;

            while (currentPeriod < millisecondsTimeout)
            {
                try
                {
                    var elementsToWaitFor = parentElement.ThrowIfNull(nameof(parentElement))
                        .FindElements(elementSearchDefinition.ThrowIfNull(nameof(elementSearchDefinition)));

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

        #endregion | Element Searching |

        #region | Inner Html Fetchers |

        /// <summary>
        /// Gets the inner HTML content value of the element whether visible or not..
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The text.</returns>
        public static string InnerHtml(this IWebElement element)
            => element.GetAttribute("innerHTML");

        /// <summary>
        /// Gets a collection of the Inner HTML for a selection of elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="trimmed">if set to <c>true</c> trim the text.</param>
        /// <returns></returns>
        public static IList<string> InnerHtmlList(this IList<IWebElement> elements, bool trimmed = false)
            => trimmed 
                ? elements.Select(el => el.InnerHtml()).Select(item => item.Trim()).ToList() 
                : elements.Select(el => el.InnerHtml()).ToList();

        /// <summary>
        /// Gets a collection of the Inner HTML for a selection of elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="trimmed">if set to <c>true</c> trim the text.</param>
        /// <returns></returns>
        public static IQueryable<string> InnerHtmlList(this IQueryable<IWebElement> elements, bool trimmed = false)
            => trimmed
                ? elements.Select(el => el.InnerHtml()).Select(item => item.Trim())
                : elements.Select(el => el.InnerHtml());

        #endregion | Inner Html Fetchers |
    }

}
