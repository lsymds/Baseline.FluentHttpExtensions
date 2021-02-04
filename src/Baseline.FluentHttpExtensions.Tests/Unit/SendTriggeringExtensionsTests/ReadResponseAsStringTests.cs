using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadResponseAsStringTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Response_First()
        {
            ConfigureMessageHandlerResultFailure(MessageHandler);

            Func<Task> func = async () => await HttpRequest.ReadResponseAsStringAsync();
            await func.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_String_Content_Successfully()
        {
            ConfigureMessageHandlerResultSuccess(MessageHandler, "hello world", "text/plain");

            var response = await HttpRequest.ReadResponseAsStringAsync();
            response.Should().Be("hello world");
        }
    }
}
