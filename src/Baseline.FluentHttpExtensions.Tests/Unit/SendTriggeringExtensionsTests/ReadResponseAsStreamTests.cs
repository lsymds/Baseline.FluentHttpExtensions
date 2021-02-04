using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadResponseAsStreamTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Response_First()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            Func<Task> act = async () => await request.ReadResponseAsStreamAsync();

            await act.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_Stream_Content_Successfully()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultSuccess(mockMessageHandler, "hello world", "text/plain");
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            var response = await request.ReadResponseAsStreamAsync();
            var streamReader = new StreamReader(response);

            (await streamReader.ReadToEndAsync()).Should().Be("hello world");
        }
    }
}
