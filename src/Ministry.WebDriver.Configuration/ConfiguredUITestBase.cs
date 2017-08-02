using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Ministry.WebDriver.Extensions;

namespace Ministry.WebDriver.Configuration
{
    /// <summary>
    /// Base class to be inherrited from by all tests.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class ConfiguredUITestBase<TTestManager>
        where TTestManager : TestManagerBase
    {
        private bool configInitialised;

        private IConfigurationRoot configuration;

        /// <summary>
        /// Gets the Test Manager.
        /// </summary>
        protected TTestManager UITest { get; set; }

        /// <summary>
        /// Gets the configuration data for the application.
        /// </summary>
        protected IConfigurationRoot Configuration
        {
            get
            {
                if (!configInitialised) InitConfiguration();
                return configuration;
            }
        }

        #region | Supporting Methods |
        
        private void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = builder.Build();

            configInitialised = true;
        }

        #endregion | Supporting Methods |
    }
}
