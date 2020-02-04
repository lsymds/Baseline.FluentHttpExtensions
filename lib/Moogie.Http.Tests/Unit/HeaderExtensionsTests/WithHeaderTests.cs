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

            await HttpRequest
                .WithRequestHeader("Authorization", "Bearer foo-bar")
                .AsAGet()
                .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task With_Header_Overwrites_An_Existing_Header()
        {
            OnRequestMade(r => Assert.Equal("bar", r.Headers.Authorization.Parameter));

            await HttpRequest
                .WithRequestHeader("Authorization", "Bearer foo")
                .WithRequestHeader("Authorization", "Bearer bar")
                .AsAGet()
                .EnsureSuccessStatusCode();
        }
    }
}
