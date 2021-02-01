using System.Threading.Tasks;
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
            OnRequestMade(r => Assert.Equal("Bearer foo", r.Headers.Authorization.ToString()));

            await HttpRequest
                .WithBearerTokenAuth(token)
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
