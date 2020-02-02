using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Moogie.Http
{
    /// <summary>
    /// Request container that is eventually translated into a HttpWebRequest instance. All extension methods hang off
    /// of this class.
    /// </summary>
    public class MoogieHttpRequest
    {
        /// <summary>
        /// Gets the underlying HttpClient used to make the requests.
        /// </summary>
        public HttpClient HttpClient { get; internal set; }

        /// <summary>
        /// Gets the Uri to make the request against.
        /// </summary>
        public Uri Uri { get; internal set; }

        /// <summary>
        /// Gets the UserAgent that will be sent with the request.
        /// </summary>
        public string UserAgent { get; internal set; }

        /// <summary>
        /// Gets the headers that will be sent with the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; internal set; }

        /// <summary>
        /// Gets the query parameters which will be added to the <see cref="Uri"/> property to form a complete URL.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; internal set; }

        /// <summary>
        /// Gets whether or not the request destination should be allowed to automatically redirect.
        /// </summary>
        public bool AllowAutoRedirect { get; internal set; }

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

        /// <summary>
        /// Sets whether the request should allow automatic redirects or not. This is defaulted to false upon
        /// initialization of a <see cref="MoogieHttpRequest"/>, but can be overriden with this method.
        /// </summary>
        /// <param name="httpRequest">The http request to set the AllowAutoRedirect property against.</param>
        /// <param name="allowAutoRedirect">Whether or not to allow automatic redirects.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest ShouldAllowAutoRedirect(this MoogieHttpRequest httpRequest,
            bool allowAutoRedirect = true)
        {
            httpRequest.AllowAutoRedirect = allowAutoRedirect;
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

        /// <summary>
        /// Sets the content type for a <see cref="MoogieHttpRequest"/> instance.
        /// </summary>
        /// <param name="httpRequest">The http request to set the content type against.</param>
        /// <param name="contentType">The content type to set against the request.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithContentType(this MoogieHttpRequest httpRequest,
            string contentType) => httpRequest.WithHeader("Content-Type", contentType);

        /// <summary>
        /// Sets the content type for a <see cref="MoogieHttpRequest"/> to be text/plain.
        /// </summary>
        /// <param name="httpRequest">The http request to set the content type against.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithPlainContentType(this MoogieHttpRequest httpRequest)
            => httpRequest.WithContentType("text/plain");

        /// <summary>
        /// Sets the content type for a <see cref="MoogieHttpRequest"/> to be application/json.
        /// </summary>
        /// <param name="httpRequest">The http request to set the content type against.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithJsonContentType(this MoogieHttpRequest httpRequest)
            => httpRequest.WithContentType("application/json");

        /// <summary>
        /// Sets the content type for a <see cref="MoogieHttpRequest"/> to be application/xml.
        /// </summary>
        /// <param name="httpRequest">The http request to set the content type against.</param>
        /// <returns>The current <see cref="MoogieHttpRequest"/>.</returns>
        public static MoogieHttpRequest WithXmlContentType(this MoogieHttpRequest httpRequest)
            => httpRequest.WithContentType("application/xml");
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

    }

    /// <summary>
    /// Contains any and all extensions that physically send a request to a remote endpoint.
    /// </summary>
    public static class MoogieHttpRequestSendTriggeringExtensions
    {
    }
}
