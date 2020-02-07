using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace Moogie.Http.Tests.Unit
{
    public abstract class UnitTest
    {
        protected const string RequestUrl = "https://www.google.com";
        protected HttpRequest HttpRequest { get; }
        protected IReturnsResult<HttpMessageHandler> MessageHandlerResult { get; private set; }

        protected UnitTest()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            MessageHandlerResult = ConfigureMessageHandlerResultSuccess(mockMessageHandler);
            HttpRequest = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));
        }

        protected IReturnsResult<HttpMessageHandler> ConfigureMessageHandlerResultSuccess(
            Mock<HttpMessageHandler> messageHandler,
            string body = "",
            string contentType = "application/json")
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(body, Encoding.UTF8, contentType)
            };

            return messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(response));
        }

        protected IReturnsResult<HttpMessageHandler> ConfigureMessageHandlerResultFailure(
            Mock<HttpMessageHandler> messageHandler)
            => messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden)));

        protected void OnRequestMade(Action<HttpRequestMessage> handler)
            => OnRequestMade(handler, MessageHandlerResult);

        protected void OnRequestMade(Action<HttpRequestMessage> handler, IReturnsResult<HttpMessageHandler> result)
            => result.Callback<HttpRequestMessage, CancellationToken>((r, c) => handler(r));
    }
}
