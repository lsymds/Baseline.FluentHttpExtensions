using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HttpResponseMessageExtensionsTests
{
    public class ReadJsonResponseAsTests : UnitTest
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class ResolvedObject
        {
            public string Name { get; set; }
        }

        [Fact]
        public async Task It_Does_Not_Dispose_Of_The_Original_Content_Stream()
        {
            ConfigureMessageHandlerResultSuccess(MessageHandler, @"{ ""Name"": ""bob smith"" }");

            var completedRequest = await HttpRequest.MakeRequestAsync();

            var response = await completedRequest.ReadJsonResponseAsAsync<ResolvedObject>();
            response.Should().NotBeNull();
            response.Name.Should().Be("bob smith");

            response = await completedRequest.ReadJsonResponseAsAsync<ResolvedObject>();
            response.Should().NotBeNull();
            response.Name.Should().Be("bob smith");
        }
    }
}
