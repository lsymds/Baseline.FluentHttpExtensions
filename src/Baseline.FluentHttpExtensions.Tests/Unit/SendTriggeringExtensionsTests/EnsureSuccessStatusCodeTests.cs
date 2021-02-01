using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class EnsureSuccessStatusCodeTests : UnitTest
    {
        [Fact]
        public async Task Failed_Status_Code_Returns_An_Error()
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            async Task Act() => await request.EnsureSuccessStatusCode();

            // Assert.
            await Assert.ThrowsAsync<HttpRequestException>(Act);
        }

        [Fact]
        public async Task Good_Status_Code_Doesnt_Return_An_Error()
        {
            // Arrange, Act & Assert.
            await HttpRequest.EnsureSuccessStatusCode();
        }
    }
}
