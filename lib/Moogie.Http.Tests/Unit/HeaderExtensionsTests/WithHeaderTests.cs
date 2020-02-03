using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.HeaderExtensionsTests
{
    public class WithHeaderTests : UnitTest
    {
        [Fact]
        public async Task With_Header_Successfully_Adds_A_New_Header()
        {
            OnRequestMade(r => Assert.Equal("foo-bar", r.Headers.Authorization.Parameter));

            await MoogieHttpRequest
                .WithHeader("Authorization", "Bearer foo-bar")
                .Get()
                .EnsureResponseSuccessful();
        }

        [Fact]
        public async Task With_Header_Overwrites_An_Existing_Header()
        {
            OnRequestMade(r => Assert.Equal("bar", r.Headers.Authorization.Parameter));

            await MoogieHttpRequest
                .WithHeader("Authorization", "Bearer foo")
                .WithHeader("Authorization", "Bearer bar")
                .Get()
                .EnsureResponseSuccessful();
        }
    }
}
