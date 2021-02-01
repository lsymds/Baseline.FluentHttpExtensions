using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions that physically send a request to a remote endpoint.
    /// </summary>
    public static class SendTriggeringExtensions
    {
        /// <summary>
        /// Makes and performs a request using the configured parameters.
        /// </summary>
        /// <param name="request">The current <see cref="HttpRequest"/>.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The response returned from the actioned request.</returns>
        public static async Task<HttpResponseMessage> MakeRequest(
            this HttpRequest request,
            CancellationToken token = default
        )
        {
            var actualRequest = new HttpRequestMessage(request.HttpMethod, request.BuildUri());

            // Build headers.
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    actualRequest.Headers.Add(header.Key, header.Value);
                }
            }

            // Set body.
            if (request.GetBodyContent != null)
            {
                actualRequest.Content = await request.GetBodyContent(token);
            }

            return await request.HttpClient.SendAsync(actualRequest, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes the request and ensures that the response is successful. If the response is not successful, an error
        /// will be thrown.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>An awaitable task.</returns>
        public static async Task EnsureSuccessStatusCode(this HttpRequest request, CancellationToken token = default)
        {
            using (var response = await request.MakeRequest(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Makes the request and reads the response as a string. This method first ensures that the status code
        /// returned is a successful one.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>An awaitable task yielding the response as a string.</returns>
        public static async Task<string> ReadResponseAsString(this HttpRequest request,
            CancellationToken token = default)
        {
            using (var response = await request.MakeRequest(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadResponseAsString().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Makes the request and deserializes the JSON response into an object. This method first ensures that the
        /// status code returned is a successful one.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="jsonSerializerOptions">Options used to configure the serializer.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <typeparam name="T">The type to deserialize the JSON into.</typeparam>
        /// <returns>An awaitable task yielding the deserialized object.</returns>
        public static async Task<T> ReadJsonResponseAs<T>(
            this HttpRequest request,
            JsonSerializerOptions jsonSerializerOptions = null,
            CancellationToken token = default
        )
        {
            using (var response = await request.MakeRequest(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadJsonResponseAs<T>(jsonSerializerOptions, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Makes the request and deserializes the XML response into an object. This method first ensures that the
        /// status code returned is a successful one.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token.</param>
        /// <returns>The deserialized representation of the XML, or null if it could not be casted.</returns>
        public static async Task<T> ReadXmlResponseAs<T>(
            this HttpRequest request,
            CancellationToken token = default
        ) where T : class
        {
            using (var response = await request.MakeRequest(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadXmlResponseAs<T>().ConfigureAwait(false);
            }
        }
    }
}
