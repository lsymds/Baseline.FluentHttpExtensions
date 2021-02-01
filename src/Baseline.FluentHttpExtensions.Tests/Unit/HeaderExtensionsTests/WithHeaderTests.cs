using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HeaderExtensionsTests
{
    public class WithHeaderTests : UnitTest
    {
        [Fact]
        public async Task With_Header_Successfully_Adds_A_New_Header()
        {
            OnRequestMade(r => r.Headers.Authorization.Parameter.Should().Be("foo-bar"));

            await HttpRequest
                .WithRequestHeader("Authorization", "Bearer foo-bar")
                .AsAGetRequest()
                .EnsureSuccessStatusCodeAsync();
        }

        [Fact]
        public async Task With_Header_Overwrites_An_Existing_Header()
        {
            OnRequestMade(r => r.Headers.Authorization.Parameter.Should().Be("bar"));

            await HttpRequest
                .WithRequestHeader("Authorization", "Bearer foo")
                .WithRequestHeader("Authorization", "Bearer bar")
                .AsAGetRequest()
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
