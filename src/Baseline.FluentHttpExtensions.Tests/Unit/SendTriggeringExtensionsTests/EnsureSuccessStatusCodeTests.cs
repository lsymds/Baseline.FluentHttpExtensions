using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class EnsureSuccessStatusCodeTests : UnitTest
    {
        [Fact]
        public async Task Failed_Status_Code_Returns_An_Error()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            Func<Task> func = async () => await request.EnsureSuccessStatusCodeAsync();

            await func.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Good_Status_Code_Doesnt_Return_An_Error()
        {
            await HttpRequest.EnsureSuccessStatusCodeAsync();
        }
    }
}
