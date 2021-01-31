using System;
using System.Collections.Generic;
using System.Text;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions related to request headers.
    /// </summary>
    public static class HeaderExtensions
    {
        /// <summary>
        /// Sets a header for a <see cref="HttpRequest"/> instance.
        /// </summary>
        /// <param name="request">The http request to set a header against.</param>
        /// <param name="headerName">The name of the header to set.</param>
        /// <param name="headerValue">The header value to set.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithRequestHeader(this HttpRequest request, string headerName, string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (request.Headers == null)
            {
                request.Headers = new Dictionary<string, string>();
            }

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
        /// Sets the Authorization header to contain basic authentication.
        /// </summary>
        /// <param name="request">The http request to set the Authorization header against.</param>
        /// <param name="username">Username for basic authentication.</param>
        /// <param name="password">Password for basic authentication.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithBasicAuth(this HttpRequest request, string username, string password)
        {
            var encoded = Encoding.UTF8.GetBytes($"{username}:{password}");
            return request.WithRequestHeader("Authorization", $"Basic {Convert.ToBase64String(encoded)}");
        }

        /// <summary>
        /// Sets the user agent for a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the UserAgent against.</param>
        /// <param name="userAgent">The user agent to set.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithUserAgent(this HttpRequest request, string userAgent)
            => request.WithRequestHeader("User-Agent", userAgent);

        /// <summary>
        /// Sets the content type that the requester can interpret. By default, this method appends multiple calls of
        /// <see cref="AcceptingResponseContentType"/> to one another. If you wish to change this behavior, set the
        /// replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="contentType">The content type that the requester can interpret.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingResponseContentType(
            this HttpRequest request,
            string contentType,
            bool replace = false
        )
        {
            var type = request.Headers != null && request.Headers.ContainsKey("Accept") && !replace
                ? $"{request.Headers["Accept"]},{contentType}"
                : contentType;

            return request.WithRequestHeader("Accept", type);
        }

        /// <summary>
        /// Sets the content type that the requester can interpret to be application/json. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingJsonResponseContentType(this HttpRequest request, bool replace = false) =>
            request.AcceptingResponseContentType("application/json", replace);

        /// <summary>
        /// Sets the content type that the requester can interpret to be application/xml. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingXmlResponseContentType(this HttpRequest request, bool replace = false)
            => request.AcceptingResponseContentType("application/xml", replace);

        /// <summary>
        /// Sets the content type that the requester can interpret to be text/plain. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingPlainResponseContentType(this HttpRequest request, bool replace = false)
            => request.AcceptingResponseContentType("text/plain", replace);

        /// <summary>
        /// Sets the content type that the requester can interpret to be text/html. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingHtmlResponseContentType(this HttpRequest request, bool replace = false)
            => request.AcceptingResponseContentType("text/html", replace);
    }
}
