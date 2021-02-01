using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HeaderExtensionsTests
{
    public class WithBearerTokenAuthTests : UnitTest
    {
        [Theory]
        [InlineData("foo")]
        [InlineData("Bearer foo")]
        public async Task Successfully_Adds_Authentication_And_Bearer_Token_Header(string token)
        {
            OnRequestMade(r => r.Headers.Authorization.ToString().Should().Be("Bearer foo"));

            await HttpRequest
                .WithBearerTokenAuth(token)
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
