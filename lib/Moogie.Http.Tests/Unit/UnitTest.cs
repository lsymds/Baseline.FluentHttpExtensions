using System;
using System.Net;
using System.Net.Http;
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
            MessageHandlerResult = ConfigureMessageHandlerResult(mockMessageHandler);
            HttpRequest = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));
        }

        protected IReturnsResult<HttpMessageHandler> ConfigureMessageHandlerResult(
            Mock<HttpMessageHandler> messageHandler)
            => messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

        protected void OnRequestMade(Action<HttpRequestMessage> handler)
            => OnRequestMade(handler, MessageHandlerResult);

        protected void OnRequestMade(Action<HttpRequestMessage> handler, IReturnsResult<HttpMessageHandler> result)
            => result.Callback<HttpRequestMessage, CancellationToken>((r, c) => handler(r));
    }
}
