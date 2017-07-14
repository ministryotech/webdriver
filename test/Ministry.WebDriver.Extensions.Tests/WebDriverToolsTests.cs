using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Ministry.WebDriver.Extensions.Tests
{
    /// <summary>
    /// NOTE: The IWebDriver interface is not mocked in some of the tests here as the methods they test are only relevant in behaviour when using FireFox.
    /// </summary>
    /// <remarks>
    /// These are Mocked Tests on the library itself.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class WebDriverToolsTests
    {
        #region | GetBrowser Tests |

        [Fact]
        public void TestThatAttemptingToObtainAWebDriverInstanceFromAnInvalidDescriptiveBrowserStringThrowsException()
            => Assert.Throws<ArgumentOutOfRangeException>("browserName", () => WebDriverTools.GetBrowser("Invalid Browser"));

        [Fact]
        public void TestThatAttemptingToObtainAWebDriverInstanceWithoutABrowserStringThrowsAnArgumentNullException()
            => Assert.Throws<ArgumentNullException>("browserName", () => WebDriverTools.GetBrowser((string)null));

        [Fact]
        public void TestThatAttemptingToObtainAWebDriverInstanceWithAnEmptyBrowserStringThrowsAnArgumentException()
            => Assert.Throws<ArgumentException>("browserName", () => WebDriverTools.GetBrowser(string.Empty));

        #endregion
    }
}
