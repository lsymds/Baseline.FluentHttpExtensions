using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Request container that is eventually translated into a HttpWebRequest instance. All extension methods hang off
    /// of this class.
    /// </summary>
    public class HttpRequest
    {
        private static HttpClient _newClientInstance;

        internal HttpClient HttpClient { get; }
        internal string Uri { get; }
        internal HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
        internal Dictionary<string, string> Headers { get; set; }
        internal List<string> PathSegments { get; set; }
        internal List<(string Name, string Value)> QueryParameters { get; set; }
        internal Func<CancellationToken, Task<HttpContent>> GetBodyContent { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequest"/> struct with a base URI and an optional,
        /// underlying <see cref="HttpClient"/> instance to use.
        /// </summary>
        /// <param name="uri">The base URI to make the request against.</param>
        /// <param name="httpClient">The underlying HttpClient to use to make the request.</param>
        public HttpRequest(string uri, HttpClient httpClient = default)
        {
            Uri = uri;

            if (httpClient == null && _newClientInstance == null)
            {
                _newClientInstance = new HttpClient();
            }

            HttpClient = httpClient ?? _newClientInstance;
        }
    }
}
