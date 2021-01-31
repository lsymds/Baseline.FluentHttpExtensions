using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace Baseline.FluentHttpExtensions.Tests.Unit
{
    public abstract class UnitTest
    {
        private Mock<HttpMessageHandler> _messageHandler = new Mock<HttpMessageHandler>();
        protected const string RequestUrl = "https://www.google.com";
        protected HttpClient HttpClient;
        protected HttpRequest HttpRequest { get; }
        protected IReturnsResult<HttpMessageHandler> MessageHandlerResult { get; private set; }

        protected UnitTest()
        {
            HttpClient = new HttpClient(_messageHandler.Object);
            MessageHandlerResult = ConfigureMessageHandlerResultSuccess(_messageHandler);
            HttpRequest = new HttpRequest(RequestUrl, HttpClient);
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
