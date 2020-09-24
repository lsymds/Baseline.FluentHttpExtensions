using System;
using System.Threading.Tasks;
using Xunit;

namespace FluentHttpExtensions.Tests.Unit.BodyContentExtensions
{
    public class WithTextBodyTests : UnitTest
    {
        [Fact]
        public void Null_Body_Results_In_An_Exception_Being_Thrown()
        {
            void Act() => HttpRequest.WithTextBody(null);

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public void Null_Content_Type_Results_In_An_Exception_Being_Thrown()
        {
            void Act() => HttpRequest.WithTextBody("abc", "  ");

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public async Task Text_Body_Is_Sent_Correctly()
        {
            OnRequestMade(r =>
            {
                var content = r.Content.ReadAsStringAsync();
                content.Wait();

                Assert.Equal("text/plain; charset=utf-8", r.Content.Headers.ContentType.ToString());
                Assert.Equal("abc", content.Result);
            });

            await HttpRequest
                .WithTextBody("abc")
                .EnsureSuccessStatusCode();
        }
    }
}
