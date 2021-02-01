using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadResponseAsStringTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Response_First()
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            async Task Act() => await request.ReadResponseAsStringAsync();

            // Assert.
            await Assert.ThrowsAsync<HttpRequestException>(Act);
        }

        [Fact]
        public async Task Returns_String_Content_Successfully()
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultSuccess(mockMessageHandler, "hello world", "text/plain");
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            var response = await request.ReadResponseAsStringAsync();

            // Assert.
            Assert.Equal("hello world", response);
        }
    }
}
