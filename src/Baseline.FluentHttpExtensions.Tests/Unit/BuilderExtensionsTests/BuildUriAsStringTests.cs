using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.BuilderExtensionsTests
{
    public class BuildUriAsStringTests
    {
        [Fact]
        public void It_Successfully_Builds_And_Returns_A_Uri()
        {
            var response = "https://www.google.com/"
                .AsAGetRequest()
                .WithPathSegment("search")
                .WithQueryParameter("q", "how much does a pug snore")
                .BuildUriAsString();

            Assert.Equal("https://www.google.com/search?q=how+much+does+a+pug+snore", response);
        }
    }
}
