using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions that physically send a request to a remote endpoint.
    /// </summary>
    public static class SendTriggeringExtensions
    {
        /// <summary>
        /// Makes and performs a request using the configured parameters. You are responsible for managing the
        /// disposal lifetimes of the response when using this method.
        /// </summary>
        /// <param name="request">The current <see cref="HttpRequest"/>.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The response returned from the actioned request.</returns>
        public static async Task<HttpResponseMessage> MakeRequestAsync(
            this HttpRequest request,
            CancellationToken token = default
        )
        {
            using (var actualRequest = new HttpRequestMessage(request.HttpMethod, request.BuildUri()))
            {
                // Build headers.
                if (request.Headers != null)
                {
                    foreach (var header in request.Headers)
                    {
                        actualRequest.Headers.Add(header.Key, header.Value);
                    }
                }

                // Set body.
                if (request.GetBodyContentAsync != null)
                {
                    actualRequest.Content = await request.GetBodyContentAsync(token);
                }

                return await request.HttpClient.SendAsync(actualRequest, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Makes the request and ensures that the response is successful. If the response is not successful, an error
        /// will be thrown.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>An awaitable task.</returns>
        public static async Task EnsureSuccessStatusCodeAsync(this HttpRequest request, CancellationToken token = default)
        {
            using (var response = await request.MakeRequestAsync(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Makes a request and reads the response as a stream. This method first ensures that the status code returned
        /// is a successful one.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token.</param>
        /// <returns>The response as a stream.</returns>
        public static async Task<Stream> ReadResponseAsStreamAsync(
            this HttpRequest request,
            CancellationToken token = default
        )
        {
            using (var response = await request.MakeRequestAsync(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadResponseAsStreamAsync();
            }
        }

        /// <summary>
        /// Makes the request and reads the response as a string. This method first ensures that the status code
        /// returned is a successful one.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>An awaitable task yielding the response as a string.</returns>
        public static async Task<string> ReadResponseAsStringAsync(
            this HttpRequest request,
            CancellationToken token = default)
        {
            using (var response = await request.MakeRequestAsync(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadResponseAsStringAsync().ConfigureAwait(false);
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
        public static async Task<T> ReadJsonResponseAsAsync<T>(
            this HttpRequest request,
            JsonSerializerOptions jsonSerializerOptions = null,
            CancellationToken token = default
        )
        {
            using (var response = await request.MakeRequestAsync(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadJsonResponseAsAsync<T>(jsonSerializerOptions, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Makes the request and deserializes the XML response into an object. This method first ensures that the
        /// status code returned is a successful one.
        /// </summary>
        /// <param name="request">The configured request to make.</param>
        /// <param name="token">The optional cancellation token.</param>
        /// <returns>The deserialized representation of the XML, or null if it could not be casted.</returns>
        public static async Task<T> ReadXmlResponseAsAsync<T>(
            this HttpRequest request,
            CancellationToken token = default
        ) where T : class
        {
            using (var response = await request.MakeRequestAsync(token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.ReadXmlResponseAsAsync<T>().ConfigureAwait(false);
            }
        }
    }
}
