using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensions
{
    public class ReadXmlResponseAsTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Status_First()
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            async Task Act() => await request.ReadXmlResponseAs<object>();

            // Assert.
            await Assert.ThrowsAsync<HttpRequestException>(Act);
        }

        [Fact]
        public async Task Returns_Xml_Successfully()
        {
            // Arrange.
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultSuccess(mockMessageHandler, "<foo><bar>abc</bar></foo>", "application/xml");
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            // Act.
            var response = await request.ReadXmlResponseAs<TestXmlObject>();

            // Assert.
            Assert.NotNull(response);
            Assert.Equal("abc", response.Bar);
        }
    }

    [XmlRoot("foo")]
    public class TestXmlObject
    {
        [XmlElement("bar")]
        public string Bar { get; set; }
    }
}
