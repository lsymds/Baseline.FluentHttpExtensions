using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadResponseAsStreamTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Response_First()
        {
            ConfigureMessageHandlerResultFailure(MessageHandler);

            Func<Task> act = async () => await HttpRequest.ReadResponseAsStreamAsync();
            await act.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_Stream_Content_Successfully()
        {
            ConfigureMessageHandlerResultSuccess(MessageHandler, "hello world", "text/plain");

            var response = await HttpRequest.ReadResponseAsStreamAsync();
            var streamReader = new StreamReader(response);

            (await streamReader.ReadToEndAsync()).Should().Be("hello world");
        }
    }
}
