using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.HeaderExtensionsTests
{
    public class WithUserAgentTests : UnitTest
    {
        [Fact]
        public async Task User_Agent_Is_Set_In_MoogieHttpRequest_Object()
        {
            OnRequestMade(r => Assert.Equal("moogie-http", r.Headers.UserAgent.ToString()));

            await HttpRequest
                .WithUserAgent("moogie-http")
                .AsAGetRequest()
                .EnsureSuccessStatusCode();
        }
    }
}
