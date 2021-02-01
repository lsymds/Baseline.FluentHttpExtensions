using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensionsTests
{
    public class MakeRequestTests : UnitTest
    {
        [Fact]
        public async Task Can_Get_The_Request_Response()
        {
            var response = await HttpRequest
                .AsAGetRequest()
                .MakeRequestAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
