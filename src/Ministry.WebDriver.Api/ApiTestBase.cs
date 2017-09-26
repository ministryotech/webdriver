using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Ministry.WebDriver.Api
{
    /// <summary>
    /// Base class to be inherrited from by all API tests.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public abstract class ApiTestBase
    {
        #region | Construction |

        /// <summary>
        /// Creates a test for a specific API.
        /// </summary>
        protected ApiTestBase()
        {
            ApiClient = new HttpClient();
        }

        #endregion

        /// <summary>
        /// Gets the Http Client.
        /// </summary>
        /// <example>
        /// ApiClient.DefaultRequestHeaders.Accept.Clear();
        /// ApiClient.DefaultRequestHeaders.Accept.Add(
        /// new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        /// ApiClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        /// 
        /// var stringTask = ApiClient.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
        ///
        /// var msg = await stringTask;
        /// Console.Write(msg);
        /// </example>
        /// <remarks>
        /// Simply creates an HttpClient https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
        /// </remarks>
        protected HttpClient ApiClient { get; }

    }
}
