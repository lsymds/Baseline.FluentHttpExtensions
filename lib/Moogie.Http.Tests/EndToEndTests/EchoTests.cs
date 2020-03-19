using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.EndToEndTests
{
    public class EchoTests
    {
        [Fact]
        public async Task Can_Make_Request_With_Auth_And_Content_Type_Headers()
        {
            var response = await "https://postman-echo.com/put"
                .AsAPutRequest()
                .WithTextBody("This is a test")
                .ReadResponseAsString();

            Assert.Contains("This is a test", response);
        }

        [Fact]
        public async Task Can_Make_A_Request_With_Basic_Auth()
        {
            await "https://postman-echo.com/basic-auth"
                .AsAGetRequest()
                .WithBasicAuth("postman", "password")
                .EnsureSuccessStatusCode();

            var failedResponse = await "https://postman-echo.com/basic-auth"
                .AsAGetRequest()
                .WithBasicAuth("postman", "wrong-password")
                .MakeRequest();
            Assert.Throws<HttpRequestException>(() => failedResponse.EnsureSuccessStatusCode());
        }

        [Fact]
        public async Task Custom_Headers_Are_Added_Correctly()
        {
            var response = await "https://postman-echo.com/headers"
                .AsAGetRequest()
                .WithRequestHeader("X-Custom-Header", "my-custom-header")
                .WithUserAgent("my-compoota")
                .WithRequestHeader("X-Help-Me", "the-robots-have-taken-over")
                .ReadResponseAsString();

            Assert.Contains(@"""x-custom-header"":""my-custom-header""", response);
            Assert.Contains(@"""user-agent"":""my-compoota""", response);
            Assert.Contains(@"""x-help-me"":""the-robots-have-taken-over""", response);
        }

        [Fact]
        public async Task Can_Post_Json()
        {
            var response = await "https://postman-echo.com/post"
                .AsAPostRequest()
                .WithJsonBody(new {Id = "1", Name = "foo"})
                .ReadResponseAsString();

            Assert.Contains(@"""Id"":""1""", response);
            Assert.Contains(@"""Name"":""foo""", response);
        }
    }
}
