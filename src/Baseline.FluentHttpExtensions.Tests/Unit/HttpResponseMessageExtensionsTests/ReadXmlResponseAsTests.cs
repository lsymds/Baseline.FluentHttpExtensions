using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HttpResponseMessageExtensionsTests
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
        public async Task It_Does_Not_Dispose_Of_The_Original_Content_Stream()
        {
            ConfigureMessageHandlerResultSuccess(MessageHandler, "<foo><bar>abc</bar></foo>", "application/xml");

            var request = await HttpRequest.MakeRequestAsync();

            var response = await request.ReadXmlResponseAsAsync<TestXmlObject>();
            response.Should().NotBeNull();
            response.Bar.Should().Be("abc");

            response = await request.ReadXmlResponseAsAsync<TestXmlObject>();
            response.Should().NotBeNull();
            response.Bar.Should().Be("abc");
        }
    }
}
