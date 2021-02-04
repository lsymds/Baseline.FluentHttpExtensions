// -------------------------------------------------------------------------------------------------- //
// THIS IS A SUPER DUPER AUTOMAGICALLY GENERATED FILE. DO NOT EDIT DIRECTLY PLEASE.                   //
// -------------------------------------------------------------------------------------------------- //
// For more information and documentation, visit the Baseline.FluentHttpExtensions Github repository. //
// https://github.com/lsymonds/Baseline.FluentHttpExtensions                                          //
// -------------------------------------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

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
        internal Func<CancellationToken, Task<HttpContent>> GetBodyContentAsync { get; set; }
        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequest"/> struct with a base URI and an optional,
        /// underlying <see cref="HttpClient"/> instance to use.
        /// </summary>
        /// <param name="uri">The base URI to make the request against.</param>
        /// <param name="httpClient">The underlying HttpClient to use to make the request.</param>
        public HttpRequest(string uri, HttpClient httpClient = default)
        {
            Uri = uri;
            if (
                httpClient == null &&
                BaselineFluentHttpExtensionsHttpClientManager.GetGlobalHttpClient() == null &&
                _newClientInstance == null
            )
            {
                _newClientInstance = new HttpClient();
            }
            HttpClient = httpClient ??
                         BaselineFluentHttpExtensionsHttpClientManager.GetGlobalHttpClient() ??
                         _newClientInstance;
        }
    }
}

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Provides the ability for a consuming application to globally set the HttpClient that will be used for all
    /// subsequent Baseline.FluentHttpExtensions.HttpRequest requests (unless an overriding HttpClient is explicitly
    /// specified to the initiating methods/constructor).
    /// </summary>
    public static class BaselineFluentHttpExtensionsHttpClientManager
    {
        private static HttpClient _client;
        /// <summary>
        /// Sets the global HttpClient to use for any subsequent requests.
        /// </summary>
        /// <param name="client">The HttpClient instance to use.</param>
        public static void SetGlobalHttpClient(HttpClient client)
        {
            _client = client;
        }
        /// <summary>
        /// Gets the global HttpClient to use. If one has not been specified, this method returns null.
        /// </summary>
        internal static HttpClient GetGlobalHttpClient()
        {
            return _client;
        }
    }
}

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
    }
}

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Extension methods related to the building of things, such as URIs. Allows consumers to make business logic
    /// decisions without having to physically submit a request.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Builds the URI from parameters defined in the request such as the base URI, path segments, query string
        /// parameters etc without submitting the request.
        /// </summary>
        /// <param name="request">The configured request to build the URI from.</param>
        /// <returns>A built URI, equivalent to what will be sent via the HttpClient.</returns>
        public static Uri BuildUri(this HttpRequest request)
        {
            var uri = new UriBuilder(request.Uri);
            if (request.QueryParameters != null)
            {
                var queryStringParameters = HttpUtility.ParseQueryString(uri.Query);
                foreach (var (name, value) in request.QueryParameters)
                {
                    queryStringParameters.Add(name, value);
                }
                uri.Query = queryStringParameters.ToString();
            }
            if (request.PathSegments != null)
            {
                uri.Path += string.Join("/", request.PathSegments);
            }
            return uri.Uri;
        }
        /// <summary>
        /// Builds a URI from the parameters defined in the request such as the base URI, path segments, query string
        /// parameters etc without submitting the request.
        /// </summary>
        /// <param name="request">The configured request to build the URI from.</param>
        /// <returns>A built URI, equivalent to what will be sent via the HttpClient, in a string format.</returns>
        public static string BuildUriAsString(this HttpRequest request)
        {
            return request.BuildUri().ToString();
        }
    }
}

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
        {
            return request.WithRequestHeader("Authorization", $"Bearer {token.Replace("Bearer ", "")}");
        }
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
        {
            return request.WithRequestHeader("User-Agent", userAgent);
        }
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
        public static HttpRequest AcceptingJsonResponseContentType(this HttpRequest request, bool replace = false)
        {
            return request.AcceptingResponseContentType("application/json", replace);
        }
        /// <summary>
        /// Sets the content type that the requester can interpret to be application/xml. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingXmlResponseContentType(this HttpRequest request, bool replace = false)
        {
            return request.AcceptingResponseContentType("application/xml", replace);
        }
        /// <summary>
        /// Sets the content type that the requester can interpret to be text/plain. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingPlainResponseContentType(this HttpRequest request, bool replace = false)
        {
            return request.AcceptingResponseContentType("text/plain", replace);
        }
        /// <summary>
        /// Sets the content type that the requester can interpret to be text/html. By default, this method
        /// appends multiple AcceptingResponseContentType calls to one another. If you wish to change this behavior,
        /// set the replace parameter to true.
        /// </summary>
        /// <param name="request">The http request to set the Accept header against.</param>
        /// <param name="replace">Whether to replace the header instead of adding to it.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AcceptingHtmlResponseContentType(this HttpRequest request, bool replace = false)
        {
            return request.AcceptingResponseContentType("text/html", replace);
        }
    }
}

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains extension methods related to the <see cref="HttpResponseMessage"/> class returned from the MakeRequest
    /// endpoint.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Reads the response's content as a stream. The new stream is entirely disconnected from the lifecycle of
        /// <see cref="HttpContent"/>.
        /// </summary>
        /// <param name="response">The response message.</param>
        /// <returns>The response as a stream.</returns>
        public static async Task<Stream> ReadResponseAsStreamAsync(this HttpResponseMessage response)
        {
            var stream = new MemoryStream();
            var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            contentStream.Seek(0, SeekOrigin.Begin);
            await contentStream.CopyToAsync(stream).ConfigureAwait(false);
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
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);
            return await JsonSerializer.DeserializeAsync<T>(
                stream,
                jsonSerializerOptions ?? new JsonSerializerOptions {PropertyNameCaseInsensitive = true},
                token
            ).ConfigureAwait(false);
        }
        /// <summary>
        /// Deserializes the XML response into an object.
        /// </summary>
        /// <param name="response">The response to retrieve the content from and deserialize.</param>
        /// <returns>The deserialized representation of the XML, or null if it could not be casted.</returns>
        public static async Task<T> ReadXmlResponseAsAsync<T>(this HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T) xmlSerializer.Deserialize(stream);
        }
    }
}

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions related to actions performed on remote endpoints.
    /// </summary>
    public static class RequestActionExtensions
    {
        private static HttpRequest SetRequestMethod(this HttpRequest request, HttpMethod method)
        {
            request.HttpMethod = method;
            return request;
        }
        /// <summary>
        /// Sets the request method to GET.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAGetRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Get);
        }
        /// <summary>
        /// Sets the request method to POST.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAPostRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Post);
        }
        /// <summary>
        /// Sets the request method to PUT.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAPutRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Put);
        }
        /// <summary>
        /// Sets the request method to PATCH.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAPatchRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(new HttpMethod("PATCH"));
        }
        /// <summary>
        /// Sets the request method to DELETE.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsADeleteRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Delete);
        }
        /// <summary>
        /// Sets the request method to TRACE.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsATraceRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Trace);
        }
        /// <summary>
        /// Sets the request method to HEAD.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAHeadRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Head);
        }
        /// <summary>
        /// Sets the request method to OPTION.
        /// </summary>
        /// <param name="request">The http request to set the method against.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest AsAnOptionsRequest(this HttpRequest request)
        {
            return request.SetRequestMethod(HttpMethod.Options);
        }
    }
}

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

namespace Baseline.FluentHttpExtensions
{
    /// <summary>
    /// Contains any and all extensions related to URLs.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, short pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, ushort pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, int pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, uint pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, long pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, ulong pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, Guid pathSegment)
        {
            return request.WithPathSegment(pathSegment.ToString());
        }
        /// <summary>
        /// Adds an additional path segment to the url specified when instantiating a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the additional path segment against.</param>
        /// <param name="pathSegment">The actual path segment.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithPathSegment(this HttpRequest request, string pathSegment)
        {
            if (string.IsNullOrWhiteSpace(pathSegment))
            {
                throw new ArgumentNullException(nameof(pathSegment));
            }
            if (request.PathSegments == null)
            {
                request.PathSegments = new List<string>();
            }
            request.PathSegments.Add(pathSegment);
            return request;
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, int value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, uint value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, short value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, ushort value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, long value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, ulong value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, Guid value)
        {
            return request.WithQueryParameter(parameterName, value.ToString());
        }
        /// <summary>
        /// Adds a query parameter to a <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The http request to set the query parameter against.</param>
        /// <param name="parameterName">The query parameter's name.</param>
        /// <param name="value">The query parameter's value.</param>
        /// <returns>The current <see cref="HttpRequest"/>.</returns>
        public static HttpRequest WithQueryParameter(this HttpRequest request, string parameterName, string value)
        {
            if (request.QueryParameters == null)
            {
                request.QueryParameters = new List<(string, string)>();
            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                request.QueryParameters.Add((parameterName, value));
            }
            return request;
        }
    }
}

