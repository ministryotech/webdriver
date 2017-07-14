using System.Diagnostics.CodeAnalysis;

namespace Ministry.WebDriver.Extensions
{
    /// <summary>
    /// Collection of options for running the browser.
    /// </summary>
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class BrowserOptions
    {
        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserOptions"/> class with default settings.
        /// </summary>
        public BrowserOptions()
        {
            IgnoreSslErrors = true;
            LoadImages = true;
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether to ignore SSL errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if you wish to ignore SSL errors; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Default value - 'true'
        /// Supported by - 'PhantomJS'
        /// Unsupported behaviour - 'false'
        /// </remarks>
        public bool IgnoreSslErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to load images.
        /// </summary>
        /// <value>
        ///   <c>true</c> if you want to load images; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Default value - 'true'
        /// Supported by - 'PhantomJS'
        /// Unsupported behaviour - 'true'
        /// </remarks>
        public bool LoadImages { get; set; }
    }
}
