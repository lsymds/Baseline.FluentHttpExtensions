using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Moogie.Http
{
    /// <summary>
    /// Request container that is eventually translated into a HttpWebRequest instance. All extension methods hang off
    /// of this class.
    /// </summary>
    public class MoogieHttpRequest
    {
        internal HttpClient HttpClient { get; set; }

        internal Uri Uri { get; set; }
        internal Dictionary<string, string> QueryParameters { get; set; }

        internal string UserAgent { get; set; }
        internal Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="MoogieHttpRequest"/> struct with a base URI.
        /// </summary>
        /// <param name="uri">The base URI to make the request against.</param>
        public MoogieHttpRequest(string uri) => Uri = new Uri(uri);

        /// <summary>
        /// Initialises a new instance of the <see cref="MoogieHttpRequest"/> struct with a base URI and an underlying
        /// <see cref="HttpClient"/> instance to use.
        /// </summary>
        /// <param name="uri">The base URI to make the request against.</param>
        /// <param name="httpClient">The underlying HttpClient to use to make the request.</param>
        public MoogieHttpRequest(string uri, HttpClient httpClient) : this(uri) => HttpClient = httpClient;
    }

    /// <summary>
    /// Contains any and all general extensions that don't belong in a specific class container.
    /// </summary>
    public static class MoogieHttpRequestGeneralExtensions
    {
        /// <summary>
        /// Sets the user agent for a <see cref="MoogieHttpRequest"/>.
        /// </summary>
        /// <param name="httpRequest">The http request to set the UserAgent against.</param>
        /// <param name="userAgent">The user agent to set.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithUserAgent(this MoogieHttpRequest httpRequest,
            string userAgent)
        {
            httpRequest.UserAgent = userAgent;
            return httpRequest;
        }
    }

    /// <summary>
    /// Contains any and all extensions related to request headers.
    /// </summary>
    public static class MoogieHttpRequestHeaderExtensions
    {
        /// <summary>
        /// Sets a header for a <see cref="MoogieHttpRequest"/> instance.
        /// </summary>
        /// <param name="httpRequest">The http request to set a header against.</param>
        /// <param name="headerName">The name of the header to set.</param>
        /// <param name="headerValue">The header value to set.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithHeader(this MoogieHttpRequest httpRequest,
            string headerName,
            string headerValue)
        {
            if (httpRequest.Headers == null)
                httpRequest.Headers = new Dictionary<string, string>();

            httpRequest.Headers[headerName] = headerValue;

            return httpRequest;
        }
    }

    /// <summary>
    /// Contains any and all extensions related to URLs.
    /// </summary>
    public static class MoogieHttpRequestUrlExtensions
    {
        /// <summary>
        /// Adds a query parameter to a <see cref="MoogieHttpRequest"/>. If the query parameter is already present,
        /// it is overwritten.
        /// </summary>
        /// <param name="httpRequest">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithQueryParameter(this MoogieHttpRequest httpRequest,
            string parameterName,
            string value)
        {
            if (httpRequest.QueryParameters == null)
                httpRequest.QueryParameters = new Dictionary<string, string>();

            httpRequest.QueryParameters[parameterName] = value;

            return httpRequest;
        }
    }

    /// <summary>
    /// Contains any and all extensions related to actions performed on remote endpoints.
    /// </summary>
    public static class MoogieHttpRequestActionExtensions
    {
        /// <summary>
        /// Sets the request method to Get.
        /// </summary>
        /// <param name="httpRequest">The http request to set the method against.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest Get(this MoogieHttpRequest httpRequest)
        {
            return httpRequest;
        }
    }

    /// <summary>
    /// Contains any and all extensions that physically send a request to a remote endpoint.
    /// </summary>
    public static class MoogieHttpRequestSendTriggeringExtensions
    {
        private static async Task<HttpResponseMessage> MakeRequest(this MoogieHttpRequest httpRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, httpRequest.Uri);

            if (!string.IsNullOrWhiteSpace(httpRequest.UserAgent))
                request.Headers.Add("User-Agent", httpRequest.UserAgent);

            if (httpRequest.Headers != null)
                foreach (var header in httpRequest.Headers)
                    request.Headers.Add(header.Key, header.Value);

            return await httpRequest.HttpClient.SendAsync(request);
        }

        /// <summary>
        /// Makes the request and ensures that the response is successful. If the response is not successful, an error
        /// will be thrown.
        /// </summary>
        /// <param name="httpRequest">The configured request to make.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task EnsureResponseSuccessful(this MoogieHttpRequest httpRequest)
        {
            var response = await httpRequest.MakeRequest();
            response.EnsureSuccessStatusCode();
        }
    }
}
