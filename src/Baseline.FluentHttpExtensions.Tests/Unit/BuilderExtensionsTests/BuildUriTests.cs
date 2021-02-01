using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.BuilderExtensionsTests
{
    public class BuildUriTests
    {
        [Fact]
        public void It_Successfully_Builds_And_Returns_A_Uri()
        {
            var response = "https://www.google.com/"
                .AsAGetRequest()
                .WithPathSegment("search")
                .WithQueryParameter("q", "how much does a pug snore")
                .BuildUri();

            response.ToString().Should().Be("https://www.google.com/search?q=how+much+does+a+pug+snore");
        }
    }
}
