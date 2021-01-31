using System.Net.Http;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Provides the ability for a consuming application to globally set the HttpClient that will be used for all
    /// subsequent Baseline.FluentHttpExtensions.HttpRequest requests (unless an overriding HttpClient is explicitly
    /// specified to the initiating methods/constructor).
    /// </summary>
    public static class BaselineFluentHttpExtensionsHttpClientManager
    {
        private static HttpClient _client;

        /// <summary>
        /// Sets the global HttpClient to use for any subsequent requests.
        /// </summary>
        /// <param name="client">The HttpClient instance to use.</param>
        public static void SetGlobalHttpClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets the global HttpClient to use. If one has not been specified, this method returns null.
        /// </summary>
        internal static HttpClient GetGlobalHttpClient()
        {
            return _client;
        }
    }
}
