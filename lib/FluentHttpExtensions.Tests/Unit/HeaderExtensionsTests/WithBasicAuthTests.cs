using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentHttpExtensions.Tests.Unit.HeaderExtensionsTests
{
    public class WithBasicAuthTests : UnitTest
    {
        [Fact]
        public async Task Can_Set_Basic_Auth_Header()
        {
            OnRequestMade(r =>
            {
                var decodedString =
                    Encoding.UTF8.GetString(Convert.FromBase64String(r.Headers.Authorization.Parameter));

                Assert.Equal("moogie:http", decodedString);
            });

            await HttpRequest
                .WithBasicAuth("moogie", "http")
                .EnsureSuccessStatusCode();
        }
    }
}
