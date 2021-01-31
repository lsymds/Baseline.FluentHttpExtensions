using System.Net.Http;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions that take a string (hopefully a url) and convert it into the fluent
    /// <see cref="HttpRequest"/> builder.
    /// </summary>
    public static class StringToHttpRequestExtensions
    {
        /// <summary>
        /// Converts a string URL into a GET HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsAGetRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsAGetRequest();
        }

        /// <summary>
        /// Converts a string URL into a POST HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsAPostRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsAPostRequest();
        }

        /// <summary>
        /// Converts a string URL into a PUT HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsAPutRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsAPutRequest();
        }

        /// <summary>
        /// Converts a string URL into a PATCH HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsAPatchRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsAPatchRequest();
        }

        /// <summary>
        /// Converts a string URL into a DELETE HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsADeleteRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsADeleteRequest();
        }

        /// <summary>
        /// Converts a string URL into a TRACE HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsATraceRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsATraceRequest();
        }

        /// <summary>
        /// Converts a string URL into a HEAD HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsAHeadRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsAHeadRequest();
        }

        /// <summary>
        /// Converts a string URL into a OPTIONS HTTP request.
        /// </summary>
        /// <param name="url">The url defined in a string to create a request from.</param>
        /// <param name="client">Optional HttpClient to use.</param>
        /// <returns>A <see cref="HttpRequest"/> instance.</returns>
        public static HttpRequest AsAnOptionsRequest(this string url, HttpClient client = default)
        {
            return new HttpRequest(url, client).AsAnOptionsRequest();
        }
    }
}
