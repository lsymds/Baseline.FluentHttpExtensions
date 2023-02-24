using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.EndToEndTests
{
    public class EchoTests
    {
        [Fact]
        public async Task Can_Make_Request_With_Auth_And_Content_Type_Headers()
        {
            var response = await "https://postman-echo.com/put"
                .AsAPutRequest(new HttpClient())
                .WithTextBody("This is a test")
                .ReadResponseAsStringAsync();

            response.Should().Contain("This is a test");
        }

        [Theory]
        [InlineData("postman", "password", false)]
        [InlineData("postman", "wrong", true)]
        public async Task Can_Make_A_Request_With_Basic_Auth(string username, string password, bool shouldThrow)
        {
            Func<Task> func = async () => await "https://postman-echo.com/basic-auth"
                .AsAGetRequest(new HttpClient())
                .WithBasicAuth(username, password)
                .EnsureSuccessStatusCodeAsync();

            if (shouldThrow)
            {
                await func.Should().ThrowExactlyAsync<HttpRequestException>();
            }
            else
            {
                await func.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task Custom_Headers_Are_Added_Correctly()
        {
            var response = await "https://postman-echo.com/headers"
                .AsAGetRequest(new HttpClient())
                .WithRequestHeader("X-Custom-Header", "my-custom-header")
                .WithUserAgent("my-compoota")
                .WithRequestHeader("X-Help-Me", "the-robots-have-taken-over")
                .ReadResponseAsStringAsync();

            response.Should().Contain(@"""x-custom-header"": ""my-custom-header""");
            response.Should().Contain(@"""user-agent"": ""my-compoota""");
            response.Should().Contain(@"""x-help-me"": ""the-robots-have-taken-over""");
        }

        [Fact]
        public async Task Can_Post_Json()
        {
            var response = await "https://postman-echo.com/post"
                .AsAPostRequest(new HttpClient())
                .WithJsonBody(new {Id = "1", Name = "foo"})
                .ReadResponseAsStringAsync();

            response.Should().Contain(@"""Id"": ""1""");
            response.Should().Contain(@"""Name"": ""foo""");
        }

        [Fact]
        public async Task Can_Post_Form_Url_Encoded()
        {
            var response = await "https://postman-echo.com/post"
                .AsAPostRequest(new HttpClient())
                .WithFormUrlEncodedBody(
                    new KeyValuePair<string, string>("a", "b"),
                    new KeyValuePair<string, string>("aTrickyOne", "a!#,")
                )
                .ReadResponseAsStringAsync();

            response.Should().Contain(@"""json""");
            response.Should().Contain(@"""a"": ""b""");
            response.Should().Contain(@"""aTrickyOne"": ""a!#,""");
        }
    }
}
