using System;
using System.Web;

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
