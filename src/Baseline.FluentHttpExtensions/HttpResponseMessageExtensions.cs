using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains extension methods related to the <see cref="HttpResponseMessage"/> class returned from the MakeRequest
    /// endpoint.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Reads the response's content as a stream.
        /// </summary>
        /// <param name="response">The response message.</param>
        /// <returns>The response as a stream.</returns>
        public static async Task<Stream> ReadResponseAsStreamAsync(this HttpResponseMessage response)
        {
            var stream = new MemoryStream();

            using (var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                await contentStream.CopyToAsync(stream).ConfigureAwait(false);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// Reads the response's content as a string.
        /// </summary>
        /// <param name="response">The response message.</param>
        /// <returns>An awaitable task yielding the response as a string.</returns>
        public static async Task<string> ReadResponseAsStringAsync(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Deserializes the JSON response into an object.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="jsonSerializerOptions">Options used to configure the serializer.</param>
        /// <param name="token">The optional cancellation token</param>
        /// <typeparam name="T">The type to deserialize the JSON into.</typeparam>
        /// <returns>An awaitable task yielding the deserialized object.</returns>
        public static async Task<T> ReadJsonResponseAsAsync<T>(
            this HttpResponseMessage response,
            JsonSerializerOptions jsonSerializerOptions = null,
            CancellationToken token = default
        )
        {
            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                stream.Seek(0, SeekOrigin.Begin);

                return await JsonSerializer.DeserializeAsync<T>(
                    stream,
                    jsonSerializerOptions ?? new JsonSerializerOptions {PropertyNameCaseInsensitive = true},
                    token
                ).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Deserializes the XML response into an object.
        /// </summary>
        /// <param name="response">The response to retrieve the content from and deserialize.</param>
        /// <returns>The deserialized representation of the XML, or null if it could not be casted.</returns>
        public static async Task<T> ReadXmlResponseAsAsync<T>(this HttpResponseMessage response)
        {
            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                stream.Seek(0, SeekOrigin.Begin);

                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T) xmlSerializer.Deserialize(stream);
            }
        }
    }
}
