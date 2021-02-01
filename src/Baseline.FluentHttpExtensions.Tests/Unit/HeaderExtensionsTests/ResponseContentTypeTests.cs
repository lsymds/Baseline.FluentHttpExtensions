using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HeaderExtensionsTests
{
    public class ResponseContentTypeTests : UnitTest
    {
        [Fact]
        public async Task Request_Type_Header_Is_Set_Correctly()
        {
            // Arrange.
            var methodsAndExpecteds = new (Func<HttpRequest, HttpRequest>, string expected)[]
            {
                (x => x.AcceptingJsonResponseContentType(), "application/json"),
                (x => x.AcceptingXmlResponseContentType(), "application/xml"),
                (x => x.AcceptingHtmlResponseContentType(), "text/html"),
                (x => x.AcceptingPlainResponseContentType(), "text/plain")
            };

            foreach (var methodAndExpected in methodsAndExpecteds)
            {
                var (handlerResult, request) = CreateBaseRequest();

                OnRequestMade(r => Assert.Equal(methodAndExpected.expected, r.Headers.Accept.ToString()), handlerResult);

                await methodAndExpected.Item1(request)
                    .EnsureSuccessStatusCodeAsync();
            }
        }

        [Fact]
        public async Task Request_Type_Header_Can_Be_Appended()
        {
            // Arrange.
            var methodsAndExpecteds = new (Func<HttpRequest, HttpRequest>, string expected)[]
            {
                (x => x.AcceptingPlainResponseContentType().AcceptingJsonResponseContentType(),
                    "text/plain, application/json"),
                (x => x.AcceptingPlainResponseContentType().AcceptingXmlResponseContentType(),
                    "text/plain, application/xml"),
                (x => x.AcceptingPlainResponseContentType().AcceptingHtmlResponseContentType(),
                    "text/plain, text/html"),
                (x => x.AcceptingJsonResponseContentType().AcceptingPlainResponseContentType(),
                    "application/json, text/plain")
            };

            foreach (var methodAndExpected in methodsAndExpecteds)
            {
                var (handlerResult, request) = CreateBaseRequest();

                OnRequestMade(r => Assert.Equal(methodAndExpected.expected, r.Headers.Accept.ToString()), handlerResult);

                await methodAndExpected.Item1(request)
                    .EnsureSuccessStatusCodeAsync();
            }
        }

        [Fact]
        public async Task Request_Type_Header_Can_Be_Replaced()
        {
            // Arrange.
            var methodsAndExpecteds = new (Func<HttpRequest, HttpRequest>, string expected)[]
            {
                (x => x.AcceptingPlainResponseContentType().AcceptingJsonResponseContentType(true), "application/json"),
                (x => x.AcceptingPlainResponseContentType().AcceptingXmlResponseContentType(true), "application/xml"),
                (x => x.AcceptingPlainResponseContentType().AcceptingHtmlResponseContentType(true), "text/html"),
                (x => x.AcceptingJsonResponseContentType().AcceptingPlainResponseContentType(true), "text/plain")
            };

            foreach (var methodAndExpected in methodsAndExpecteds)
            {
                var (handlerResult, request) = CreateBaseRequest();

                OnRequestMade(r => Assert.Equal(methodAndExpected.expected, r.Headers.Accept.ToString()), handlerResult);

                await methodAndExpected.Item1(request)
                    .EnsureSuccessStatusCodeAsync();
            }
        }

        private (IReturnsResult<HttpMessageHandler>, HttpRequest) CreateBaseRequest()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var handlerResult = ConfigureMessageHandlerResultSuccess(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            return (handlerResult, request);
        }
    }
}
