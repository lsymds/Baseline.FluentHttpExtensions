using System.Net.Http;

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
