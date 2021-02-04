using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class ReadXmlResponseAsTests : UnitTest
    {
        [XmlRoot("foo")]
        public class TestXmlObject
        {
            [XmlElement("bar")]
            public string Bar { get; set; }
        }

        [Fact]
        public async Task Checks_Success_Status_First()
        {
            ConfigureMessageHandlerResultFailure(MessageHandler);

            Func<Task> func = async () => await HttpRequest.ReadXmlResponseAsAsync<object>();
            await func.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_Xml_Successfully()
        {
            ConfigureMessageHandlerResultSuccess(MessageHandler, "<foo><bar>abc</bar></foo>", "application/xml");

            var response = await HttpRequest.ReadXmlResponseAsAsync<TestXmlObject>();
            response.Should().NotBeNull();
            response.Bar.Should().Be("abc");
        }
    }
}
