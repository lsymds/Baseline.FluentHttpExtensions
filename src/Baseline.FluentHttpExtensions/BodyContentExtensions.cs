using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions that set a request's body content.
    /// </summary>
    public static class BodyContentExtensions
    {
        /// <summary>
        /// Sets the request's body to be that of an object with a request body content type of application/json.
        /// This method, like all request body method, evaluates when the request is actually performed.
        /// </summary>
        /// <param name="request">The http request to set the body against.</param>
        /// <param name="body">The object to serialize and send as JSON.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithJsonBody<T>(this HttpRequest request, T body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            request.GetBodyContentAsync = async token =>
            {
                // No, I don't need to have a using statement. StreamContent will automatically dispose of it when
                // .Dispose() is called on it.
                var stream = new MemoryStream();

                await JsonSerializer.SerializeAsync(stream, body, cancellationToken: token).ConfigureAwait(false);
                stream.Seek(0, SeekOrigin.Begin);

                var content = new StreamContent(stream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return content;
            };

            return request;
        }

        /// <summary>
        /// Sets the request's body to be that of a string. This method, like all request body method, evaluates
        /// when the request is actually performed.
        /// </summary>
        /// <param name="request">The http request to set the body against.</param>
        /// <param name="body">The string content to set as the request's body.</param>
        /// <param name="contentType">The content type to be set.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithTextBody(this HttpRequest request, string body, string contentType = "text/plain")
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            request.GetBodyContentAsync = _ =>
                Task.FromResult((HttpContent) new StringContent(body, Encoding.UTF8, contentType));

            return request;
        }

        /// <summary>
        /// Sets the request's body to be that of a form URL encoded (i.e. x-www-form-url-encoded) collection of
        /// key value pairs.
        /// </summary>
        /// <param name="request">The http request to set the body against.</param>
        /// <param name="body">The collection of key value pairs to url encode and set in the body.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithFormUrlEncodedBody(
            this HttpRequest request,
            params KeyValuePair<string, string>[] body
        )
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            request.GetBodyContentAsync = _ => Task.FromResult((HttpContent) new FormUrlEncodedContent(body));

            return request;
        }
    }
}
