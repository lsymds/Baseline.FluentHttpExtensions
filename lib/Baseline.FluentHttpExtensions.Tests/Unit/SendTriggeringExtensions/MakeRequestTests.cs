using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.SendTriggeringExtensions
{
    public class MakeRequestTests : UnitTest
    {
        [Fact]
        public async Task Can_Get_The_Request_Response()
        {
            var response = await HttpRequest
                .AsAGetRequest()
                .MakeRequest();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
