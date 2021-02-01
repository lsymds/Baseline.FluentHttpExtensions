using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

// ReSharper disable NotNullMemberIsNotInitialized
#pragma warning disable 8618

namespace Baseline.FluentHttpExtensions.Tests.Unit.BodyContentExtensionsTests
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
            Action func = () => HttpRequest.WithJsonBody((object)null!);

            // Act & Assert.
            func.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Body_Is_Converted_And_Sent_Correctly()
        {
            OnRequestMade(r =>
            {
                var streamResponse = r.Content.ReadAsStringAsync();
                streamResponse.Wait();

                var convertedResponse = JsonSerializer.Deserialize<TestBody>(streamResponse.Result);

                convertedResponse.Name.Should().Be("Bob Smith");
                convertedResponse.Age.Should().Be(47);
            });

            await HttpRequest
                .AsAGetRequest()
                .WithJsonBody(new TestBody {Name = "Bob Smith", Age = 47})
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
