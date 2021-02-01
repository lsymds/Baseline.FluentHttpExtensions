using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadXmlResponseAsTests : UnitTest
    {
        [Fact]
        public async Task Checks_Success_Status_First()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultFailure(mockMessageHandler);
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            Func<Task> func = async () => await request.ReadXmlResponseAsAsync<object>();

            await func.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_Xml_Successfully()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            ConfigureMessageHandlerResultSuccess(mockMessageHandler, "<foo><bar>abc</bar></foo>", "application/xml");
            var request = new HttpRequest(RequestUrl, new HttpClient(mockMessageHandler.Object));

            var response = await request.ReadXmlResponseAsAsync<TestXmlObject>();

            response.Should().NotBeNull();
            response.Bar.Should().Be("abc");
        }
    }

    [XmlRoot("foo")]
    public class TestXmlObject
    {
        [XmlElement("bar")]
        public string Bar { get; set; }
    }
}
