using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Moogie.Http
{
    /// <summary>
    /// Request container that is eventually translated into a HttpWebRequest instance. All extension methods hang off
    /// of this class.
    /// </summary>
    public class HttpRequest
    {
        internal HttpClient HttpClient { get; }
        internal string Uri { get; }
        internal HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
        internal List<(string Name, string Value)> QueryParameters { get; set; }
        internal Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequest"/> struct with a base URI.
        /// </summary>
        /// <param name="uri">The base URI to make the request against.</param>
        public HttpRequest(string uri) => Uri = uri;

        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequest"/> struct with a base URI and an underlying
        /// <see cref="HttpClient"/> instance to use.
        /// </summary>
        /// <param name="uri">The base URI to make the request against.</param>
        /// <param name="httpClient">The underlying HttpClient to use to make the request.</param>
        public HttpRequest(string uri, HttpClient httpClient) : this(uri) => HttpClient = httpClient;
    }

    /// <summary>
    /// Contains any and all general extensions that don't belong in a specific class container.
    /// </summary>
    public static class MoogieHttpRequestGeneralExtensions
    {
    }

    /// <summary>
    /// Contains any and all extensions related to request headers.
    /// </summary>
    public static class MoogieHttpRequestHeaderExtensions
    {
        /// <summary>
        /// Sets a header for a <see cref="HttpRequest"/> instance.
        /// </summary>
        /// <param name="request">The http request to set a header against.</param>
        /// <param name="headerName">The name of the header to set.</param>
        /// <param name="headerValue">The header value to set.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithRequestHeader(this HttpRequest request,
            string headerName,
            string headerValue)
        {
            if (request.Headers == null)
                request.Headers = new Dictionary<string, string>();

            request.Headers[headerName] = headerValue;

            return request;
        }

        /// <summary>
        /// Sets the Authorization header to contain a bearer token. This method does not care if your token has the
        /// "Bearer " prefix.
        /// </summary>
        /// <param name="request">The http request to set the Authorization header against.</param>
        /// <param name="token">The bearer token.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithBearerTokenAuth(this HttpRequest request, string token)
            => request.WithRequestHeader("Authorization", $"Bearer {token.Replace("Bearer ", "")}");

        /// <summary>
        /// Sets the user agent for a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the UserAgent against.</param>
        /// <param name="userAgent">The user agent to set.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithUserAgent(this HttpRequest request,
            string userAgent) => request.WithRequestHeader("User-Agent", userAgent);

        /// <summary>
        /// Sets the content type that the requester can interpret. By default, this method appends multiple calls of
        /// <see cref="AcceptingResponseContentType"/> to one another. If you wish to change this behavior, set the
        /// <see cref="replace"/> parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="contentType">The content type that the requester can interpret.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingResponseContentType(this HttpRequest request,
            string contentType,
            bool replace = false)
        {
            string type = request.Headers != null && request.Headers.ContainsKey("Accept") && !replace
                ? $"{request.Headers["Accept"]},{contentType}"
                : contentType;

            return request.WithRequestHeader("Accept", type);
        }

        /// <summary>
        /// Sets the content type that the requester can interpret to be application/json. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the <see cref="replace"/> parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingJsonResponseContentType(this HttpRequest request,
            bool replace = false) => request.AcceptingResponseContentType("application/json", replace);

        /// <summary>
        /// Sets the content type that the requester can interpret to be application/xml. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the <see cref="replace"/> parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingXmlResponseContentType(this HttpRequest request,
            bool replace = false) => request.AcceptingResponseContentType("application/xml", replace);

        /// <summary>
        /// Sets the content type that the requester can interpret to be text/plain. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the <see cref="replace"/> parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingPlainResponseContentType(this HttpRequest request,
            bool replace = false) => request.AcceptingResponseContentType("text/plain", replace);

        /// <summary>
        /// Sets the content type that the requester can interpret to be text/html. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the <see cref="replace"/> parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingHtmlResponseContentType(this HttpRequest request,
            bool replace = false) => request.AcceptingResponseContentType("text/html", replace);
    }

    /// <summary>
    /// Contains any and all extensions related to URLs.
    /// </summary>
    public static class MoogieHttpRequestUrlExtensions
    {
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>. If the query parameter is already present,
        /// it is overwritten.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request,
            string parameterName,
            string value)
        {
            if (request.QueryParameters == null)
                request.QueryParameters = new List<(string, string)>();

            request.QueryParameters.Add((parameterName, value));

            return request;
        }
    }

    /// <summary>
    /// Contains any and all extensions related to actions performed on remote endpoints.
    /// </summary>
    public static class MoogieHttpRequestActionExtensions
    {
        private static HttpRequest SetRequestMethod(this HttpRequest request, HttpMethod method)
        {
            request.HttpMethod = method;
            return request;
        }

        /// <summary>
        /// Sets the request method to Get.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAGet(this HttpRequest request)
            => request.SetRequestMethod(HttpMethod.Get);
    }

    /// <summary>
    /// Contains any and all extensions that physically send a request to a remote endpoint.
    /// </summary>
    public static class MoogieHttpRequestSendTriggeringExtensions
    {
        private static async Task<HttpResponseMessage> MakeRequest(this HttpRequest request)
        {
            // Build Uri from Uri and query string parameters.
            var uri = new UriBuilder(request.Uri);
            if (request.QueryParameters != null)
            {
                var queryStringParameters = HttpUtility.ParseQueryString(uri.Query);
                foreach (var queryParams in request.QueryParameters)
                    queryStringParameters.Add(queryParams.Name, queryParams.Value);
                uri.Query = queryStringParameters.ToString();
            }

            var actualRequest = new HttpRequestMessage(HttpMethod.Get, uri.Uri);

            // Build headers.
            if (request.Headers != null)
                foreach (var header in request.Headers)
                    actualRequest.Headers.Add(header.Key, header.Value);

            return await request.HttpClient.SendAsync(actualRequest);
        }

        /// <summary>
        /// Makes the request and ensures that the response is successful. If the response is not successful, an error
        /// will be thrown.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task EnsureSuccessStatusCode(this HttpRequest request)
        {
            var response = await request.MakeRequest();
            response.EnsureSuccessStatusCode();
        }
    }
}
