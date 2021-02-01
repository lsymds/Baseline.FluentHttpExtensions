using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.BodyContentExtensionsTests
{
    public class WithTextBodyTests : UnitTest
    {
        [Fact]
        public void Null_Body_Results_In_An_Exception_Being_Thrown()
        {
            Action func = () => HttpRequest.WithTextBody(null);

            func.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Null_Content_Type_Results_In_An_Exception_Being_Thrown()
        {
            Action func = () => HttpRequest.WithTextBody("abc", "  ");

            func.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Text_Body_Is_Sent_Correctly()
        {
            OnRequestMade(r =>
            {
                var content = r.Content.ReadAsStringAsync();
                content.Wait();

                r.Content.Headers.ContentType.ToString().Should().Be("text/plain; charset=utf-8");
                content.Result.Should().Be("abc");
            });

            await HttpRequest
                .WithTextBody("abc")
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
