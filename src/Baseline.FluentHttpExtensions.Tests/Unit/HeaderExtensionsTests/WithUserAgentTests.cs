using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.HeaderExtensionsTests
{
    public class WithUserAgentTests : UnitTest
    {
        [Fact]
        public async Task User_Agent_Is_Set_In_HttpRequest_Object()
        {
            OnRequestMade(r => r.Headers.UserAgent.ToString().Should().Be("baseline"));

            await HttpRequest
                .WithUserAgent("baseline")
                .AsAGetRequest()
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
