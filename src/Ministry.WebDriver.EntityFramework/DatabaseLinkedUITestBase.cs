using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Ministry.WebDriver.Configuration;
using Ministry.WebDriver.Extensions;

namespace Ministry.WebDriver.EntityFramework
{
    /// <summary>
    /// Base class to be inherited from by tests requiring access to a database layer.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class DatabaseLinkedUITestBase<TDbContext, TTestManager> : ConfiguredUITestBase<TTestManager>
        where TDbContext : DbContext
        where TTestManager : TestManagerBase
    {
        private bool databaseInitialised;
        
        private TDbContext database;

        /// <summary>
        /// Gets the database.
        /// </summary>
        protected TDbContext Database
        {
            get
            {
                if (!databaseInitialised) InitDatabase();
                return database;
            }
        }

        /// <summary>
        /// Gets the database instance to use.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <returns>A DbContext to enable the test to interract with entities.</returns>
        protected abstract TDbContext GetDbContext(DbContextOptionsBuilder<TDbContext> optionsBuilder);

        #region | Supporting Methods |

        private void InitDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            database = GetDbContext(optionsBuilder);

            databaseInitialised = true;
        }

        #endregion | Supporting Methods |
    }
}
