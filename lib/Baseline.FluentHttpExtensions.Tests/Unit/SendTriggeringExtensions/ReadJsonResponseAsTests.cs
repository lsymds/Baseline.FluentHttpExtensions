using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;
// ReSharper disable NotNullMemberIsNotInitialized
// ReSharper disable UnusedAutoPropertyAccessor.Local
#pragma warning disable 8618

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensions
{
    public class ReadJsonResponseAsTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Status_First()
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            async Task Act() => await request.ReadJsonResponseAs<object>();

            // Assert.
            await Assert.ThrowsAsync<HttpRequestException>(Act);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ResolvedObject
        {
            public string Name { get; set; }
        }

        [Theory]
        [InlineData(@"{ ""Name"": ""bob smith"" }")]
        [InlineData(@"{ ""name"": ""bob smith"" }")]
        public async Task Successfully_Resolves_Into_An_Object(string json)
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultSuccess(mockMessageHandler, json);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            var response = await request
                .ReadJsonResponseAs<ResolvedObject>();

            // Assert.
            Assert.NotNull(response);
            Assert.Equal("bob smith", response.Name);
        }
    }
}
