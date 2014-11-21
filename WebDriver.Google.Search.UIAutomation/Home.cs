using System;
using System.Collections.Generic;
using System.Linq;
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

        #region | Elements |

        public IWebElement SearchBox { get { return Browser.FindElement(By.Name("q")); } }
        public IWebElement SearchButton { get { return Browser.FindElement(By.Name("btnG"), 1000); } }
        public IList<IWebElement> ResultLinks { get { return Browser.FindElements(By.XPath("//*[@id='rso']//a"), 5000); } }

        #endregion

        public bool HasResults
        {
            get
            {
                try
                {
                    return ResultLinks.Any();
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }
        }

        #region | Methods |

        /// <summary>
        /// Gets a falg to indicate if the results are valid for the query
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public bool HasResultsFor(string searchString)
        {
            return GetResultListStrings(searchString).Any();
        }

        /// <summary>
        /// Gets the result list strings for the query.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public IList<string> GetResultListStrings(string searchString)
        {
            try
            {
                return (from str in ResultLinks
                        select str.Text).Where(str => str.ToLower().Contains(searchString.ToLower())).ToList();
            }
            catch (NoSuchElementException)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Gets the all of the result list strings.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetResultListStrings()
        {
            try
            {
                return (from str in ResultLinks
                        select str.Text).ToList();
            }
            catch (NoSuchElementException)
            {
                return new List<string>();
            }
        }

        #endregion

    }
}
