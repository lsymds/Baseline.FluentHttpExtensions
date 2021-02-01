using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadResponseAsStringTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Response_First()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            Func<Task> func = async () => await request.ReadResponseAsStringAsync();

            await func.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_String_Content_Successfully()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultSuccess(mockMessageHandler, "hello world", "text/plain");
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            var response = await request.ReadResponseAsStringAsync();

            response.Should().Be("hello world");
        }
    }
}
