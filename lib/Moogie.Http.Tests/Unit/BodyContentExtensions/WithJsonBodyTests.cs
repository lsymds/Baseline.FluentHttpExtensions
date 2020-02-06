using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.BodyContentExtensions
{
    public class TestBody
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class WithJsonBodyTests : UnitTest
    {
        [Fact]
        public void Null_Argument_Results_In_An_Exception_Being_Thrown()
        {
            // Arrange.
            void Act() => HttpRequest.WithJsonBody((object)null);

            // Act & Assert.
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public async Task Body_Is_Converted_And_Sent_Correctly()
        {
            OnRequestMade(r =>
            {
                var streamResponse = r.Content.ReadAsStringAsync();
                streamResponse.Wait();

                var convertedResponse = JsonSerializer.Deserialize<TestBody>(streamResponse.Result);

                Assert.Equal("Bob Smith", convertedResponse.Name);
                Assert.Equal(47, convertedResponse.Age);
            });

            await HttpRequest
                .AsAGetRequest()
                .WithJsonBody(new TestBody {Name = "Bob Smith", Age = 47})
                .EnsureSuccessStatusCode();
        }
    }
}
