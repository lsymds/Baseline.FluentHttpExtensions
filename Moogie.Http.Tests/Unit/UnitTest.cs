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
        protected MoogieHttpRequest MoogieHttpRequest { get; }
        protected IReturnsResult<HttpMessageHandler> MessageHandlerResult { get; private set; }

        protected UnitTest()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            MessageHandlerResult = mockMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            MoogieHttpRequest = new MoogieHttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));
        }

        protected void OnRequestMade(Action<HttpRequestMessage> handler)
            => MessageHandlerResult.Callback<HttpRequestMessage, CancellationToken>((r, c) => handler(r));
    }
}
