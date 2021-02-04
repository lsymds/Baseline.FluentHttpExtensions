using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HttpResponseMessageExtensionsTests
{
    public class ReadResponseAsStreamTests : UnitTest
    {
        [Fact]
        public async Task It_Does_Not_Dispose_Of_The_Original_Content_Stream()
        {
            ConfigureMessageHandlerResultSuccess(MessageHandler, "hello world", "text/plain");

            var request = await HttpRequest.MakeRequestAsync();

            var response = await request.ReadResponseAsStreamAsync();
            var streamReader = new StreamReader(response);
            (await streamReader.ReadToEndAsync()).Should().Be("hello world");

            response = await request.ReadResponseAsStreamAsync();
            streamReader = new StreamReader(response);
            (await streamReader.ReadToEndAsync()).Should().Be("hello world");
        }
    }
}
